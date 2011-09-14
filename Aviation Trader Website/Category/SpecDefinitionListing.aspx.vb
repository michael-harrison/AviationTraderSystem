Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Spec Definition Listing
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Specgroup ID
'*
'***************************************************************************************

Partial Class Category_SpecDefinitionListing
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
        displaySpecDefinitions()

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.SpecGroupEditor
        leftmenu.Add(Specgroup.Name, Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.SpecDefinitionListing
        leftmenu.Add("Spec Definitions", Loader.Target, True)


    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "ADD Spec Definition"
    End Sub

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.SpecGroup
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displaySpecDefinitions()


        Loader.NextASPX = Loader.ASPX.SpecDefinitionEditor
        For Each specdefinition As SpecDefinition In Specgroup.SpecDefinitions
            Loader.ObjectID = specdefinition.ID
            specdefinition.NavTarget = Loader.Target
        Next

        SpecDefinitionList.DataSource = Specgroup.SpecDefinitions
        SpecDefinitionList.DataBind()

    End Sub

    Private Sub addSpecDefinition()
        '
        ' add a new spec definition
        '
        Dim specDefinition As New SpecDefinition
        specDefinition.SpecGroupID = Specgroup.ID
        specDefinition.Name = "New Spec definition"
        specDefinition.SortKey = "Z"
        specDefinition.ValueList = ""
        specDefinition.DisplayType = ATLib.SpecDefinition.DisplayTypes.Text
        Loader.ObjectID = specDefinition.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.SpecDefinitionEditor
        Response.Redirect(Loader.Target)

    End Sub

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addSpecDefinition()
        End Select
    End Sub


End Class
