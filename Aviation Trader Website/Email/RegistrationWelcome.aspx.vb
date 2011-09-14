Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Registration Welcome email
'*
'*
'*  Loader: objectID = Usr ID
'*
'***************************************************************************************

Partial Class RegistrationWelcome
    Inherits System.Web.UI.Page
    Protected Sys As ATSystem
    Private Loader As Loader
    Protected Usr As Usr

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Sys = New ATSystem
        Sys.Retrieve()
        '
        ' there is no slot for emails
        '

        ''Dim qs As String = "15008000000000000000150000000000"
        ''Loader = New Loader(qs)

        Loader = New Loader(Request.QueryString(0))
        Loader.ApplicationPath = Sys.ExternalURL           'external url for email access

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim usrs As New Usrs
        Usr = usrs.Retrieve(Loader.ObjectID)
        displayContent()

    End Sub

    Private Sub displayContent()
        ATlogo.ImageUrl = Sys.ExternalURL & "/Graphics/AVT_web_tagline.png"
    End Sub


End Class
