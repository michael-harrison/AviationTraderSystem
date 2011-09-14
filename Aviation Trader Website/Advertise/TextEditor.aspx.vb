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

Partial Class Advertise_TextEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Ad As Ad


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

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)


        If Not IsPostBack Then
            displayContent()
        End If


        displaytabBar()
        displayNavButtons()

    End Sub


    Private Sub displayNavButtons()
        Loader.NextASPX = ATLib.Loader.ASPX.AdCategorySelector
        BtnCategorySelector.NavigateURL = Loader.Target

    End Sub

    Private Sub displaytabBar()
        '
        ' tab bar is in postback mode - see event handler below
        '
        Dim topnode As MenuNode

        topnode = New MenuNode("A", "Text", "", True)
        tabbar.Nodes.Add(topnode)

        topnode = New MenuNode("B", "Images", "", False)
        tabbar.Nodes.Add(topnode)

        topnode = New MenuNode("C", "PDF Upload", "", False)
        tabbar.Nodes.Add(topnode)

        ''topnode = New MenuNode("D", "Specs", "", False)
        ''tabbar.Nodes.Add(topnode)

        topnode = New MenuNode("E", "Requests", "", False)
        tabbar.Nodes.Add(topnode)

    End Sub



    Private Sub displayContent()
        '
        '  set the prodns status drop down
        '
        AdText.Text = Ad.Text
    End Sub


    Private Sub updateContent()
        Ad.Text = AdText.Text
        Ad.Update()

    End Sub

    Protected Sub tabbar_TopMenuEvent(ByVal sender As Object, ByVal TabID As String) Handles tabbar.TopMenuEvent
        updateContent()
        Select Case TabID
            Case "A" : Loader.NextASPX = ATLib.Loader.ASPX.AdTextEditor
            Case "B" : Loader.NextASPX = ATLib.Loader.ASPX.AdImageUploader
            Case "C" : Loader.NextASPX = ATLib.Loader.ASPX.AdPDFUploader
            Case "D" : Loader.NextASPX = ATLib.Loader.ASPX.AdSpecEditor
            Case "E" : Loader.NextASPX = ATLib.Loader.ASPX.AdProdnNote
        End Select
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub BtnCategorySelector_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCategorySelector.Click
        updateContent()
        Loader.NextASPX = ATLib.Loader.ASPX.AdCategorySelector
        Response.Redirect(Loader.Target)
    End Sub

    Protected Sub BtnReview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReview.Click
        updateContent()
        Loader.NextASPX = ATLib.Loader.ASPX.AdReview
        Response.Redirect(Loader.Target)
    End Sub
End Class
