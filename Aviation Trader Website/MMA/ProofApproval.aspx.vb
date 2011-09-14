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
'*          selectedTab = tab to return to
'*          
'*
'***************************************************************************************

Partial Class ProofApproval
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private sys As ATSystem
    Private Ad As Ad
    Private Slot As Slot
    Private selectedTab As Integer


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

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
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
        displayButtonBar()
        displaytabBar()

    End Sub


    Private Sub displayButtonBar()
        ButtonBar.B0.Text = "Click to disapprove ad"
        ButtonBar.B1.Text = "Click to approve ad"
        ButtonBar.B2.Text = "Return to list"

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        leftmenu.Add("Manage My Ads", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.MMAProofApproval
        leftmenu.Add(Ad.Adnumber, Loader.Target, True)

        CatLabel.Text = Ad.Classification.Category.Name
        ClsLabel.Text = Ad.Classification.Name
        Pic.ImageUrl = Ad.THBURL
        statusLabel.Text = Ad.ProdnStatus.ToString

    End Sub



    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.MMAPreview
        topnode = New MenuNode("G", "Preview", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.MMAPrices
        topnode = New MenuNode("H", "Pricing", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

Loader.NextASPX = ATLib.Loader.ASPX.MMAProdnNote
        topnode = New MenuNode("H", "Requests", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.MMAProofApproval
        topnode = New MenuNode("H", "Proof Approval", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

    End Sub



    Private Sub displayContent()
        ProdnRequest.Text = Ad.ProdnRequest
        ProdnResponse.Text = Ad.ProdnResponse
    End Sub


    Private Sub approve(ByVal approved As Boolean)
        If approved Then
            Ad.ProdnStatus = ATLib.Ad.ProdnState.Approved
            ButtonBar.Msg = Constants.UserApproved
        Else
            Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
            ButtonBar.Msg = Constants.UserUnapproved
        End If
        Ad.ProdnRequest = ProdnRequest.Text
        Ad.Update()
        displayLeftMenu()
    End Sub

   Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        Loader.SelectedTab = selectedTab
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0 : approve(False)
            Case 1 : approve(True)
            Case 2 : return2List()

        End Select
    End Sub


End Class
