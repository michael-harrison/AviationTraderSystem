Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Cetegory Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = category ID
'*
'***************************************************************************************

Partial Class CategoryEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Category As Category


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

        Dim categories As New Categories
        Category = categories.Retrieve(Loader.ObjectID)

        displayLeftMenu()
        displayButtonBar()
        displaynavbar()


      
        If Not IsPostBack Then
            displayCategory()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Delete Category"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.Category
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.ObjectID = sys.ID
        Loader.NextASPX = ATLib.Loader.ASPX.CategoryListing
        leftmenu.Add("Categories", Loader.Target, False)

        Loader.ObjectID = Category.ID
        Loader.NextASPX = ATLib.Loader.ASPX.CategoryEditor
        leftmenu.Add(Category.Name, Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.ClassificationListing
        leftmenu.Add("Classifications", Loader.Target, False)


    End Sub

    Private Sub displayCategory()

        NameBox.Text = Category.Name
        SortKeyBox.Text = Category.SortKey
        AdCountLabel.Text = Category.AdCount.ToString
        BarCheck.Checked = Category.IncludeInBar
        '
        ' set category dd
        '
        CategoryDD.DataSource = sys.Categories
        CategoryDD.DataBind()
        CategoryDD.SelectedValue = Category.hexID

    End Sub

    Private Sub updateCategory()

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        Category.Name = IV.ValidateText(NameBox, NameError)
        Category.SortKey = IV.ValidateText(SortKeyBox, SortKeyError)

        Category.IncludeInBar = BarCheck.Checked

        '
        ' move classifications if necessary
        '
        Dim newCatID As Integer = CommonRoutines.Hex2Int(CategoryDD.SelectedValue)
        If newCatID <> Category.ID Then
            For Each Classification As Classification In Category.Classifications
                Classification.CategoryID = newCatID
                Classification.Update()
            Next
        End If

        If IV.ErrorCount = 0 Then
            Category.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.CategoryEditor
            Loader.ObjectID = Category.ID
            Response.Redirect(Loader.Target)         'refresh entire page
        End If
       
    End Sub

    Private Sub deleteCategory()
        If Category.ClassificationCount = 0 Then
            Category.Deleted = True
            Category.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.CategoryListing
            Loader.ObjectID = sys.ID
            Response.Redirect(Loader.Target)
        Else
            ButtonBar.Msg = Constants.NoCategoryDelete
        End If
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : deleteCategory()
            Case 2 : updateCategory()
        End Select
    End Sub

End Class
