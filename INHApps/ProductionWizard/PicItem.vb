Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* AT Pic Item
'*
'* AUDIT TRAIL
'* 
'* V1.000   01-Dec-2009  BA  Original
'*
'* This is pic display for the ad
'*
'*
'*
'*
'***************************************************************************************

Public Class PicItem

    Private _image As Image
    Private _thbHeight As Integer
    Private _loresHeight As Integer

    Public Enum CheckDirn As Integer
        Checkout = 0
        Checkin = 1
    End Enum

    Public Event CheckInOut(ByVal sender As PicItem, ByVal dirn As CheckDirn)
    Public Event ImageCopy(ByVal sender As PicItem)

    Public Sub New(ByVal Image As Image)
        _image = Image
        InitializeComponent()
    End Sub


    Public Property Image() As Image
        Get
            Return _image
        End Get
        Set(ByVal value As Image)
            _image = value
        End Set
    End Property

    Public Property LoresHeight() As Integer
        Get
            Return _loresHeight
        End Get
        Set(ByVal value As Integer)
            _loresHeight = value
        End Set
    End Property

    Public Property THBHeight() As Integer
        Get
            Return _thbHeight
        End Get
        Set(ByVal value As Integer)
            _thbHeight = value
        End Set
    End Property


    Public Sub Render()
        btnCheckout.Text = Form1.checkoutText
        btnCheckout.BackColor = Color.Beige              'its not checked out anywhere
        filenameBox.Text = _image.ShortFileName
        '
        ' if the file is offline, change the filename and suppress checkout button
        '
        If Not IO.File.Exists(_image.WorkingSourceFileName) Then
            btnCheckout.Enabled = False
            filenameBox.Text &= " (Offline)"
        End If

        PixelSizeBox.Text = _image.PixelWidth & " x " & _image.PixelHeight
        mmSizeBox.Text = Convert.ToInt32(25.4 * _image.PixelWidth / _image.Resolution) & _
         " x " & _
         Convert.ToInt32(25.4 * _image.PixelHeight / _image.Resolution)
        ResolutionBox.Text = _image.Resolution.ToString
        '
        ' Note - cant use image.fromfile since it leaves file open. Copy to memory stream first
        '
        Try
            Using ms As New IO.MemoryStream
                Using fs As New IO.FileStream(_image.LoResFilename, IO.FileMode.Open)
                    ms.SetLength(fs.Length)
                    fs.Read(ms.GetBuffer(), 0, Convert.ToInt32(fs.Length))
                    ms.Flush()
                    fs.Close()
                End Using
                PicBox.BackgroundImage = System.Drawing.Image.FromStream(ms)

            End Using
        Catch ex As Exception
        End Try


    End Sub

    Private Sub checkout()

        Try
            Process.Start("Photoshop.exe", Image.WorkingSourceFileName)             'start indesign and open file
            btnCheckout.Text = Form1.checkinText
            btnCheckout.BackColor = Color.Coral
            btnRecover.Enabled = False
            _image.IsCheckedOut = True
            _image.Update()

        Catch ex As Exception
  
        End Try

    End Sub

   

    Private Sub checkin()
        '
        ' regenerate new low res and thumbnail image
        '
        _image.IsCheckedOut = False
        generateSubsamples()            'includes image.update
        btnCheckout.BackColor = Color.Beige
        btnRecover.Enabled = True
        Render()          'and redraw control

    End Sub

    Private Sub recoverOriginal()
        '
        ' copies the original image into the working image
        '
        Dim sourceFilename As String = Image.OriginalSourceFileName
        Dim destnFilename As String = Image.WorkingSourceFileName
        IO.File.Copy(sourceFilename, destnFilename, True)
        generateSubsamples()
        Render()

    End Sub

    Private Sub copyImage()
        '
        ' makes a copy of the image object and all its files
        '
        Dim newImage As New Image
        newImage.OriginalSourcePath = Image.OriginalSourcePath
        newImage.WorkingSourcePath = Image.WorkingSourcePath
        newImage.PhysicalApplicationPath = Image.PhysicalApplicationPath
        newImage.AdID = Image.AdID
        newImage.Type = Image.Type
        newImage.ProdnStatus = ATLib.Image.ProdnState.Initial
        newImage.PreviewSequence = 0
        newImage.PixelHeight = Image.PixelHeight
        newImage.PixelWidth = Image.PixelWidth
        newImage.IsMainImage = False
        newImage.Resolution = Image.Resolution
        newImage.Update()            'generates image.id
        '
        ' copy image components
        '
        IO.File.Copy(Image.OriginalSourceFileName, newImage.OriginalSourceFileName, True)
        IO.File.Copy(Image.WorkingSourceFileName, newImage.WorkingSourceFileName, True)
        IO.File.Copy(Image.THBFilename, newImage.THBFilename, True)
        IO.File.Copy(Image.LoResFilename, newImage.LoResFilename, True)

        RaiseEvent ImageCopy(Me)            'tell the caller a new image is there

    End Sub

    Private Sub generateSubsamples()
        '
        ' get image size from bitmap if its a regular image
        '

        Select Case _image.Type
            Case ATLib.Image.ImageTypes.PDF
                _image.PixelHeight = 0
                _image.PixelWidth = 0
                _image.Resolution = 1

            Case ATLib.Image.ImageTypes.EPS
                _image.PixelHeight = 0
                _image.PixelWidth = 0
                _image.Resolution = 1

            Case Else
                '
                ' may not work if invalid image format
                '
                Try
                    Using bitmap As New System.Drawing.Bitmap(_image.WorkingSourceFileName)
                        Dim size As System.Drawing.SizeF = bitmap.PhysicalDimension
                        _image.PixelWidth = Convert.ToInt32(size.Width)
                        _image.PixelHeight = Convert.ToInt32(size.Height)
                        _image.Resolution = Convert.ToInt32(bitmap.HorizontalResolution)
                    End Using
                Catch ex As Exception
                    _image.PixelWidth = 0
                    _image.PixelHeight = 0
                    _image.Resolution = 1
                End Try


        End Select
        '
        ' generate thb and lores subsamples
        '
        _image.PreviewSequence += 1          'bump preview sequence
        _image.GenerateSubsample(Image.THBFilename, _thbHeight)
        _image.GenerateSubsample(Image.LoResFilename, _loresHeight)
        _image.Update()

    End Sub






    Private Sub btnCheckout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckout.Click
        If _image.IsCheckedOut Then
            checkin()
            RaiseEvent CheckInOut(Me, CheckDirn.Checkin)        'ripple to caller            checkout()
        Else
            checkout()
            RaiseEvent CheckInOut(Me, CheckDirn.Checkout)        'ripple to caller
        End If
    End Sub

    Private Sub PicBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PicBox.MouseDown
        Dim files() As String = {_image.WorkingSourceFileName}
        Dim data As DataObject = New DataObject(DataFormats.FileDrop, files)
        picBox.DoDragDrop(data, DragDropEffects.Copy)
    End Sub


    Private Sub btnRecover_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecover.Click
        recoverOriginal()
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        copyImage()
    End Sub
End Class
