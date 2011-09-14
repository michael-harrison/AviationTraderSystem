Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Logout page
'*
'* 
'*
'*
'***************************************************************************************

Partial Class Logout
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.skin

        logout()                'logout here to get new guest slot

        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        displayContent()
    End Sub

    Private Sub logout()
        Slot.Logout()         'log this user out now
        Slot = New Slot       'and re-login as guest
        Dim X As System.Net.IPAddress = Net.IPAddress.Parse(Request.ServerVariables("REMOTE_ADDR"))
        Slot.IPAddr = X.ToString
        Slot.SessionID = Session.SessionID

        Slot.Login(Constants.GuestName, Constants.GuestPassword)
        Loader.SlotID = Slot.ID
    End Sub


    Private Sub displayContent()

        BackPicCaption.Text = sys.BackPicCaption
        BackPicImage.ImageUrl = sys.BackImageURL
        backpicimage.imagetype = RotatorAd.Types.Flash

    End Sub
End Class
