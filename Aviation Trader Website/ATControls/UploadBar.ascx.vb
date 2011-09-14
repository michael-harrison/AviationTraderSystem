Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Image Upload bar
'*
'*
'***************************************************************************************

Partial Class ATControls_UploadBar
    Inherits System.Web.UI.UserControl
    Public Event ImageUploadEvent(ByVal Image As Image)
    Public Event ImageDeleteEvent()

    Private _name As String
    Private _xtn As String
    Private _type As Image.ImageTypes
    Private _Ad As Ad
    Private _sys As ATSystem
   
    Public Property Ad() As Ad
        Get
            Return _Ad
        End Get
        Set(ByVal value As Ad)
            _Ad = value
        End Set
    End Property

    Public Property Text() As String
        Get
            Return BtUpload.Text
        End Get
        Set(ByVal value As String)
            BtUpload.Text = value
        End Set
    End Property

    Public Property Sys() As ATSystem
        Get
            Return _sys
        End Get
        Set(ByVal value As ATSystem)
            _sys = value
        End Set
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return FileUpload1.FileName
        End Get
    End Property

    Public ReadOnly Property SourceFileName() As String
        Get
            Return FileUpload1.PostedFile.FileName
        End Get
    End Property

    Public ReadOnly Property Type() As Image.ImageTypes
        Get
            Return _type
        End Get
    End Property

    Public Property Msg() As String
        Get
            Return msgbox.Text
        End Get
        Set(ByVal value As String)
            msgbox.Text = value
        End Set
    End Property

    Public Sub SaveAs(ByVal filename As String)
        FileUpload1.SaveAs(filename)
    End Sub

    Private Sub processRequest()

        msgbox.Text = ""
        If Not FileUpload1.HasFile Then
            msgbox.Text = Constants.UploadSelect
        Else
            '
            ' get the file type from the browser determine image file type
            '
            Dim fileInfo As System.IO.FileInfo
            fileInfo = My.Computer.FileSystem.GetFileInfo(FileUpload1.FileName)
            _name = fileInfo.Name
            _xtn = fileInfo.Extension.ToLower
            _type = CommonRoutines.Xtn2Type(_xtn)

            If _type = Image.ImageTypes.Unknown Then
                msgbox.Text = Constants.UnsupportedFileType
            Else
                '
                ' create image object, save file and notify via event handler
                '
                Dim Image As New Image
                Image.OriginalSourcePath = _sys.SourceImageOriginalFolder
                Image.WorkingSourcePath = _sys.SourceImageWorkingFolder
                Image.PhysicalApplicationPath = _sys.PhysicalApplicationPath
                Image.AdID = _Ad.ID
                Image.Type = _type
                Image.ProdnStatus = ATLib.Image.ProdnState.Initial
                Image.PreviewSequence = 0
                Image.IsWebEnabled = True                 'show web image as default
                '
                ' if this is the first image, set the main image on
                ' dont call ad.images.count since it loads the collection
                '
                Image.IsMainImage = False
                Dim myImages As New Images
                myImages.RetrieveSet(_Ad.ID)
                If myImages.Count = 0 Then Image.IsMainImage = True
                '
                ' get image size from bitmap if its a regular image
                '

                Select Case _type
                    Case ATLib.Image.ImageTypes.PDF
                        Image.PixelHeight = 0
                        Image.PixelWidth = 0
                        Image.Resolution = 1

                    Case ATLib.Image.ImageTypes.EPS
                        Image.PixelHeight = 0
                        Image.PixelWidth = 0
                        Image.Resolution = 1

                    Case Else
                        '
                        ' may not work if invalid image format
                        '
                        Try
                            Using bitmap As New System.Drawing.Bitmap(FileUpload1.FileContent)
                                Dim size As System.Drawing.SizeF = bitmap.PhysicalDimension
                                Image.PixelWidth = Convert.ToInt32(size.Width)
                                Image.PixelHeight = Convert.ToInt32(size.Height)
                                Image.Resolution = Convert.ToInt32(bitmap.HorizontalResolution)
                            End Using
                        Catch ex As Exception
                            Image.PixelWidth = 100
                            Image.PixelHeight = 100
                            Image.Resolution = 100
                        End Try


                End Select

                Image.Update()            'generates image.id
                '
                ' save uploaded image to original folder and make a copy to working folder
                '
                FileUpload1.SaveAs(Image.OriginalSourceFileName)
                IO.File.Copy(Image.OriginalSourceFileName, Image.WorkingSourceFileName, True)

                If Image.Type = ATLib.Image.ImageTypes.PDF Then
                    generateJPGfromPDF(Image)       'engine generates images and updates size
                Else
                    '
                    ' generate thb and lores subsamples
                    '
                    Image.GenerateSubsample(Image.THBFilename, _sys.THBImageHeight)
                    Image.GenerateSubsample(Image.LoResFilename, _sys.LRImageHeight)
                    '
                    ' for eps images, set the size and res from the low res preview
                    '
                    If Image.Type = ATLib.Image.ImageTypes.EPS Then
                        Try
                            Using bitmap As New System.Drawing.Bitmap(Image.LoResFilename)
                                Dim size As System.Drawing.SizeF = bitmap.PhysicalDimension
                                Image.PixelWidth = Convert.ToInt32(size.Width)
                                Image.PixelHeight = Convert.ToInt32(size.Height)
                                Image.Resolution = Convert.ToInt32(bitmap.HorizontalResolution)
                            End Using
                        Catch ex As Exception
                            Image.PixelWidth = 100
                            Image.PixelHeight = 100
                            Image.Resolution = 100
                        End Try
                        Image.Update()
                    End If




                End If

                RaiseEvent ImageUploadEvent(Image)
            End If
        End If
    End Sub


    Private Sub generateJPGfromPDF(ByVal image As Image)
        '
        ' calls the engine to generate a jpg pic from the supplied PDF
        '
        Try
            Dim Q As New EQItem
            Q.ObjectID = image.ID
            Q.Command = EQItem.CommandBits.JPGfromPDF Or EQItem.CommandBits.SuspendUntilComplete
            Dim Engine As Engine = Sys.MapEngine(ATSystem.EngineModes.Client)
            Q = Engine.Enqueue(Q)
        Catch ex As Exception
            '
            ' use the default pdf images
            '
            image.GenerateSubsample(image.THBFilename, _sys.THBImageHeight)
            image.GenerateSubsample(image.LoResFilename, _sys.LRImageHeight)
        End Try


    End Sub

    Protected Sub Upload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtUpload.Click
        processRequest()
    End Sub

    Protected Sub Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtDelete.Click
        RaiseEvent ImageDeleteEvent()
    End Sub

End Class

