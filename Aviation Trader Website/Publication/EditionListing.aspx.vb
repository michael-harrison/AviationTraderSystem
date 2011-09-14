Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Edition Listing
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Publicationy ID
'*
'***************************************************************************************

Partial Class Publication_EditionListing
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Publication As Publication
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


        Dim publications As New Publications
        Publication = publications.Retrieve(Loader.ObjectID)

        displayLeftMenu()
        displayButtonBar()
        displayNavbar()
        displayEditions()

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.PublicationEditor
        leftmenu.Add(Publication.Name, Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.ProductListing
        leftmenu.Add("Products", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.Editionlisting
        leftmenu.Add("Editions", Loader.Target, True)

    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "ADD Edition"
    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.Publication
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayEditions()

        '
        ' set up nav Editions
        '
        Dim mysys As New ATSystem
        mysys.Retrieve()

        AdCountTotal = 0
        Loader.NextASPX = Loader.ASPX.EditionEditor
        For Each Edition As Edition In Publication.Editions
            Loader.ObjectID = Edition.ID
            Edition.NavTarget = Loader.Target
            AdCountTotal += Edition.AdCount
        Next

        EditionList.DataSource = Publication.Editions
        EditionList.DataBind()

    End Sub

    Private Sub addEdition()
        '
        ' add a new Edition
        '
        Dim Edition As New Edition
        Edition.publicationID = Publication.ID
        Edition.Name = "New Edition"
        Edition.ShortName = ""
        Edition.Description = ""
        Edition.OnsaleDate = Today
        Edition.ProdnDeadline = Today
        Edition.AdDeadline = Today
        Edition.SortKey = ""
        Edition.ProdnStatus = ATLib.Edition.ProdnState.Closed
        Edition.Visibility = ATLib.Edition.VisibleState.Future
        Loader.ObjectID = Edition.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.EditionEditor
        Response.Redirect(Loader.Target)

    End Sub

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addEdition()
        End Select
    End Sub


End Class
