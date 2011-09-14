Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Slideshow demo - displays a slideshow
'*
'* ON ENTRY:
'*
'*  Loader: objectID = ClassificationID
'*          param1 = current page number
'*
'***************************************************************************************

Partial Class Slideshow
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

    
     
        displayLeftMenu()

 
    End Sub

   

    Private Sub displayLeftMenu()
  

    End Sub

  
End Class
