Option Explicit On
Option Strict On
Imports ATLib
Imports EngineLib
Imports System
Imports System.IO
Imports InDesign


'***************************************************************************************
'*
'* DisplayWizard
'*
'* AUDIT TRAIL
'* 
'* V1.000   25-NOV-2009  BA  Original
'*
'* Display ad wizard
'*
'* to get the type of and IND object use:  Dim s As String = Information.TypeName(ob)
'* 
'*
'*
'***************************************************************************************
Module Dump

    Private IDS As EngineLib.INDService
    Private Sys As ATSystem
    Private INDDoc As InDesign.Document
    Private currentPage As InDesign.Page
    Private mainbodyTF As InDesign.TextFrame
    Private columnhdrsTF As InDesign.TextFrame
    '
    ' reserved styles
    '
    Private catHdrPStyle As ParagraphStyle
    Private colHdrPStyle As ParagraphStyle
    Private classadPStyle As ParagraphStyle
    Private classadNoLinePStyle As ParagraphStyle
    Private clsHdrPstyle As ParagraphStyle
    Private keyCStyle As CharacterStyle
    Private noneCStyle As CharacterStyle

    Private currentColumnNumber As Integer



    Friend Sub Dump(ByVal pub As Publication, ByVal edition As Edition, ByVal ProdnStatus As Ad.ProdnState, ByVal INDDfilename As String, ByVal BillFilename As String)
        '
        ' connect to IND and open file
        '
        Sys = New ATSystem
        Sys.Retrieve()
        IDS = New EngineLib.INDService()
        IDS.InitializeINDD()
        IDS.OpenDoc(INDDfilename)
        INDDoc = IDS.INDDoc
        ' ''
        ' '' Open the billing file, for overwrite
        ' ''
        ''Dim fs As New FileStream(BillFilename, FileMode.Create)
        ''Dim sw As New StreamWriter(fs)
        ''sw.WriteLine(formatBillHdr)
        '
        ' find the column and main body text frames by xml tag
        '
        currentPage = CType(INDDoc.Pages(1), InDesign.Page)        'set to first page
        currentColumnNumber = 0
        '
        ' order of text frames on page is very important!! Dont change template
        '
        columnhdrsTF = CType(currentPage.TextFrames(2), TextFrame)
        mainbodyTF = CType(currentPage.TextFrames(1), TextFrame)
        '
        ' get the key styles
        '
        For Each ps As ParagraphStyle In INDDoc.ParagraphStyles
            If ps.Name.ToLower = "classad" Then classadPStyle = ps
            If ps.Name.ToLower = "classad no line" Then classadNoLinePStyle = ps
            If ps.Name.ToLower = "colhdr" Then colHdrPStyle = ps
            If ps.Name.ToLower = "cathdr" Then catHdrPStyle = ps
            If ps.Name.ToLower = "header + rules" Then clsHdrPstyle = ps
        Next

        For Each cs As CharacterStyle In INDDoc.CharacterStyles
            If cs.Name.ToLower = "firstwords" Then keyCStyle = cs
            If cs.Name.ToLower = "[none]" Then noneCStyle = cs
        Next
        '
        ' get the set of instances for the edition- returns in Category:Classification:SortKey order
        '
        Dim AdInstances As New AdInstances
        AdInstances.RetrieveDumpSet(edition.ID, ProdnStatus)
        '
        ' set the max progress bar
        '
        Form1.DumpProgress.Maximum = AdInstances.Count
        '
        ' output instances
        '
        Dim currentClassID As Integer = ATSystem.SysConstants.nullValue
        Dim currentCatID As Integer = ATSystem.SysConstants.nullValue
        Dim firstAd As Boolean = True
        For Each AI As AdInstance In AdInstances
            Dim Ad As Ad = AI.Ad            'get the ad
            Form1.DumpProgress.Increment(1)
            '
            ' if the category changes, dump the cat header
            '
            Dim cat As Category = Ad.Classification.Category
            If cat.ID <> currentCatID Then
                currentCatID = cat.ID
                dumpHdr(catHdrPStyle, cat.Name)
            End If
            '
            ' if the classification changes, dump the class header,except for the first ad
            '
            If Ad.ClassificationID <> currentClassID Then
                currentClassID = Ad.ClassificationID
                ''       If Not firstAd Then dumpHdr(clsHdrPstyle, Ad.ClassificationName)
                dumpHdr(clsHdrPstyle, Ad.ClassificationName)
            End If
            firstAd = False

            dumpAd(Ad, classadPStyle, AI.ProductType)
            setBillFlag(Ad)
            '' billAd(sw, Ad)
            '
            ' output top column header if a new column is encountered
            '
            Dim n As Integer = getCurrentColumnNumber()
            If n <> currentColumnNumber Then
                currentColumnNumber = n
                addColHdr(Ad.Classification.Name)
            End If
        Next
        '
        ' close the billing file
        '
        '' sw.Close()


    End Sub

    Private Function getCurrentColumnNumber() As Integer
        '
        ' routine to find which column is being populated. Assumes max of 8 pages and 4 columns per page
        '
        Dim rtnval As Integer = 0
        Dim nextTF As TextFrame = mainbodyTF

        Do While Not nextTF Is Nothing
            rtnval += nextTF.TextColumns.Count
            nextTF = CType(nextTF.NextTextFrame, TextFrame)
        Loop
        Return rtnval

    End Function


    Private Sub addColHdr(ByVal text As String)
        '
        ' adds to the fixed column headers
        '
        Dim parentStory As InDesign.Story = columnhdrsTF.ParentStory

        Dim textToken As InsertionPoint
        textToken = CType(parentStory.InsertionPoints.LastItem, InsertionPoint)
        textToken.Contents = text & vbCr
        textToken.ApplyCharacterStyle(noneCStyle)
        textToken.ApplyParagraphStyle(colHdrPStyle)
    End Sub

    Private Sub dumpHdr(ByVal pstyle As InDesign.ParagraphStyle, ByVal text As String)
        Dim parentStory As InDesign.Story = mainbodyTF.ParentStory

        Dim textToken As InsertionPoint
        textToken = CType(parentStory.InsertionPoints.LastItem, InsertionPoint)
        textToken.Contents = text & vbCr
        textToken.ApplyCharacterStyle(noneCStyle)
        textToken.ApplyParagraphStyle(pstyle)

    End Sub

    Private Sub dumpAd(ByVal Ad As Ad, ByVal style As ParagraphStyle, ByVal productType As Product.Types)

        Dim parentStory As InDesign.Story = mainbodyTF.ParentStory

        Dim textToken As InsertionPoint
        textToken = CType(parentStory.InsertionPoints.LastItem, InsertionPoint)
        '
        ' insert pic if required but only if found
        '
        If productType = Product.Types.ClassadColorPic Or productType = Product.Types.ClassadMonoPic Then
            If Not Ad.MainImage Is Nothing Then
                insertPic(textToken, Ad.MainImage)
            End If
        End If

        textToken.Contents = Ad.KeyWords
        textToken.ApplyCharacterStyle(keyCStyle)

        textToken = CType(parentStory.InsertionPoints.LastItem, InsertionPoint)
        textToken.Contents = " " & Ad.TrimmedText & vbCr
        textToken.ApplyCharacterStyle(noneCStyle)
        textToken.ApplyParagraphStyle(style)

    End Sub


    Private Sub insertPic(ByVal textToken As InDesign.InsertionPoint, ByVal Image As ATLib.Image)
        Dim anchoredpicheight As Double = Sys.ClassadPicHeight / 1000
        Dim anchoredpic As TextFrame = textToken.TextFrames.Add
        anchoredpic.AnchoredObjectSettings.AnchoredPosition = idAnchorPosition.idInlinePosition
        anchoredpic.AnchoredObjectSettings.AnchorYoffset = 1
        anchoredpic.AnchoredObjectSettings.AnchorPoint = idAnchorPoint.idBottomRightAnchor
        '
        ' T L B R
        '
        Dim anchoredPicWidth As Double = Convert.ToDouble(mainbodyTF.TextFramePreferences.TextColumnFixedWidth)
        Dim picbounds() As Double = {0, 0, anchoredpicheight, anchoredPicWidth}
        anchoredpic.GeometricBounds = picbounds
        anchoredpic.ContentType = idContentType.idGraphicType
        anchoredpic.Place(Image.WorkingSourceFileName)

        If anchoredpic.AllGraphics.Count > 0 Then
            Dim placedObject As Object = anchoredpic.AllGraphics(1)
            Dim objectType As String = Information.TypeName(placedObject)

            Dim arr As System.Array = Nothing
            Dim im As InDesign.Image = Nothing
            Dim eps As InDesign.EPS = Nothing
            Dim pdf As InDesign.PDF = Nothing

            Select Case objectType
                Case "Image"
                    im = CType(placedObject, InDesign.Image)
                    arr = CType(im.GeometricBounds, System.Array)

                Case "EPS"
                    eps = CType(placedObject, InDesign.EPS)
                    arr = CType(eps.GeometricBounds, System.Array)

                Case "PDF"
                    pdf = CType(placedObject, InDesign.PDF)
                    arr = CType(pdf.GeometricBounds, System.Array)

                Case Else
                    Throw New Exception("Image type unknown in placed ad")

            End Select
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
            Dim boxAR As Double = anchoredPicWidth / anchoredpicheight

            If boxAR < picAR Then
                height = anchoredpicheight
                width = height * picAR
                B = T + height
                L = L - (width - anchoredPicWidth) / 2
                R = L + width
            Else
                width = anchoredPicWidth
                height = width / picAR
                T = T - (height - anchoredpicheight) / 2
                B = T + height
                R = L + width
            End If

            arr.SetValue(T, 0)
            arr.SetValue(L, 1)
            arr.SetValue(B, 2)
            arr.SetValue(R, 3)

            Select Case objectType
                Case "Image" : im.GeometricBounds = arr
                Case "EPS" : eps.GeometricBounds = arr
                Case "PDF" : pdf.GeometricBounds = arr
            End Select

        End If

    End Sub

    Private Sub setBillFlag(ByVal ad As Ad)
        '
        ' updates the billing status to ready
        '
        ad.BillMe = True
        ad.Update()
    End Sub

    Private Sub billAd(ByVal sw As streamwriter, ByVal Ad As Ad)
        '
        ' note - this is called of the adinstance loop. If the customer books the same ad to have multiple instances
        ' then multiple bill records will be produced
        '
        For Each AI As AdInstance In Ad.Instances
            sw.WriteLine(formatInstance(AI))
        Next
    End Sub

    Private Function formatBillHdr() As String
        Dim valueArray() As String = { _
   "Ad Number", _
   "Category", _
   "Classification", _
   "Product", _
   "Product Type", _
   "Edition", _
   "Instance Price", _
   "Price Adj", _
   "Total Price", _
   "ACCT Alias", _
   "Email", _
   "Name", _
   "Company" _
}
        Return String.Join(vbTab, valueArray)
    End Function


    Private Function formatInstance(ByVal AI As AdInstance) As String
        Dim valueArray() As String = { _
   AI.AdNumber, _
   AI.Ad.CategoryName, _
   AI.Ad.ClassificationName, _
   AI.Product.Name, _
   AI.ProductType.ToString, _
   AI.Edition.Name, _
   CommonRoutines.Integer2Dollars(AI.Price), _
   CommonRoutines.Integer2Dollars(AI.PriceAdjust), _
   CommonRoutines.Integer2Dollars(AI.Price + AI.PriceAdjust), _
   AI.Ad.Usr.AcctAlias, _
   AI.Ad.Usr.EmailAddr, _
   AI.Ad.Usr.FullName, _
   AI.Ad.Usr.Company _
}
        Return String.Join(vbTab, valueArray)
    End Function


End Module
