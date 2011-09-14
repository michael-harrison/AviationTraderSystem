Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Edit System Parameters 1
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Folder ID to display
'*          selectedTab = tab number to display
'*
'***************************************************************************************

Partial Class Production_ProofList
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Status As Ad.ProdnState
    Protected ItemCount As Integer
    Private folderID As Integer


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.Skin
        '
        ' remember folderID from loader
        '
        folderID = Loader.ObjectID
        '
        ' put the slot into proof mode 
        '
        Slot.SearchMode = ATLib.Slot.SearchModes.Proof
        Slot.Update()

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '
        ' map tab to prodn status
        '
        Status = mapTab2Status(Loader.SelectedTab)

        displaytabBar()
        If Not IsPostBack Then
            displayLeftMenu()
            displayRightMenu()
            displayAds(Status)
        End If


    End Sub

    Private Function mapTab2Status(ByVal selectedTab As Integer) As Ad.ProdnState

        Select Case selectedTab
            Case 0 : Return Ad.ProdnState.Initial
            Case 1 : Return Ad.ProdnState.Saved
            Case 2 : Return Ad.ProdnState.Submitted
            Case 3 : Return Ad.ProdnState.Proofed
            Case 4 : Return Ad.ProdnState.Approved
            Case 5 : Return Ad.ProdnState.Archived

        End Select

    End Function

    Private Sub displayLeftMenu()
        '
        ' display all classifications within the selected category
        '

        Dim folders As New Folders
        Dim currentFolder As Folder = folders.Retrieve(folderID)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofList

        For Each folder As Folder In sys.Folders
            Loader.ObjectID = folder.ID
            Dim mi As New LeftMenuItem(folder.Name, Loader.Target, False)
            If folderID = folder.ID Then mi.Selected = True
            leftmenu.Add(mi)
        Next

    End Sub

    Private Sub displayRightMenu()
        '
        ' Alias selector
        '
        AliasBox.Text = ""
        If Slot.AliasIDFilter <> ATSystem.SysConstants.nullValue Then
            Dim myUsrs As New Usrs
            myUsrs.Retrieve(Slot.AliasIDFilter)
            If myUsrs.Count > 0 Then AliasBox.Text = myUsrs(0).AcctAlias
        End If
        '
        ' sort order selector
        '
        Dim EA As New EnumAssistant(New Ad.SortOrders)
        AdSortDD.DataSource = EA
        AdSortDD.DataBind()
        AdSortDD.SelectedValue = Convert.ToString(Slot.AdSortOrder)
        '
        ' cat - class selector
        '
        CategoryDD.DataSource = sys.GetCatClassList
        CategoryDD.DataBind()
        CategoryDD.SelectedValue = CommonRoutines.Int2ShortHex(Slot.ProofObjectType) & CommonRoutines.Int2Hex(Slot.ProofObjectID)

    End Sub

   


    Private Sub displaytabBar()

        '
        ' save current tab number
        '
        Dim selectedTab As Integer = Loader.SelectedTab

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        Loader.SelectedTab = 0
        topnode = New MenuNode("D", "Initial", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 1
        topnode = New MenuNode("B", "Saved", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 2
        topnode = New MenuNode("A", "Submitted", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 3
        topnode = New MenuNode("A", "Proofed", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 4
        topnode = New MenuNode("A", "Approved", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.SelectedTab = 5
        topnode = New MenuNode("A", "Archive", Loader.Target, False)
        tabbar.Nodes.Add(topnode)
       
        '
        ' put the selected tab down
        '
        tabbar.Nodes(selectedTab).Selected = True
        Loader.SelectedTab = selectedTab          'restore current tab number

    End Sub


    Private Sub displayAds(ByVal Status As Ad.ProdnState)

        Dim ads As New Ads

        Dim catID As Integer = ATSystem.SysConstants.nullValue
        Dim clsID As Integer = ATSystem.SysConstants.nullValue

        Select Case Slot.ProofObjectType
            Case ATSystem.ObjectTypes.Category : catID = Slot.ProofObjectID
            Case ATSystem.ObjectTypes.Classification : clsID = Slot.ProofObjectID
        End Select

        ads.RetrieveSet(catID, clsID, Slot.AliasIDFilter, folderID, Status, Slot.AdSortOrder)
        '
        ' set nav targets
        '
        Loader.NextASPX = ATLib.Loader.ASPX.ProofTextEditor
        For Each Ad As Ad In ads
            Loader.Param1 = folderID       'set list ID to to return to
            Loader.ObjectID = Ad.ID

            Ad.NavTarget = Loader.Target
        Next
        ItemCount = ads.Count
        adList.DataSource = ads
        adList.DataBind()



    End Sub


    Protected Sub btnFindAd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindAd.Click
        '
        ' finds the ad from the number and if found pulls up the ad proof text editor
        '
        FindError.Visible = False
        Dim myads As New Ads
        Dim myAd As Ad = myads.RetrieveByNumber(AdNumberbox.Text)
        If myAd Is Nothing Then
            FindError.Text = Constants.NoAd
            FindError.Visible = True
            displayLeftMenu()
        Else
            Loader.NextASPX = ATLib.Loader.ASPX.ProofTextEditor
            Loader.ObjectID = myAd.ID
            Response.Redirect(Loader.Target)
        End If

    End Sub

    Protected Sub btnSearchPrefs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchPrefs.Click
        '
        ' lookup the AliasID from the supplied alias name
        '
        Slot.AliasIDFilter = ATSystem.SysConstants.nullValue
        If AliasBox.Text.Length > 0 Then
            Dim myUsrs As New Usrs
            myUsrs.RetrieveByAcctAlias(AliasBox.Text)
            If myUsrs.Count > 0 Then
                Slot.AliasIDFilter = myUsrs(0).ID
            End If
        End If
        '
        ' update the slot with the current aliasID , categoryID and sort order
        '
        Slot.AdSortOrder = CType(AdSortDD.SelectedValue, Ad.SortOrders)
        Slot.ProofObjectType = CType(CategoryDD.SelectedValue.Substring(0, 2), ATSystem.ObjectTypes)
        Slot.proofObjectID = CommonRoutines.Hex2Int(CategoryDD.SelectedValue.Substring(2, 8))
        Slot.Update()

        displayLeftMenu()
        displayAds(Status)

    End Sub
End Class
