Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* AdList - web site ad listing - null ad page
'*
'* ON ENTRY:
'*
'*  Loader: objectID = ClassificationID
'*          param1 = current page number
'*
'***************************************************************************************

Partial Class NoBrowseAds
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem



    Private classificationID As Integer
    Private firstEditionID As Integer
    Private listingCount As Integer



    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.skin

        Dim classifications As New Classifications
        Dim classification As Classification = classifications.Retrieve(Loader.ObjectID)

        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = classification.CategoryID

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        displayLeftMenu(Loader.ObjectID)
        displayContent()

    End Sub



    Private Sub displayLeftMenu(ByVal classificationID As Integer)
        Loader.SelectedTab = 0
        If Slot.SearchMode = ATLib.Slot.SearchModes.Browse Then
            '
            ' display all classifications within the selected category
            '

            Dim classifications As New Classifications
            Dim currentClass As Classification = classifications.Retrieve(classificationID)
            Dim currentCat As Category = currentClass.Category

            Loader.NextASPX = ATLib.Loader.ASPX.BrowseList

            For Each cls As Classification In currentCat.Classifications
                Loader.ObjectID = cls.ID
                Dim mi As New LeftMenuItem(cls.Name, Loader.Target, False)
                If classificationID = cls.ID Then mi.Selected = True
                leftmenu.Add(mi)
            Next
        End If
    End Sub

    Private Sub displayContent()

    End Sub


End Class
