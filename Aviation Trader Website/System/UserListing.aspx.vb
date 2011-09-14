Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* User Listing
'*
'* ON ENTRY:
'*
'*  Loader: objectID = 
'*          SelectedTab=selected tab
'*
'***************************************************************************************

Partial Class UserListing
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Protected AdCountTotal As Integer


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

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

 
        displaytabBar()
        displayButtonBar()
        displayNavbar()
        displayLeftMenu()

        If Not IsPostBack Then
            Dim selector As Usr.Selectors = mapTab2Selector(Loader.SelectedTab)
            displayUsrs(selector)
        End If

    

    End Sub

    Private Sub displaytabBar()
        Dim selectedTab As Integer = Loader.SelectedTab     'save selected tab

        Dim topnode As MenuNode
        Loader.NextASPX = ATLib.Loader.ASPX.UserListing

        Loader.SelectedTab = 0
        topnode = New MenuNode("A", "New Users", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 1
        topnode = New MenuNode("B", "A-D", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 2
        topnode = New MenuNode("C", "E-H", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 3
        topnode = New MenuNode("D", "I-L", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 4
        topnode = New MenuNode("D", "M-P", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 5
        topnode = New MenuNode("D", "Q-T", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 6
        topnode = New MenuNode("D", "U-Z", Loader.Target, False)
        tabbar.Nodes.Add(topnode)
        '
        ' put the selected tab down
        '
        tabbar.Nodes(selectedTab).Selected = True
        Loader.SelectedTab = selectedTab            'restore selected tab
    End Sub

    Private Function mapTab2Selector(ByVal selectedTab As Integer) As Usr.Selectors

        Select Case selectedTab
            Case 0 : Return Usr.Selectors.NewUsr
            Case 1 : Return Usr.Selectors.AD
            Case 2 : Return Usr.Selectors.EH
            Case 3 : Return Usr.Selectors.IL
            Case 4 : Return Usr.Selectors.MP
            Case 5 : Return Usr.Selectors.QT
            Case 6 : Return Usr.Selectors.UZ
        End Select

    End Function

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.System
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Dim selectedTab As Integer = Loader.SelectedTab       'save selected tab
        Loader.SelectedTab = 0

        Select Case Slot.LoginLevel

            Case Usr.LoginLevels.SysAdmin

                Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
                leftmenu.Add("System Parameters", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.Technotes
                leftmenu.Add("Technotes", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.FolderListing
                Loader.ObjectID = sys.FirstFolderID
                leftmenu.Add("Folders", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.PublicationListing
                leftmenu.Add("Publications", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.CategoryListing
                leftmenu.Add("Categories", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.RotatorListing
                leftmenu.Add("Rotator Ads", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.NewsListing
                leftmenu.Add("News", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserListing
                leftmenu.Add("Users", Loader.Target, True)

                Loader.NextASPX = ATLib.Loader.ASPX.ProofList
                leftmenu.Add("Proof Reader", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserImpersonate
                leftmenu.Add("Impersonate User...", Loader.Target, False)

            Case Usr.LoginLevels.Advertiser, Usr.LoginLevels.Subscriber

                Loader.NextASPX = ATLib.Loader.ASPX.UserImpersonate
                leftmenu.Add("Impersonate User...", Loader.Target, False)
        End Select

        Loader.SelectedTab = selectedTab          'restore selected tab

    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "ADD User"
    End Sub



    Private Sub displayUsrs(ByVal Selector As Usr.Selectors)
        '
        ' setup user type drop down 
        '
        Dim EA As EnumAssistant
        EA = New EnumAssistant(New Usr.LoginLevels, Usr.LoginLevels.Guest, Usr.LoginLevels.Unspecified)
        UserTypeDD.DataSource = EA
        UserTypeDD.DataBind()
        UserTypeDD.SelectedValue = Convert.ToString(Slot.UserTypeFilter)
        '
        ' get the users filtered by alpa category and user type. 
        '
        Dim Usrs As New Usrs
        Usrs.RetrieveFilteredSet(sys.ID, Selector, Slot.UserTypeFilter)
        '
        ' spin around users and set the target url
        '
        AdCountTotal = 0
        Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
        For Each Usr As Usr In Usrs
            Loader.ObjectID = Usr.ID
            Usr.NavTarget = Loader.Target
            AdCountTotal += Usr.AdCount
        Next

        UserList.DataSource = Usrs
        UserList.DataBind()


    End Sub

    Private Sub addUsr()
        '
        ' add a new Publication
        '
        Dim usr As New Usr
        usr.SystemID = sys.ID

        usr.EmailAddr = ""
        usr.Password = ""
        usr.FName = "New"
        usr.LName = "User"

        Dim EA As New EnumAssistant(New ATSystem.Skins, ATSystem.Skins.ATSkin, ATSystem.Skins.ATSkin)
        usr.Skin = EA(0).Description
      
        usr.Addr1 = ""
        usr.Addr2 = ""
        usr.Suburb = ""
        usr.Postcode = ""
        usr.State = ""

        usr.Countrycode = ATSystem.countrycodes.AU.ToString
        usr.LoginLevel = ATLib.Usr.LoginLevels.Advertiser
        usr.EditionVisibility = Edition.VisibleState.Active
     
        usr.Phone = ""
        usr.AHPhone = ""
        usr.Fax = ""
        usr.Company = ""
        usr.AcctAlias = ""
        usr.ACN = ""
        usr.Mobile = ""
        usr.WebSite = ""

        usr.UAM = 0
        usr.IdentVisible = 0      'nothing visible to end users yet
        
        Loader.ObjectID = usr.Update()        'create new usr

        Loader.NextASPX = ATLib.Loader.ASPX.UserEditor2
        Response.Redirect(Loader.Target)

    End Sub

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addUsr()
        End Select
    End Sub


    Protected Sub UserTypeDD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserTypeDD.SelectedIndexChanged
        '
        ' update slot with new value and re-display
        '
        Slot.UserTypeFilter = CType(UserTypeDD.SelectedValue, Usr.LoginLevels)
        Slot.Update()
        Dim selector As Usr.Selectors = mapTab2Selector(Loader.SelectedTab)
        displayUsrs(selector)
    End Sub
End Class
