Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Impersonate another user so you can take ads on his behalf.
'*
'* ON ENTRY:
'*
'*  Loader: objectID = 
'*
'***************************************************************************************

Partial Class Impersonate
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
        Page.Theme = Slot.skin

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        displayLeftMenu()
    

        If Not IsPostBack Then
            displayUsrs()
        End If

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0
        leftmenu.Items.Clear()

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        leftmenu.Add("System Parameters", Loader.Target, False)

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
        leftmenu.Add("Impersonate User...", Loader.Target, True)

        '
        ' if impersonation is on, add in the other user's menu items
        '
        If Slot.UsrID <> Slot.ImpersonateUsrID Then
            Loader.ObjectID = Slot.ImpersonateUsrID
            '
            ' get the impersonated user's login level
            '
            Select Case Slot.ImpersonatedUsr.LoginLevel

                Case Usr.LoginLevels.AdvSub
                    Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                    leftmenu.Add("Create New Ad", Loader.Target, False)

                    Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                    leftmenu.Add("Manage My Profile", Loader.Target, False)

                    Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                    leftmenu.Add("Manage My Ads", Loader.Target, False)


                    Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                    leftmenu.Add("Manage My Subscription", Loader.Target, False)


                Case Usr.LoginLevels.Advertiser
                    Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                    leftmenu.Add("Create New Ad", Loader.Target, False)

                    Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                    leftmenu.Add("Manage My Profile", Loader.Target, False)

                    Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                    leftmenu.Add("Manage My Ads", Loader.Target, False)

                Case Usr.LoginLevels.Subscriber
                    Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                    leftmenu.Add("Manage My Subscription", Loader.Target, False)

            End Select
        End If

    End Sub

   

    Private Sub displayUsrs()
        '
        ' get the entire table for now. Should be just advertisers
        '
        Dim Usrs As New Usrs
        Usrs.Retrieve()
        '
        ' spin around users and set the CSS for the impersonation cell
        ' borrow the navtarget field for this
        '
        For Each Usr As Usr In Usrs
            Usr.NavTarget = "impersonateoff"
            If Usr.ID <> Slot.UsrID Then          'ie not myself!
                If Usr.ID = Slot.ImpersonateUsrID Then Usr.NavTarget = "impersonateon"
            End If
        Next

        usrlist.DataSource = Usrs
        usrlist.DataBind()


    End Sub

    Protected Sub Impersonate_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '
        ' recover user id from value and set into slot.
        '

        Dim clickCell As ATWebToolkit.ClickableCell = DirectCast(sender, ATWebToolkit.ClickableCell)
        '
        ' if impersonating someelse, or no-one set it to selected usr.
        ' If this is a click on the currently impersonated usr, set it off
        '
        Dim impUsrID As Integer = CommonRoutines.Hex2Int(clickCell.Value)
        If impUsrID = Slot.ImpersonateUsrID Then
            Slot.ImpersonateUsrID = Slot.UsrID
        Else
            Slot.ImpersonateUsrID = impUsrID
        End If

        Slot.Update()
        ''headerbar.UpdateMyAT()     'update header
        ''displayLeftMenu()             'redisplay menu
        ''displayUsrs()

        '
        ' go to home page
        '
        Loader.NextASPX = ATLib.Loader.ASPX.HomeINH
        Response.Redirect(Loader.Target)

    End Sub



End Class
