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

Partial Class Advertise_ImageUploader
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
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue


        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ads As New Ads
        Ad = ads.Retrieve(Loader.ObjectID)
        UploadBar.sys = sys
        UploadBar.ad = Ad


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

        topnode = New MenuNode("A", "Text", "", False)
        tabbar.Nodes.Add(topnode)

        topnode = New MenuNode("B", "Images", "", True)
        tabbar.Nodes.Add(topnode)

        topnode = New MenuNode("C", "PDF Upload", "", False)
        tabbar.Nodes.Add(topnode)

        ''topnode = New MenuNode("D", "Specs", "", False)
        ''tabbar.Nodes.Add(topnode)

        topnode = New MenuNode("E", "Requests", "", False)
        tabbar.Nodes.Add(topnode)

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
            Dim isMainImageRadio As ATWebToolkit.GroupRadioButton = DirectCast(r.FindControl("ismainimageradio"), ATWebToolkit.GroupRadioButton)
            Dim image As Image = Ad.Images(i)
            image.IsMainImage = isMainImageRadio.Checked
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
        displayContent()

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

    Protected Sub UploadBar_ImageDeleteEvent() Handles UploadBar.ImageDeleteEvent
        updateContent()
    End Sub

    Protected Sub UploadBar_ImageUploadEvent(ByVal Image As ATLib.Image) Handles UploadBar.ImageUploadEvent
        displayContent()
    End Sub


End Class
