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

Partial Class SubmitConfirmation
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

        Loader.NextASPX = ATLib.Loader.ASPX.NewAd
        BtnNewAd.NavigateURL = Loader.Target

        Loader.NextASPX = ATLib.Loader.ASPX.AdManager

        displayadinstances()

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
        errorMsg.Text = Constants.EmailSent
    End Sub


    Protected Sub btnEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmail.Click
        emailConfirmation()
    End Sub

End Class
