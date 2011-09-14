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

Partial Class Production_SpecEditor
    Inherits System.Web.UI.Page

    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Ad As Ad
    Private selectedTab As Integer
    Private listID As Integer


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.Skin
        '
        ' save return params
        '
        selectedTab = Loader.SelectedTab
        listID = Loader.Param1

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

        displayLeftMenu()
        displaytabBar()
        displayButtonBar()

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Return to list"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.ProofTextEditor
        topnode = New MenuNode("A", "Text", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofImageUploader
        topnode = New MenuNode("B", "Images", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofSpecEditor
        topnode = New MenuNode("C", "Specs", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofSellerInfo
        topnode = New MenuNode("D", "Seller", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofProdnNoteEditor
        topnode = New MenuNode("E", "Prodn", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofProductEditor
        topnode = New MenuNode("F", "Products", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofPreview
        topnode = New MenuNode("G", "Preview", Loader.Target, False, "Please wait - building previews")
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofPriceEditor
        topnode = New MenuNode("H", "Pricing", Loader.Target, False, "Please wait - building previews")
        tabbar.Nodes.Add(topnode)
    End Sub
    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Dim adID As Integer = Loader.ObjectID     'save ad id
        leftmenu.Items.Clear()
        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        Loader.ObjectID = listID
        Loader.SelectedTab = selectedTab
        leftmenu.Add("Proof Reader", Loader.Target, False)
        Loader.ObjectID = adID                    'restore ad id

        Loader.NextASPX = ATLib.Loader.ASPX.ProofTextEditor
        leftmenu.Add(Ad.Adnumber, Loader.Target, True)

        Pic.ImageUrl = Ad.THBURL
        statusLabel.Text = Ad.ProdnStatus.ToString
        AliasLabel.Text = Ad.Usr.AcctAlias
        CatLabel.Text = Ad.Classification.Category.Name
        ClsLabel.Text = Ad.Classification.Name
        FolderLabel.Text = Ad.Folder.Name

    End Sub


    Private Sub displayContent()
        '
        ' update the ad to get the latest specs
        '
        Ad.AddSpecs()
        '
        ' bind the spec groups to the outside repeater
        '
        Dim Specgroups As New SpecGroups
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

        ButtonBar.Msg = Constants.Saved
    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        Loader.ObjectID = listID
        Loader.SelectedTab = selectedTab
        Response.Redirect(Loader.Target)
    End Sub



    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : return2List()
            Case 2 : updateContent()
        End Select
    End Sub
End Class
