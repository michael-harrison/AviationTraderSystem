Option Explicit On
Option Strict On
Imports ATLib
Imports System.IO
Imports InDesign

'***************************************************************************************
'*
'* Engine
'*
'* AUDIT TRAIL
'* 
'* V1.000   19-NOV-2009  BA  Original
'*
'* front end driver for InDesign
'*
'*
'*
'*
'***************************************************************************************

Module ProcessJob


    Private Const eol As String = ControlChars.Lf
    '
    ' reserved styles
    '
    Private classadPStyle As ParagraphStyle
    Private keyCStyle As CharacterStyle
    Private noneCStyle As CharacterStyle


    Friend Sub processClassad(ByVal Q As EQItem)
        '
        ' builds proof image and PDF for classad
        '
        Dim AdInstance As AdInstance = Nothing
        Dim StopWatch As New System.Diagnostics.Stopwatch
        StopWatch.Start()

        Dim faciaMsg As String = ""
        Try
            '
            ' get the ad instance
            '
            Dim AdInstances As New AdInstances
            AdInstance = AdInstances.Retrieve(Q.ObjectID)
            AdInstance.PhysicalApplicationPath = Sys.PhysicalApplicationPath
            AdInstance.IsPreviewValid = False         'invalidate current preview
            Try
                IO.File.Delete(AdInstance.PreviewPDFFilename)   'delete current files
                IO.File.Delete(AdInstance.PreviewImageFilename)
            Catch
            End Try
            AdInstance.PreviewSequence += 1                  'bump transaction number
            '
            ' open template in InDesign
            '
            GL.INDService.OpenDoc(Sys.ClassadTemplate)

            '
            ' get the key styles
            '
            For Each ps As ParagraphStyle In INDService.INDDoc.ParagraphStyles
                If ps.Name.ToLower = "classad no line" Then classadPStyle = ps
            Next

            For Each cs As CharacterStyle In INDService.INDDoc.CharacterStyles
                If cs.Name.ToLower = "firstwords" Then keyCStyle = cs
                If cs.Name.ToLower = "[none]" Then noneCStyle = cs
            Next
            '
            ' check that the styles exist
            '
            If classadPStyle Is Nothing Then Throw New Exception("Missing Classad No Line paragraph style")
            If keyCStyle Is Nothing Then Throw New Exception("Missing Classad Key Style")
            If noneCStyle Is Nothing Then Throw New Exception("Missing [None] Style")


            buildAd(AdInstance)
            '
            ' adjust dimensions and return height in 1000's of mm including the pic
            '
            Dim adsize() As Double = GL.INDService.GetClassadSize()
            AdInstance.ExactWidth = Convert.ToInt32(adsize(0) * 1000)
            AdInstance.ExactHeight = Convert.ToInt32(adsize(1) * 1000)
            AdInstance.CalculateClassadSize()
            '
            ' produce output files
            '
            GL.INDService.ExportLoRes(AdInstance.PreviewImageFilename, 400)
            GL.INDService.ExportProofPDF(AdInstance.PreviewPDFFilename)
            '
            ' if we get to here, set valid preview bit
            '
            AdInstance.IsPreviewValid = True
            '
            ' test for timeout
            '
            Q.SetStatusBits(EQItem.StatusBits.Complete)
            If Q.TestStatusBits(EQItem.StatusBits.Timeout) Then
                faciaMsg = "Ad " & AdInstance.Ad.Adnumber & " timed out"
            Else
                faciaMsg = "Ad " & AdInstance.Ad.Adnumber & " processed successfully"
            End If

        Catch ex As Exception
            '
            ' we may not have an instance object
            '
            If AdInstance Is Nothing Then
                faciaMsg = " Failed to process AdInstance ID = " & Q.ObjectID.ToString & " because " & ex.Message
            Else
                faciaMsg = " Failed to process Ad " & Q.ObjectID.ToString & ":" & AdInstance.Ad.Adnumber & " because " & ex.Message
            End If
            Q.SetStatusBits(EQItem.StatusBits.Errored)
        Finally
            '
            ' update instance, close inddoc, show complete and resume client if required
            '
            AdInstance.Update()
            StopWatch.Stop()
            Q.SetStatusBits(EQItem.StatusBits.ResumeClient)

            faciaMsg &= " in " & Convert.ToInt32(StopWatch.ElapsedMilliseconds) & " ms"
            EngineForm.Facia1.Msg(faciaMsg)
            GL.INDService.AbortDoc()

        End Try
    End Sub

    Private Sub buildAd(ByVal AdInstance As AdInstance)

        Dim currentPage As InDesign.Page = CType(INDService.INDDoc.Pages(1), InDesign.Page)        'set to first page

        Dim tf As InDesign.TextFrame = CType(currentPage.TextFrames(1), InDesign.TextFrame)

        Dim parentStory As InDesign.Story = tf.ParentStory

        Dim textToken As InsertionPoint
        textToken = CType(parentStory.InsertionPoints.LastItem, InsertionPoint)
        '
        ' insert pic if required
        '
        If AdInstance.ProductType = Product.Types.ClassadColorPic Or AdInstance.ProductType = Product.Types.ClassadMonoPic Then
            insertPic(Convert.ToDouble(tf.TextFramePreferences.TextColumnFixedWidth), textToken, AdInstance.Ad.MainImage)
        End If

        textToken.Contents = AdInstance.Ad.KeyWords
        textToken.ApplyCharacterStyle(keyCStyle)

        textToken = CType(parentStory.InsertionPoints.LastItem, InsertionPoint)
        textToken.Contents = " " & AdInstance.Ad.TrimmedText & vbCr
        textToken.ApplyCharacterStyle(noneCStyle)
        textToken.ApplyParagraphStyle(classadPStyle)

    End Sub


    Private Sub insertPic(ByVal AnchoredPicWidth As Double, ByVal textToken As InDesign.InsertionPoint, ByVal Image As ATLib.Image)
        '
        ' throw exception if there is no pic to insert
        '
        If Image Is Nothing Then Throw New Exception("Classad with pic requested but ad has no pic")
        Dim anchoredPicHeight As Double = Sys.ClassadPicHeight / 1000
        Dim anchoredpic As TextFrame = textToken.TextFrames.Add
        anchoredpic.AnchoredObjectSettings.AnchoredPosition = idAnchorPosition.idInlinePosition
        anchoredpic.AnchoredObjectSettings.AnchorYoffset = 0
        anchoredpic.AnchoredObjectSettings.AnchorPoint = idAnchorPoint.idBottomRightAnchor
        '
        ' T L B R
        '

        Dim picbounds() As Double = {0, 0, anchoredPicHeight, AnchoredPicWidth}
        anchoredpic.GeometricBounds = picbounds
        anchoredpic.ContentType = idContentType.idGraphicType
        anchoredpic.Place(Image.WorkingSourceFileName)

        If anchoredpic.AllGraphics.Count > 0 Then
            Dim placedObject As Object = anchoredpic.AllGraphics(1)
            Dim objectType As String = Information.TypeName(placedObject)

            Dim arr As System.Array
            Dim im As InDesign.Image = Nothing
            Dim eps As InDesign.EPS = Nothing

            If objectType = "EPS" Then
                eps = CType(placedObject, InDesign.EPS)
                arr = CType(eps.GeometricBounds, System.Array)
            ElseIf objectType = "Image" Then
                im = CType(placedObject, InDesign.Image)
                arr = CType(im.GeometricBounds, System.Array)
            Else
                Throw New Exception("Image type unknown in placed ad")
            End If
            '
            ' get image bounds and scale pic to fit laterally and be vertically centred in box.
            '
            Dim T As Double = Convert.ToDouble(arr.GetValue(0))
            Dim L As Double = Convert.ToDouble(arr.GetValue(1))
            Dim B As Double = Convert.ToDouble(arr.GetValue(2))
            Dim R As Double = Convert.ToDouble(arr.GetValue(3))

            Dim width As Double = R - L
            Dim height As Double = B - T
            Dim picAR As Double = width / height
            Dim boxAR As Double = anchoredPicWidth / anchoredPicHeight

            If boxAR < picAR Then
                height = anchoredPicHeight
                width = height * picAR
                B = T + height
                L = L - (width - anchoredPicWidth) / 2
                R = L + width
            Else
                width = anchoredPicWidth
                height = width / picAR
                T = T - (height - anchoredPicHeight) / 2
                B = T + height
                R = L + width
            End If

            arr.SetValue(T, 0)
            arr.SetValue(L, 1)
            arr.SetValue(B, 2)
            arr.SetValue(R, 3)

            If objectType = "EPS" Then
                eps.GeometricBounds = arr
            Else
                im.GeometricBounds = arr
            End If
        End If

    End Sub


    Friend Sub processJPGfromPDF(ByVal Q As EQItem)
        '
        ' builds proof image for finished art
        '
        Dim Image As ATLib.Image = Nothing
        Dim StopWatch As New System.Diagnostics.Stopwatch
        StopWatch.Start()

        Dim faciaMsg As String = ""

        Try
            '
            ' get the image object
            '
            Dim Images As New ATLib.Images
            Image = Images.Retrieve(Q.ObjectID)
            Image.PhysicalApplicationPath = Sys.PhysicalApplicationPath
            '
            ' create new document in InDesign
            '
            Dim docsize() As Double = {200, 200}
            GL.INDService.NewDoc(docsize)

            Dim picBox As InDesign.Rectangle = GL.INDService.INDDoc.Rectangles.Add
            picBox.ContentType = idContentType.idGraphicType
            picBox.Place(Image.WorkingSourceFileName, False)

            Dim boxsize() As Double = {0, 0, 200, 200}
            picBox.GeometricBounds = boxsize
            picBox.Fit(idFitOptions.idFrameToContent)
            '
            ' set the image size in pixels and resolution so that mm can be derived
            ' choose arbitary resolution
            '
            Dim arr As System.Array = CType(picBox.GeometricBounds, System.Array)
            Dim T As Double = Convert.ToDouble(arr.GetValue(0))
            Dim L As Double = Convert.ToDouble(arr.GetValue(1))
            Dim B As Double = Convert.ToDouble(arr.GetValue(2))
            Dim R As Double = Convert.ToDouble(arr.GetValue(3))

            Dim h As Double = (B - T) * 0.0393700787    'mm --> inches
            Dim w As Double = (R - L) * 0.0393700787     'mm --> inches


            Dim resolution As Integer = 300
            Image.PixelHeight = Convert.ToInt32(h * resolution)
            Image.PixelWidth = Convert.ToInt32(w * resolution)
            Image.Resolution = resolution
            '
            ' get a temporary filename and use it to generate lores and thb subsamples
            '
            Dim tempfilename As String = IO.Path.GetTempFileName
            GL.INDService.ExportPicBox(picBox, tempfilename)
            Image.GenerateSubsample(tempfilename, Image.LoResFilename, Sys.LRImageHeight)
            Image.GenerateSubsample(tempfilename, Image.THBFilename, Sys.THBImageHeight)
            IO.File.Delete(tempfilename)
            '
            ' test for timeout
            '
            Q.SetStatusBits(EQItem.StatusBits.Complete)
            If Q.TestStatusBits(EQItem.StatusBits.Timeout) Then
                faciaMsg = "Image " & Image.ShortFileName & " timed out"
            Else
                faciaMsg = "Image " & Image.ShortFileName & " processed successfully"
            End If

        Catch ex As Exception
            '
            ' we may not have an instance object
            '
            If Image Is Nothing Then
                faciaMsg = " Failed to process image ID = " & Q.ObjectID.ToString & " because " & ex.Message
            Else
                faciaMsg = " Failed to process image " & Q.ObjectID.ToString & ":" & Image.ShortFileName & " because " & ex.Message
            End If
            Q.SetStatusBits(EQItem.StatusBits.Errored)
        Finally
            '
            ' update image, close inddoc, show complete and resume client if required
            '
            Image.Update()
            StopWatch.Stop()

            Q.SetStatusBits(EQItem.StatusBits.ResumeClient)

            faciaMsg &= " in " & Convert.ToInt32(StopWatch.ElapsedMilliseconds) & " ms"
            EngineForm.Facia1.Msg(faciaMsg)
            GL.INDService.AbortDoc()

        End Try
    End Sub

    Private Sub autosize(ByVal Adinstance As AdInstance)
        '
        ' convert the exact size to col cm, rounding up
        '
        Dim h As Double = Adinstance.ExactHeight / 10000
        Dim hfloor As Double = Math.Floor(h)
        If hfloor <> h Then hfloor += 1
        Adinstance.Height = Convert.ToInt32(hfloor)
        If Adinstance.Height > GL.Sys.DisplayColumnHeight Then
            Adinstance.Height = Convert.ToInt32(GL.Sys.DisplayColumnHeight / 10)
        End If
        If Adinstance.Height < 2 Then
            Adinstance.Height = 2
        End If
        '
        ' allow a gutter width tolerance on the width
        '
        Dim cw As Integer = GL.Sys.DisplayColumnWidth
        Dim gw As Integer = GL.Sys.DisplayGutterWidth
        Dim w As Integer = Adinstance.ExactWidth
        If w <= cw + gw + cw + gw Then
            Adinstance.Width = RateTable.DisplayWidths.Col2
        ElseIf w < cw + gw + cw + gw + cw + gw Then
            Adinstance.Width = RateTable.DisplayWidths.Col3
        ElseIf w < cw + gw + cw + gw + cw + gw + cw + gw Then
            Adinstance.Width = RateTable.DisplayWidths.Col4
        ElseIf w < cw + gw + cw + gw + cw + gw + cw + gw + cw + gw Then
            Adinstance.Width = RateTable.DisplayWidths.Col5
        ElseIf w < cw + gw + cw + gw + cw + gw + cw + gw + cw + gw + cw + gw + cw Then
            Adinstance.Width = RateTable.DisplayWidths.Col7
        Else
            Adinstance.Width = RateTable.DisplayWidths.Col7
        End If
    End Sub

    Friend Sub processTextFromPDF(ByVal Q As EQItem)
        '
        ' builds proof image for finished art
        '
        Dim Ads As New Ads
        Dim Ad As Ad = Ads.Retrieve(Q.ObjectID)

        Dim StopWatch As New System.Diagnostics.Stopwatch
        StopWatch.Start()
        Dim faciaMsg As String = ""

        Try
            Dim PDFAssistant As New EngineLib.PDFAssistant
            Ad.Text = PDFAssistant.ExtractTextFromPDF(Ad.MainImage.WorkingSourceFileName)
            '
            ' remap ad status if necessary
            '
            Select Case Ad.ProdnStatus
                Case ATLib.Ad.ProdnState.Approved : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
                Case ATLib.Ad.ProdnState.Proofed : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
            End Select
            '
            ' test for timeout
            '
            Q.SetStatusBits(EQItem.StatusBits.Complete)
            If Q.TestStatusBits(EQItem.StatusBits.Timeout) Then
                faciaMsg = "Text Extraction for " & Ad.Adnumber & " timed out"
            Else
                faciaMsg = "Text Extraction for " & Ad.Adnumber & " processed successfully"
            End If

        Catch ex As Exception
            '
            ' we may not have an instance object
            '
            If Ad Is Nothing Then
                faciaMsg = " Failed to process ad ID = " & Q.ObjectID.ToString & " because " & ex.Message
            Else
                faciaMsg = " Failed to process ad " & Q.ObjectID.ToString & ":" & Ad.Adnumber & " because " & ex.Message
            End If
            Q.SetStatusBits(EQItem.StatusBits.Errored)
        Finally
            '
            ' update ad, show complete and resume client if required
            '
            Ad.Update()
            StopWatch.Stop()
            Q.SetStatusBits(EQItem.StatusBits.ResumeClient)
            faciaMsg &= " in " & Convert.ToInt32(StopWatch.ElapsedMilliseconds) & " ms"
            EngineForm.Facia1.Msg(faciaMsg)
        End Try
    End Sub
End Module
