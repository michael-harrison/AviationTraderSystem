Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* News Listing
'*
'* ON ENTRY:
'*
'*  Loader: objectID = SystemID
'*          selectedTab = tab number        
'*
'***************************************************************************************

Partial Class NewsListing
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

        Dim status As NewsItem.ProdnState = mapTab2Status(Loader.SelectedTab)
        displaytabBar()
        displayLeftMenu()
        displayButtonBar()
        displayItems(status)

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
        leftmenu.Add("News", Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.UserListing
        leftmenu.Add("Users", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
         leftmenu.Add("Proof Reader", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.UserImpersonate
        leftmenu.Add("Impersonate User...", Loader.Target, False)

        Loader.SelectedTab = selectedTab          'restore selected tab

    End Sub

    Private Sub displaytabBar()
        '
        ' save current tab number
        '
        Dim selectedTab As Integer = Loader.SelectedTab

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.NewsListing
        Loader.SelectedTab = 0
        topnode = New MenuNode("A", "In prodn", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 1
        topnode = New MenuNode("B", "Latest", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 2
        topnode = New MenuNode("B", "Current", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 3
        topnode = New MenuNode("C", "Archived", Loader.Target, False)
        tabbar.Nodes.Add(topnode)
        '
        ' put the selected tab down
        '
        tabbar.Nodes(selectedTab).Selected = True
        Loader.SelectedTab = selectedTab
    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "Create News Item"
    End Sub

    Private Function mapTab2Status(ByVal selectedTab As Integer) As NewsItem.ProdnState

        Select Case selectedTab
            Case 0 : Return NewsItem.ProdnState.Incomplete
            Case 1 : Return NewsItem.ProdnState.Latest
            Case 2 : Return NewsItem.ProdnState.Current
            Case 3 : Return NewsItem.ProdnState.Archived
        End Select

    End Function


    Private Sub displayItems(ByVal Status As NewsItem.ProdnState)
        '
        ' sey navtargets
        '
        Dim myItems As ATLib.NewsItems = sys.NewsItems(Status)
        Loader.NextASPX = ATLib.Loader.ASPX.NewsEditor
        For Each NewsItem As NewsItem In myItems
            Loader.ObjectID = NewsItem.ID
            NewsItem.NavTarget = Loader.Target
        Next

        NewsItemList.DataSource = myItems
        NewsItemList.DataBind()

    End Sub

    Private Sub addItem()
        '
        ' add a new rotator
        '
        Dim NewsItem As New NewsItem
        NewsItem.systemID = sys.ID
        NewsItem.Name = "New News Item"
        NewsItem.Intro = ""
        NewsItem.Body = ""
        NewsItem.PicCaption = ""
        NewsItem.ProdnStatus = ATLib.NewsItem.ProdnState.Incomplete
        NewsItem.Update()

        Loader.ObjectID = NewsItem.ID
        Loader.NextASPX = ATLib.Loader.ASPX.NewsEditor
        Response.Redirect(Loader.Target)
    End Sub


    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addItem()
        End Select
    End Sub


End Class
