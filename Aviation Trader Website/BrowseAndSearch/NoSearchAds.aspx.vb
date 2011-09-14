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
'*  Loader: objectID = undefined
'*
'***************************************************************************************

Partial Class NoSearchAds
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

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            displayLeftMenu(Loader.ObjectID)
        End If

    End Sub



    Private Sub displayLeftMenu(ByVal classificationID As Integer)
       
    End Sub

End Class
