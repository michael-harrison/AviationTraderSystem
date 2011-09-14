Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* New ad
'*
'* ON ENTRY:
'*
'*  Loader: objectID = undefined
'*          
'*
'***************************************************************************************

Partial Class Advertise_NewAd
    Inherits System.Web.UI.Page

    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
        ' if the user has not logged in, put up the login screen
        ' otherwise create a new ad and go to the ad entry screen
        '
        If Slot.LoginLevel = Usr.LoginLevels.Guest Then
            Loader.NextASPX = ATLib.Loader.ASPX.Login
        Else
            Loader.ObjectID = createAd()
            Loader.NextASPX = ATLib.Loader.ASPX.AdProductSelector
        End If

        Server.Transfer(Loader.Target)

    End Sub

    Private Function createAd() As Integer
        '
        ' creates a new ad and returns its id
        '
        Dim newAd As New Ad
        newAd.UsrID = Slot.ImpersonateUsrID
        newAd.Adnumber = sys.GetNextAdNumber
        newAd.ProdnRequest = ""
        newAd.ProdnResponse = ""
        newAd.KeyWords = ""
        newAd.GenerateSortKey()
        newAd.ItemPrice = ""
        newAd.YoutubeVideoTag = ""
        newAd.ProdnStatus = ATLib.Ad.ProdnState.Initial
        newAd.BillStatus = ATLib.Ad.BillState.NotReady
        newAd.ClassificationID = sys.Categories(0).Classifications(0).ID            'default to first classification
        newAd.OriginalText = ""
        newAd.Text = ""
        newAd.Summary = ""
        newAd.IsLatestListing = False
        '
        ' put the ad into the first folder, as returned in sort key order
        '
        newAd.FolderID = sys.FirstFolderID
        newAd.Update()
        newAd.AddSpecs()          'add in spec list
        Return newAd.ID
     
    End Function

End Class
