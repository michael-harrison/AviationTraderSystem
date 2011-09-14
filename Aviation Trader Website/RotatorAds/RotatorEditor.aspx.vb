Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Rotator Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = RotatorItemID
'*
'***************************************************************************************

Partial Class RotatorEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Rotator As RotatorAd


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.skin

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim RotatorAds As New ATLib.RotatorAds
        Rotator = RotatorAds.Retrieve(Loader.ObjectID)
        displayLeftMenu()
        displayButtonBar()

        If Not IsPostBack Then
            displayRotator()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B0.Text = "Delete"
        ButtonBar.B1.Text = "RETURN TO LIST"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.NextASPX = ATLib.Loader.ASPX.RotatorListing
        leftmenu.Add("Rotator Ads", Loader.Target, False)


        Loader.NextASPX = ATLib.Loader.ASPX.RotatorEditor
        leftmenu.Add(Rotator.Name, Loader.Target, True)
    End Sub


    Private Sub displayRotator()


        NameBox.Text = Rotator.Name
        ImageURLbox.Text = Rotator.ImageURL
        ClickURLBox.Text = Rotator.ClickURL
        BGColorBox.Text = Rotator.BGColorHTML
        WidthBox.Text = Rotator.Width.ToString
        HeightBox.Text = Rotator.Height.ToString
        MarginTopBox.Text = Rotator.MarginTop.ToString
        MarginBottomBox.Text = Rotator.MarginBottom.ToString
        MarginleftBox.Text = Rotator.MarginLeft.ToString
        MarginrightBox.Text = Rotator.MarginRight.ToString
        usageCount.Text = Rotator.UsageCount.ToString
        clickCount.Text = Rotator.ClickCount.ToString
        conversionRate.Text = Rotator.ConversionRate.ToString("p1")

        Dim EA As EnumAssistant

        EA = New EnumAssistant(New RotatorAd.Categories)
        CategoryDD.DataSource = EA
        CategoryDD.DataBind()
        CategoryDD.SelectedValue = Convert.ToString(Rotator.Category)
 
        EA = New EnumAssistant(New RotatorAd.Types)
        TypeDD.DataSource = EA
        TypeDD.DataBind()
        TypeDD.SelectedValue = Convert.ToString(Rotator.Type)


    End Sub

    Private Sub updateItem()

        Rotator.Category = CType(categoryDD.SelectedValue, RotatorAd.Categories)
        Rotator.Type = CType(TypeDD.SelectedValue, RotatorAd.Types)
 
        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength
        Rotator.Name = IV.ValidateText(NameBox, NameError)
        Rotator.ImageURL = IV.ValidateURL(ImageURLbox, ImageURLError)
        Rotator.BGColor = IV.ValidateHex(BGColorBox, bgcolorError)

        IV.MinStringLength = 0         'allow nullstring
        Rotator.ClickURL = IV.ValidateURL(ClickURLBox, ClickURLError)

        IV.MinValue = 20
        IV.MaxValue = 700
        Rotator.Width = IV.ValidateInteger(WidthBox, widthError)
        Rotator.Height = IV.ValidateInteger(HeightBox, heightError)

        IV.MinValue = 0
        IV.MaxValue = 100
        Rotator.MarginTop = IV.ValidateInteger(MarginTopBox, MarginTopError)
        Rotator.MarginBottom = IV.ValidateInteger(MarginBottomBox, MarginBottomError)
        Rotator.MarginLeft = IV.ValidateInteger(MarginleftBox, MarginleftError)
        Rotator.MarginRight = IV.ValidateInteger(MarginrightBox, MarginrightError)


        If IV.ErrorCount = 0 Then
            Rotator.Update()
            ButtonBar.Msg = Constants.Saved
        End If
    End Sub

    Private Sub return2List()
        '
        ' go back to the RotatorItem list
        '
        Loader.NextASPX = Loader.ASPX.RotatorListing
        Response.Redirect(Loader.Target)

    End Sub

    Private Function mapCateogry2Tab(ByVal Category As RotatorAd.Categories) As Integer

        Select Case Category
            Case ATLib.RotatorAd.Categories.InActive : Return 0
            Case ATLib.RotatorAd.Categories.Left : Return 1
            Case ATLib.RotatorAd.Categories.Right : Return 2
            Case ATLib.RotatorAd.Categories.MastHead : Return 3
            Case ATLib.RotatorAd.Categories.Homeleft : Return 4
            Case ATLib.RotatorAd.Categories.HomeRight : Return 5
        End Select

    End Function

    Private Sub deleteItem()
        Rotator.Deleted = True
        Rotator.Update()
        return2List()

    End Sub


    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0 : deleteItem()
            Case 1 : return2List()
            Case 2 : updateItem()
        End Select
    End Sub

End Class
