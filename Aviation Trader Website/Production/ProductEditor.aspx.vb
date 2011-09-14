Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Ad Porrof Product Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Ad number
'*          
'*
'***************************************************************************************

Partial Class ProductEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private selectedTab As Integer
    Private listID As Integer
    Protected Ad As Ad


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
        End If

        If Not IsPostBack Then
            displayProducts()
        End If
        displayLeftMenu()
        displaytabBar()
        displayButtonBar()

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Return to list"
        ButtonBar.B2.Text = "Save Changes"
        ButtonBar2.B1.Text = "Return to list"
        ButtonBar2.B2.Text = "Save Changes"
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

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.ProofTextEditor
        topnode = New MenuNode("A", "Text", Loader.Target, False)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofImageUploader
        topnode = New MenuNode("B", "Images", Loader.Target, False)
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

        Loader.NextASPX = ATLib.Loader.ASPX.proofProductEditor
        topnode = New MenuNode("F", "Products", Loader.Target, True)
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofPreview
        topnode = New MenuNode("G", "Preview", Loader.Target, False, "Please wait - building previews")
        tabbar.Nodes.Add(topnode)

        Loader.NextASPX = ATLib.Loader.ASPX.ProofPriceEditor
        topnode = New MenuNode("H", "Pricing", Loader.Target, False, "Please wait - building previews")
        tabbar.Nodes.Add(topnode)
    End Sub



    Private Sub displayProducts()

        LatestListingCheck.Checked = Ad.IsLatestListing
        PDFHintCheck.Checked = Ad.IsPDFHint
        '
        ' set the category and class drop downs
        '
        CatDD.DataSource = sys.Categories
        CatDD.DataBind()
        CatDD.SelectedValue = Ad.Classification.Category.hexID
        '
        ClsDD.DataSource = Ad.Classification.Category.Classifications
        ClsDD.DataBind()
        ClsDD.SelectedValue = Ad.Classification.hexID
        '
        ' set the selected values
        '
        ClsDD.SelectedValue = CommonRoutines.Int2Hex(Ad.ClassificationID)
        CatDD.SelectedValue = CommonRoutines.Int2Hex(Ad.Classification.CategoryID)
        '
        ' bind publications to outside repeater
        '
        Dim publications As Publications = sys.Publications
        PublicationList.DataSource = publications
        PublicationList.DataBind()

        '
        ' bind products and open editions to two inside repeaters
        '

        Dim i As Integer = 0
        For Each r1 As RepeaterItem In PublicationList.Items
            Dim productlist As Repeater = CType(r1.FindControl("ProductList"), Repeater)
            Dim products As Products = publications(i).Products

            Dim editionlist As Repeater = CType(r1.FindControl("EditionList"), Repeater)
            Dim editions As Editions = publications(i).Editions(Edition.ProdnState.Open)
            '
            ' put the first edition -  show in different color
            '
            If editions.Count > 0 Then
                editions(0).CSSClass = "error"
            End If

            '
            ' spin thru intances for the ad and set the check matrix
            '
            For Each AdInstance As AdInstance In Ad.Instances
                For Each Product As Product In products
                    If Product.ID = AdInstance.ProductID Then Product.Checked = True
                Next

                For Each edition As Edition In editions
                    '
                    ' put the first edition on and show in different color
                    '
                    If edition.ID = AdInstance.EditionID Then edition.Checked = True
                Next
            Next

            productlist.DataSource = products
            productlist.DataBind()

            editionlist.DataSource = editions
            editionlist.DataBind()
            i += 1
        Next

    End Sub

    Private Sub updateInstances()

        Ad.IsLatestListing = LatestListingCheck.Checked
        Ad.IsPDFHint = PDFHintCheck.Checked
        Ad.Update()
        '
        ' discard the current ad specs and get a new set based on the first classification
        '
        Dim classID As Integer = CommonRoutines.Hex2Int(ClsDD.SelectedValue)
        '
        ' if class changes, delete specs and get a new set
        '
        If Ad.ClassificationID <> classID Then
            Ad.ClassificationID = classID
            Ad.DeleteSpecs()
            Ad.AddSpecs()
        End If
        '
        ' iterate thru repeaters and update instances
        ' The proof reader algorithm has no checks
        '
        ButtonBar.Msg = ""
        ButtonBar2.Msg = ""

        For Each r0 As RepeaterItem In PublicationList.Items
            Dim errorMsg As Label = CType(r0.FindControl("errorMsg"), Label)
            Dim productList As Repeater = CType(r0.FindControl("productList"), Repeater)
            Dim editionList As Repeater = CType(r0.FindControl("editionList"), Repeater)
            '
            ' iterate thru products and editions
            '
            Dim productIDs As New List(Of Integer)
            For Each r1 As RepeaterItem In productList.Items
                Dim productField As HiddenField = CType(r1.FindControl("ProductID"), HiddenField)
                Dim productID As Integer = CommonRoutines.Hex2Int(productField.Value)
                Dim productCheck As CheckBox = CType(r1.FindControl("ProductCheck"), CheckBox)

                For Each r2 As RepeaterItem In editionList.Items
                    Dim editionField As HiddenField = CType(r2.FindControl("EditionID"), HiddenField)
                    Dim editionID As Integer = CommonRoutines.Hex2Int(editionField.Value)
                    Dim editionCheck As CheckBox = CType(r2.FindControl("EditionCheck"), CheckBox)

                    If (productCheck.Checked And editionCheck.Checked) Then
                        '
                        ' add the instance only if it is not there already
                        '
                        If Ad.GetInstance(productID, editionID) Is Nothing Then Ad.AddInstance(productID, editionID)
                    Else
                        Ad.DeleteInstance(productID, editionID)
                    End If
                Next
            Next
        Next

        Ad.Update()
        ButtonBar.Msg = Constants.Saved
        ButtonBar2.Msg = Constants.Saved
        displayLeftMenu()
    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        Loader.ObjectID = listID
        Loader.SelectedTab = selectedTab
        Response.Redirect(Loader.Target)
    End Sub
    Protected Sub CatDD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CatDD.SelectedIndexChanged
        '
        ' rebind the cls dropdown
        '
        Dim selectedCat As Integer = CatDD.SelectedIndex
        Dim classifications As Classifications = sys.Categories(selectedCat).Classifications
        ClsDD.DataSource = classifications
        ClsDD.DataBind()
        ClsDD.SelectedValue = classifications(0).hexID
        '
        ' delete current chain and create new specs if category changes
        '
        Ad.DeleteSpecs()
        Ad.ClassificationID = classifications(0).ID
        Ad.AddSpecs()
        '
        ' remap ad status if necessary
        '
        Select Case Ad.ProdnStatus
            Case ATLib.Ad.ProdnState.Approved : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
            Case ATLib.Ad.ProdnState.Proofed : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
        End Select
        Ad.Update()
        displayLeftMenu()

    End Sub



    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent, ButtonBar2.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : return2List()
            Case 2 : updateInstances()
        End Select
    End Sub

End Class
