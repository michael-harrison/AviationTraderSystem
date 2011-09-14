Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Ad Review
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class Review
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Protected Ad As Ad


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.skin

        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)
        setButtons()


    End Sub

    Private Sub setButtons()


        Loader.NextASPX = ATLib.Loader.ASPX.AdTextEditor
        BtnContentEditor.NavigateURL = Loader.Target

    End Sub

   
    Private Sub submitAd()
        Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
        Ad.OriginalText = Ad.Text
        Ad.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.AdSubmitConfirmation
        Response.Redirect(Loader.Target)
    End Sub

    Private Sub saveAd()
        Ad.ProdnStatus = ATLib.Ad.ProdnState.Saved
        Ad.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.AdSaveConfirmation
        Response.Redirect(Loader.Target)
    End Sub

    Private Sub cancelAd()
        Ad.ProdnStatus = ATLib.Ad.ProdnState.Cancelled
        Ad.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.AdCancelConfirmation
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click
        submitad()
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        saveAd()
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        cancelAd()
    End Sub


End Class
