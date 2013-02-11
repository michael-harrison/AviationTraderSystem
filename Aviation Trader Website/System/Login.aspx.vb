Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Login page
'*
'* AUDIT TRAIL
'* 
'* V1.000   02-AUG-2009  BA  Original
'*
'*
'***************************************************************************************

Partial Class Login
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
 
        sys = New ATSystem
        sys.Retrieve()
        
		'
		' firs time thru - no query string. Login as guest
		'
		If Request.QueryString.Count = 0 Then
			Slot = New Slot
			Dim X As System.Net.IPAddress = Net.IPAddress.Parse(Request.ServerVariables("REMOTE_ADDR"))
			Slot.IPAddr = X.ToString
			Slot.SessionID = Session.SessionID

			Slot.Login(Constants.GuestName, Constants.GuestPassword)
			Loader = New Loader
			Loader.SlotID = Slot.ID
		Else
			Loader = New Loader(Request.QueryString(0))
			Dim slots As New Slots
			Slot = slots.Retrieve(Loader.SlotID)

		End If

        Page.Theme = Slot.skin
        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        displayLeftMenu()
        displaycontent()
        setDefaultButton("btnLogin")

    End Sub

    Private Sub setDefaultButton(ByVal btnName As String)
        form1.Attributes.Add("onkeypress", "javascript:return FireDefaultButton(event,'" & btnName & "')")
    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.Login
        leftmenu.Add("Sign In", Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.Register
        Loader.Param1 = Usr.LoginLevels.Advertiser
        leftmenu.Add("Register as new user", Loader.Target, False)

    End Sub


    Private Sub displayContent()

        ''EmailBox.Text = "brian@wavefront.com.au"
        ''PWBox.Text = "aaaaaa"
        Loader.NextASPX = ATLib.Loader.ASPX.ForgotPW
        forgotpw.Attributes.Add("onclick", "popup('" & Loader.Target & "','a')")

    End Sub

    Private Sub login()

        Dim emailAddr As String = ""
        Dim pw As String = ""

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength


        emailAddr = IV.ValidateEmail(EmailBox, emailerror)
        pw = IV.ValidateText(PWBox, PWError)
        If IV.ErrorCount = 0 Then
            '
            ' tries the login with a new slot
            '
            Slot = New Slot
            Dim X As System.Net.IPAddress = Net.IPAddress.Parse(Request.ServerVariables("REMOTE_ADDR"))
            Slot.IPAddr = X.ToString
            Slot.SessionID = Session.SessionID
            Dim slotstate As Slot.LoginStates = Slot.Login(emailAddr, pw)

            Select Case slotstate
                Case ATLib.Slot.LoginStates.InvalidUserName
                    emailerror.Text = Constants.UnknownUsr
                    emailerror.Visible = True

                Case ATLib.Slot.LoginStates.InvalidPassword
                    PWError.Text = Constants.InvalidPassword
                    PWError.Visible = True


                Case Else
                    '
                    ' go to the correct home page as a function of login level
                    '
                    Select Case Slot.LoginLevel
                        Case Usr.LoginLevels.INHProdn, Usr.LoginLevels.SysAdmin : Loader.NextASPX = ATLib.Loader.ASPX.HomeINH
                        Case Usr.LoginLevels.Guest : Loader.NextASPX = ATLib.Loader.ASPX.Login
                        Case Else : Loader.NextASPX = ATLib.Loader.ASPX.HomeRegistered
                    End Select

                    Loader.SlotID = Slot.ID               'change slot to registered user
                    Response.Redirect(Loader.Target)

            End Select
        End If

    End Sub

    Protected Sub BtnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLogin.Click
        login()
    End Sub

End Class
