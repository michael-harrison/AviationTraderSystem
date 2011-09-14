Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Rotator Listing
'*
'* ON ENTRY:
'*
'*  Loader: objectID = SystemID
'*          selectedTab = tab number        
'*
'***************************************************************************************

Partial Class RotatorListing
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

        Dim category As RotatorAd.Categories = mapTab2Category(Loader.SelectedTab)
        displaytabBar()
        displayLeftMenu()
        displayButtonBar()
        displayItems(category)

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

        Loader.NextASPX = ATLib.Loader.ASPX.CategoryListing
        leftmenu.Add("Categories", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.RotatorListing
        leftmenu.Add("Rotator Ads", Loader.Target, True)

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

    Private Sub displaytabBar()
        '
        ' save current tab number
        '
        Dim selectedTab As Integer = Loader.SelectedTab

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.RotatorListing
        Loader.SelectedTab = 0
        topnode = New MenuNode("A", "Inactive", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 1
        topnode = New MenuNode("B", "Left", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 2
        topnode = New MenuNode("B", "Right", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 3
        topnode = New MenuNode("C", "Masthead", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 4
        topnode = New MenuNode("C", "Home Left", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 5
        topnode = New MenuNode("C", "Home Right", Loader.Target, False)
        tabbar.Nodes.Add(topnode)
        '
        ' put the selected tab down
        '
        tabbar.Nodes(selectedTab).Selected = True
        Loader.SelectedTab = selectedTab
    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "Create Rotator Ad"
    End Sub

    Private Function mapTab2Category(ByVal selectedTab As Integer) As RotatorAd.Categories

        Select Case selectedTab
            Case 0 : Return RotatorAd.Categories.InActive
            Case 1 : Return RotatorAd.Categories.Left
            Case 2 : Return RotatorAd.Categories.Right
            Case 3 : Return RotatorAd.Categories.MastHead
            Case 4 : Return RotatorAd.Categories.Homeleft
            Case 5 : Return RotatorAd.Categories.HomeRight
        End Select

    End Function


    Private Sub displayItems(ByVal Category As RotatorAd.Categories)
        '
        ' sey navtargets
        '
        Dim myRotators As ATLib.RotatorAds = sys.RotatorAds(Category)
        Loader.NextASPX = ATLib.Loader.ASPX.RotatorEditor
        For Each RotatorAd As RotatorAd In myRotators
            Loader.ObjectID = RotatorAd.ID
            RotatorAd.NavTarget = Loader.Target
        Next

        RotatorList.DataSource = myRotators
        RotatorList.DataBind()

    End Sub

    Private Sub addItem()
        '
        ' add a new rotator
        '
        Dim Rotator As New RotatorAd
        Rotator.SystemID = sys.ID
        Rotator.Name = "New Rotator Ad"
        Rotator.ImageURL = "http://"
        Rotator.ClickURL = ""
        rotator.width = 0
        Rotator.Height = 0
        Rotator.MarginTop = 0
        Rotator.MarginBottom = 0
        Rotator.MarginLeft = 0
        Rotator.MarginRight = 0
        Rotator.ClickCount = 0
        rotator.BGColor = 0
        rotator.UsageCount = 0
        Rotator.Category = RotatorAd.Categories.InActive
        Rotator.Type = RotatorAd.Types.Image
        rotator.Update()

        Loader.ObjectID = Rotator.ID
        Loader.NextASPX = ATLib.Loader.ASPX.RotatorEditor
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
