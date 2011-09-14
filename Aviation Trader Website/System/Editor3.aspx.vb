Option Strict On
Option Explicit On
Imports ATLib
Imports TwitterVB2

'***************************************************************************************
'*
'* Edit System Parameters 3
'*
'* ON ENTRY:
'*
'*  Loader: objectID = undefined
'*
'***************************************************************************************

Partial Class Editor3
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

        Page.Theme = Slot.Skin

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

 

        displaytabBar()
        displayLeftMenu()
        displayButtonBar()

        If Not IsPostBack Then
            displaySystem()

            If Not Request("oauth_token") Is Nothing Then
                saveOAuthKeys()
                displaySystem()
            End If
        End If


    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Get OAuth Tokens"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        leftmenu.Add("System Parameters", Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.Technotes
        leftmenu.Add("Technotes", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.FolderListing
        Loader.ObjectID = sys.FirstFolderID
        leftmenu.Add("Folders", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.PublicationListing
        leftmenu.Add("Publications", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.CategoryListing
        leftmenu.Add("Categories", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.RotatorListing
        leftmenu.Add("Rotator Ads", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.NewsListing
        leftmenu.Add("News", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.UserListing
        leftmenu.Add("Users", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        leftmenu.Add("Proof Reader", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.UserImpersonate
        leftmenu.Add("Impersonate User...", Loader.Target, False)


    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        topnode = New MenuNode("A", "Website", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor2
        topnode = New MenuNode("B", "Email", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor3
        topnode = New MenuNode("B", "Twitter", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor4
        topnode = New MenuNode("D", "Production", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

    End Sub

    Private Sub displaySystem()

        TwitUserNameBox.Text = sys.TwitUserName
        TwitConsumerKeyBox.Text = sys.TwitConsumerKey
        TwitConsumerKeySecretBox.Text = sys.TwitConsumerKeySecret
        TwitOAuthTokenBox.Text = sys.TwitOAuthToken
        TwitOAuthTokenSecretBox.Text = sys.TwitOAuthTokenSecret

    End Sub

    Private Sub saveOAuthKeys()
        '
        ' Exchange the rquest token for an access token
        '
        Try

     
            Dim tw As New TwitterAPI
            tw.GetAccessTokens(sys.TwitConsumerKey, sys.TwitConsumerKeySecret, Request("oauth_token"), Request("oauth_verifier"))
            '
            ' write returned tokens to db
            '
            sys.TwitOAuthToken = tw.OAuth_Token
            sys.TwitOAuthTokenSecret = tw.OAuth_TokenSecret
            sys.Update()
            ButtonBar.Msg = Constants.oAuthSuccess
        Catch ex As Exception
            ButtonBar.Msg = ex.InnerException.Message
        End Try
    End Sub

    Private Sub getTwitterOauthTokens()
        Dim tw As New TwitterAPI
        Try
            Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor3
            Dim callbackURL As String = sys.ExternalURL & Loader.Target
            callbackURL = Server.UrlPathEncode(callbackURL)
            Dim x As String = tw.GetAuthorizationLink(sys.TwitConsumerKey, sys.TwitConsumerKeySecret, callbackURL)
            Response.Redirect(x)
        Catch ex As Exception
            ButtonBar.Msg = ex.InnerException.Message
        End Try

    End Sub

    Private Sub updateconsumerkeys()
        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        sys.TwitUserName = IV.ValidateText(TwitUserNameBox, TwitUserNameError)
        sys.TwitConsumerKey = IV.ValidateText(TwitConsumerKeyBox, TwitConsumerKeyError)
        sys.TwitConsumerKeySecret = IV.ValidateText(TwitConsumerKeySecretBox, TwitConsumerKeySecretError)
        '
        ' consumer keys updated means new oauth tokens are needed.
        '
        sys.TwitOAuthToken = ""
        sys.TwitOAuthTokenSecret = ""
        If IV.ErrorCount = 0 Then
            sys.Update()
            ButtonBar.Msg = Constants.Saved
        End If
    End Sub

  


    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : getTwitterOauthTokens()
            Case 2 : updateConsumerKeys()
        End Select
    End Sub

End Class
