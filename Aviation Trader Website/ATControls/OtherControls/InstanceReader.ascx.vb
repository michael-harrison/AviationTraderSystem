Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Ad Listing
'*
'*  Displays the listing format of an ad
'*
'*
'***************************************************************************************

Partial Class InstanceReader
    Inherits System.Web.UI.UserControl

    Private _adInstance As AdInstance

    Public Property AdInstance() As AdInstance
        Get
            '
            ' on a postback recover object from viewstate id
            '
            If _adInstance Is Nothing Then
                Dim adInstanceID As Integer = CommonRoutines.Hex2Int(ViewState.Item("AdInstanceID").ToString)
                Dim adinstances As New AdInstances
                _adInstance = adinstances.Retrieve(adInstanceID)
            End If
            Return _adInstance
        End Get

        Set(ByVal value As AdInstance)
            _adInstance = value
            '
            ' save instance ID in viewstate
            '
            ViewState.Add("AdInstanceID", AdInstance.hexID)
        End Set
    End Property


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        configureControl()
    End Sub


    Private Sub configureControl()

        Basic.Visible = False
        Premium.Visible = False
        PDF.Visible = False
        PDFText.Visible = False
        Featured.Visible = False
        Classad.Visible = False
        DisplayAd.Visible = False

        Dim Ad As Ad = AdInstance.Ad

        '
        ' set up click actions
        '
        Dim inlineClick As String = "incrAdClickCount('" & Ad.hexID & "');self.location.href='" & _adInstance.NavTarget & "'"
        Dim popupClick As String = "incrAdClickCount('" & Ad.hexID & "');popup('" & _adInstance.PreviewPDFURL & "','b')"

        Select Case _adInstance.ProductType

            Case Product.Types.WebBasic
                BasicPic.ImageUrl = Ad.THBURL
                BasicKeyWords.Text = Ad.KeyWords
                BasicItemPrice.Text = Ad.ItemPrice
                BasicText.Text = Ad.Summary

                BasicPic.Attributes.Add("onclick", inlineClick)
                BasicLink.Attributes.Add("onclick", inlineClick)

                If Ad.IsLatestListing Then Basic.CssClass &= " latestlisting"
                Basic.Visible = True

            Case Product.Types.WebPremium
                PremiumPic.ImageUrl = Ad.THBURL
                PremiumKeyWords.Text = Ad.KeyWords
                PremiumItemPrice.Text = Ad.ItemPrice
                PremiumText.Text = Ad.Summary

                PremiumPic.Attributes.Add("onclick", inlineClick)
                PremiumLink.Attributes.Add("onclick", inlineClick)

                If Ad.IsLatestListing Then Premium.CssClass &= " latestlisting"
                Premium.Visible = True

            Case Product.Types.WebPDF
                Dim sys As New ATSystem
                sys.Retrieve()
                Ad.PhysicalApplicationPath = sys.PhysicalApplicationPath
                PDFPic.ImageUrl = Ad.THBURL
                PDFKeyWords.Text = Ad.KeyWords
                PDFItemPrice.Text = Ad.ItemPrice

                PDFPic.Attributes.Add("onclick", popupClick)
                PDFLink.Attributes.Add("onclick", popupClick)

                If Ad.IsLatestListing Then PDF.CssClass &= " latestlisting"
                PDF.Visible = True

            Case Product.Types.WebPDFText
                Dim sys As New ATSystem
                sys.Retrieve()
                Ad.PhysicalApplicationPath = sys.PhysicalApplicationPath
                PDFTextPic.ImageUrl = Ad.THBURL
                PDFTextKeyWords.Text = Ad.KeyWords
                PDFTextItemPrice.Text = Ad.ItemPrice
                PDFTextText.Text = Ad.Summary

                PDFTextPic.Attributes.Add("onclick", popupClick)
                PDFTextLink.Attributes.Add("onclick", popupClick)

                If Ad.IsLatestListing Then PDFText.CssClass &= " latestlisting"
                PDFText.Visible = True

            Case Product.Types.WebFeaturedAd
                FeaturedPic.ImageUrl = Ad.THBURL
                FeaturedKeywords.Text = Ad.KeyWords

                FeaturedPic.Attributes.Add("onclick", inlineClick)

                FeaturedText.Text = Ad.ItemPrice
                ''     If Ad.IsLatestListing Then Featured.CssClass &= " latestlisting"
                Featured.Visible = True

            Case Product.Types.ClassadColorPic, Product.Types.ClassadMonoPic, Product.Types.ClassadTextOnly
                ClassadPreview.ImageUrl = _adInstance.PreviewImageURL
                PDFTextPic.Attributes.Add("onclick", popupClick)
                Classad.Visible = True

            Case Product.Types.DisplayModule, Product.Types.DisplayComposite, Product.Types.DisplayFinishedArt, Product.Types.DisplayStandAlone
                DisplayPreview.ImageUrl = _adInstance.PreviewImageURL
                PDFTextPic.Attributes.Add("onclick", popupClick)
                DisplayAd.Visible = True

            Case Else



        End Select

    End Sub



End Class
