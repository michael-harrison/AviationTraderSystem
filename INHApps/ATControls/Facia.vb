Option Strict On
Option Explicit On


Public Class Facia

    Public Event ButtonClick(ByVal EnabledMask As ButtonMask, ByVal selectedMask As ButtonMask, ByVal buttonID As ButtonMask)

    Dim Orange As Color = Color.FromArgb(255, 254, 102, 2)
    Dim Green As Color = Color.FromArgb(255, 0, 255, 0)
    Dim DarkGrey As Color = Color.FromArgb(255, 60, 60, 60)
    Dim LightGrey As Color = Color.FromArgb(255, 200, 200, 200)

    Private _msgcount As Integer = 0


    Public Enum ButtonMask
        Run = &H1
        Pause = &H2
        Busy = &H4
        Err = &H8
        MoreorLess = &H10
        Quit = &H20
    End Enum

    Private _detailVisible As Boolean
    Private _busy As Boolean
    Private _err As Boolean

    Private _enabled As ButtonMask
    Private _selected As ButtonMask

    Public Property DetailVisible() As Boolean
        Get
            Return _detailVisible
        End Get
        Set(ByVal value As Boolean)
            _detailVisible = value
            ''         showhidedetail()
        End Set
    End Property


    Private Sub Facia_Load(ByVal sender As Object, ByVal e As system.EventArgs) Handles Me.Load
        makeButtonBar(ButtonBar1)
        ListBox1.Top = ButtonBar1.Bottom + 3
        ListBox1.Height = Me.Bottom - 3 - ListBox1.Top
    End Sub


    Private Sub makeButtonBar(ByVal ButtonBar As ButtonBar)
        '
        ' make up the ButtonBar
        '
        Dim Orange As Color = Color.FromArgb(255, 254, 102, 2)
        Dim Green As Color = Color.FromArgb(255, 0, 255, 0)
        Dim DarkGrey As Color = Color.FromArgb(255, 60, 60, 60)
        Dim LightGrey As Color = Color.FromArgb(255, 200, 200, 200)

        ButtonBar.Add(New ButtonBarButton("Run", False, True, False, DarkGrey, LightGrey, Green))
        ButtonBar.Add(New ButtonBarButton("Pause", False, True, False, DarkGrey, LightGrey, Green))
        ButtonBar.Add(New ButtonBarButton("Busy", False, False, False, Orange, DarkGrey, DarkGrey))
        ButtonBar.Add(New ButtonBarButton("Error", False, False, False, Color.Red, DarkGrey, DarkGrey))
        ButtonBar.Add(New ButtonBarButton("Less", True, True, False, DarkGrey, LightGrey, DarkGrey))
        ButtonBar.Add(New ButtonBarButton("Exit", False, True, False, DarkGrey, LightGrey, Green))

        Me.Controls.Add(ButtonBar)
        ButtonBar.Left = 0
        ButtonBar.Top = 0

    End Sub

    Public Sub SetEnabled(ByVal enabledMask As ButtonMask)
        '
        ' sets the enabled status of the button set
        '
        If (enabledMask And ButtonMask.Run) <> 0 Then ButtonBar1.Buttons(0).Enabled = True
        If (enabledMask And ButtonMask.Pause) <> 0 Then ButtonBar1.Buttons(1).Enabled = True
        If (enabledMask And ButtonMask.Busy) <> 0 Then ButtonBar1.Buttons(2).Enabled = True
        If (enabledMask And ButtonMask.Err) <> 0 Then ButtonBar1.Buttons(3).Enabled = True
        If (enabledMask And ButtonMask.MoreorLess) <> 0 Then ButtonBar1.Buttons(4).Enabled = True
        If (enabledMask And ButtonMask.Quit) <> 0 Then ButtonBar1.Buttons(5).Enabled = True
        Application.DoEvents()
        Me.Refresh()

    End Sub

    Public Sub ClearEnabled(ByVal enabledMask As ButtonMask)
        '
        ' sets the enabled status of the button set
        '
        If (enabledMask And ButtonMask.Run) <> 0 Then ButtonBar1.Buttons(0).Enabled = False
        If (enabledMask And ButtonMask.Pause) <> 0 Then ButtonBar1.Buttons(1).Enabled = False
        If (enabledMask And ButtonMask.Busy) <> 0 Then ButtonBar1.Buttons(2).Enabled = False
        If (enabledMask And ButtonMask.Err) <> 0 Then ButtonBar1.Buttons(3).Enabled = False
        If (enabledMask And ButtonMask.MoreorLess) <> 0 Then ButtonBar1.Buttons(4).Enabled = False
        If (enabledMask And ButtonMask.Quit) <> 0 Then ButtonBar1.Buttons(5).Enabled = False

    End Sub

    Public Sub Msg(ByVal value As String)
        '
        ' auto clear after 500 lines
        '
        _msgcount += 1
        If _msgcount > 500 Then
            ClearMsg()
            Msg("Auto-cleared after 500 lines")
        End If
        '
        ' if the line contains a newline sequence, break it at the first one.
        '
        Dim n As Integer = value.IndexOf(ControlChars.Lf)
        If n >= 0 Then
            Dim yy As String = value.Substring(0, n)
            Dim xx As String = value.Substring(n + 1, value.Length - (n + 1))

            Msg(value.Substring(0, n))
            Msg(value.Substring(n + 1, value.Length - (n + 1)))
        Else
            '
            ' if value is too long for one line, break into two or more lines
            '
            Dim maxlinelength As Integer = 100
            If value.Length > maxlinelength Then
                Msg(value.Substring(0, maxlinelength))
                Msg(value.Substring(maxlinelength, value.Length - maxlinelength))
            Else
                Dim timestamp As String = Format(Now, "M/d/yyyy hh:mm tt : ")
                ListBox1.Items.Add(timestamp & value)
                ListBox1.TopIndex = ListBox1.Items.Count - 1        'go to bottom of list
            End If
        End If
    End Sub

    Public Sub ClearMsg()
        ListBox1.Items.Clear()
        _msgcount = 0
    End Sub

    Private Sub showhidedetail(ByVal showhide As Boolean)
        '
        ' sets the control size according to the detail and fires
        ' event so parent can readjust its own size
        '
        If Not showhide Then
            Me.Height = ListBox1.Bottom + 3
        Else
            Me.Height = ButtonBar1.Bottom
        End If
    End Sub

    Private Sub button_Click(ByVal ButtonBar As ButtonBar, ByVal Button As ButtonBarButton) Handles ButtonBar1.Click
        Select Case Button.Index

            Case 0      'run
                RaiseEvent ButtonClick(_enabled, _selected, ButtonMask.Run)

            Case 1      'pause
                RaiseEvent ButtonClick(_enabled, _selected, ButtonMask.Pause)

            Case 4      'more or less
                If Button.Selected Then
                    Button.Text = "More"
                Else
                    Button.Text = "Less"
                End If
                showhidedetail(Button.Selected)
                RaiseEvent ButtonClick(_enabled, _selected, ButtonMask.MoreorLess)

            Case 5      'exit
                RaiseEvent ButtonClick(_enabled, _selected, ButtonMask.Quit)

        End Select
    End Sub


End Class
