Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Ad content editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class Advertise_SpecEditor
    Inherits System.Web.UI.Page

    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Ad As Ad


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.skin

        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)


        If Not IsPostBack Then
            displayContent()
        End If

        displaytabBar()
        displayNavButtons()

    End Sub

    Private Sub displayNavButtons()
        Loader.NextASPX = ATLib.Loader.ASPX.AdCategorySelector
        BtnCategorySelector.NavigateURL = Loader.Target


    End Sub


    Private Sub displaytabBar()
        '
        ' tab bar is in postback mode - see event handler below
        '
        Dim topnode As MenuNode

        topnode = New MenuNode("A", "Text", "", True)
        tabbar.Nodes.Add(topnode)

        topnode = New MenuNode("B", "Images", "", False)
        tabbar.Nodes.Add(topnode)

        topnode = New MenuNode("C", "PDF Upload", "", False)
        tabbar.Nodes.Add(topnode)

        ''topnode = New MenuNode("D", "Specs", "", False)
        ''tabbar.Nodes.Add(topnode)

        topnode = New MenuNode("E", "Requests", "", False)
        tabbar.Nodes.Add(topnode)

    End Sub



    Private Sub displayContent()
        '
        ' bind the spec groups to the outside repeater
        '
        Dim Specgroups As New Specgroups
        Specgroups.retrieveSet(Ad.ClassificationID)
        grouplist.DataSource = Specgroups
        grouplist.DataBind()
        '
        ' bind the spec collection to the inside repeater
        '
        Dim i As Integer = 0
        For Each r1 As RepeaterItem In grouplist.Items
            Dim speclist As Repeater = CType(r1.FindControl("speclist"), Repeater)
            Dim SpecgroupID As Integer = Specgroups(i).ID
            Dim specs As Specs = Ad.Specs(SpecgroupID)
            '
            ' suppress group display if it has no specs
            '
            If specs.Count = 0 Then r1.Visible = False

            speclist.DataSource = specs
            speclist.DataBind()

            i += 1
        Next

    End Sub


    Private Sub updateContent()
        Dim i As Integer = 0
        For Each r1 As RepeaterItem In grouplist.Items
            Dim speclist As Repeater = CType(r1.FindControl("speclist"), Repeater)
            For Each r2 As RepeaterItem In speclist.Items
                Dim spec As ATControls_SpecBuilder = DirectCast(r2.FindControl("spec"), ATControls_SpecBuilder)
                spec.Update()
            Next
        Next

    End Sub

    Private Sub submitad()
        '
        ' deprecated
        '

        Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
        Ad.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.AdSubmitConfirmation
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        submitad()

    End Sub


    Protected Sub tabbar_TopMenuEvent(ByVal sender As Object, ByVal TabID As String) Handles tabbar.TopMenuEvent
        updateContent()
        Select Case TabID
            Case "A" : Loader.NextASPX = ATLib.Loader.ASPX.AdTextEditor
            Case "B" : Loader.NextASPX = ATLib.Loader.ASPX.AdImageUploader
            Case "C" : Loader.NextASPX = ATLib.Loader.ASPX.AdPDFUploader
            Case "D" : Loader.NextASPX = ATLib.Loader.ASPX.AdSpecEditor
            Case "E" : Loader.NextASPX = ATLib.Loader.ASPX.AdProdnNote
        End Select
        Response.Redirect(Loader.Target)
    End Sub
End Class
