Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* All News Stories
'*
'* ON ENTRY:
'*
'*  Loader: objectID = SystemID
'*          selectedTab = tab number        
'*
'***************************************************************************************

Partial Class AllStories
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
        displayItems(status)

    End Sub

    

    Private Sub displaytabBar()
        '
        ' save current tab number
        '
        Dim selectedTab As Integer = Loader.SelectedTab

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.AllStories

        Loader.SelectedTab = 0
        topnode = New MenuNode("A", "Latest News", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 1
        topnode = New MenuNode("B", "Other Current News", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 2
        topnode = New MenuNode("C", "Archived News", Loader.Target, False)
        tabbar.Nodes.Add(topnode)
        '
        ' put the selected tab down
        '
        tabbar.Nodes(selectedTab).Selected = True
        Loader.SelectedTab = selectedTab
    End Sub



    Private Function mapTab2Status(ByVal selectedTab As Integer) As NewsItem.ProdnState

        Select Case selectedTab
            Case 0 : Return NewsItem.ProdnState.Latest
            Case 1 : Return NewsItem.ProdnState.Current
            Case 2 : Return NewsItem.ProdnState.Archived
        End Select

    End Function


    Private Sub displayItems(ByVal Status As NewsItem.ProdnState)
        
        NewsList.DataSource = sys.NewsItems(Status)
        NewsList.DataBind()

    End Sub



End Class
