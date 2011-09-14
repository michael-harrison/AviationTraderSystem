Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Text Reader
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          param1 = current page number
'*
'***************************************************************************************

Partial Class Images
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private tabID As String
    Protected Ad As Ad


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
            topnode = New MenuNode("B", "All Images", Loader.Target, True)
            tabbar.Nodes.Add(topnode)
        End If

        If Ad.YoutubeVideoTag.Length > 0 Then
            Loader.NextASPX = ATLib.Loader.ASPX.VideoReader
            topnode = New MenuNode("B", "Video", Loader.Target, False)
            tabbar.Nodes.Add(topnode)
        End If

        If Ad.ActiveSpecs.Count > 0 Then
            Loader.NextASPX = ATLib.Loader.ASPX.SpecReader
            topnode = New MenuNode("C", "Specs", Loader.Target, False)
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
    End Sub

    <WebMethod()> <System.Web.Script.Services.ScriptMethod()> _
  Public Shared Function GetImages(ByVal AdID As Integer) As AdPic()
        '
        ' this is an ajax callback via pagemethods. Note that function must be marked as shared - equivalent of C# static function
        '
        Dim sys As New ATSystem
        sys.Retrieve()
        Dim ads As New Ads
        Dim ad As Ad = ads.Retrieve(AdID)

        Dim myPics(ad.WebImages.Count - 1) As AdPic

        For i As Integer = 0 To ad.WebImages.Count - 1
            Dim image As Image = ad.WebImages(i)
            Dim aspectRatio As Double = image.PixelWidth / image.PixelHeight

            Dim loresWidth As Integer = Convert.ToInt32(sys.LRImageHeight * aspectRatio)
            Dim THBWidth As Integer = Convert.ToInt32(sys.THBImageHeight * aspectRatio)

            Dim pic As New AdPic
            pic.THBURL = image.THBURL
            pic.LoresURL = image.LoResURL
            pic.THBWidth = THBWidth
            pic.LoresWidth = loresWidth
            myPics(i) = pic
        Next
        Return myPics
    End Function

    Public Class AdPic
        '
        ' this object will be returned in the client side javascript array via json.
        '
        Public LoresURL As String
        Public THBURL As String
        Public LoresWidth As Integer
        Public THBWidth As Integer
    End Class

End Class
