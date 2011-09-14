Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Ad Submit Confirmation
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class SubmitAd
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private tabID As String

    Protected Ad As Ad


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

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)
        '
        ' update the ad status to submitted
        '
        Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
        Ad.Update()


        Loader.NextASPX = ATLib.Loader.ASPX.AdManager
        displayLeftMenu()
        displayButtonBar()
        If Not IsPostBack Then
            displayadinstances()
        End If

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Dim selectedTab As Integer = Loader.SelectedTab           'save selected tab
        Loader.SelectedTab = 0
        Loader.ObjectID = Slot.ImpersonateUsrID
        '
        ' if impersonation is on, add in the other user's login level
        '
        Dim myloginlevel As Usr.LoginLevels = Slot.LoginLevel
        If Slot.UsrID <> Slot.ImpersonateUsrID Then myloginlevel = Slot.ImpersonatedUsr.LoginLevel

        Select Case myloginlevel

            Case Usr.LoginLevels.AdvSub
                Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                leftmenu.Add("Create New Ad", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                leftmenu.Add("Manage My Ads", Loader.Target, True)

                Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                leftmenu.Add("Manage My Subscription", Loader.Target, False)

            Case Usr.LoginLevels.Advertiser
                Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                leftmenu.Add("Create New Ad", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                leftmenu.Add("Manage My Ads", Loader.Target, False)


            Case Usr.LoginLevels.Subscriber
                Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                leftmenu.Add("Manage My Subscription", Loader.Target, False)
        End Select

        Loader.SelectedTab = selectedTab          'restore selected tab



    End Sub


    Private Sub displayButtonBar()
        ButtonBar.B2.Text = "Return to List"
    End Sub
    Private Sub displayadinstances()
        '
        ' bind publications to outside repeater
        '
        Dim publications As Publications = sys.Publications
        PublicationList.DataSource = publications
        PublicationList.DataBind()

        '
        ' bind open editions to 2nd repeater - suppress display if no open editions
        '
        Dim i As Integer = 0
        For Each r1 As RepeaterItem In PublicationList.Items
            If Ad.PublicationInstances(publications(i).ID).Count = 0 Then
                r1.Visible = False
            Else

                Dim editions As Editions = publications(i).Editions(Edition.ProdnState.Open)
                Dim editionlist As Repeater = CType(r1.FindControl("EditionList"), Repeater)
                If editions.Count = 0 Then r1.Visible = False
                editionlist.DataSource = editions
                editionlist.DataBind()
                '
                ' bind products to 3rd repeater only if ad is in the edition
                '
                Dim j As Integer = 0
                For Each r2 As RepeaterItem In editionlist.Items
                    If Ad.EditionInstances(editions(j).ID).Count = 0 Then
                        r2.Visible = False
                    Else

                        Dim productlist As Repeater = CType(r2.FindControl("ProductList"), Repeater)
                        Dim products As Products = publications(i).Products
                        productlist.DataSource = products
                        productlist.DataBind()
                        '
                        ' suppress display of product if it it is not selected for the ad
                        '
                        Dim k As Integer = 0
                        For Each r3 As RepeaterItem In productlist.Items
                            If Ad.ProductEditionInstances(products(k).ID, editions(j).ID).Count = 0 Then r3.Visible = False

                            k += 1
                        Next
                    End If
                    j += 1
                Next
            End If
            i += 1
        Next

    End Sub

    Private Sub emailConfirmation()

        Dim EA As New EmailAssistant(sys)
        EA.SendAdConfirmation(Ad)

        ButtonBar.Msg = Constants.EmailSent
    End Sub



    Protected Sub btnEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmail.Click
        emailConfirmation()
        displayadinstances()
    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
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
