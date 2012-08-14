Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Home page
'*
'* 
'*
'*
'***************************************************************************************

Partial Class HomeGuest
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

        NewsRotator.Loader = Loader.Copy
        NewsRotator.currentnewsitemid = ATSystem.SysConstants.nullValue

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
        '
        ' put the search mode into single shot for featured ads no return to list
        '
        Slot.SearchMode = ATLib.Slot.SearchModes.SingleShot
        Slot.Update()

        displayLeftMenu()
        displayContent()

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

    End Sub

    Private Sub displayContent()

        FrontPicImage.ImageUrl = sys.FrontImageURL
        frontpicimage.imagetype = sys.FrontPicType
        FrontPicCaption.Text = sys.FrontPicCaption
        '
        ' display the featured ads
        '
        Dim firstWebPub As Publication = sys.GetFirstWebPublication
        Dim featuredAds As AdInstances = firstWebPub.FeaturedAds(Slot.EditionVisibility)
        '
        ' unsort the retrieved set into a randomised array
        '
        Dim count As Integer = featuredAds.Count - 1
        Dim indexList(count) As Integer
        
        CommonRoutines.RandomizeArray(indexList)
        Dim randomisedFeaturedAds As New List(Of AdInstance)

        For i As Integer = 0 To count
            randomisedFeaturedAds.Add(featuredAds(indexList(i)))
        Next

        '
        ' set navtargets
        '
        Loader.NextASPX = ATLib.Loader.ASPX.TextReader
        For Each AdInstance As AdInstance In randomisedFeaturedAds
            Loader.ObjectID = AdInstance.AdID
            AdInstance.NavTarget = Loader.Target
        Next
        beltWidth = Convert.ToInt32(192 * featuredAds.Count)
        featuredAdsList.DataSource = randomisedFeaturedAds
        featuredAdsList.DataBind()


        twitlink.NavigateUrl = "http://www.twitter.com/" & sys.TwitUserName


    End Sub


End Class
