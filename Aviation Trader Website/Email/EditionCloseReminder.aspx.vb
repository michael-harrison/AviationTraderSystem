Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Edition Close Reminder email
'*
'*
'*  Loader: objectID = Ad ID
'*
'***************************************************************************************

Partial Class EditionCloseReminder
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


        Loader = New Loader(Request.QueryString(0))
        Loader.ApplicationPath = Sys.ExternalURL           'external url for email access

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)
        setMMALink()

    End Sub

    Private Sub setMMALink()
        '
        ' get a slot for the user and send him to mma
        '
        Dim usr As Usr = Ad.Usr
        Dim slot As New Slot
        slot.IPAddr = "0.0.0.0"
        slot.Login(usr.EmailAddr, usr.Password)
        Loader.SlotID = slot.ID     'no object, slot defines user
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        Loader.SelectedTab = 0        'saved ads
        MMALink.NavigateUrl = Loader.Target
    End Sub

End Class
