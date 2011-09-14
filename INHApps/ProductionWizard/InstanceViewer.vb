Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Instance Viewer
'*
'* AUDIT TRAIL
'* 
'* V1.000   01-Dec-2009  BA  Original
'*
'* This is a viewer for an ad instance
'*
'*
'*
'*
'***************************************************************************************


Public Class InstanceViewer

    Private AdInstance As AdInstance
    Private _instanceID As Integer
    Private _physicalApplicationPath As String
    Private _prodnPDFFolder As String
    Private _thbHeight As Integer
    Private _loresHeight As Integer
    Private _INDService As EngineLib.INDService
    Private _textDragMode As Boolean = False
    Private sys As ATSystem

    Private dragText As String = "Drag Mode"
    Private selectText As String = "Select Mode"


    Public Sub New()
        AdInstance = New AdInstance
        InitializeComponent()
    End Sub

    Public Property INDService() As EngineLib.INDService
        Get
            Return _INDService
        End Get
        Set(ByVal value As EngineLib.INDService)
            _INDService = value
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

    Public Property LoresHeight() As Integer
        Get
            Return _loresHeight
        End Get
        Set(ByVal value As Integer)
            _loresHeight = value
        End Set
    End Property

    Public Property InstanceID() As Integer
        Get
            Return _instanceID
        End Get
        Set(ByVal value As Integer)
            _instanceID = value
        End Set
    End Property

    Public Property PhysicalApplicationPath() As String
        Get
            Return _physicalApplicationPath
        End Get
        Set(ByVal value As String)
            _physicalApplicationPath = value
        End Set
    End Property

    Public Property ProdnPDFFolder() As String
        Get
            Return _prodnPDFFolder
        End Get
        Set(ByVal value As String)
            _prodnPDFFolder = value
        End Set
    End Property


    Public Sub Render()
        '
        ' get a new instance each time
        '
        sys = New ATSystem
        sys.Retrieve()

        Dim myInstances As New AdInstances
        AdInstance = myInstances.Retrieve(_instanceID)
        AdInstance.PhysicalApplicationPath = _physicalApplicationPath
        AdInstance.ProdnPDFFolder = _prodnPDFFolder

        Dim myAd As Ad = AdInstance.Ad
        '
        ' selectively display classad or display tab depending on instance type
        '
        Me.TabControl1.TabPages.Clear()
        Select Case AdInstance.ProductType
            Case Product.Types.ClassadColorPic, Product.Types.ClassadMonoPic, Product.Types.ClassadTextOnly
                Me.TabControl1.TabPages.Add(ClassadPage)
                classadSize()

            Case Product.Types.DisplayComposite, Product.Types.DisplayFinishedArt, Product.Types.DisplayModule, Product.Types.DisplayStandAlone
                Me.TabControl1.TabPages.Add(DisplayPage)
                displaySize()

        End Select

        Me.TabControl1.TabPages.Add(TextPage)
        Me.TabControl1.TabPages.Add(PixPage)
        Me.TabControl1.TabPages.Add(ProdnPage)
        Me.TabControl1.TabPages.Add(billPage)
        '
        ' ad panel
        '
        btnCheckout.Enabled = False
        If IO.File.Exists(AdInstance.INDDFilename) Then
            btnCheckout.Enabled = True
        Else
            btnCheckout.Enabled = False
            ErrorBox.Text = "Warning - the indesign file cannot be found - please select it."
        End If
        btnINDDFile.Enabled = True

        btnCheckout.Text = Form1.CheckoutText
        btnCheckout.BackColor = Color.Beige
        btnForceCheckin.Enabled = False
        If AdInstance.IsCheckedOut Then
            btnCheckout.Text = Form1.CheckinText
            btnCheckout.BackColor = Color.Coral
            btnForceCheckin.Enabled = True
        End If

        AdNumberBox.Text = AdInstance.AdNumber
        Dim ea As New EnumAssistant(New Product.Types, AdInstance.ProductType, AdInstance.ProductType)
        Dim productDescr As String = ea(0).Description
        ProductBox.Text = productDescr
        ClassadProductBox.Text = productDescr

        ClassadNumberBox.Text = AdInstance.AdNumber
        UsrBox.Text = myAd.Usr.AcctAlias
         ClassadUsrBox.Text = myAd.Usr.AcctAlias
        INDDBox.Text = AdInstance.INDDShortFilename
        StatusBox.Text = myAd.ProdnStatus.ToString
        ClassadStatusBox.Text = myAd.ProdnStatus.ToString
        CategoryBox.Text = myAd.Classification.Category.Name & " - " & myAd.Classification.Name
        ClassadCategoryBox.Text = myAd.Classification.Category.Name & " - " & myAd.Classification.Name
        '
        ' current folder dd
        '
        SendDD.DisplayMember = "Name"
        SendDD.ValueMember = "HexID"
        SendDD.DataSource = sys.ProdnFolders
        SendDD.SelectedValue = Int2Hex(myAd.FolderID)
        '
        ' Note - cant copy image file since it leaves file open. Copy to memory stream first
        '
        Try

            Dim previewFilename As String = AdInstance.PreviewImageFilename
            If Not AdInstance.IsPreviewValid Then
                previewFilename = IO.Path.Combine(AdInstance.PhysicalApplicationPath, Constants.InvalidPreviewImage)
            End If
            Using ms As New IO.MemoryStream
                Using fs As New IO.FileStream(previewFilename, IO.FileMode.Open)
                    ms.SetLength(fs.Length)
                    fs.Read(ms.GetBuffer(), 0, Convert.ToInt32(fs.Length))
                    ms.Flush()
                    fs.Close()
                End Using
                InstancePic.BackgroundImage = System.Drawing.Image.FromStream(ms)
                InstancePic.BackgroundImageLayout = ImageLayout.Zoom
                ClassadInstancePic.BackgroundImage = System.Drawing.Image.FromStream(ms)
                ClassadInstancePic.BackgroundImageLayout = ImageLayout.Zoom

            End Using

        Catch ex As Exception
        End Try
        '
        ' text panel
        '
        btnDragSelect.Text = selectText
        If _textDragMode Then btnDragSelect.Text = dragText
        AdTextBox.Text = myAd.Text
        KeywordsBox.Text = myAd.KeyWords
        ItemPrice.Text = myAd.ItemPrice
        SummaryBox.Text = myAd.Summary
        '
        ' image panel
        '
        Dim pad As Integer = 10
        Dim curX As Integer = pad
        Dim curY As Integer = pad

        AdInstance.Ad.ClearImages()            'force image reload
        PixPanel.Controls.Clear()                 'and clear all controls

        For Each Image As Image In AdInstance.Ad.Images
            Image.PhysicalApplicationPath = AdInstance.PhysicalApplicationPath
            Dim picitem As New PicItem(Image)
            picitem.LoresHeight = _loresHeight
            picitem.THBHeight = _thbHeight
            picitem.Left = 0
            picitem.Top = curY
            AddHandler picitem.CheckInOut, AddressOf PicCheckInOut
            AddHandler picitem.ImageCopy, AddressOf ImageCopy
            PixPanel.Controls.Add(picitem)
            curY += picitem.Height + pad
            picitem.Render()
        Next
        '
        ' prodn panel
        '
        AdvertiserBox.Text = AdInstance.Ad.ProdnRequest
        ProdnBox.Text = AdInstance.Ad.ProdnResponse
        
    End Sub

    Private Sub displaySize()
        '
        ' set the width and height dd
        '
        exactSizeBox.Text = "Size: " & CommonRoutines.Integer2mm(AdInstance.ExactHeight, 3) & " mm x " & CommonRoutines.Integer2mm(AdInstance.ExactWidth, 3) & " mm"

        Dim EA As EnumAssistant
        EA = New EnumAssistant(New RateTable.DisplayWidths)
        widthDD.DisplayMember = "Description"
        widthDD.ValueMember = "value"
        widthDD.DataSource = EA
        widthDD.SelectedValue = Convert.ToInt32(AdInstance.Width)

        EA = New EnumAssistant(New RateTable.DisplayHeights)
        heightDD.DisplayMember = "Description"
        heightDD.ValueMember = "value"

        heightDD.DataSource = EA
        heightDD.SelectedValue = AdInstance.Height
    End Sub

    Private Sub classadSize()
        '
        ' show the classad lines
        '
        Dim EA As EnumAssistant
        EA = New EnumAssistant(New RateTable.ClassadHeights)
        LinesDD.DisplayMember = "Description"
        LinesDD.ValueMember = "value"
        LinesDD.DataSource = EA
        LinesDD.SelectedValue = AdInstance.Height
    End Sub


    Private Sub checkout()
        ErrorBox.Text = ""

        Try
            '
            ' on the first checkout, load indesign
            '
            INDService.InitializeINDD()
            INDService.OpenDoc(AdInstance.INDDFilename)
            btnCheckout.Text = Form1.CheckinText
            btnCheckout.BackColor = Color.Coral
            AdInstance.IsPreviewValid = False          'show invalid preview
            AdInstance.Update()
            AdInstance.IsCheckedOut = True
            AdInstance.Update()
        Catch ex As Exception
            ErrorBox.Text = "failed to checkout because: " & ex.Message

        End Try

    End Sub

    Private Sub checkin()
        INDService.InitializeINDD()
        INDService.OpenDoc(AdInstance.INDDFilename)

        AdInstance.PreviewSequence += 1
        _INDService.ExportLoRes(AdInstance.PreviewImageFilename, InstancePic.Width - 10)
        ''    _INDService.AddWaterMark()
        _INDService.ExportProofPDF(AdInstance.PreviewPDFFilename)
        ''     _INDService.RemoveWaterMark()
        _INDService.ExportProdnPDF(AdInstance.ProdnPDFFilename)
        _INDService.SaveDoc()
        '
        ' calc price
        '
        Dim rt As New RateTable
        Dim myInstances As New AdInstances
        rt.LoadDisplayTable(Constants.DisplayRates)
        AdInstance.Price = rt.GetDisplayRate(AdInstance)
        AdInstance.IsPreviewValid = True          'show valid preview
        updateWebPDFPreviews()                        'update all web previews from this one
        AdInstance.IsCheckedOut = False
        AdInstance.Update()

        btnCheckout.BackColor = Color.Beige
        btnCheckout.Text = Form1.CheckoutText
        Render()         'update display
    End Sub

    Private Sub forceCheckin()
        '
        'emergency checkin- updates ad instance.
        '
        AdInstance.IsCheckedOut = False
        AdInstance.Update()
        btnCheckout.BackColor = Color.Beige
        btnCheckout.Text = Form1.CheckoutText
        Render()         'update display
    End Sub

    Private Sub updateWebPDFPreviews()
        Dim sourceFilename As String = AdInstance.PreviewPDFFilename
        Dim parentAd As Ad = AdInstance.Ad
        For Each myInstance As AdInstance In parentAd.Instances
            myInstance.PhysicalApplicationPath = AdInstance.PhysicalApplicationPath
            Select Case myInstance.ProductType
                Case Product.Types.WebBasic, Product.Types.WebFeaturedAd, Product.Types.WebPDF, Product.Types.WebPDFText, Product.Types.WebPremium
                    myInstance.PreviewSequence += 1
                    Dim dstnFilename As String = myInstance.PreviewPDFFilename
                    IO.File.Copy(sourceFilename, dstnFilename, True)
                    myInstance.IsPreviewValid = True
                    myInstance.Update()

                Case Else
                    '
                    ' this is a print ad
                    '
            End Select
        Next

    End Sub

    Private Sub manualsizeDoc()
        '
        ' manual size from combox 
        '
        AdInstance.Width = CType(widthDD.SelectedValue, RateTable.DisplayWidths)
        AdInstance.Height = CType(heightDD.SelectedValue, RateTable.DisplayHeights)
        AdInstance.Update()

    End Sub


    Private Sub autosizeDoc()
        '
        ' call INDD to get doc size
        ' file must be checked to autosize
        '
        ErrorBox.Text = ""
        If Not AdInstance.IsCheckedOut Then
            ErrorBox.Text = "You must check out file first to autosize"
        Else
            Dim mysys As New ATSystem
            mysys.Retrieve()

            Dim adsize() As Double = INDService.getDocSize
            AdInstance.ExactWidth = Convert.ToInt32(adsize(0) * 1000)
            AdInstance.ExactHeight = Convert.ToInt32(adsize(1) * 1000)
            Try
                AdInstance.CalculateDisplaySize()
            Catch ex As Exception
                ErrorBox.Text = ex.Message
            End Try

            AdInstance.Update()
            displaySize()

        End If

    End Sub

    Private Sub updateText()
        '
        ' chainging the text for an instance invalidates all instances
        '
        Dim myAd As Ad = AdInstance.Ad

        '
        ' Only if the keywords fields or summary is empty, split the ad text on the first and second separator
        ' and plug to keyword and/or summary fields
        '
        Dim sep() As Char = {Constants.TextSeparator}
        Dim arr() As String = AdTextBox.Text.Split(sep, 3)

        Select Case arr.Length
            Case 1
                If SummaryBox.Text.Length = 0 Then SummaryBox.Text = arr(0)

            Case Else
                If KeywordsBox.Text.Length = 0 Then KeywordsBox.Text = arr(0)
                If SummaryBox.Text.Length = 0 Then SummaryBox.Text = arr(1)

        End Select

        myAd.Text = AdTextBox.Text
        myAd.KeyWords = KeywordsBox.Text
        myAd.Summary = SummaryBox.Text
        myAd.GenerateSortKey()
        myAd.ItemPrice = ItemPrice.Text
        myAd.InvalidateInstancePreviews()
        '
        ' remap ad status if necessary
        '
        Select Case myAd.ProdnStatus
            Case ATLib.Ad.ProdnState.Approved : myAd.ProdnStatus = ATLib.Ad.ProdnState.Submitted
            Case ATLib.Ad.ProdnState.Proofed : myAd.ProdnStatus = ATLib.Ad.ProdnState.Submitted
        End Select
        myAd.Update()

    End Sub

    Private Sub buildPreview()
        '
        ' rebuilds the preview for the instance
        '
        btnRebuild.BackColor = Color.Coral
        btnRebuild.Enabled = False


        Try
            Dim sys As New ATSystem
            sys.Retrieve()
            Dim Q As New EQItem
            Q.ObjectID = AdInstance.ID
            Q.Command = EQItem.CommandBits.Classad Or EQItem.CommandBits.SuspendUntilComplete
            Dim Engine As Engine = sys.MapEngine(ATSystem.EngineModes.Client)
            Q = Engine.Enqueue(Q)
        Catch ex As Exception
        End Try
        btnRebuild.BackColor = Color.Beige
        btnRebuild.Enabled = True
        Render()

    End Sub

    Private Sub ImageCopy(ByVal picitem As PicItem)
        '
        ' an image has been copied - refresh display
        '
        Render()
    End Sub


    Private Sub btnINDDFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnINDDFile.Click
        '
        ' assigns or re-assigns indd file to ad instance
        '
        INDDFileDialog.Filter = "Indesign files|*.INDD"

        If INDDFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            '
            ' copy selected template into working file with specified name
            '
            Dim srcFilename As String = INDDFileDialog.FileName
             Dim sys As New ATSystem
            sys.Retrieve()
            AdInstance.generateINDDName(sys.DisplayAdFolder)
            IO.File.Copy(srcFilename, AdInstance.INDDFilename, True)
            AdInstance.Update()
            INDDBox.Text = AdInstance.INDDShortFilename
            ErrorBox.Text = ""
            btnCheckout.Enabled = True        'enable button
        End If

    End Sub

  


    Private Sub btnCheckout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckout.Click
        If Not AdInstance.IsCheckedOut Then
            checkout()
        Else
            checkin()
        End If
    End Sub

    Private Sub AdTextBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AdTextBox.MouseDown
        If _textDragMode Then AdTextBox.DoDragDrop(AdTextBox.Text, DragDropEffects.Copy)
    End Sub

    Private Sub AdNumberBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AdNumberBox.MouseDown
        AdNumberBox.DoDragDrop(AdNumberBox.Text, DragDropEffects.Copy)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        updateText()
    End Sub

    Private Sub btnDragSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDragSelect.Click
        _textDragMode = Not _textDragMode
        btnDragSelect.Text = selectText
        If _textDragMode Then btnDragSelect.Text = dragText
    End Sub

    Private Sub PicCheckInOut(ByVal picitem As PicItem, ByVal dirn As PicItem.CheckDirn)
        '
        ' if checking out or in, show ad preview as now invalid
        '
        AdInstance.IsPreviewValid = False
        AdInstance.Update()
    End Sub

    Private Sub InstancePic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles InstancePic.MouseDown
        Dim files() As String = {AdInstance.ProdnPDFFilename}
        Dim data As DataObject = New DataObject(DataFormats.FileDrop, files)
        InstancePic.DoDragDrop(data, DragDropEffects.Copy)
    End Sub


    Private Sub btnProdnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProdnUpdate.Click
        AdInstance.Ad.ProdnResponse = ProdnBox.Text
        AdInstance.Ad.FolderID = Hex2Int(SendDD.SelectedValue.ToString)
        AdInstance.Ad.Update()
    End Sub

    Private Sub btnAutosize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutosize.Click
        autosizeDoc()
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        manualsizeDoc()
    End Sub


    Private Sub btnForceCheckin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForceCheckin.Click
        forceCheckin()
    End Sub

    Private Sub btnRebuild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRebuild.Click
        buildPreview()
    End Sub

   
    Private Sub DisplayPage_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles DisplayPage.Enter
        Render()
    End Sub

    Private Sub ClassadPage_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClassadPage.Enter
        Render()
    End Sub

    Private Sub BillPage_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles BillPage.Enter
        radioOn.Checked = AdInstance.Ad.BillMe
        radioOff.Checked = Not radioOn.Checked
    End Sub




    Private Sub radioOn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioOn.CheckedChanged
        AdInstance.Ad.BillMe = radioOn.Checked
        AdInstance.Ad.Update()
    End Sub

 
    ''Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged

    ''    '' use this to detect tab click changed
    ''    Dim i As Integer = TabControl1.SelectedIndex
    ''End Sub

 
End Class
