Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* CategoryEditor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = 
'*
'***************************************************************************************

Partial Class Category_CategoryListing
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Protected ClassificationCountTotal As Integer
    Protected AdCountTotal As Integer


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

        displayLeftMenu()
        displayButtonBar()
        displaynavbar()
        displayCategories()

    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.System
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
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
        leftmenu.Add("Categories", Loader.Target, True)

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


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "ADD CATEGORY"
    End Sub


    Private Sub displayCategories()

        '
        ' set up nav targets
        '
        Dim mysys As New ATSystem
        mysys.Retrieve()

        ClassificationCountTotal = 0
        AdCountTotal = 0
        Dim categories As Categories = mysys.Categories
        Loader.NextASPX = Loader.ASPX.CategoryEditor
        For Each cat As Category In categories
            Loader.ObjectID = cat.ID
            cat.NavTarget = Loader.Target
            ClassificationCountTotal += cat.ClassificationCount
            AdCountTotal += cat.AdCount
        Next

        CategoryList.DataSource = categories
        CategoryList.DataBind()

    End Sub

    Private Sub addCategory()
        '
        ' add a new category
        '
        Dim cat As New Category
        cat.systemID = sys.ID
        cat.Name = "New Category"
        cat.SortKey = "Z"
        Loader.ObjectID = cat.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.CategoryEditor
        Response.Redirect(Loader.Target)

    End Sub

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addCategory()
        End Select
    End Sub


End Class
