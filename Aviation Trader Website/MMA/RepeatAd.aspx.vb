Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* RepeatAd
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class RepeatAd
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private tabID As String


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
        Dim sourceAd As Ad = ads.Retrieve(Loader.ObjectID)
        displayLeftMenu()
        displayButtonBar()

        If Not IsPostBack Then
            repeatAd(sourceAd)
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
        ButtonBar.B2.Text = "Return to List"
    End Sub

    Private Sub repeatAd(ByVal sourceAd As Ad)
        '
        ' makes a copy of the supplied ad, generates an insertion set in the next open edition
        ' generates an INDD file if applicable and files it directly to submitted ads.
        '
        Dim newAd As New Ad
        newAd.UsrID = sourceAd.UsrID
        newAd.Adnumber = sys.GetNextAdNumber
        adnumber.Text = newAd.Adnumber           'echo to screen

        newAd.ProdnRequest = ""
        newAd.ProdnResponse = ""
        newAd.KeyWords = sourceAd.KeyWords
        newAd.SortKey = sourceAd.SortKey
        newAd.Summary = sourceAd.Summary
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
        newAd.ProdnStatus = Ad.ProdnState.Submitted                     'file to submitted
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
            copyFile(currentImage.OriginalSourceFileName, newimage.OriginalSourceFileName)
            copyFile(currentImage.WorkingSourceFileName, newimage.WorkingSourceFileName)
            copyFile(currentImage.THBFilename, newimage.THBFilename)
            copyFile(currentImage.LoResFilename, newimage.LoResFilename)
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
        '
        ' generate new instances
        '
        generateInstances(sourceAd, newAd)


    End Sub

    Private Sub generateInstances(ByVal sourceAd As Ad, ByVal newAd As Ad)
        '
        ' spins thru all instances of the  source ad and generates an equivalent
        ' instance in the newAd, but where the first open edition is used
        '
        '
        ' for each current instance of this ad, create another in the first open edition
        '
        For Each currentInstance As AdInstance In sourceAd.Instances
            Dim publication As Publication = currentInstance.Edition.Publication
            Dim firstOpenEdition As Edition = publication.FirstOpenEdition

            If firstOpenEdition IsNot Nothing Then
                '
                ' This loop collapses multiple editions for a product into as single first open edition
                '
                Dim newInstance As AdInstance = newAd.GetInstance(currentInstance.ProductID, firstOpenEdition.ID)
                If newInstance Is Nothing Then
                    newInstance = newAd.AddInstance(currentInstance.ProductID, firstOpenEdition.ID)
                    '
                    ' copy size from old to new
                    '
                    newInstance.ExactHeight = currentInstance.ExactHeight
                    newInstance.ExactWidth = currentInstance.ExactWidth
                    newInstance.Width = currentInstance.Width
                    newInstance.Height = currentInstance.Height
                    newInstance.PhysicalApplicationPath = sys.PhysicalApplicationPath
                    currentInstance.PhysicalApplicationPath = sys.PhysicalApplicationPath
                    newInstance.ProdnPDFFolder = sys.ProdnPDFFolder
                    currentInstance.ProdnPDFFolder = sys.ProdnPDFFolder
                    '
                    ' if there is an indesign file, dupe it and attach to new instance
                    '
                    If currentInstance.INDDFilename.Length > 0 Then
                        newInstance.generateINDDName(sys.DisplayAdFolder)
                        copyFile(currentInstance.INDDFilename, newInstance.INDDFilename)
                    End If
                    generatePreview(newInstance)
                End If
            Else
            End If

        Next
    End Sub



    Private Sub generatePreview(ByVal AdInstance As AdInstance)
        '
        ' Also see similar code in production/priceEditor
        '
        ' if the preview is marked as out of date then depending on
        ' the product type, a new preview must be regenerated
        '

        Select Case AdInstance.ProductType
            Case Product.Types.ClassadColorPic, Product.Types.ClassadMonoPic, Product.Types.ClassadTextOnly
                buildPreview(AdInstance, EQItem.CommandBits.Classad Or EQItem.CommandBits.SuspendUntilComplete)
                setClassadPrice(AdInstance)

            Case Product.Types.DisplayModule, Product.Types.DisplayComposite
                '
                ' will be done in Prodn Wizard
                '
            Case Product.Types.DisplayFinishedArt
                buildPDFPreview(AdInstance)
                setdisplayPrice(AdInstance)

            Case Product.Types.WebBasic, Product.Types.WebFeaturedAd, Product.Types.WebPDF, Product.Types.WebPDFText, Product.Types.WebPremium
                setWebPrice(AdInstance)

        End Select

    End Sub


    Private Sub buildPreview(ByVal AdInstance As AdInstance, ByVal command As EQItem.CommandBits)
        '
        ' rebuilds the preview for the instance
        '
        Try
            Dim Q As New EQItem
            Q.ObjectID = AdInstance.ID
            Q.Command = command
            Dim Engine As Engine = sys.MapEngine(ATSystem.EngineModes.Client)
            Q = Engine.Enqueue(Q)
        Catch ex As Exception
        End Try

    End Sub

    Private Sub buildPDFPreview(ByVal AdInstance As AdInstance)
        '
        ' Creates instance pdfs from the main image of the ad
        ' for the op to work, the main image type must be a pdf
        '
        Dim sourceFilename As String
        Dim dstnFilename As String
        AdInstance.PhysicalApplicationPath = sys.PhysicalApplicationPath
        AdInstance.ProdnPDFFolder = sys.ProdnPDFFolder
        AdInstance.PreviewSequence = 0        'always use 0 for now

        If (AdInstance.Ad.MainImage Is Nothing) OrElse (AdInstance.Ad.MainImage.Type <> Image.ImageTypes.PDF) Then
            '
            ' use invalid pdf options
            '
            ' web viewable image
            '
            sourceFilename = IO.Path.Combine(sys.SourceImageWorkingFolder, Constants.InvalidFinishedArt)
            dstnFilename = AdInstance.PreviewImageFilename
            copyFile(sourceFilename, dstnFilename)
            '
            ' web viewable pdf
            '
            sourceFilename = IO.Path.Combine(sys.ProdnPDFFolder, Constants.InvalidFinishedArtPDF)
            dstnFilename = AdInstance.PreviewPDFFilename
            copyFile(sourceFilename, dstnFilename)
            '
            ' prodn pdf
            '
            dstnFilename = AdInstance.ProdnPDFFilename
            copyFile(sourceFilename, dstnFilename)
        Else
            ' web viewable image
            '
            Dim mainImage As Image = AdInstance.Ad.MainImage
            mainImage.PhysicalApplicationPath = sys.PhysicalApplicationPath
            sourceFilename = mainImage.LoResFilename
            dstnFilename = AdInstance.PreviewImageFilename
            copyFile(sourceFilename, dstnFilename)
            '
            ' web viewable pdf
            '
            sourceFilename = mainImage.WorkingSourceFileName
            dstnFilename = AdInstance.PreviewPDFFilename
            copyFile(sourceFilename, dstnFilename)
            '
            ' prodn pdf
            '
            dstnFilename = AdInstance.ProdnPDFFilename
            copyFile(sourceFilename, dstnFilename)
            '
            ' set the adinstance size in mm from the image pixel dimensions
            '
            Dim w As Double = mainImage.PixelWidth / mainImage.Resolution     'inches
            Dim h As Double = mainImage.PixelHeight / mainImage.Resolution     'inches
            AdInstance.ExactWidth = Convert.ToInt32(25400 * w)              'mm * 1000
            AdInstance.ExactHeight = Convert.ToInt32(25400 * h)             'mm * 1000
            Try
                AdInstance.CalculateDisplaySize()
            Catch ex As Exception
                ButtonBar.Msg = ex.Message
            End Try
        End If

        AdInstance.IsPreviewValid = True
        AdInstance.Update()

    End Sub


    Private Sub setClassadPrice(ByVal AdInstance As AdInstance)
        '
        ' get a new instance to get the latest engine data
        '
        Dim rt As New RateTable
        Dim myInstances As New AdInstances
        Dim myAdInstance As AdInstance = myInstances.Retrieve(AdInstance.ID)
        rt.LoadClassadTable(Constants.ClassadRates)
        myAdInstance.Price = rt.GetClassadRate(myAdInstance)
        myAdInstance.Update()

    End Sub

    Private Sub setdisplayPrice(ByVal AdInstance As AdInstance)
        '
        ' get a new instance to get the latest engine data
        '
        Dim rt As New RateTable
        Dim myInstances As New AdInstances
        Dim myAdInstance As AdInstance = myInstances.Retrieve(AdInstance.ID)
        rt.LoadDisplayTable(Constants.DisplayRates)
        myAdInstance.Price = rt.GetDisplayRate(myAdInstance)
        myAdInstance.Update()

    End Sub

    Private Sub setWebPrice(ByVal AdInstance As AdInstance)
        '
        ' get the product loading
        '
        AdInstance.Price = AdInstance.Product.InstanceLoading
        AdInstance.Update()

    End Sub


    Private Sub copyFile(ByVal src As String, ByVal dstn As String)
        '
        ' try to copy file, quietly ignoring any errors, but report to user
        '
        Try
            IO.File.Copy(src, dstn, False)
        Catch ex As Exception
            ButtonBar.Msg = "Warning - Some production files may be missing"
        End Try

    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        Response.Redirect(Loader.Target)
    End Sub

  

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : return2List()
        End Select
    End Sub



End Class
