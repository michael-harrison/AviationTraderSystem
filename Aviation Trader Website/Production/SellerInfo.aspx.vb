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

Partial Class Production_SellerInfo
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
        ButtonBar.B1.Text = "Return to List"
        ButtonBar.B2.Text = "Re-Assign Ad"
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
        topnode = New MenuNode("D", "Seller", Loader.Target, True)
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
      
        Pic.ImageUrl = Ad.THBURL
        
        Dim Usr As Usr = Ad.Usr
        Aliasx.Text = Usr.AcctAlias
        Name.Text = Usr.FullName
        Email.Text = Usr.EmailAddr
        Phone.Text = Usr.Phone
        AHPhone.Text = Usr.AHPhone
        Mobile.Text = Usr.Mobile
        Fax.Text = Usr.Fax
        Addr.Text = Usr.HTMLAddress
        Company.Text = Usr.Company
        '
        ' display web addr - add in http:// if its not there already
        '
        Website.Text = Usr.WebSite
        Dim webaddr As String = Usr.WebSite
        If webaddr.Length > 0 Then
            If Not webaddr.StartsWith("http://") Then webaddr = "http://" & Usr.WebSite
        End If
        Website.NavigateUrl = webaddr

        Dim myUsrs As New Usrs
        myUsrs.Retrieve()

        AliasDD.DataSource = myUsrs
        AliasDD.DataBind()
        AliasDD.SelectedValue = Usr.hexID

    End Sub

    Private Sub return2List()
        Loader.NextASPX = ATLib.Loader.ASPX.ProofList
        Loader.ObjectID = listID
        Loader.SelectedTab = selectedTab
        Response.Redirect(Loader.Target)
    End Sub

    Private Sub reAssignAd()
        Dim newUsrID As Integer = CommonRoutines.Hex2Int(AliasDD.SelectedValue)
        Ad.UsrID = newUsrID
        Ad.Update()
        displayContent()
        displayLeftMenu()
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : return2List()
            Case 2 : ReAssignAd()
        End Select
    End Sub

End Class
