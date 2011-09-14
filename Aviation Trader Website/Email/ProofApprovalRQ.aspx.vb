Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Proof Approval Request
'*
'*
'*  Loader: objectID = Ad number
'*
'***************************************************************************************

Partial Class ProofApprovalRQ
    Inherits System.Web.UI.Page
    Protected Sys As ATSystem
    Private Loader As Loader
    Protected Ad As Ad

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()
        '
        ' there is no slot for emails
        '


        Loader = New Loader(Request.QueryString(0))

        ''Loader = New Loader("12008000000000000000E20000000000")
        Loader.ApplicationPath = Sys.ExternalURL           'external url for email access

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)
        displayContent()

    End Sub

    Private Sub displayContent()
        '
        ' create a slot that the target user will login as
        '
        Dim usrSlot As New Slot
        usrSlot.IPAddr = "0.0.0.0"                  'fake out the user slot ip - this wont be correct"
        usrSlot.Login(Ad.Usr.EmailAddr, Ad.Usr.Password)
        Loader.SlotID = usrSlot.ID
        Loader.NextASPX = ATLib.Loader.ASPX.MMAPreview
        GreetingLabel.Text = "Hello " & Ad.Usr.FullName
        AdDescrLabel.Text = "A proof of your booking: <b>" & Ad.KeyWords & " </b>is now available for your approval."
        prooflink.NavigateUrl = Loader.Target
    End Sub

End Class
