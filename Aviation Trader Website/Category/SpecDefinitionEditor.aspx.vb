Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* SpecDefinition Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = spec definition ID
'*
'***************************************************************************************

Partial Class SpecDefinitionEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private SpecDefinition As SpecDefinition


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

        Dim SpecDefinitions As New SpecDefinitions
        SpecDefinition = SpecDefinitions.Retrieve(Loader.ObjectID)

        displayLeftMenu()
        displayButtonBar()
        displayNavbar()

        If Not IsPostBack Then
            displaySpecDefinition()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Delete Spec Definition"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.SpecDefinition
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0
      
        Loader.ObjectID = SpecDefinition.SpecGroupID
        Loader.NextASPX = ATLib.Loader.ASPX.SpecDefinitionListing
        leftmenu.Add("Spec Definitions", Loader.Target, False)

        Loader.ObjectID = SpecDefinition.ID
        Loader.NextASPX = ATLib.Loader.ASPX.SpecDefinitionEditor
        leftmenu.Add(SpecDefinition.Name, Loader.Target, True)

    End Sub

    Private Sub displaySpecDefinition()

        NameBox.Text = SpecDefinition.Name
        SortKeyBox.Text = SpecDefinition.SortKey
        valuelistbox.Text = SpecDefinition.ValueList
        '
        ' bind display type dd and spec group dd
        '
        Dim EA As New EnumAssistant(New SpecDefinition.DisplayTypes)
        displaytypeDD.DataSource = EA
        displaytypeDD.SelectedValue = Convert.ToString(SpecDefinition.DisplayType)
        displaytypeDD.DataBind()

        specgroupDD.DataSource = SpecDefinition.SpecGroup.Classification.SpecGroups
        specgroupDD.SelectedValue = CommonRoutines.Int2Hex(SpecDefinition.SpecGroupID)
        specgroupDD.DataBind()

    End Sub

    Private Sub updateSpecDefinition()

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        SpecDefinition.Name = IV.ValidateText(NameBox, NameError)
        SpecDefinition.SortKey = IV.ValidateText(SortKeyBox, SortKeyError)

        SpecDefinition.DisplayType = CType(displaytypeDD.SelectedValue, SpecDefinition.DisplayTypes)

        IV.MinStringLength = 0         'allow nullstring
        SpecDefinition.ValueList = valuelistbox.Text

        SpecDefinition.SpecGroupID = CommonRoutines.Hex2Int(specgroupDD.SelectedValue)

        If IV.ErrorCount = 0 Then
            SpecDefinition.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.SpecDefinitionEditor
            Loader.ObjectID = SpecDefinition.ID
            Response.Redirect(Loader.Target)         'refresh entire page
        End If

    End Sub

    Private Sub deleteSpecDefinition()
        '
        ' unconditionally deletes the spec definiton and all of the specs associated with it
        '
        Loader.ObjectID = SpecDefinition.SpecGroupID
        SpecDefinition.Deleted = True
        SpecDefinition.Update()
        Loader.NextASPX = ATLib.Loader.ASPX.SpecDefinitionListing

        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : deleteSpecDefinition()
            Case 2 : updateSpecDefinition()
        End Select
    End Sub

End Class
