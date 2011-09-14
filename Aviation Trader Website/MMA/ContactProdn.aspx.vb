Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Contact Prodn
'*
'* ON ENTRY:
'*
'*  Loader: objectID = AdID
'*
'***************************************************************************************

Partial Class ContactProdn
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
        Page.Theme = Slot.Skin

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
        displayButtonBar()


    End Sub


    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Save Changes"
        ButtonBar.B2.Text = "Return to List"
    End Sub



    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        leftmenu.Add("Manage My Ads", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.MMAContactProdn
        leftmenu.Add(Ad.Adnumber, Loader.Target, True)

        CatLabel.Text = Ad.Classification.Category.Name
        ClsLabel.Text = Ad.Classification.Name
        Pic.ImageUrl = Ad.THBURL
        statusLabel.Text = Ad.ProdnStatus.ToString

    End Sub


    Private Sub displayContent()
        ProdnRequest.Text = Ad.ProdnRequest
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
