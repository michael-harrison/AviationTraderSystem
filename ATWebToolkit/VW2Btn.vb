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
Public Class VW2Btn
    Inherits WebControls.WebControl
    Implements IPostBackEventHandler

    Public Event Click As EventHandler

    Private _baseClass As String
    Private _navigateURL As String
    Private _onClientClick As String
    Private _text As String
    Private _scriptmanager As System.Web.UI.ScriptManager
    Private _scriptmanagerName As String

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Property IsPostBackMode() As Boolean
        Get
            Dim b As Boolean = Convert.ToBoolean(ViewState("IsPostBackMode"))
            Return b
        End Get

        Set(ByVal Value As Boolean)
            ViewState("IsPostBackMode") = Value
        End Set
    End Property



    Public Property OnClientClick() As String
        Get
            Return _onClientClick
        End Get
        Set(ByVal value As String)
            _onClientClick = value
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Property NavigateURL() As String
        Get
            Dim s As String = CStr(ViewState("NavigateURL"))
            If s Is Nothing Then
                Return "[" + Me.ID + "]"
            Else
                Return s
            End If
        End Get

        Set(ByVal Value As String)
            ViewState("NavigateURL") = Value
        End Set
    End Property

    Public Property ScriptManager() As ScriptManager
        Get
            Return _scriptmanager
        End Get
        Set(ByVal value As ScriptManager)
            _scriptmanager = value
        End Set
    End Property

    Public Property ScriptManagerName() As String
        Get
            Return _scriptmanagerName
        End Get
        Set(ByVal value As String)
            _scriptmanagerName = value
        End Set
    End Property


    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property


    Public Overrides Property CSSClass() As String
        Get
            Return _baseClass
        End Get
        Set(ByVal value As String)
            _baseClass = value
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



    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        '
        ' wrap the base class in a div which specifies the button class
        '
        Dim div As New HtmlControls.HtmlGenericControl("div")
        Me.Controls.Add(div)
        div.Attributes.Add("id", UniqueID)
        div.Attributes.Add("class", _baseClass)
        '
        ' add an <a> tag and set url or postback url
        '
        Dim a As New HtmlControls.HtmlGenericControl("a")
        div.Controls.Add(a)
        If IsPostBackMode Then
            Dim CSM As ClientScriptManager = Me.Page.ClientScript
            Dim pbURL As String = "javascript:" & CSM.GetPostBackEventReference(Me, Me.ID.ToString)
            a.Attributes.Add("href", pbURL)
            If _onClientClick <> "" Then a.Attributes.Add("onclick", _onClientClick)
        Else
            If NavigateURL <> "" Then a.Attributes.Add("href", NavigateURL)
            If _onClientClick <> "" Then a.Attributes.Add("onclick", _onClientClick)
        End If
        '
        ' add span as inner text
        '
        Dim span As New HtmlControls.HtmlGenericControl("span")
        a.Controls.Add(span)
        span.InnerText = _text
        '
        ' render the control
        '

        Me.RenderContents(writer)

    End Sub

  

End Class
