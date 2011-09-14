Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

'***************************************************************************************
'*
'* Implements a server side click event for a table cell, not provided by standard asp.net
'*
'* AUDIT TRAIL
'* 
'* V1.000   18-AUG-2008  BA  Original
'*
'***************************************************************************************



<DefaultProperty("Text"), ToolboxData("<{0}:ClickableCell runat=server></{0}:ClickableCell>")> _
Public Class ClickableCell
    Inherits WebControls.TableCell
    Implements IPostBackEventHandler

    Public Event Click As EventHandler

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Property Text() As String
        Get
            Dim s As String = CStr(ViewState("Text"))
            If s Is Nothing Then
                Return "[" + Me.ID + "]"
            Else
                Return s
            End If
        End Get

        Set(ByVal Value As String)
            ViewState("Text") = Value
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Property Value() As String
        Get
            Dim s As String = CStr(ViewState("Value"))
            If s Is Nothing Then
                Return "[" + Me.ID + "]"
            Else
                Return s
            End If
        End Get

        Set(ByVal Value As String)
            ViewState("Value") = Value
        End Set
    End Property



    Private Sub Cell_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
        'the following two lines allow a click on the custom control to post back 
        '
        Dim CSM As ClientScriptManager = Me.Page.ClientScript
        Me.Attributes.Add("OnClick", CSM.GetPostBackEventReference(Me, Me.ID.ToString))
    End Sub

    Protected Overridable Sub OnClick(ByVal e As EventArgs)
        RaiseEvent Click(Me, e)
    End Sub

    Public Sub RaisePostBackEvent(ByVal eventArgument As String) Implements IPostBackEventHandler.RaisePostBackEvent
        OnClick(New EventArgs())
    End Sub



    Protected Overrides Sub RenderContents(ByVal output As HtmlTextWriter)
        output.Write(Text)
    End Sub

End Class
