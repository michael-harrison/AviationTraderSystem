Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Edit System Parameters 4
'*
'* ON ENTRY:
'*
'*  Loader: objectID = 
'*
'***************************************************************************************

Partial Class Editor4
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.skin

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        displaytabBar()
        displayLeftMenu()
        displayButtonBar()

        If Not IsPostBack Then
            displaySystem()
        End If

    End Sub


    Private Sub displayButtonBar()
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0
      
        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        leftmenu.Add("System Parameters", Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.Technotes
        leftmenu.Add("Technotes", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.FolderListing
        Loader.ObjectID = sys.FirstFolderID
        leftmenu.Add("Folders", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.PublicationListing
        leftmenu.Add("Publications", Loader.Target, False)

        Loader.NextASPX = ATLib.loader.aspx.categorylisting
        leftmenu.Add("Categories", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.RotatorListing
        leftmenu.Add("Rotator Ads", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.NewsListing
        leftmenu.Add("News", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.UserListing
        leftmenu.Add("Users", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        leftmenu.Add("Proof Reader", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.UserImpersonate
        leftmenu.Add("Impersonate User...", Loader.Target, False)

    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        topnode = New MenuNode("A", "Website", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor2
        topnode = New MenuNode("B", "Email", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor3
        topnode = New MenuNode("B", "Twitter", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor4
        topnode = New MenuNode("D", "Production", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

    End Sub


    Private Sub displaySystem()

        SourceImageOriginalFolderBox.Text = sys.SourceImageOriginalFolder
        SourceImageWorkingFolderBox.Text = sys.SourceImageWorkingFolder
        DisplayAdFolderBox.Text = sys.DisplayAdFolder
        ClassadfolderBox.Text = sys.ClassAdFolder
        ProdnPDFFolderBox.Text = sys.ProdnPDFFolder
        RatesBox.Text = sys.RateSpreadsheet
        DisplayBox.Text = sys.DisplaySheet
        ClassifiedBox.Text = sys.ClassifiedSheet
        engineNameLabel.Text = sys.EngineName
        EngineAddressLabel.Text = CommonRoutines.IPInt2String(sys.EngineAddress)
        EnginePortBox.Text = sys.EnginePort.ToString
        JobTimeoutBox.Text = sys.JobTimeout.ToString
        ClassadTemplateBox.Text = sys.ClassadTemplate

        LatestListingLoadingBox.Text = CommonRoutines.Integer2Dollars(sys.LatestListingLoading)
        LatestListingKillTimeBox.Text = sys.LatestListingKillTime.ToShortDateString

     
        ClassadLineHeightBox.Text = CommonRoutines.Integer2mm(sys.ClassadLineHeight, 3)
        ClassadPicHeightBox.Text = CommonRoutines.Integer2mm(sys.ClassadPicHeight, 3)

        DisplayColumnHeightBox.Text = CommonRoutines.Integer2mm(sys.DisplayColumnHeight, 3)
        DisplayColumnWidthBox.Text = CommonRoutines.Integer2mm(sys.DisplayColumnWidth, 3)
        DisplayGutterWidthBox.Text = CommonRoutines.Integer2mm(sys.DisplayGutterWidth, 3)
        DisplayColumnCountBox.Text = sys.DisplayColumnCount.ToString



    End Sub

    Private Sub updatesystem()
        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength
       
        sys.ClassadTemplate = IV.ValidatePath(ClassadTemplateBox, ClassadTemplateError)
        sys.ClassadLineHeight = IV.Validatemm(ClassadLineHeightBox, ClassadLineHeighterror)
        sys.ClassadPicHeight = IV.Validatemm(ClassadPicHeightBox, ClassadPicHeightError)

        sys.DisplayColumnHeight = IV.Validatemm(DisplayColumnHeightBox, displaycolumnheighterror)
        sys.DisplayColumnWidth = IV.Validatemm(DisplayColumnWidthBox, displaycolumnwidtherror)
        sys.DisplayGutterWidth = IV.Validatemm(DisplayGutterWidthBox, DisplayGutterWidthError)

        sys.LatestListingLoading = IV.ValidateDollars(LatestListingLoadingBox, LatestListingLoadingError)
        sys.LatestListingKillTime = IV.validateDateTime(LatestListingKillTimeBox, LatestListingKillTimeError)

        IV.MinValue = 1
        IV.MaxValue = 7
        sys.DisplayColumnCount = IV.ValidateInteger(DisplayColumnCountBox, DisplayColumnCountError)

        sys.ProdnPDFFolder = IV.ValidatePath(ProdnPDFFolderBox, ProdnPDFFolderError)
        sys.SourceImageOriginalFolder = IV.ValidatePath(SourceImageOriginalFolderBox, SourceImageOriginalFolderError)
        sys.SourceImageWorkingFolder = IV.ValidatePath(SourceImageWorkingFolderBox, SourceImageWorkingFolderError)
        sys.DisplayAdFolder = IV.ValidatePath(DisplayAdFolderBox, DisplayAdFolderError)
        sys.ClassAdFolder = IV.ValidatePath(ClassadfolderBox, ClassadFolderError)
        sys.RateSpreadsheet = IV.ValidatePath(RatesBox, RatesError)
        sys.DisplaySheet = IV.ValidatePath(DisplayBox, DisplayError)
        sys.ClassifiedSheet = IV.ValidatePath(ClassifiedBox, ClassifiedError)
        IV.MinValue = 100
        IV.MaxValue = 100000
        sys.EnginePort = IV.ValidateInteger(EnginePortBox, EnginePortError)
        IV.MinValue = 10
        IV.MaxValue = 10000
        sys.JobTimeout = IV.ValidateInteger(JobTimeoutBox, JobTimeoutError)

        If IV.ErrorCount = 0 Then
            sys.Update()
            ButtonBar.Msg = Constants.Saved
        End If

    End Sub

    Private Sub importDisplayRates()
        Try

        
            Dim rt As New RateTable
            rt.ImportDisplayRates(sys.RateSpreadsheet, sys.DisplaySheet)
            rt.ExportDisplayRates(Constants.DisplayRates)
            ButtonBar.Msg = "Display Rates successfully imported"
        Catch ex As Exception
            ButtonBar.Msg = "Failed to import because: " & ex.Message
        End Try

    End Sub

    Private Sub importClassifiedRates()
        Try
            Dim rt As New RateTable
            rt.ImportClassadRates(sys.RateSpreadsheet, sys.ClassifiedSheet)
            rt.ExportClassadRates(Constants.ClassadRates)
            ButtonBar.Msg = "Classad Rates successfully imported"
        Catch ex As Exception
            ButtonBar.Msg = "Failed to import because: " & ex.Message
        End Try
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : updatesystem()
        End Select
    End Sub

    Protected Sub BtnImportDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImportDisplay.Click
        importDisplayRates()
    End Sub

    Protected Sub BtnImportClassified_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImportclassified.Click
        importClassifiedRates()
    End Sub
End Class
