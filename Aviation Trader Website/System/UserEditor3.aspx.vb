Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* User Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = user ID
'*          selectedTab = selected tab from listing
'*
'***************************************************************************************

Partial Class UserEditor3
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Usr As Usr


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


        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim usrs As New Usrs
        Usr = usrs.Retrieve(Loader.ObjectID)
        displayNavbar()
        displaytabBar()
        displayLeftMenu()
        displayButtonBar()

        If Not IsPostBack Then
            displayUser()
        End If

    End Sub

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.Usr
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub

    Private Sub displayButtonBar()
        If Slot.LoginLevel = ATLib.Usr.LoginLevels.SysAdmin Then
            ButtonBar.B1.Text = "Return to list"
        End If
        ButtonBar.B2.Text = "Save Changes"
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Dim selectedTab As Integer = Loader.SelectedTab           'save current tab
        Loader.SelectedTab = 0

        Select Case Slot.LoginLevel
            Case ATLib.Usr.LoginLevels.SysAdmin
                If (Slot.ImpersonateUsrID <> Slot.UsrID) And (Slot.ImpersonateUsrID = Loader.ObjectID) Then
                    Loader.ObjectID = Slot.ImpersonateUsrID
                    '
                    ' get the login level of the impersonated user
                    '
                    Select Case Slot.ImpersonatedUsr.LoginLevel
                        Case ATLib.Usr.LoginLevels.AdvSub
                            Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                            leftmenu.Add("Create New Ad", Loader.Target, False)

                            Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                            leftmenu.Add("Manage My Profile", Loader.Target, True)

                            Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                            leftmenu.Add("Manage My Ads", Loader.Target, False)


                            Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                            leftmenu.Add("Manage My Subscription", Loader.Target, False)

                        Case ATLib.Usr.LoginLevels.Advertiser
                            Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                            leftmenu.Add("Create New Ad", Loader.Target, False)

                            Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                            leftmenu.Add("Manage My Profile", Loader.Target, True)

                            Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                            leftmenu.Add("Manage My Ads", Loader.Target, False)


                        Case ATLib.Usr.LoginLevels.Subscriber
                            Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                            leftmenu.Add("Manage My Subscription", Loader.Target, False)
                    End Select

                Else
                    Loader.NextASPX = ATLib.Loader.ASPX.UserListing
                    leftmenu.Add("Users", Loader.Target, False)

                    Loader.ObjectID = Slot.UsrID
                    Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                    leftmenu.Add(Usr.FullName, Loader.Target, True)
                End If

            Case ATLib.Usr.LoginLevels.Advertiser
                Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                leftmenu.Add("Create New Ad", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, True)

                Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                leftmenu.Add("Manage My Ads", Loader.Target, False)


            Case ATLib.Usr.LoginLevels.AdvSub
                Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                leftmenu.Add("Create New Ad", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, True)

                Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                leftmenu.Add("Manage My Ads", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                leftmenu.Add("Manage My Subscription", Loader.Target, False)

            Case ATLib.Usr.LoginLevels.Subscriber
                Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                leftmenu.Add("Manage My Subscription", Loader.Target, False)

            Case Usr.LoginLevels.RegisteredReader
                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, True)
        End Select

        Loader.SelectedTab = selectedTab

    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
        topnode = New MenuNode("A", "Web Visibility", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.UserEditor3
        topnode = New MenuNode("A", "My Details", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        If Slot.LoginLevel = ATLib.Usr.LoginLevels.SysAdmin Then
            Loader.NextASPX = ATLib.Loader.ASPX.UserEditor2
            topnode = New MenuNode("C", "Admin", Loader.Target, False)
            tabbar.Nodes.Add(topnode)
        End If

    End Sub

    Private Sub displayUser()

        FNameBox.Text = Usr.FName
        LNameBox.Text = Usr.LName
        CompanyBox.Text = Usr.Company
        EmailBox.Text = Usr.EmailAddr
        PWBox.Text = Usr.Password
        Addr1Box.Text = Usr.Addr1
        Addr2Box.Text = Usr.Addr2
        SuburbBox.Text = Usr.Suburb
        StateBox.Text = Usr.State
        PostcodeBox.Text = Usr.Postcode
        ACNBox.Text = Usr.ACN
        PhoneBox.Text = Usr.Phone
        AHPhoneBox.Text = Usr.AHPhone
        MobileBox.Text = Usr.Mobile
        FaxBox.Text = Usr.Fax
        WebsiteBox.Text = Usr.WebSite
        '
        ' setup user type drop down 
        '
        Dim EA As EnumAssistant
        EA = New EnumAssistant(New Usr.LoginLevels, Usr.LoginLevels.RegisteredReader, Usr.LoginLevels.AdvSub)
        UserTypeDD.DataSource = EA
        UserTypeDD.DataBind()
        UserTypeDD.SelectedValue = Convert.ToString(Usr.LoginLevel)
        '
        ' setup country drop down
        '
        EA = New EnumAssistant(New ATSystem.countrycodes)
        CountryDD.DataSource = EA
        CountryDD.DataBind()
        CountryDD.SelectedValue = Usr.Countrycode


    End Sub

    




    Private Sub UpdateUser()
        '
        ' create a new user record and log the user in
        '
        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        usr.Password = IV.ValidateText(PWBox, PWError)
        usr.FName = IV.ValidateText(FNameBox, FNameError)
        usr.LName = IV.ValidateText(LNameBox, LNameError)
        usr.Phone = IV.ValidateText(PhoneBox, PhoneError)
        usr.Addr1 = IV.ValidateText(Addr1Box, Addr1Error)
        usr.Suburb = IV.ValidateText(SuburbBox, SuburbError)
        usr.Postcode = IV.ValidateText(PostcodeBox, PostcodeError)
        usr.State = IV.ValidateText(StateBox, StateError)
        '
        ' remaining fields are optional
        '
        IV.MinStringLength = 0

        usr.WebSite = IV.ValidateURL(WebsiteBox, WebsiteError)
        usr.Company = IV.ValidateText(CompanyBox, CompanyError)
        usr.ACN = IV.ValidateText(ACNBox, ACNError)
        usr.Addr2 = IV.ValidateText(Addr2Box, Addr2Error)
        usr.Mobile = IV.ValidateText(MobileBox, MobileError)
        usr.AHPhone = IV.ValidateText(AHPhoneBox, AHPhoneError)
        usr.Fax = IV.ValidateText(faxBox, FaxError)


        usr.Countrycode = CountryDD.SelectedValue
        usr.LoginLevel = CType(UserTypeDD.SelectedValue, Usr.LoginLevels)
        
        '
        ' missing validation stuff here
        '
        If IV.ErrorCount = 0 Then
            usr.Update() 'update user
            ButtonBar.Msg = Constants.Saved

        End If
      
    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.UserListing
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : return2List()
            Case 2 : UpdateUser()
        End Select
    End Sub
End Class
