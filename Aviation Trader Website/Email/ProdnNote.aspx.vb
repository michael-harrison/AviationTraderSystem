Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Prodn Note email
'*
'*
'*  Loader: objectID = Ad ID
'*
'***************************************************************************************

Partial Class ProdnNote
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

    End Sub

End Class
