Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* CopyAd
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class CopyAd
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private tabID As String

    Protected sourceAd As Ad


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.Skin

        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ads As New Ads
        sourceAd = ads.Retrieve(Loader.ObjectID)
        displayLeftMenu()
        displayButtonBar()

        If Not IsPostBack Then
            CopyAd()

        End If
     
    End Sub
    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Dim selectedTab As Integer = Loader.SelectedTab           'save selected tab
        Loader.SelectedTab = 0
        Loader.ObjectID = Slot.ImpersonateUsrID
        '
        ' if impersonation is on, add in the other user's login level
        '
        Dim myloginlevel As Usr.LoginLevels = Slot.LoginLevel
        If Slot.UsrID <> Slot.ImpersonateUsrID Then myloginlevel = Slot.ImpersonatedUsr.LoginLevel

        Select Case myloginlevel

            Case Usr.LoginLevels.AdvSub
                Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                leftmenu.Add("Create New Ad", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                leftmenu.Add("Manage My Ads", Loader.Target, True)

                Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                leftmenu.Add("Manage My Subscription", Loader.Target, False)

            Case Usr.LoginLevels.Advertiser
                Loader.NextASPX = ATLib.Loader.ASPX.NewAd
                leftmenu.Add("Create New Ad", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.AdManager
                leftmenu.Add("Manage My Ads", Loader.Target, False)


            Case Usr.LoginLevels.Subscriber
                Loader.NextASPX = ATLib.Loader.ASPX.SubsManager
                leftmenu.Add("Manage My Subscription", Loader.Target, False)
        End Select

        Loader.SelectedTab = selectedTab          'restore selected tab



    End Sub


    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Go to Saved Ads"
        ButtonBar.B2.Text = "Return to List"
    End Sub

    Protected Sub CopyAd()
        '
        ' makes a copy of the supplied ad and saves it to the user's saved ads area
        '
        Dim newAd As New Ad
        newAd.UsrID = sourceAd.UsrID
        newAd.Adnumber = sys.GetNextAdNumber
        '
        ' echo to screen
        '
        adnumber.Text = newAd.Adnumber
        newAd.ProdnRequest = ""
        newAd.ProdnResponse = ""
        newAd.KeyWords = ""
        newAd.SortKey = ""
        newAd.Summary = ""
        newAd.OriginalText = sourceAd.OriginalText
        newAd.Text = sourceAd.Text
        newAd.YoutubeVideoTag = sourceAd.YoutubeVideoTag
        '
        ' put the ad into the first folder, as returned in sort key order
        '
        newAd.FolderID = sys.FirstFolderID

        newAd.ItemPrice = sourceAd.ItemPrice
        newAd.ProdnStatus = ATLib.Ad.ProdnState.Saved       'save to user
        newAd.BillStatus = ATLib.Ad.BillState.NotReady
        newAd.ClassificationID = sourceAd.ClassificationID            'default to first classification

        newAd.Update()
        '
        ' copy pics
        '
        For Each currentImage As Image In sourceAd.Images
            Dim newimage As New Image
            currentImage.OriginalSourcePath = sys.SourceImageOriginalFolder
            currentImage.WorkingSourcePath = sys.SourceImageWorkingFolder
            currentImage.PhysicalApplicationPath = sys.PhysicalApplicationPath
            newimage.OriginalSourcePath = sys.SourceImageOriginalFolder
            newimage.WorkingSourcePath = sys.SourceImageWorkingFolder
            newimage.PhysicalApplicationPath = sys.PhysicalApplicationPath
            newimage.PixelWidth = currentImage.PixelWidth
            newimage.PixelHeight = currentImage.PixelHeight
            newimage.IsMainImage = currentImage.IsMainImage
            newimage.Type = currentImage.Type
            newimage.PreviewSequence = 0
            newimage.ProdnStatus = currentImage.ProdnStatus
            newimage.Resolution = currentImage.Resolution
            newimage.AdID = newAd.ID

            newimage.Update()        'generates id
            IO.File.Copy(currentImage.OriginalSourceFileName, newimage.OriginalSourceFileName)
            IO.File.Copy(currentImage.WorkingSourceFileName, newimage.WorkingSourceFileName)
            IO.File.Copy(currentImage.THBFilename, newimage.THBFilename)
            IO.File.Copy(currentImage.LoResFilename, newimage.LoResFilename)
        Next

        '
        ' copy specs
        '
        For Each currentSpec As Spec In sourceAd.Specs
            Dim newSpec As New Spec
            newSpec.IsActive = currentSpec.IsActive
            newSpec.Value = currentSpec.Value
            newSpec.SpecDefinitionID = currentSpec.SpecDefinitionID
            newSpec.AdID = newAd.ID
            newSpec.Update()
        Next
        

    End Sub


    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        Response.Redirect(Loader.Target)
    End Sub

    Private Sub return2SavedAds()
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        Loader.SelectedTab = 0            'saved ads first tab
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : return2SavedAds()
            Case 2 : return2List()
        End Select
    End Sub



End Class
