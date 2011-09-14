Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Publication Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Publication ID
'*
'***************************************************************************************

Partial Class PublicationEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Publication As Publication


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
        displaynavbar()

        If Not IsPostBack Then
            displayPublication()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Delete Publication"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.Publication
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.PublicationListing
        leftmenu.Add("Publications", Loader.Target, False)


        Loader.NextASPX = ATLib.Loader.ASPX.PublicationEditor
        leftmenu.Add(Publication.Name, Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.ProductListing
        leftmenu.Add("Products", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.Editionlisting
        leftmenu.Add("Editions", Loader.Target, False)

    End Sub

    Private Sub displayPublication()

        NameBox.Text = Publication.Name
        SortKeyBox.Text = Publication.SortKey
        descriptionbox.text = Publication.Description
        AdCountLabel.Text = Publication.AdCount.ToString
        ProductCountLabel.Text = Publication.ProductCount.ToString
        EditionCountLabel.Text = Publication.EditionCount.ToString

        Dim EA As New EnumAssistant(New Publication.Types)


        TypeDD.DataSource = EA
        TypeDD.DataBind()
        TypeDD.SelectedValue = Convert.ToString(Publication.Type)
        '
        ' set Publication dd
        '
        PublicationDD.DataSource = sys.Publications
        PublicationDD.DataBind()
        PublicationDD.SelectedValue = Publication.hexID

    End Sub

    Private Sub updatePublication()

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        Publication.Name = IV.ValidateText(NameBox, NameError)
        Publication.SortKey = IV.ValidateText(SortKeyBox, SortKeyError)

        IV.MaxStringLength = ATSystem.SysConstants.textCharLength
        Publication.Description = IV.ValidateText(DescriptionBox, DescriptionError)

        Publication.Type = CType(TypeDD.SelectedValue, Publication.Types)

        If IV.ErrorCount = 0 Then
            '
            ' move editions and products
            '
            Dim newPublicationID As Integer = CommonRoutines.Hex2Int(PublicationDD.SelectedValue)
            If newPublicationID <> Publication.ID Then
                For Each edition As Edition In Publication.Editions
                    edition.publicationID = newPublicationID
                    edition.Update()
                Next
                For Each product As Product In Publication.Products
                    product.publicationID = newPublicationID
                    product.Update()
                Next
            End If
            Publication.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.PublicationEditor
            Loader.ObjectID = Publication.ID
            Response.Redirect(Loader.Target)         'refresh entire page
        End If
       
    End Sub

    Private Sub deletePublication()
        If (Publication.EditionCount = 0) And (Publication.ProductCount = 0) Then
            Publication.Deleted = True
            Publication.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.PublicationListing
            Loader.ObjectID = sys.ID
            Response.Redirect(Loader.Target)
        Else
            ButtonBar.Msg = Constants.NoPublicationDelete
        End If
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : deletePublication()
            Case 2 : updatePublication()
        End Select
    End Sub

End Class
