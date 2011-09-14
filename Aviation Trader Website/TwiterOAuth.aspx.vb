Imports TwitterVB2
Partial Class TwiterOAuth
    Inherits System.Web.UI.Page

    Public ConsumerKey As String = "Fg54Hd3ymdrN77esKUEgA"
    Public ConsumerKeySecret As String = "BZ5Kn8sIsTILSSD6kNdpol21pxQ9amKsFwBFDtLm2M"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim tw As New TwitterAPI
        If Not IsPostBack Then
            If Not Request("oauth_token") Is Nothing Then
                echoKeys()

            End If
        End If
    End Sub

    Private Sub echoKeys()
        Dim tw As New TwitterAPI
        ' Exchange the rquest token for an access token
        tw.GetAccessTokens(ConsumerKey, ConsumerKeySecret, Request("oauth_token"), Request("oauth_verifier"))

        '' You've got all the tokens now.  You can write them to a database, cookies, or whatever.
        Dim t As String = tw.OAuth_Token
        Dim ts As String = tw.OAuth_TokenSecret
        oAuthTokenbox.Text = t
        oAuthTokenSecretbox.Text = ts
    End Sub

    Protected Sub getkey_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles getkey.Click
        Dim tw As New TwitterAPI
        Dim x As String = tw.GetAuthorizationLink(ConsumerKey, ConsumerKeySecret, "http://localhost:1671/Aviation%20Trader%20Website/TwiterOAuth.aspx?zzzz")
        Response.Redirect(x)

    End Sub
End Class



