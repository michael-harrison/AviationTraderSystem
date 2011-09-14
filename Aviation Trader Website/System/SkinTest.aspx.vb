Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Home page
'*
'* 
'*
'*
'***************************************************************************************

Partial Class SkinTest
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Protected beltWidth As Integer

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.Skin

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue + 1

        Page.Theme = Slot.Skin

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim EA As New EnumAssistant(New ATSystem.Skins)
            skindd.datasource = EA
            skindd.databind()
            skindd.selectedvalue = Slot.Skin
        End If

        displayLeftMenu()
        displayRightPanel()
        displayContent()

    End Sub

    Private Sub displayLeftMenu()
        Dim s As String = ""
        Dim length As Integer = Convert.ToInt32(leftLength.text)
        For i As Integer = 1 To length
            s &= "left " & i & "<br>"
        Next
        Left.Text = s

    End Sub

    Private Sub displayRightPanel()
        Dim s As String = ""
        Dim length As Integer = Convert.ToInt32(rightLength.text)
        For i As Integer = 1 To length
            s &= "right " & i & "<br>"
        Next
        Right.Text = s

    End Sub

    Private Sub displayContent()
        Dim s As String = ""
        Dim length As Integer = Convert.ToInt32(contentLength.Text)
        For i As Integer = 1 To length
            s &= "content " & i & "<br>"
        Next
        content2.Text = s

    End Sub

    Private Sub refresh()
        Slot.Usr.Skin = skinDD.SelectedValue
        Slot.Usr.Update()
    End Sub







    Protected Sub BtnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRefresh.Click
        refresh()
    End Sub
End Class
