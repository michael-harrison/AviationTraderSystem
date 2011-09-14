Option Strict On
Option Explicit On


Public Class ButtonBarButton

    Private _disabledColor As Color
    Private _enabledColor As Color
    Private _selectedColor As Color
    Private _index As Integer

    Private _Enabled As Boolean
    Private _selected As Boolean
    Private _radioMode As Boolean

    Public Shadows Event Click(ByVal index As Integer)

    Public Property Index() As Integer
        Get
            Return _index
        End Get
        Set(ByVal value As Integer)
            _index = value
        End Set
    End Property


    Public Overrides Property Text() As String
        Get
            Return btn.Text
        End Get
        Set(ByVal value As String)
            btn.Text = value
        End Set
    End Property



    Public Property DisabledColor() As Color
        Get
            Return _disabledColor
        End Get
        Set(ByVal value As Color)
            _disabledColor = value
        End Set
    End Property

    Public Property EnabledColor() As Color
        Get
            Return _enabledColor
        End Get
        Set(ByVal value As Color)
            _enabledColor = value
        End Set
    End Property

    Public Property SelectedColor() As Color
        Get
            Return _selectedColor
        End Get
        Set(ByVal value As Color)
            _selectedColor = value
        End Set
    End Property


    Public Overloads Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            If value Then
                btn.ForeColor = _enabledColor
            Else
                btn.ForeColor = _disabledColor
                _selected = False                 'disabled cant be selected
            End If
            _Enabled = value
        End Set
    End Property

    Public Property Selected() As Boolean
        Get
            Return _selected
        End Get
        Set(ByVal value As Boolean)
            '
            ' can only select enabed buttons
            '
            If _Enabled Then
                If value Then
                    btn.ForeColor = _selectedColor
                Else
                    btn.ForeColor = _enabledColor
                End If
                _selected = value
            End If
        End Set
    End Property

    Public Property RadioMode() As Boolean
        Get
            Return _radioMode
        End Get
        Set(ByVal value As Boolean)
            _radioMode = value
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
    End Sub


    Public Sub New(ByVal vtext As String, _
    ByVal vradiomode As Boolean, _
    ByVal vEnabled As Boolean, _
    ByVal vSelected As Boolean, _
    ByVal venabledColor As Color, _
    ByVal vdisabledColor As Color, _
    ByVal vselectedColor As Color)

        InitializeComponent()
        _index = -1             'this must get set by caller
        Text = vtext
        _enabledColor = venabledColor
        _disabledColor = vdisabledColor
        _selectedColor = vselectedColor

        Enabled = vEnabled
        RadioMode = vradiomode
        If _Enabled Then Selected = vSelected

    End Sub


    Private Sub FaciaButton_Click(ByVal sender As Object, ByVal e As system.EventArgs) Handles btn.Click
        '
        ' handles the way the button reacts to a click. the click is passed up to the caller
        '
        If _Enabled Then
            If _radioMode Then
                Selected = Not Selected
                RaiseEvent Click(_index)
            Else
                If Not Selected Then
                    Selected = True
                    RaiseEvent Click(_index)
                End If
            End If
        End If
    End Sub


End Class
