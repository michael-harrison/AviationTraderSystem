Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Ajax test page
'*
'* ON ENTRY:
'*

'*
'***************************************************************************************

Partial Class BrowseAndSearch_AjaxTest
    Inherits System.Web.UI.Page

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Page.EnableViewState = True
    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            displayContent()
            displayThirdPanel()

        End If
        displayThirdPanel()


    End Sub

    Private Sub displayContent()
        time1.Text = Now.ToString
        time2.Text = Now.ToString


        Dim EA As New EnumAssistant(New Product.Types)

        DropDownList1.DataSource = EA
        DropDownList1.DataBind()

        DropdownList2.DataSource = EA
        DropdownList2.DataBind()




    End Sub

    Private Sub displayThirdPanel()
        time3.Text = Now.ToString
    End Sub


    ''Protected Sub VW2Btn1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles VW2Btn1.Click
    ''    time1.Text = Now.ToString

    ''End Sub

    ''Protected Sub VW2btn2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles VW2btn2.Click
    ''    time2.Text = Now.ToString
    ''End Sub


    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        time1.Text = Now.ToString
        time3.Text = Now.ToString
    End Sub

    Protected Sub DropdownList2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownList2.SelectedIndexChanged
        time2.Text = Now.ToString
        time3.Text = Now.ToString
    End Sub

  
    Protected Sub btn1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn1.Click
        time1.Text = Now.ToString
        time2.Text = Now.ToString
    End Sub

    Protected Sub btn2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn2.Click
        time2.Text = Now.ToString
    End Sub
End Class
