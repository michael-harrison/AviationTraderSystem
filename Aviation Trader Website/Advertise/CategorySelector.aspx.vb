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

Partial Class CategorySelector
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private tabID As String

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
            displayContent("A")
        End If

    End Sub


    Private Sub displayContent(ByVal tabID As String)
        '
        ' set the category and class drop downs
        '
        CatDD.DataSource = sys.Categories
        '
        ' get the right set of classifications for the current category
        '
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

    End Sub


    Private Sub updateCategory()
        '
        ' if the classification has changed get the new classification, discard current specs
        ' and get a new set based on the classificationID
        '
        Dim selectedClassifiationID As Integer = CommonRoutines.Hex2Int(ClsDD.SelectedValue)
        If selectedClassifiationID <> Ad.ClassificationID Then
            Ad.DeleteSpecs()
            Ad.ClassificationID = selectedClassifiationID
            Ad.AddSpecs()
        End If
        Ad.Update()
    End Sub


    Protected Sub CatDD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CatDD.SelectedIndexChanged
        '
        ' rebind the cls dropdown
        '
        Dim selectedCat As Integer = CatDD.SelectedIndex
        Dim classifications As Classifications = sys.Categories(selectedCat).Classifications
        ClsDD.DataSource = classifications
        ClsDD.DataBind()
        '
        ' discard the current ad specs and get a new set based on the first classification
        '
        Ad.DeleteSpecs()
        Ad.ClassificationID = classifications(0).ID
        Ad.AddSpecs()
        Ad.Update()

    End Sub


    Protected Sub BtnContentEditor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnContentEditor.Click
        updateCategory()
        Loader.NextASPX = ATLib.Loader.ASPX.AdTextEditor
        Response.Redirect(Loader.Target)
    End Sub



    Protected Sub BtnProductSelector_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnProductSelector.Click
        updateCategory()
        Loader.NextASPX = ATLib.Loader.ASPX.AdProductSelector
        Response.Redirect(Loader.Target)
    End Sub


End Class
