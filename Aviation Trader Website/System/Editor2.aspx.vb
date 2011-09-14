Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Edit System Parameters 1
'*
'* ON ENTRY:
'*
'*  Loader: objectID = undefined
'*
'***************************************************************************************

Partial Class Editor2
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
        topnode = New MenuNode("B", "Email", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor3
        topnode = New MenuNode("B", "Twitter", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor4
        topnode = New MenuNode("D", "Production", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

    End Sub

    Private Sub displaySystem()



        SMTPServerBox.Text = sys.SMTPHost
        SMTPPortBox.Text = sys.SMTPPort.ToString
        SMTPPasswordBox.Text = sys.SMTPPassword
        SMTPUserBox.Text = sys.SMTPUser
        BCCEmailAddrBox.Text = sys.BCCEmailAddr
        TestEmailAddrBox.Text = sys.TestEmailAddr
        EditionCloseDaysBox.Text = sys.EditionCloseDays.ToString
        TestEmailCheck.Checked = sys.IsEmailTestMode
    End Sub

    Private Sub updatesystem()

        sys.IsEmailTestMode = TestEmailCheck.Checked

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        sys.SMTPHost = IV.ValidateText(SMTPServerBox, SMTPServerError)
        IV.MinStringLength = 0         'null string ok

        sys.SMTPUser = IV.ValidateText(SMTPUserBox, SMTPUserError)
        sys.SMTPPassword = IV.ValidateText(SMTPPasswordBox, SMTPPasswordError)
        sys.BCCEmailAddr = IV.ValidateEmail(BCCEmailAddrBox, BCCEmailAddrError)
        sys.TestEmailAddr = IV.ValidateEmail(TestEmailAddrBox, TestEmailAddrError)

        IV.MinValue = 1
        IV.MaxValue = 20
        sys.EditionCloseDays = IV.ValidateInteger(EditionCloseDaysBox, EditionCLoseDaysError)

        IV.MinValue = 10
        IV.MaxValue = 65000
        sys.SMTPPort = IV.ValidateInteger(SMTPPortBox, SMTPPortError)

        If IV.ErrorCount = 0 Then
            sys.Update()
            ButtonBar.Msg = Constants.Saved
        End If

    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : updatesystem()
        End Select
    End Sub

End Class
