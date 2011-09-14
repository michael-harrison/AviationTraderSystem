Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* SearchList - web site ad search listing
'*
'* ON ENTRY:
'*
'*  Loader:     param1 = current page number
'*  Slot:       SearchObjectType - system, category or classification
'*              SearchObjectID - systemID, categoryID or classificationID
'*              SearchKey - text words to search on
'*
'***************************************************************************************

Partial Class SearchList
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem

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
        ' put the slot into search mode
        '
        Slot.SearchMode = ATLib.Slot.SearchModes.Search
        Slot.Update()
        '
        Page.Theme = Slot.skin
        '
        
        curPageNumber = Loader.Param1
        '
        ' set up page header
        '
        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PageBar2.Loader = Loader.Copy


        AdInstances = New AdInstances
        firstWebPubID = sys.GetFirstWebPublication.ID
        Dim startIndex As Integer = Convert.ToInt32(curPageNumber * Slot.AdsPerPage) + 1

        listingCount = AdInstances.RetrievePagedSearchSet(Slot.SearchObjectType, Slot.SearchObjectID, firstWebPubID, Slot.EditionVisibility, Ad.ProdnState.Approved, Slot.SearchKey, startIndex, Slot.AdsPerPage)
        If listingCount = 0 Then
            Loader.NextASPX = ATLib.Loader.ASPX.NoSearchAds
            Server.Transfer(Loader.Target)
        Else
            displayPageBar()
            displayLeftMenu()
            displayInstances()
        End If

    End Sub

    Private Sub displayPageBar()
        PageBar2.PageCount = Convert.ToInt32(Math.Ceiling(listingCount / Slot.AdsPerPage))
        PageBar2.Loader.NextASPX = ATLib.Loader.ASPX.SearchList
        PageBar2.Loader.ObjectID = ATSystem.SysConstants.nullValue
        PageBar2.Loader.Param1 = curPageNumber
    End Sub


    Private Sub displayLeftMenu()
 
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

End Class
