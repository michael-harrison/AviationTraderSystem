Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols


'***************************************************************************************
'*
'* Ad Portal
'*
'* AUDIT TRAIL
'* 
'* V1.000   02-AUG-2009  BA  Original
'*
'*
'***************************************************************************************

Partial Class AdManager
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

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        displayLeftMenu()
        displaytabBar()
        displayContent()

    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0
        Loader.ObjectID = Slot.ImpersonateUsrID
        '
        ' if impersonation is on, add in the other user's login level
        '
        Dim myloginlevel As Usr.LoginLevels = Slot.LoginLevel
        If Slot.UsrID <> Slot.ImpersonateUsrID Then myloginlevel = Slot.ImpersonatedUsr.LoginLevel

        Select Case myloginlevel

            Case Usr.LoginLevels.AdvSub
                Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                leftmenu.Add("Create New Ad", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                leftmenu.Add("Manage My Ads", Loader.Target, True)


                Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                leftmenu.Add("Manage My Subscription", Loader.Target, False)

            Case Usr.LoginLevels.Advertiser
                Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                leftmenu.Add("Create New Ad", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                leftmenu.Add("Manage My Ads", Loader.Target, True)


            Case Usr.LoginLevels.Subscriber
                Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                leftmenu.Add("Manage My Subscription", Loader.Target, False)
        End Select



    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

         Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        Loader.SelectedTab = 0
        topnode = New MenuNode("A", "My Saved Ads", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 1
        topnode = New MenuNode("A", "Submitted Ads", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 2
        topnode = New MenuNode("F", "Ads Waiting for Approval", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 3
        topnode = New MenuNode("F", "My Approved Ads", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 4
        topnode = New MenuNode("F", "My Archived Ads", Loader.Target, False)
        tabbar.Nodes.Add(topnode)
     

    End Sub


    Private Sub displayContent()
  

    End Sub

End Class
