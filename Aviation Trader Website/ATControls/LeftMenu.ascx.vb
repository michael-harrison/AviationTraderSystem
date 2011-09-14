Option Explicit On
Option Strict On
Imports System.ComponentModel
Imports ATLib


'***************************************************************************************
'*
'* Menu
'*
'* AUDIT TRAIL
'* 
'* V1.000   10-JUN-2009  BA  Original
'*
'* Displays the menu bar as a function of login level UAM and current node
'*
'***************************************************************************************
Partial Class ATControls_LeftMenu
    Inherits System.Web.UI.UserControl

    Private _nodeItems As New Generic.List(Of LeftMenuItem)


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.EnableViewState = False
    End Sub

    Protected Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        MenuList.DataSource = _nodeItems
        MenuList.DataBind()
    End Sub


    Public Sub Add(ByVal Name As String, ByVal NavTarget As String, ByVal Selected As Boolean)
        '
        ' adds an item to the icon list  
        '
        Dim ni As New LeftMenuItem()
        ni.NavTarget = NavTarget
        ni.Selected = Selected
        ni.text = Name
        _nodeItems.Add(ni)

    End Sub

    Public Sub Add(ByVal leftMenuItem As LeftMenuItem)
        _nodeItems.Add(LeftMenuItem)
    End Sub

    Public ReadOnly Property Items() As Generic.List(Of LeftMenuItem)
        Get
            Return _nodeItems
        End Get
    End Property

    Public ReadOnly Property Items(ByVal index As Integer) As LeftMenuItem
        Get
            Return _nodeItems(index)
        End Get
    End Property




End Class

Public Class LeftMenuItem
    '
    ' Used to databind to the display control
    '
    Private _NavTarget As String
    Private _Text As String
    Private _cssClass As String
    Private _Selected As Boolean

    Public Sub New()

    End Sub

    Public Sub New(ByVal Text As String, ByVal Navtarget As String, ByVal Selected As Boolean)
        _Text = Text
        _NavTarget = Navtarget
        _Selected = Selected
    End Sub

    Public Property NavTarget() As String
        Get
            Return _NavTarget
        End Get
        Set(ByVal value As String)
            _NavTarget = value

        End Set
    End Property

    Public Property Text() As String
        Get
            Return _Text
        End Get
        Set(ByVal value As String)
            _Text = value
        End Set
    End Property

    Public Property Selected() As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal value As Boolean)
            _Selected = value
            _cssClass = ""
            If _Selected Then _cssClass = "selected"
        End Set
    End Property


    Public ReadOnly Property CSSClass() As String
        Get
            Return _cssClass
        End Get
    End Property
End Class





