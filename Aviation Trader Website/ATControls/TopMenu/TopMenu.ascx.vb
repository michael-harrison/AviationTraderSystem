Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Top Menu
'*
'* 
'*
'*
'***************************************************************************************

Partial Class ATControls_TopMenu
    Inherits System.Web.UI.UserControl

    Public Event TopMenuEvent(ByVal sender As Object, ByVal TabID As String)

    Private _Nodes As New List(Of MenuNode)
    Private _CSSClass As String
    Private _isPostBackMode As Boolean
    Private _currentTabID As String

    Public ReadOnly Property Nodes() As List(Of MenuNode)
        Get
            Return _Nodes
        End Get
    End Property

    Public Property CSSClass() As String
        Get
            Return _CSSClass
        End Get
        Set(ByVal value As String)
            _CSSClass = value
        End Set
    End Property

    Public Property IsPostBackMode() As Boolean
        Get
            Return _isPostBackMode
        End Get
        Set(ByVal value As Boolean)
            _isPostBackMode = value
        End Set
    End Property

    Public ReadOnly Property CurrentTabID() As String
        Get
            Return _currentTabid
        End Get

    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

      
        Me.EnableViewState = False

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        buildControl()
    End Sub


    Private Sub buildControl()
        '
        ' build the UL control tree dynamically from the menu list
        '
        Me.Controls.Clear()

        Dim div As New HtmlControls.HtmlGenericControl

        div.TagName = "div"
        div.ID = "div"

        Me.Controls.Add(div)

        Dim UL As New HtmlControls.HtmlGenericControl
        UL.TagName = "ul"
        UL.ID = "ul"

        UL.Attributes.Add("class", _CSSClass)
        div.Controls.Add(UL)

		Dim i As Integer = 0

        For Each level1node As MenuNode In _Nodes
            Dim LI As New HtmlControls.HtmlGenericControl
            LI.TagName = "li"

		UL.Controls.Add(LI)

		Dim span As New HtmlControls.HtmlGenericControl
		If level1node.Selected Then span.Attributes.Add("class", "selected")
		span.InnerText = level1node.Text

		If _isPostBackMode Then
			Dim LB As New LinkButton
			LB.ID = level1node.ID
			If level1node.PoleText.Length > 0 Then
				LB.Attributes.Add("onclick", "showPole(500);")
			End If
			AddHandler LB.Click, AddressOf LBHandler
			If level1node.Selected Then LB.CssClass = "selected"
			LI.Controls.Add(LB)
			LB.Controls.Add(span)
		Else
			Dim HL As New HyperLink
			HL.ID = level1node.ID
			If level1node.PoleText.Length > 0 Then
				HL.Attributes.Add("onclick", "showPole(500);")
			End If
			HL.NavigateUrl = level1node.NavTarget
			If level1node.Selected Then HL.CssClass = "selected"
			LI.Controls.Add(HL)
			HL.Controls.Add(span)
		End If

            i += 1

            If level1node.Nodes.Count > 0 Then
                Dim UL2 As New HtmlControls.HtmlGenericControl
                UL2.TagName = "ul"
                LI.Controls.Add(UL2)

                For Each level2node As MenuNode In level1node.Nodes
                    Dim LI2 As New HtmlControls.HtmlGenericControl
                    LI2.TagName = "li"
                    UL2.Controls.Add(LI2)
                    Dim A2 As New HtmlControls.HtmlGenericControl
                    A2.TagName = "a"
                    A2.Attributes.Add("href", level2node.NavTarget)
                    If level2node.StyleModifier.Length > 0 Then A2.Attributes.Add("Style", level2node.StyleModifier)
                    A2.InnerText = level2node.Text
                    LI2.Controls.Add(A2)
                Next
            End If
        Next
    End Sub

    Public Sub SetCurrentTab(ByVal TabID As String)
        '
        ' called on a postback only to set a new tab number in the top level
        '
        If _isPostBackMode And Me.IsPostBack Then

            Dim div As Control = Me.FindControl("div")
            Dim ul As Control = div.FindControl("ul")
            For Each li As Control In ul.Controls
                For Each c As Control In li.Controls
                    Dim lb As LinkButton = DirectCast(li.Controls(0), LinkButton)
                    lb.CssClass = ""
                    If lb.ID = TabID Then lb.CssClass = "selected"
                    Dim x As Control = lb.Controls(0)
                    Dim span As HtmlControl = DirectCast(x, HtmlControl)
                    span.Attributes.Clear()
                    If lb.ID = TabID Then span.Attributes.Add("class", "selected")
                Next
            Next
        End If
    End Sub

    Protected Sub LBHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        '
        ' ripple event to caller
        '
        Dim LB As LinkButton = DirectCast(sender, LinkButton)
        _currentTabID = LB.ID
        RaiseEvent TopMenuEvent(Me, _currentTabID)
    End Sub


End Class



Public Class MenuNode
    Private _nodes As New List(Of MenuNode)
    Private _NavTarget As String
    Private _Text As String
    Private _Selected As Boolean
    Private _ID As String
    Private _PoleText As String
    Private _styleModifier As String

    Public Sub New()
        _PoleText = ""
        _styleModifier = ""
    End Sub

    Public Sub New(ByVal ID As String, ByVal Text As String, ByVal NavTarget As String, ByVal Selected As Boolean)
        _ID = ID
        _NavTarget = NavTarget
        _Text = Text
        _Selected = Selected
        _PoleText = ""
        _styleModifier = ""
    End Sub

    Public Sub New(ByVal ID As String, ByVal Text As String, ByVal NavTarget As String, ByVal Selected As Boolean, ByVal PoleText As String)
        _ID = ID
        _NavTarget = NavTarget
        _Text = Text
        _Selected = Selected
        _PoleText = Poletext
    End Sub

    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
        End Set
    End Property


    Public ReadOnly Property Nodes() As List(Of MenuNode)
        Get
            Return _nodes
        End Get
    End Property

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


    Public Property StyleModifier() As String
        Get
            Return _styleModifier
        End Get
        Set(ByVal value As String)
            _styleModifier = value
        End Set
    End Property


    Public Property PoleText() As String
        Get
            Return _PoleText
        End Get
        Set(ByVal value As String)
            _PoleText = value
        End Set
    End Property

    Public Property Selected() As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal value As Boolean)
            _Selected = value
        End Set
    End Property



    Public Sub Add(ByVal menuNode As MenuNode)
        _nodes.Add(menuNode)
    End Sub



End Class
