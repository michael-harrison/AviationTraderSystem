Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Regiseter as new user
'*
'* ON ENTRY:
'*
'*  Loader: param1 = requested login level
'*
'*
'***************************************************************************************

Partial Class Register
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Loader = New Loader(Request.QueryString(0))
        Dim slots As New Slots
        Slot = slots.Retrieve(Loader.SlotID)

        Page.Theme = Slot.skin
        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        displayLeftMenu()
        If Not IsPostBack Then displayContent()
        clearAllErrors()

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.Register
        leftmenu.Add("Register as new user", Loader.Target, True)

    End Sub

    Private Sub displayContent()
        '
        ' setup user type drop down 
        '
        Dim EA As EnumAssistant
        EA = New EnumAssistant(New Usr.LoginLevels, Usr.LoginLevels.RegisteredReader, Usr.LoginLevels.AdvSub)
        UserTypeDD.DataSource = EA
        UserTypeDD.DataBind()
        UserTypeDD.SelectedValue = Loader.Param1.ToString

        '
        ' setup country drop down - default to AU
        '
        EA = New EnumAssistant(New ATSystem.countrycodes)
        CountryDD.DataSource = EA
        CountryDD.DataBind()
        CountryDD.SelectedValue = "AU"


        ''EmailBox.Text = "brian@wavefront.com.au"
        ''PWBox.Text = "aaaaaa"

    End Sub

    Private Sub clearAllErrors()
        '
        ' resets the vis of all error messages
        '
        emailError.Visible = False
        PWError.Visible = False

    End Sub

  


    Private Sub registerNewUser()
        '
        ' create a new user record and log the user in
        '
        Dim usr As New Usr
        usr.EditionVisibility = Edition.VisibleState.Active

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength


        usr.EmailAddr = IV.ValidateEmail(EmailBox, emailerror)
        usr.Password = IV.ValidateText(PWBox, PWError)
        usr.FName = IV.ValidateText(FNameBox, FNameError)
        usr.LName = IV.ValidateText(LNameBox, LNameError)
        usr.Phone = IV.ValidateText(PhoneBox, PhoneError)
        usr.Addr1 = IV.ValidateText(Addr1Box, Addr1Error)
        usr.Suburb = IV.ValidateText(SuburbBox, SuburbError)
        usr.Postcode = IV.ValidateText(PostcodeBox, PostcodeError)
        usr.State = IV.ValidateText(StateBox, StateError)
        usr.Discount = 0

        
 
        Dim EA As New EnumAssistant(New ATSystem.Skins, ATSystem.Skins.ATSkin, ATSystem.Skins.ATSkin)
        usr.Skin = EA(0).Description
        '
        ' remaining fields are optional
        '
        IV.MinStringLength = 0

        usr.WebSite = IV.ValidateURL(WebsiteBox, WebsiteError)
        usr.Company = IV.ValidateText(CompanyBox, CompanyError)
        usr.ACN = IV.ValidateText(ACNBox, ACNError)
        usr.Addr2 = IV.ValidateText(Addr2Box, Addr2Error)
       

        usr.Countrycode = CountryDD.SelectedValue
        usr.LoginLevel = CType(UserTypeDD.SelectedValue, Usr.LoginLevels)
        '
        ' remaining fields not required at this point
        '
        usr.AHPhone = ""
        usr.Fax = ""
        usr.AcctAlias = ""
        usr.Mobile = ""

        usr.SystemID = sys.ID
        usr.UAM = 0
        usr.IdentVisible = 0      'nothing visible to end users yet
        '
        ' missing validation stuff here
        '
        If IV.ErrorCount = 0 Then
            '
            ' check that the email address does not already exist
            '
            Dim usrs As New Usrs
            usrs.RetrieveByEmailAddr(usr.EmailAddr)
            If usrs.Count > 0 Then
                emailerror.Visible = True
                emailerror.Text = Constants.DuplicateUsr
            Else
                usr.Update()        'create new usr
                SendEmail(usr)      'send welcome email
                login(usr)          'and log in
            End If
        End If
    End Sub

    Private Sub login(ByVal Usr As Usr)
        '
        ' tries the login with a new slot
        '
        Slot = New Slot
        Dim X As System.Net.IPAddress = Net.IPAddress.Parse(Request.ServerVariables("REMOTE_ADDR"))
        Slot.IPAddr = X.ToString
        Slot.SessionID = Session.SessionID
        Dim slotstate As Slot.LoginStates = Slot.Login(Usr.EmailAddr, Usr.Password)

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


    End Sub

    Private Sub SendEmail(ByVal usr As Usr)

        Dim EA As New EmailAssistant(sys)
        EA.SendRegistrationWelcome(usr)

    End Sub



    Protected Sub BTNRegister2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNRegister.Click

        registerNewuser()

    End Sub
End Class
