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
'*          selectedTab = tab number to return to
'*          param1 = list ID to return to
'*          
'*
'***************************************************************************************

Partial Class Production_TextEditor
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

        If Not IsPostBack Then
            displayContent()
        End If

        displayLeftMenu()
        displaytabBar()
        displaybuttonbar()

    End Sub

    Private Sub displayButtonBar()
        '
        ' if the main image is a pdf then show the extract button
        '
        If Ad.MainImage IsNot Nothing Then
            If Ad.MainImage.Type = Image.ImageTypes.PDF Then
                ButtonBar.B0.Text = "Get Text from PDF"
            End If
        End If
        ButtonBar.B1.Text = "Return to List"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displaytabBar()

        Dim topnode As MenuNode

        Loader.NextASPX = ATLib.Loader.ASPX.ProofTextEditor
        topnode = New MenuNode("A", "Text", Loader.Target, True)
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
        
        AdText.Text = Ad.Text
        OriginalTextBox.Text = Ad.OriginalText
        KeyWords.Text = Ad.KeyWords
        ItemPrice.Text = Ad.ItemPrice
        SummaryBox.Text = Ad.Summary
        YoutubeVideoTagBox.Text = Ad.YoutubeVideoTag

    End Sub


    Private Sub updateContent()
        '
        ' Only if the keywords fields or summary is empty, split the ad text on the first and second separator
        ' and plug to keyword and/or summary fields
        '
        Dim sep() As Char = {Constants.TextSeparator}
        Dim arr() As String = AdText.Text.Split(sep, 3)

        Select Case arr.Length
            Case 1
                If SummaryBox.Text.Length = 0 Then SummaryBox.Text = arr(0)

            Case Else
                If KeyWords.Text.Length = 0 Then KeyWords.Text = arr(0)
                If SummaryBox.Text.Length = 0 Then SummaryBox.Text = arr(1)

        End Select

        Ad.Text = AdText.Text
        Ad.KeyWords = KeyWords.Text
        Ad.Summary = SummaryBox.Text
        Ad.GenerateSortKey()
        Ad.ItemPrice = ItemPrice.Text
        Ad.YoutubeVideoTag = YoutubeVideoTagBox.Text
        Ad.InvalidateInstancePreviews()
        '
        ' remap ad status if necessary
        '
        Select Case Ad.ProdnStatus
            Case ATLib.Ad.ProdnState.Approved : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
            Case ATLib.Ad.ProdnState.Proofed : Ad.ProdnStatus = ATLib.Ad.ProdnState.Submitted
        End Select
        Ad.Update()
        ButtonBar.Msg = Constants.Saved
        displayLeftMenu()
    End Sub

    Private Sub getTextfromPDF()
        '
        ' call the engine to extract text from pdf
        '
        Try
            Dim Q As New EQItem
            Q.ObjectID = Ad.ID
            Q.Command = EQItem.CommandBits.TextfromPDF Or EQItem.CommandBits.SuspendUntilComplete
            Dim Engine As Engine = sys.MapEngine(ATSystem.EngineModes.Client)
            Q = Engine.Enqueue(Q)
            '
            ' re-read ad to get text
            '
            Dim myads As New Ads
            Ad = myads.Retrieve(Ad.ID)
            displayContent()
            displayLeftMenu()

        Catch ex As Exception
            ButtonBar.Msg = Constants.NoTextFromPDF
        End Try

    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        Loader.ObjectID = listID
        Loader.SelectedTab = selectedTab
        Response.Redirect(Loader.Target)
    End Sub


    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0 : getTextfromPDF()
            Case 1 : return2List()
            Case 2 : updateContent()
        End Select
    End Sub
End Class
