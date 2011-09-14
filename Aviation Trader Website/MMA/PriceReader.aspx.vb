Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Ad Proof Price Selector
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class PriceReader
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private selectedTab As Integer

    Protected Ad As Ad


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
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)
        displayButtonBar()
        displaytabbar()
        displayInstances()
        displayLeftMenu()


    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B2.Text = "Return to List"
    End Sub


    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.MMAPreview
        topnode = New MenuNode("G", "Preview", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.MMAPrices
        topnode = New MenuNode("H", "Pricing", Loader.Target, True)
        tabbar.Nodes.Add(topnode)
        '
        ' do not show this button for archived ads
        '
        If Ad.ProdnStatus <> ATLib.Ad.ProdnState.Archived Then
            Loader.NextASPX = ATLib.Loader.ASPX.MMAProdnNote
            topnode = New MenuNode("H", "Requests", Loader.Target, False)
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


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        leftmenu.Add("Manage My Ads", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.MMAPriceReader
        leftmenu.Add(Ad.Adnumber, Loader.Target, True)

        CatLabel.Text = Ad.Classification.Category.Name
        ClsLabel.Text = Ad.Classification.Name
        Pic.ImageUrl = Ad.THBURL
        statusLabel.Text = Ad.ProdnStatus.ToString

    End Sub

    Private Sub displayInstances()

        InstanceList.DataSource = Ad.Instances
        InstanceList.DataBind()
        sumPrices()

    End Sub

    Private Sub sumPrices()
        Dim subTotal As Integer = 0
        Dim discount As Integer = 0
        Dim discountedSubtotal As Integer = 0
        Dim latestListing As Integer = 0

        For Each instance As AdInstance In Ad.Instances
            subTotal += instance.Subtotal
        Next

        subtotalLabel.Text = "$" & CommonRoutines.Integer2Dollars(subTotal)
        ' 
        ' display latest listing if applicable
        '

        latestlistingrow.visible = False
        If Ad.IsLatestListing Then
            latestlistingrow.visible = True
            latestListing = sys.LatestListingLoading
            latestlistingLabel.Text = "$" & CommonRoutines.Integer2Dollars(latestListing)
        End If

        discount = Convert.ToInt32((subTotal + latestListing) * Ad.Usr.Discount / 100)
        DiscountLabel.Text = "$" + CommonRoutines.Integer2Dollars(discount)
        '
        ' only show discount line if he gets a discount
        '
        discountRow.Visible = False
        If Ad.Usr.Discount <> 0 Then discountRow.Visible = True
        discountedSubtotal = subTotal + latestListing - discount
        Dim gst As Integer = 0
        If Not Ad.Usr.IsGSTExempt Then gst = Convert.ToInt32((discountedSubtotal) * 0.1)
        GSTLabel.Text = "$" & CommonRoutines.Integer2Dollars(gst)
        TotalLabel.Text = "$" & CommonRoutines.Integer2Dollars(discountedSubtotal + gst)

    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        Loader.SelectedTab = selectedTab
        Response.Redirect(Loader.Target)
    End Sub


    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : return2List()
        End Select
    End Sub


End Class
