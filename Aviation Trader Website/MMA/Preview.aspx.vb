Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Ad Proof Preview
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class Preview
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private selectedTab As Integer
    Protected Ad As Ad


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))

        '' Loader = New Loader("4C14000001C700000000E30000000000")
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.Skin
        '
        ' save return params
        '
        selectedTab = Loader.SelectedTab

        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
        ' if the user has not logged in, put up the login screen
        ' otherwise retrieve the ad
        '
        If Slot.LoginLevel = Usr.LoginLevels.Guest Then
            Loader.NextASPX = ATLib.Loader.ASPX.Login
            Response.Redirect(Loader.Target)
        Else
            Dim ads As New Ads
            Ad = ads.Retrieve(Loader.ObjectID)
            Ad.PhysicalApplicationPath = sys.PhysicalApplicationPath
        End If

        If Not IsPostBack Then
            ''''''   updatePreviews()              'should not do this - can not be out of date
            displayInstances()
        End If
        displayLeftMenu()
        displaytabBar()
        displayButtonBar()

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        leftmenu.Add("Manage My Ads", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.MMAPreview
        leftmenu.Add(Ad.Adnumber, Loader.Target, True)

        CatLabel.Text = Ad.Classification.Category.Name
        ClsLabel.Text = Ad.Classification.Name
        Pic.ImageUrl = Ad.THBURL
        statusLabel.Text = Ad.ProdnStatus.ToString

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B2.Text = "Return to List"
    End Sub


    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.MMAPreview
        topnode = New MenuNode("G", "Preview", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.MMAPrices
        topnode = New MenuNode("H", "Pricing", Loader.Target, False)
        tabbar.Nodes.Add(topnode)
        '
        ' do not show this button for archived ads
        '
        If Ad.ProdnStatus <> ATLib.Ad.ProdnState.Archived Then
            Loader.NextASPX = ATLib.Loader.ASPX.MMAProdnNote
            topnode = New MenuNode("H", "Requests", Loader.Target, False)
            tabbar.Nodes.Add(topnode)
        End If
        '
        ' only show this button if the status is waiting for approval
        '
        If Ad.ProdnStatus = ATLib.Ad.ProdnState.Proofed Then
            Loader.NextASPX = ATLib.Loader.ASPX.MMAProofApproval
            topnode = New MenuNode("H", "Proof Approval", Loader.Target, False)
            tabbar.Nodes.Add(topnode)
        End If
    End Sub



    Private Sub updatePreviews()
        '
        ' if the preview is marked as out of date then depending on
        ' the product type, a new preview must be regenerated
        '
        For Each AdInstance As AdInstance In Ad.Instances
            If Not AdInstance.IsPreviewValid Then

                Select Case AdInstance.ProductType
                    Case Product.Types.ClassadColorPic, Product.Types.ClassadMonoPic, Product.Types.ClassadTextOnly
                        buildPreview(AdInstance, EQItem.CommandBits.Classad Or EQItem.CommandBits.SuspendUntilComplete)
                        setClassadPrice(AdInstance)

                    Case Product.Types.DisplayModule, Product.Types.DisplayComposite
                        '
                        ' will be done in DisplayWizard
                        '
                    Case Product.Types.DisplayFinishedArt
                        buildPDFPreview(AdInstance)
                        setdisplayPrice(AdInstance)

                    Case Else           'web ads - previews derived inline - See AdInstance.vb


                End Select



            End If

        Next


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
            IO.File.Copy(sourceFilename, dstnFilename, True)
            '
            ' web viewable pdf
            '
            sourceFilename = IO.Path.Combine(sys.ProdnPDFFolder, Constants.InvalidFinishedArtPDF)
            dstnFilename = AdInstance.PreviewPDFFilename
            IO.File.Copy(sourceFilename, dstnFilename, True)
            '
            ' prodn pdf
            '
            dstnFilename = AdInstance.ProdnPDFFilename
            IO.File.Copy(sourceFilename, dstnFilename, True)
        Else
            ' web viewable image
            '
            Dim mainImage As Image = AdInstance.Ad.MainImage
            mainImage.PhysicalApplicationPath = sys.PhysicalApplicationPath
            sourceFilename = mainImage.LoResFilename
            dstnFilename = AdInstance.PreviewImageFilename
            IO.File.Copy(sourceFilename, dstnFilename, True)
            '
            ' web viewable pdf
            '
            sourceFilename = mainImage.WorkingSourceFileName
            dstnFilename = AdInstance.PreviewPDFFilename
            IO.File.Copy(sourceFilename, dstnFilename, True)
            '
            ' prodn pdf
            '
            dstnFilename = AdInstance.ProdnPDFFilename
            IO.File.Copy(sourceFilename, dstnFilename, True)
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




    Private Sub displayInstances()

        Ad.ClearInstances()               'cause lazy reload

        Loader.NextASPX = ATLib.Loader.ASPX.TextReader
        For Each adInstance As AdInstance In Ad.Instances
            Loader.ObjectID = adInstance.AdID
            adInstance.NavTarget = Loader.Target
        Next

        InstanceList.DataSource = Ad.Instances
        InstanceList.DataBind()

    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.MyAds
        Loader.SelectedTab = selectedTab
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
