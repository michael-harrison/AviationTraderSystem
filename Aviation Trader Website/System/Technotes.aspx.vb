Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Technotes
'*
'* ON ENTRY:
'*
'*  Loader: objectID = SystemID
'*          selectedTab = tab number        
'*
'***************************************************************************************

Partial Class Technotes
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
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim status As Technote.State = mapTab2Status(Loader.SelectedTab)
        displayNavbar()
        displaytabBar()
        displayLeftMenu()
        displayButtonBar()
        displayNotes(status)

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Dim selectedTab As Integer = Loader.SelectedTab       'save selected tab
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        leftmenu.Add("System Parameters", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.Technotes
        leftmenu.Add("Technotes", Loader.Target, True)

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

        Loader.SelectedTab = selectedTab          'restore selected tab

    End Sub

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.System
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displaytabBar()
        '
        ' save current tab number
        '
        Dim selectedTab As Integer = Loader.SelectedTab

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.Technotes
        Loader.SelectedTab = 0
        topnode = New MenuNode("A", "Open", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 1
        topnode = New MenuNode("B", "Closed", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 2
        topnode = New MenuNode("C", "Discuss", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 3
        topnode = New MenuNode("D", "New Feature", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 4
        topnode = New MenuNode("E", "Warranty", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 5
        topnode = New MenuNode("F", "Wishlist", Loader.Target, False)
        tabbar.Nodes.Add(topnode)
        '
        ' put the selected tab down
        '
        tabbar.Nodes(selectedTab).Selected = True
        Loader.SelectedTab = selectedTab
    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "Create Note"
    End Sub

    Private Function mapTab2Status(ByVal selectedTab As Integer) As Technote.State

        Select Case selectedTab
            Case 0 : Return Technote.State.Open
            Case 1 : Return Technote.State.Closed
            Case 2 : Return Technote.State.Discuss
            Case 3 : Return Technote.State.NewFeature
            Case 4 : Return Technote.State.Warranty
            Case 5 : Return Technote.State.WishList
        End Select

    End Function


    Private Sub displayNotes(ByVal Status As Technote.State)
        '
        ' sey navtargets
        '
        Dim myNotes As ATLib.Technotes = sys.Technotes(Status)
        Loader.NextASPX = ATLib.Loader.ASPX.TechnoteEditor
        For Each Technote As Technote In myNotes
            Loader.ObjectID = Technote.ID
            Technote.NavTarget = Loader.Target
        Next



        TechnoteList.DataSource = sys.Technotes(Status)
        TechnoteList.DataBind()

    End Sub

    Private Sub addNote()
        '
        ' add a new rotator
        '
        Dim technote As New Technote
        technote.systemID = sys.ID
        technote.Name = "New Technote"
        technote.ReportedBy = getReporterFromLogin
        technote.FixedBy = technote.ReportedBy
        technote.Resolution = ATLib.Technote.Resolutions.Active
        technote.ProblemDescription = ""
        technote.ProblemFix = ""
        technote.Status = ATLib.Technote.State.Open
        technote.Update()

        Loader.ObjectID = technote.ID
        Loader.NextASPX = ATLib.Loader.ASPX.TechnoteEditor
        Response.Redirect(Loader.Target)
    End Sub

    Private Function getReporterFromLogin() As Technote.Reporters
        Select Case Slot.EmailAddr.ToLower
            Case "brian@wavefront.com.au" : Return Technote.Reporters.BA
            Case "admin@aviationtrader.com.au" : Return Technote.Reporters.JD
            Case "kevingosling@aviationtrader.com.au" : Return Technote.Reporters.KG
            Case "tony1@aviationtrader.com.au" : Return Technote.Reporters.TS
            Case Else : Return Technote.Reporters.BA
        End Select
    End Function

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addNote()
        End Select
    End Sub


End Class
