Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* BrowseList - web site ad browse listing
'*
'* ON ENTRY:
'*
'*  Loader: objectID = ClassificationID
'*          param1 = current page number
'*
'***************************************************************************************

Partial Class BrowseList
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem

    Private classificationID As Integer
    Private curPageNumber As Integer
    Private firstWebPubID As Integer
    Private listingCount As Integer
    Private AdInstances As AdInstances



    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        '
        ' put the slot into browse mode
        '
        Slot.SearchMode = ATLib.Slot.SearchModes.Browse
        Slot.Update()
        '
        Page.Theme = Slot.skin
        '
        ' remember classificationID from loader and get first web pub id
        '
        classificationID = Loader.ObjectID
        curPageNumber = Loader.Param1
        Dim classifications As New Classifications
        Dim classification As Classification = classifications.Retrieve(classificationID)
        firstWebPubID = sys.GetFirstWebPublication.ID()
        '
        ' set up page header
        '
        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = classification.CategoryID

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PageBar2.Loader = Loader.Copy

        If Not IsPostBack Then

            AdInstances = New AdInstances
            firstWebPubID = sys.GetFirstWebPublication.ID
            Dim startIndex As Integer = Convert.ToInt32(curPageNumber * Slot.AdsPerPage) + 1
            listingCount = AdInstances.RetrievePagedDisplaySet(classificationID, firstWebPubID, Slot.EditionVisibility, Ad.ProdnState.Approved, startIndex, Slot.AdsPerPage)
            If listingCount = 0 Then
                Loader.NextASPX = ATLib.Loader.ASPX.NoBrowseAds
                Server.Transfer(Loader.Target)
            Else
                displayPageBar()
                displayLeftMenu()
                displayNavbar()
                displayInstances()
            End If
        End If
 
    End Sub

    Private Sub displayPageBar()
        PageBar2.PageCount = Convert.ToInt32(Math.Ceiling(listingCount / Slot.AdsPerPage))
        PageBar2.Loader.NextASPX = ATLib.Loader.ASPX.BrowseList
        PageBar2.Loader.ObjectID = classificationID
        PageBar2.Loader.Param1 = curPageNumber
    End Sub


    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.ClassificationBrowse
        NavBar.ObjectID = classificationID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub

    Private Sub displayLeftMenu()
        '
        ' display all classifications within the selected category
        '

        Dim classifications As New Classifications
        Dim currentClass As Classification = classifications.Retrieve(classificationID)
        Dim currentCat As Category = currentClass.Category

        Loader.SelectedTab = 0
        Loader.NextASPX = ATLib.Loader.ASPX.BrowseList

        For Each cls As Classification In currentCat.Classifications
            Loader.ObjectID = cls.ID
            Loader.Param1 = 0             'start at page 0
            Dim mi As New LeftMenuItem(cls.Name, Loader.Target, False)
            If classificationID = cls.ID Then mi.Selected = True
            leftmenu.Add(mi)
        Next

    End Sub

    Private Sub displayInstances()
        '
        ' set nav targets - loader.param1 = current page number
        '
        Loader.Param1 = curPageNumber
        Loader.NextASPX = ATLib.Loader.ASPX.TextReader

        For Each adInstance As AdInstance In AdInstances
            Loader.ObjectID = adInstance.AdID
            adInstance.NavTarget = Loader.Target
        Next

        adList.DataSource = AdInstances
        adList.DataBind()
        '
        ' summarise page info
        '
        Dim first As Integer = curPageNumber * Slot.AdsPerPage
        Dim last As Integer = first + Slot.AdsPerPage - 1
        If last > listingCount - 1 Then last = listingCount - 1

        pageinfo.Text = "Displaying " & (first + 1).ToString & " to " & (last + 1).ToString & " of " & listingCount & " ads"

    End Sub

    Protected Sub ScriptManager1_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles ScriptManager1.AsyncPostBackError

    End Sub
End Class
