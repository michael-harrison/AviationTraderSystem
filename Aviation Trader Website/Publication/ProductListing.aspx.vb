Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Product Listing
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Publicationy ID
'*
'***************************************************************************************

Partial Class Publication_ProductListing
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


        Dim Publications As New Publications
        Publication = Publications.Retrieve(Loader.ObjectID)

        displayLeftMenu()
        displayButtonBar()
        displayNavbar()
        displayProducts()

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0
        Loader.ObjectID = Publication.ID

        Loader.NextASPX = ATLib.Loader.ASPX.PublicationEditor
        leftmenu.Add(Publication.Name, Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.ProductListing
        leftmenu.Add("Products", Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.Editionlisting
        leftmenu.Add("Editions", Loader.Target, False)

     
    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "ADD Product"
    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.Publication
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayProducts()

        '
        ' set up nav Products
        '
        Dim mysys As New ATSystem
        mysys.Retrieve()

        AdCountTotal = 0
        Loader.NextASPX = Loader.ASPX.ProductEditor
        For Each Product As Product In Publication.Products
            Loader.ObjectID = Product.ID
            Product.NavTarget = Loader.Target
            AdCountTotal += Product.AdCount
        Next

        ProductList.DataSource = Publication.Products
        ProductList.DataBind()

    End Sub

    Private Sub addProduct()
        '
        ' add a new Product
        '
        Dim Product As New Product
        Product.publicationID = Publication.ID
        Product.Name = "New Product"
        Product.SortKey = "Z"
        Product.Description = ""
        Product.Type = ATLib.Product.Types.WebBasic
        Loader.ObjectID = Product.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.ProductEditor
        Response.Redirect(Loader.Target)

    End Sub

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addProduct()
        End Select
    End Sub


End Class
