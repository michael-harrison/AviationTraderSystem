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

Partial Class ProdnNote
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
        ButtonBar.B1.Text = "Save Changes"
        ButtonBar.B2.Text = "Return to list"

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        leftmenu.Add("Manage My Ads", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.MMAProdnNote
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
        '
        ' do not show this button for archived ads
        '
        If Ad.ProdnStatus <> ATLib.Ad.ProdnState.Archived Then
            Loader.NextASPX = ATLib.Loader.ASPX.MMAProdnNote
            topnode = New MenuNode("H", "Requests", Loader.Target, True)
            tabbar.Nodes.Add(topnode)
        End If
        '
        ' only show this button if the status is waiting for approval
        '
        If Ad.ProdnStatus = ATLib.Ad.ProdnState.Proofed Then
            Loader.NextASPX = ATLib.Loader.ASPX.MMAProofApproval
            topnode = New MenuNode("H", "Proof Approval", Loader.Target, False)
            tabbar.Nodes.Add(topnode)
        End If

    End Sub



    Private Sub displayContent()
        ProdnRequest.Text = Ad.ProdnRequest
        ProdnResponse.Text = Ad.ProdnResponse
    End Sub


    Private Sub updateContent()
        '
        ' generate email to prodn staff - but only if text really has changed - prevent multiple emails on hitting save
        '

        If Ad.ProdnRequest <> ProdnRequest.Text Then
            Ad.ProdnRequest = ProdnRequest.Text
            Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
            Ad.Update()
            Dim EA As New EmailAssistant(sys)
            EA.SendProdnNote(Ad)
            ButtonBar.Msg = Constants.ProdnEmailSent
        End If
        displayLeftMenu()
    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        Loader.SelectedTab = selectedTab
        Response.Redirect(Loader.Target)
    End Sub
    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : updateContent()
            Case 2 : return2List()

        End Select
    End Sub


End Class
