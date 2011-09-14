Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Spec Reader
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          param1 = current page number
'*
'***************************************************************************************

Partial Class Specs
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private tabID As String
    Private Ad As Ad


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

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)

        displaybuttonbar()
        displayContent()
        displaytabBar()
       
    End Sub

    Private Sub displaybuttonbar()
        '
        ' examine slot search mode to return to correct page
        ' for single shot (eg from twitter), suppress display of button
        '
        Loader.ObjectID = Ad.ClassificationID
        Select Case Slot.SearchMode
            Case ATLib.Slot.SearchModes.Browse : Loader.NextASPX = ATLib.Loader.ASPX.BrowseList
            Case ATLib.Slot.SearchModes.Search : Loader.NextASPX = ATLib.Loader.ASPX.SearchList
            Case ATLib.Slot.SearchModes.Manage : Loader.NextASPX = ATLib.Loader.ASPX.MyAds
            Case ATLib.Slot.SearchModes.SingleShot : return2list.Visible = False
            Case ATLib.Slot.SearchModes.Proof
                Loader.NextASPX = ATLib.Loader.ASPX.ProofPreview
                Loader.ObjectID = Ad.ID
        End Select

        return2list.NavigateURL = Loader.Target

    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        tabbar.Nodes.Clear()
        Loader.ObjectID = Ad.ID
        Loader.NextASPX = ATLib.Loader.ASPX.TextReader
        topnode = New MenuNode("A", "Details", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        If Ad.Images.Count > 0 Then
            Loader.NextASPX = ATLib.Loader.ASPX.ImageReader
            topnode = New MenuNode("B", "All Images", Loader.Target, False)
            tabbar.Nodes.Add(topnode)
        End If

        If Ad.YoutubeVideoTag.Length > 0 Then
            Loader.NextASPX = ATLib.Loader.ASPX.VideoReader
            topnode = New MenuNode("B", "Video", Loader.Target, False)
            tabbar.Nodes.Add(topnode)
        End If

        If Ad.ActiveSpecs.Count > 0 Then
            Loader.NextASPX = ATLib.Loader.ASPX.SpecReader
            topnode = New MenuNode("C", "Specs", Loader.Target, True)
            tabbar.Nodes.Add(topnode)
        End If


        Loader.NextASPX = ATLib.Loader.ASPX.SellerReader
        topnode = New MenuNode("D", "Seller Info", Loader.Target, False)
        tabbar.Nodes.Add(topnode)


    End Sub

    Private Sub displayContent()
        Pic.ImageUrl = Ad.THBURL
        KeyWords.Text = Ad.KeyWords
        ItemPrice.Text = Ad.ItemPrice
        '
        ' get a set of Specgroups only if there are any specs in the group
        '
        Dim Specgroups As New SpecGroups
        Specgroups.retrieveSet(Ad.ClassificationID)

        Dim mySpecgroups As New List(Of SpecGroup)

        For Each sg As SpecGroup In Specgroups
            If Ad.ActiveSpecs(sg.ID).Count > 0 Then
                mySpecgroups.Add(sg)
            End If
        Next
        '
        ' bind the spec groups to the outside repeater
        ' 
        grouplist.DataSource = mySpecgroups
        grouplist.DataBind()
        '
        ' bind the spec collection to the inside repeater
        '

        Dim i As Integer = 0
        For Each r1 As RepeaterItem In grouplist.Items
            Dim speclist As Repeater = CType(r1.FindControl("speclist"), Repeater)
            Dim SpecgroupID As Integer = mySpecgroups(i).ID
            Dim activespecs As ATLib.Specs = Ad.ActiveSpecs(SpecgroupID)
            speclist.DataSource = activespecs
            speclist.DataBind()
            i += 1
        Next
    End Sub

End Class
