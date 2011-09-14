Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Spec Group Listing
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Classificationy ID
'*
'***************************************************************************************

Partial Class Category_SpecgroupListing
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
        displaySpecgroups()

    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0


        Loader.NextASPX = ATLib.Loader.ASPX.ClassificationEditor
        leftmenu.Add(Classification.Name, Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.SpecGroupListing
        leftmenu.Add("Spec Groups", Loader.Target, True)

    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "ADD Spec group"
    End Sub

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.Classification
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displaySpecgroups()

        Loader.NextASPX = Loader.ASPX.SpecGroupEditor
        For Each specgroup As SpecGroup In Classification.SpecGroups
            Loader.ObjectID = specgroup.ID
            specgroup.NavTarget = Loader.Target
        Next

        specgroupList.DataSource = Classification.SpecGroups
        specgroupList.DataBind()

    End Sub

    Private Sub addSpecgroup()
        '
        ' add a new spec group
        '
        Dim specgroup As New SpecGroup
        specgroup.ClassificationID = Classification.ID
        specgroup.Name = "New Specgroup"
        specgroup.SortKey = "Z"
        Loader.ObjectID = specgroup.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.SpecGroupEditor
        Response.Redirect(Loader.Target)

    End Sub

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addSpecgroup()
        End Select
    End Sub


End Class
