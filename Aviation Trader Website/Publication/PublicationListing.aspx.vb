Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* PublicationEditor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = 
'*
'***************************************************************************************

Partial Class Publication_PublicationListing
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Protected ProductCountTotal As Integer
    Protected EditionCounttotal As Integer
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
        displayNavbar()
        displayPublications()

    End Sub

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
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        leftmenu.Add("System Parameters", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.Technotes
        leftmenu.Add("Technotes", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.FolderListing
        Loader.ObjectID = sys.FirstFolderID
        leftmenu.Add("Folders", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.PublicationListing
        leftmenu.Add("Publications", Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.CategoryListing
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


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "ADD Publication"
    End Sub


    Private Sub displayPublications()

        '
        ' set up nav targets
        '
        Dim mysys As New ATSystem
        mysys.Retrieve()

        ProductCountTotal = 0
        EditionCounttotal = 0
        AdCountTotal = 0


        Dim Publications As Publications = mysys.Publications
        Loader.NextASPX = Loader.ASPX.PublicationEditor
        For Each pub As Publication In Publications
            Loader.ObjectID = pub.ID
            pub.NavTarget = Loader.Target
            AdCountTotal += pub.AdCount
            ProductCountTotal += pub.ProductCount
            EditionCounttotal += pub.EditionCount
        Next

        PublicationList.DataSource = Publications
        PublicationList.DataBind()

    End Sub

    Private Sub addPublication()
        '
        ' add a new Publication
        '
        Dim pub As New Publication
        pub.SystemID = sys.ID
        pub.Name = "New Publication"
        pub.Description = ""
        pub.SortKey = "Z"
        Loader.ObjectID = pub.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.PublicationEditor
        Response.Redirect(Loader.Target)

    End Sub

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addPublication()
        End Select
    End Sub


End Class
