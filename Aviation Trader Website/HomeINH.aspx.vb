Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Home page - INH users
'*
'* 
'*
'*
'***************************************************************************************

Partial Class HomeINH
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Protected beltWidth As Integer

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

        Page.Theme = Slot.Skin
        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue + 1

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '
        ' only for testing
        '
        ''sys.PhysicalApplicationPath = "C:\EDrive\SourceVS2008\AT1.1\Aviation Trader Website"
        ''sys.PhysicalApplicationPath = "\\wftserver\EDrive\aviationtraderwebsite"
        ''sys.Update()


        displayLeftMenu()
        displayContent()

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

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
        leftmenu.Add("Impersonate User...", Loader.Target, False)

        '
        ' if impersonation is on, add in the other user's login level and set loader obj id to
        ' impersonated usr
        '
        If Slot.UsrID <> Slot.ImpersonateUsrID Then
            Loader.ObjectID = Slot.ImpersonateUsrID

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

                    Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                    leftmenu.Add("Manage My Profile", Loader.Target, False)

                    Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                    leftmenu.Add("Manage My Subscription", Loader.Target, False)

                Case Usr.LoginLevels.RegisteredReader
                    Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                    leftmenu.Add("Manage My Profile", Loader.Target, False)

            End Select

        End If




    End Sub

    Private Sub displayContent()
        twitlink.NavigateUrl = "http://www.twitter.com/" & sys.TwitUserName
        Loader.NextASPX = ATLib.Loader.ASPX.Slideshow
        slideshowlink.NavigateUrl = Loader.Target
    End Sub


End Class
