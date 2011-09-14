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

Partial Class UserEditor2
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

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.Usr
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub

  
    Private Sub displayButtonBar()
        ButtonBar.B0.Text = "Return to list"
        ButtonBar.B1.Text = "Delete user"
        ButtonBar.B2.Text = "Save Changes"

        Loader.NextASPX = ATLib.Loader.ASPX.SkinTest
        BtnSkinTest.NavigateURL = Loader.Target

    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Dim selectedTab As Integer = Loader.SelectedTab           'save current tab
        Loader.SelectedTab = 0

        If (Slot.ImpersonateUsrID <> Slot.UsrID) And (Slot.ImpersonateUsrID = Loader.ObjectID) Then
            Loader.ObjectID = Slot.ImpersonateUsrID
            Loader.NextASPX = ATLib.Loader.ASPX.NewAd
            leftmenu.Add("Create New Ad", Loader.Target, False)

            Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
            leftmenu.Add("Manage My Profile", Loader.Target, True)

            Loader.NextASPX = ATLib.Loader.ASPX.AdManager
            leftmenu.Add("Manage My Ads", Loader.Target, False)


        Else
            Loader.NextASPX = ATLib.Loader.ASPX.UserListing
            leftmenu.Add("Users", Loader.Target, False)

            Loader.ObjectID = Slot.UsrID
            Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
            leftmenu.Add(Usr.FullName, Loader.Target, True)
        End If

        Loader.SelectedTab = selectedTab                  'restore current tab

    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
        topnode = New MenuNode("A", "Web Visibility", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

Loader.NextASPX = ATLib.Loader.ASPX.UserEditor3
        topnode = New MenuNode("A", "My Details", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.UserEditor2
        topnode = New MenuNode("C", "Admin", Loader.Target, True)
        tabbar.Nodes.Add(topnode)


    End Sub

    Private Sub displayUser()

        Dim EA As EnumAssistant

        EA = New EnumAssistant(New Usr.LoginLevels)
        LoginDD.DataSource = EA
        LoginDD.DataBind()
        LoginDD.SelectedValue = Convert.ToString(Usr.LoginLevel)

        EA = New EnumAssistant(New Edition.VisibleState)
        EditionVisibilityDD.DataSource = EA
        EditionVisibilityDD.DataBind()
        EditionVisibilityDD.SelectedValue = Convert.ToString(Usr.EditionVisibility)

        EA = New EnumAssistant(New ATSystem.Skins)
        SkinDD.DataSource = EA
        SkinDD.DataBind()
        SkinDD.SelectedValue = Usr.Skin

        EmailBox.Text = Usr.EmailAddr
        PWBox.Text = Usr.Password

        UsrDD.DataSource = sys.Usrs
        UsrDD.DataBind()
        UsrDD.SelectedValue = Usr.hexID

        AcctAliasBox.Text = Usr.AcctAlias
        GSTCheck.Checked = Usr.IsGSTExempt

        DiscountBox.Text = Usr.Discount.ToString

    End Sub

    Private Sub updateUser()

        Dim newUsrID As Integer

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        Dim newEmailAddr As String = IV.ValidateEmail(EmailBox, emailError)
        Usr.Password = IV.ValidateText(PWBox, PWError)

        IV.MinValue = 0
        IV.MaxValue = 100
        Usr.Discount = IV.ValidateInteger(DiscountBox, DiscountError)

        Usr.AcctAlias = IV.ValidateText(AcctAliasBox, AcctAliasError)
        Usr.LoginLevel = CType(LoginDD.SelectedValue, Usr.LoginLevels)
        Usr.EditionVisibility = CType(EditionVisibilityDD.SelectedValue, Edition.VisibleState)
        Usr.Skin = SkinDD.SelectedValue

        Usr.IsGSTExempt = GSTCheck.Checked

        If IV.ErrorCount = 0 Then
            '
            ' check that the email address does not already exist (if it has beenchanged)
            '
            If Usr.EmailAddr <> newEmailAddr Then
                Dim usrs As New Usrs
                usrs.RetrieveByEmailAddr(newEmailAddr)
                If usrs.Count > 0 Then
                    emailError.Visible = True
                    emailError.Text = Constants.DuplicateUsr
                Else
                    Usr.EmailAddr = newEmailAddr
                    '
                    ' move users ads if requested
                    '
                    newUsrID = CommonRoutines.Hex2Int(UsrDD.SelectedValue)
                    If newUsrID <> Usr.ID Then
                        For Each Ad As Ad In Usr.Ads
                            Ad.UsrID = newUsrID
                            Ad.Update()
                        Next
                    End If
                    Usr.Update()
                    ButtonBar.Msg = Constants.Saved
                End If
            Else
                '
                ' move users ads if requested
                '
                newUsrID = CommonRoutines.Hex2Int(UsrDD.SelectedValue)
                If newUsrID <> Usr.ID Then
                    For Each Ad As Ad In Usr.Ads
                        Ad.UsrID = newUsrID
                        Ad.Update()
                    Next
                End If
                Usr.Update()
                ButtonBar.Msg = Constants.Saved
            End If
        End If
    End Sub

    Private Sub deleteAds()
        '
        ' deletes all the ads for a user
        '
        Usr.DeleteAds()
        ButtonBar.Msg = Constants.Saved

    End Sub

    Private Sub deleteUser()
        If Usr.ID = Slot.UsrID Then
            ButtonBar.Msg = Constants.noselfdelete
        Else
            Try
                Usr.Deleted = True
                Usr.Update()
                Loader.NextASPX = ATLib.Loader.ASPX.UserListing
                Response.Redirect(Loader.Target)
            Catch ex As Exception
                ButtonBar.Msg = Constants.NoUserDelete
            End Try
        End If
    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.UserListing
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0 : return2list()
            Case 1 : deleteUser()
            Case 2 : updateUser()
        End Select
    End Sub
End Class
