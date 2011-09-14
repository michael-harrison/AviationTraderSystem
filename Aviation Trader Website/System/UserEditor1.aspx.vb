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
'*
'***************************************************************************************

Partial Class UserEditor1
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
        Page.Theme = Slot.skin

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

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.Usr
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
        topnode = New MenuNode("A", "Web Visibility", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.UserEditor3
        topnode = New MenuNode("A", "My Details", Loader.Target, False)
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
        EmailBox.Text = Usr.EmailAddr
        Addr1Box.Text = Usr.Addr1
        Addr2Box.Text = Usr.Addr2
        SuburbBox.Text = Usr.Suburb
        Statebox.Text = Usr.State
        PostcodeBox.Text = Usr.Postcode
        CompanyBox.Text = Usr.Company
        ACNBox.Text = Usr.ACN
        PhoneBox.Text = Usr.Phone
        AHPhoneBox.Text = Usr.AHPhone
        MobileBox.Text = Usr.Mobile
        FaxBox.Text = Usr.Fax
        WebsiteBox.Text = Usr.WebSite
        CountryBox.Text = Usr.Countrycode
        '
        ' setup the checkboxes
        '
        emailcheck.Checked = Usr.IsDisplayEmail
        FNameCheck.Checked = Usr.IsDisplayFName
        LNameCheck.Checked = Usr.IsDisplayLName
        CompanyCheck.Checked = Usr.IsDisplayCompany
        PhoneCheck.Checked = Usr.IsDisplayPhone
        AHPhoneCheck.Checked = Usr.IsDisplayAHPhone
        MobileCheck.Checked = Usr.IsDisplayMobile
        FaxCheck.Checked = Usr.IsDisplayFax
        AddrCheck.Checked = Usr.IsDisplayAddr
        ACNCheck.Checked = Usr.IsDisplayACN
        WebsiteCheck.Checked = Usr.IsDisplayWebsite


    End Sub

    Private Sub updateUser()
        '
        ' update the checkboxes
        '
        Usr.IsDisplayEmail = emailcheck.Checked
        Usr.IsDisplayFName = FNameCheck.Checked
        Usr.IsDisplayLName = LNameCheck.Checked
        Usr.IsDisplayCompany = CompanyCheck.Checked
        Usr.IsDisplayPhone = PhoneCheck.Checked
        Usr.IsDisplayAHPhone = AHPhoneCheck.Checked
        Usr.IsDisplayMobile = MobileCheck.Checked
        Usr.IsDisplayFax = FaxCheck.Checked
        Usr.IsDisplayAddr = AddrCheck.Checked
        Usr.IsDisplayACN = ACNCheck.Checked
        Usr.IsDisplayWebsite = WebsiteCheck.Checked
        Usr.Update()
            ButtonBar.Msg = Constants.Saved
    
    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.UserListing
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : return2List()
            Case 2 : updateUser()
        End Select
    End Sub
End Class
