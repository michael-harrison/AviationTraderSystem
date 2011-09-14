Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Classification Listing
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Categoryy ID
'*
'***************************************************************************************

Partial Class Category_ClassificationListing
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Category As Category
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
        displayNavbar()
        displayClassifications()

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.CategoryEditor
        leftmenu.Add(Category.Name, Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.ClassificationListing
        leftmenu.Add("Classifications", Loader.Target, True)


    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "ADD Classification"
    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.Category
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayClassifications()

        '
        ' set up nav Classifications
        '
        Dim mysys As New ATSystem
        mysys.Retrieve()

        ClassificationCountTotal = 0
        AdCountTotal = 0

        Loader.NextASPX = Loader.ASPX.ClassificationEditor
        For Each classification As Classification In Category.Classifications
            Loader.ObjectID = classification.ID
            classification.NavTarget = Loader.Target
            AdCountTotal += classification.AdCount
        Next

        ClassificationList.DataSource = Category.Classifications
        ClassificationList.DataBind()

    End Sub

    Private Sub addClassification()
        '
        ' add a new Classification
        '
        Dim classification As New Classification
        classification.CategoryID = Category.ID
        classification.Name = "New Classification"
        classification.SortKey = "Z"
        Loader.ObjectID = classification.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.ClassificationEditor
        Response.Redirect(Loader.Target)

    End Sub

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addClassification()
        End Select
    End Sub


End Class
