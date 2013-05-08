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

Partial Class ATControls_AdReader
    Inherits System.Web.UI.UserControl

    Private _ad As Ad

    Public Property Ad() As Ad
        Get
            '
            ' on a postback recover object from viewstate id
            '
            If _ad Is Nothing Then
                Dim adID As Integer = CommonRoutines.Hex2Int(ViewState.Item("AdID").ToString)
                Dim ads As New Ads
                _ad = ads.Retrieve(adID)
            End If
            Return _ad
        End Get

        Set(ByVal value As Ad)
            _ad = value
            '
            ' save instance ID in viewstate
            '
            ViewState.Add("AdID", Ad.hexID)
        End Set
    End Property


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        configureControl()
    End Sub


    Private Sub configureControl()

        BasicPic.ImageUrl = Ad.THBURL
        BasicKeyWords.Text = Ad.KeyWords
        BasicItemPrice.Text = Ad.ItemPrice
        BasicText.Text = Ad.Summary

    End Sub



End Class
