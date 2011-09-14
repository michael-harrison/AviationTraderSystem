Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* AdList
'*
'* ON ENTRY:
'*
'*  Loader: objectID = undefined
'*          selectedTab = tab number to display
'*
'***************************************************************************************

Partial Class Advertise_MyAds
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Status As Ad.ProdnState
    Protected ItemCount As Integer
    Private selectedTab As Integer


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Slot.SearchMode = ATLib.Slot.SearchModes.Manage     'put the slot to manage mode
        Slot.Update()

        Page.Theme = Slot.Skin
        selectedTab = Loader.SelectedTab          'save selected tab

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '
        ' map tab to prodn status
        '
        Status = mapTab2Status(Loader.SelectedTab)

        displaytabBar()
        displayLeftMenu()

        displayAds(Status)


    End Sub

    Private Function mapTab2Status(ByVal selectedTab As Integer) As Ad.ProdnState

        Select Case selectedTab
            Case 0 : Return Ad.ProdnState.Saved
            Case 1 : Return Ad.ProdnState.Submitted
            Case 2 : Return Ad.ProdnState.Proofed
            Case 3 : Return Ad.ProdnState.Approved
            Case 4 : Return Ad.ProdnState.Archived
        End Select

    End Function

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

        '
        ' save current tab number
        '
        Dim selectedTab As Integer = Loader.SelectedTab

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
        '
        ' put the selected tab down
        '
        tabbar.Nodes(selectedTab).Selected = True
        Loader.SelectedTab = selectedTab          'restore current tab number

    End Sub


    Private Sub displayAds(ByVal Status As Ad.ProdnState)

        Dim ads As New Ads
        ads.RetrieveSet(ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, Slot.ImpersonateUsrID, ATSystem.SysConstants.nullValue, Status, Ad.SortOrders.Keywords)

        ItemCount = ads.Count
        adList.DataSource = ads
        adList.DataBind()


        Dim i As Integer = 0
        For Each r As RepeaterItem In adList.Items

            Loader.ObjectID = ads(i).ID
            Loader.SelectedTab = selectedTab
            '
            ' set button visibility as a function of ad status
            '

            Select Case ads(i).ProdnStatus

                Case Ad.ProdnState.Saved
                    enableBtn(r, "btnRead", ATLib.Loader.ASPX.TextReader)
                    enableBtn(r, "btnEditAd", ATLib.Loader.ASPX.AdProductSelector)
                    enableBtn(r, "btnSubmitAd", ATLib.Loader.ASPX.SubmitAd)
                    enableBtn(r, "btnDeleteAd", ATLib.Loader.ASPX.MyAds)

                Case Ad.ProdnState.Submitted
                    enableBtn(r, "btnRead", ATLib.Loader.ASPX.TextReader)
                    enableBtn(r, "btnContactProdn", ATLib.Loader.ASPX.MMAContactProdn)
                    enableBtn(r, "btnCopyAd", ATLib.Loader.ASPX.CopyAd)

                Case Ad.ProdnState.Proofed
                    enableBtn(r, "btnRead", ATLib.Loader.ASPX.TextReader)
                    enableBtn(r, "btnPreview", ATLib.Loader.ASPX.MMAPreview)
 
                Case Ad.ProdnState.Approved
                    enableBtn(r, "btnRead", ATLib.Loader.ASPX.TextReader)
                    enableBtn(r, "btnPreview", ATLib.Loader.ASPX.MMAPreview)
                    enableBtn(r, "btnContactProdn", ATLib.Loader.ASPX.MMAContactProdn)
                    enableBtn(r, "btnCopyAd", ATLib.Loader.ASPX.CopyAd)
                    enableBtn(r, "btnRepeatAd", ATLib.Loader.ASPX.RepeatAd)

                Case Ad.ProdnState.Archived
                    enableBtn(r, "btnRead", ATLib.Loader.ASPX.TextReader)
                    enableBtn(r, "btnPreview", ATLib.Loader.ASPX.MMAPreview)
                    enableBtn(r, "btnCopyAd", ATLib.Loader.ASPX.CopyAd)
                    enableBtn(r, "btnRepeatAd", ATLib.Loader.ASPX.RepeatAd)
   
            End Select
            i += 1

        Next

    End Sub

    Private Sub enableBtn(ByVal r As RepeaterItem, ByVal btnName As String, ByVal target As Loader.ASPX)
        Dim btn As ATWebToolkit.VW2Btn = CType(r.FindControl(btnName), ATWebToolkit.VW2Btn)
        Loader.NextASPX = target
        btn.NavigateURL = Loader.Target
        btn.Visible = True
    End Sub

    Private Function getAd(ByVal sender As Object) As Ad
        '
        ' find ad via repeater
        '
        Dim btn As ATWebToolkit.VW2Btn = CType(sender, ATWebToolkit.VW2Btn)
        Dim r As RepeaterItem = CType(btn.Parent, RepeaterItem)
        Dim adreader As ATControls_AdReader = CType(r.FindControl("adreader"), ATControls_AdReader)
        Return adreader.Ad

    End Function

    Private Sub echoItemMessage(ByVal sender As Object, ByVal msg As String)
        '
        ' finds the error msg control in the current repeater item and plugs it
        '
        Dim btn As ATWebToolkit.VW2Btn = CType(sender, ATWebToolkit.VW2Btn)
        Dim r As RepeaterItem = CType(btn.Parent, RepeaterItem)
        Dim errormsg As Label = CType(r.FindControl("errormsg"), Label)
        errormsg.Text = msg
    End Sub

    Protected Sub deleteAd(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ad As Ad = getAd(sender)
        ad.Deleted = True
        ad.Update()
        displayAds(Status)
    End Sub


End Class
