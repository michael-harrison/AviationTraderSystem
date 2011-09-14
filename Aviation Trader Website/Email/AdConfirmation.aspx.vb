Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Ad Confirmation email
'*
'*
'*  Loader: objectID = Ad ID
'*
'***************************************************************************************

Partial Class AdConfirmation
    Inherits System.Web.UI.Page
    Protected Sys As ATSystem
    Private Loader As Loader
    Protected Ad As Ad

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Sys = New ATSystem
        Sys.Retrieve()
        '
        ' there is no slot for emails
        '
        ''Dim ss As String = "514D00000059000000025C0000000400"
        ''Loader = New Loader(ss)
        Loader = New Loader(Request.QueryString(0))
        Loader.ApplicationPath = Sys.ExternalURL           'external url for email access

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)
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


End Class
