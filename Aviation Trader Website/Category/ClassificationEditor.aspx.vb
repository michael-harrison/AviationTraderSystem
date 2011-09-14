Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Classification Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = classification ID
'*
'***************************************************************************************

Partial Class ClassificationEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Classification As Classification


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

        Dim classifications As New Classifications
        Classification = classifications.Retrieve(Loader.ObjectID)

        displayLeftMenu()
        displayButtonBar()
        displayNavbar()

        If Not IsPostBack Then
            displayClassification()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Delete Classification"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.Classification
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.ObjectID = Classification.CategoryID
        Loader.NextASPX = ATLib.Loader.ASPX.ClassificationListing
        leftmenu.Add("Classifications", Loader.Target, False)

        Loader.ObjectID = Classification.ID
        Loader.NextASPX = ATLib.Loader.ASPX.ClassificationEditor
        leftmenu.Add(Classification.Name, Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.SpecGroupListing
        leftmenu.Add("Spec Groups", Loader.Target, False)

    End Sub

    Private Sub displayClassification()

        NameBox.Text = Classification.Name
        SortKeyBox.Text = Classification.SortKey
        AdCountLabel.Text = Classification.AdCount.ToString
        CategoryDD.DataSource = sys.Categories
        CategoryDD.DataBind()
        CategoryDD.SelectedValue = CommonRoutines.Int2Hex(Classification.CategoryID)
        '
        ' set Classification dd
        '
        ClassificationDD.DataSource = Classification.Category.Classifications
        ClassificationDD.DataBind()
        ClassificationDD.SelectedValue = Classification.hexID

    End Sub

    Private Sub updateClassification()

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        Classification.Name = IV.ValidateText(NameBox, NameError)
        Classification.SortKey = IV.ValidateText(SortKeyBox, SortKeyError)
        '
        ' move ads if necessary, losing all specs of each ad
        '
        Dim newClsID As Integer = CommonRoutines.Hex2Int(ClassificationDD.SelectedValue)
        If newClsID <> Classification.ID Then
            For Each Ad As Ad In Classification.Ads
                Ad.DeleteSpecs()
                Ad.ClassificationID = newClsID
                Ad.Update()
            Next
        End If

        If IV.ErrorCount = 0 Then
            Classification.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.ClassificationEditor
            Loader.ObjectID = Classification.ID
            Response.Redirect(Loader.Target)         'refresh entire page
        End If

    End Sub

    Private Sub deleteClassification()
        If Classification.AdCount = 0 Then
            Loader.ObjectID = Classification.CategoryID
            Classification.Deleted = True
            Classification.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.ClassificationListing
            Response.Redirect(Loader.Target)
        Else
            ButtonBar.Msg = Constants.NoClassificationDelete
        End If
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : deleteClassification()
            Case 2 : updateClassification()
        End Select
    End Sub

End Class
