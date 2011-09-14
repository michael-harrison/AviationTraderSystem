﻿Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Advertiser home page
'*
'* 
'*
'*
'***************************************************************************************

Partial Class AdHome
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
        displayButtonBar()
        displaytabBar()
        displayContent()

    End Sub

    Private Sub displayButtonBar()

        Loader.Param1 = Usr.LoginLevels.Advertiser
        Loader.NextASPX = ATLib.Loader.ASPX.Register
        btnRegister.NavigateURL = Loader.Target
    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '

        Loader.NextASPX = ATLib.Loader.ASPX.AboutHome
        leftmenu.Add("About Us", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.Weboffer
        leftmenu.Add("Our Web Product", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.AdHome
        leftmenu.Add("Advertise", Loader.Target, True)

        Loader.NextASPX = ATLib.Loader.ASPX.subsHome
        leftmenu.Add("Subscribe", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.ContactHome
        leftmenu.Add("Contact Us", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.FAQ
        leftmenu.Add("FAQ's", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.Testimonials
        leftmenu.Add("Testimonials", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.Deadlines
        leftmenu.Add("Deadlines", Loader.Target, False)



    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.AdHome
        topnode = New MenuNode("A", "Welcome", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.AdOptions
        topnode = New MenuNode("B", "Advertising options", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.AdRates
        topnode = New MenuNode("C", "Rates", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.AdSpecs
        topnode = New MenuNode("D", "Ad Specs", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

    End Sub

    Private Sub displayContent()


    End Sub

End Class
