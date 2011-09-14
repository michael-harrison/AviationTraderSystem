Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Product Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Product ID
'*
'***************************************************************************************

Partial Class ProductEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Product As Product


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

        Dim Products As New Products
        Product = Products.Retrieve(Loader.ObjectID)

        displayLeftMenu()
        displayButtonBar()
        displayNavbar()

        If Not IsPostBack Then
            displayProduct()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Delete Product"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.Product
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.ObjectID = Product.publicationID
        Loader.NextASPX = ATLib.Loader.ASPX.ProductListing
        leftmenu.Add("Products", Loader.Target, False)

        Loader.ObjectID = Product.ID
        Loader.NextASPX = ATLib.Loader.ASPX.ProductEditor
        leftmenu.Add(Product.Name, Loader.Target, True)


    End Sub

    Private Sub displayProduct()

        NameBox.Text = Product.Name
        SortKeyBox.Text = Product.SortKey
        AdCountLabel.Text = Product.AdCount.ToString
        DescriptionBox.Text = Product.Description
        InstanceLoadingBox.Text = CommonRoutines.Integer2Dollars(Product.InstanceLoading)
        publicationdd.datasource = sys.Publications
        publicationdd.databind()
        PublicationDD.SelectedValue = CommonRoutines.Int2Hex(Product.publicationID)
        '
        ' product type dd
        '
        Dim EA As New EnumAssistant(New Product.Types)
        ProductTypeDD.DataSource = EA
        ProductTypeDD.DataBind()
        ProductTypeDD.SelectedValue = Convert.ToString(Product.Type)
        '
        ' set Product dd
        '
        ProductDD.DataSource = Product.Publication.Products
        ProductDD.DataBind()
        ProductDD.SelectedValue = Product.hexID

    End Sub

    Private Sub updateProduct()

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring

        IV.MaxStringLength = ATSystem.SysConstants.charLength
        Product.Name = IV.ValidateText(NameBox, NameError)
        Product.SortKey = IV.ValidateText(SortKeyBox, SortKeyError)
        Product.InstanceLoading = IV.ValidateDollars(InstanceLoadingBox, InstanceLoadingError)
        Product.Type = CType(ProductTypeDD.SelectedValue, Product.Types)

        IV.MaxStringLength = ATSystem.SysConstants.textCharLength
        Product.Description = IV.ValidateText(DescriptionBox, DescriptionError)
        '
        ' move ads if necessary
        '
        Dim newProductID As Integer = CommonRoutines.Hex2Int(ProductDD.SelectedValue)
        If newProductID <> Product.ID Then
            For Each Adinstance As AdInstance In Product.AdInstances
                Adinstance.ProductID = newProductID
                Adinstance.Update()
            Next
        End If
        '
        ' only allow sending of this product to another pub if it has no ads
        '
        Dim newPublicationID As Integer = CommonRoutines.Hex2Int(PublicationDD.SelectedValue)
        If (Product.AdCount > 0) And (newPublicationID <> Product.publicationID) Then
            ButtonBar.Msg = Constants.NoProductSend
        Else
            Product.publicationID = newPublicationID
    
            If IV.ErrorCount = 0 Then
                Product.Update()
                Loader.NextASPX = ATLib.Loader.ASPX.ProductEditor
                Loader.ObjectID = Product.ID
                Response.Redirect(Loader.Target)         'refresh entire page
            End If
        End If

    End Sub

    Private Sub deleteProduct()
        If Product.AdCount = 0 Then
            Loader.ObjectID = Product.publicationID
            Product.Deleted = True
            Product.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.ProductListing
            Response.Redirect(Loader.Target)
        Else
            ButtonBar.Msg = Constants.NoProductDelete
        End If
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : deleteProduct()
            Case 2 : updateProduct()
        End Select
    End Sub

End Class
