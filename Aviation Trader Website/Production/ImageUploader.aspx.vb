Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Ad content editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class Production_ImageUploader
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Ad As Ad
    Private selectedTab As Integer
    Private listID As Integer


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.Skin
        '
        ' save return params
        '
        selectedTab = Loader.SelectedTab
        listID = Loader.Param1

        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue
      

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)
        uploadbar.sys = sys
        uploadbar.ad = Ad


        If Not IsPostBack Then
            displayContent()
        End If

        displayLeftMenu()
        displaytabBar()
        displayButtonBar()

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Return to List"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.ProofTextEditor
        topnode = New MenuNode("A", "Text", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofImageUploader
        topnode = New MenuNode("B", "Images", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofSpecEditor
        topnode = New MenuNode("C", "Specs", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofSellerInfo
        topnode = New MenuNode("D", "Seller", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofProdnNoteEditor
        topnode = New MenuNode("E", "Prodn", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofProductEditor
        topnode = New MenuNode("F", "Products", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofPreview
        topnode = New MenuNode("G", "Preview", Loader.Target, False, "Please wait - building previews")
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofPriceEditor
        topnode = New MenuNode("H", "Pricing", Loader.Target, False, "Please wait - building previews")
        tabbar.Nodes.Add(topnode)
    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Dim adID As Integer = Loader.ObjectID     'save ad id
        leftmenu.Items.Clear()
        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        Loader.ObjectID = listID
        Loader.SelectedTab = selectedTab
        leftmenu.Add("Proof Reader", Loader.Target, False)
        Loader.ObjectID = adID                'restore ad id

        Loader.NextASPX = ATLib.Loader.ASPX.ProofTextEditor
        leftmenu.Add(Ad.Adnumber, Loader.Target, True)

        Pic.ImageUrl = Ad.THBURL
        statusLabel.Text = Ad.ProdnStatus.ToString
        AliasLabel.Text = Ad.Usr.AcctAlias
        CatLabel.Text = Ad.Classification.Category.Name
        ClsLabel.Text = Ad.Classification.Name
        FolderLabel.Text = Ad.Folder.Name

    End Sub

    Private Sub displayContent()

        ImageList.DataSource = Ad.Images
        ImageList.DataBind()

    End Sub


    Private Sub updateContent()
        '
        ' deletes images and sets the main image from the list
        '
        Dim i As Integer = 0
        Dim isMainImageDeleted As Boolean = False     'flag to say trying to delete main image

        For Each r As RepeaterItem In ImageList.Items
            Dim deleteCheck As CheckBox = DirectCast(r.FindControl("deletecheck"), CheckBox)
            Dim webEnabledCheck As CheckBox = DirectCast(r.FindControl("webenabledcheck"), CheckBox)
            Dim isMainImageRadio As ATWebToolkit.GroupRadioButton = DirectCast(r.FindControl("ismainimageradio"), ATWebToolkit.GroupRadioButton)
            Dim image As Image = Ad.Images(i)

            '
            ' main image must be web enabled
            '
            image.IsMainImage = isMainImageRadio.Checked
            image.IsWebEnabled = webEnabledCheck.Checked
            If image.IsMainImage Then image.IsWebEnabled = True
            image.Deleted = deleteCheck.Checked
            If image.IsMainImage And image.Deleted Then isMainImageDeleted = True
            i += 1
        Next
        '
        ' dont allow delete of main image
        '
        If Ad.Images.Count > 1 Then
            If isMainImageDeleted Then
                '
                ' find the first non-deleted image and set to main image
                ' they might all be deleted - that's OK
                '
                For Each img As Image In Ad.Images
                    If Not img.Deleted Then
                        img.IsMainImage = True
                        Exit For
                    End If
                Next
            End If
        End If
        Ad.Images.Update()
        Ad.InvalidateInstancePreviews()         'set preview invalid if anything changes (may be deletion)
        '
        ' remap ad status if necessary
        '
        Select Case Ad.ProdnStatus
            Case ATLib.Ad.ProdnState.Approved : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
            Case ATLib.Ad.ProdnState.Proofed : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
        End Select
        Ad.Update()
        ButtonBar.Msg = Constants.Saved
        displayContent()
        displayLeftMenu()

    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        Loader.ObjectID = listID
        Loader.SelectedTab = selectedTab
        Response.Redirect(Loader.Target)
    End Sub


    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : return2List()
            Case 2 : updateContent()
        End Select
    End Sub

    Protected Sub UploadBar_ImageDeleteEvent() Handles UploadBar.ImageDeleteEvent
        updateContent()
    End Sub

    Protected Sub UploadBar_ImageUploadEvent(ByVal Image As ATLib.Image) Handles UploadBar.ImageUploadEvent
        Ad.InvalidateInstancePreviews()
        '
        ' remap ad status if necessary
        '
        Select Case Ad.ProdnStatus
            Case ATLib.Ad.ProdnState.Approved : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
            Case ATLib.Ad.ProdnState.Proofed : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
        End Select
        Ad.Update()
        displayContent()
        displayLeftMenu()
    End Sub
End Class
