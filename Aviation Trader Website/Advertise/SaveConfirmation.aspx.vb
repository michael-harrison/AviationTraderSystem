Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Ad Save Confirmation
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class SaveConfirmation
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

    

    End Sub


End Class
