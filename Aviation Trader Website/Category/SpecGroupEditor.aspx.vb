Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Specgroup Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Specgroup ID
'*
'***************************************************************************************

Partial Class SpecgroupEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Specgroup As SpecGroup


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

        Dim Specgroups As New SpecGroups
        Specgroup = Specgroups.Retrieve(Loader.ObjectID)

        displayLeftMenu()
        displayButtonBar()
        displayNavbar()

        If Not IsPostBack Then
            displaySpecGroup()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Delete Specgroup"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.SpecGroup
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0
      

        Loader.ObjectID = Specgroup.ClassificationID
        Loader.NextASPX = ATLib.Loader.ASPX.SpecGroupListing
        leftmenu.Add("Spec Groups", Loader.Target, False)

        Loader.ObjectID = Specgroup.ID
        Loader.NextASPX = ATLib.Loader.ASPX.SpecGroupEditor
        leftmenu.Add(Specgroup.Name, Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.SpecDefinitionListing
        leftmenu.Add("Spec Definitions", Loader.Target, False)


    End Sub

    Private Sub displaySpecgroup()

        NameBox.Text = SpecGroup.Name
        SortKeyBox.Text = SpecGroup.SortKey
        '
        ' set Specgroup dd
        '
        SpecgroupDD.DataSource = Specgroup.Classification.SpecGroups
        SpecGroupDD.DataBind()
        SpecGroupDD.SelectedValue = SpecGroup.hexID

    End Sub

    Private Sub updateSpecgroup()

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        SpecGroup.Name = IV.ValidateText(NameBox, NameError)
        SpecGroup.SortKey = IV.ValidateText(SortKeyBox, SortKeyError)
        '
        ' move specdefinitions if necessary
        '
        Dim newSpecgroupID As Integer = CommonRoutines.Hex2Int(SpecgroupDD.SelectedValue)
        If newSpecgroupID <> Specgroup.ID Then
            For Each specdefinition As SpecDefinition In Specgroup.SpecDefinitions
                specdefinition.SpecGroupID = newSpecgroupID
                specdefinition.Update()
            Next
        End If

        If IV.ErrorCount = 0 Then
            Specgroup.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.SpecGroupEditor
            Loader.ObjectID = Specgroup.ID
            Response.Redirect(Loader.Target)         'refresh entire page
        End If

    End Sub

    Private Sub deleteSpecgroup()
        If Specgroup.SpecDefinitions.Count = 0 Then
            Loader.ObjectID = Specgroup.ClassificationID
            Specgroup.Deleted = True
            Specgroup.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.SpecGroupListing
            Response.Redirect(Loader.Target)
        Else
            ButtonBar.Msg = Constants.NoSpecGroupDelete
        End If
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : deleteSpecGroup()
            Case 2 : updateSpecGroup()
        End Select
    End Sub

End Class
