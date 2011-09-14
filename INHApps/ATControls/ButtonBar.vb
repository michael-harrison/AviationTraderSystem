Option Strict On
Option Explicit On

Public Class ButtonBar

    Private List As New List(Of ButtonBarButton)
    Private _selectedButton As ButtonBarButton = Nothing
    Private selectedIndex As Integer = -1

    Public Shadows Event Click(ByVal obj As ButtonBar, ByVal Button As ButtonBarButton)

    Default Public Property Item(ByVal index As Integer) As ButtonBarButton
        Get
            Return List(index)
        End Get
        Set(ByVal value As ButtonBarButton)
            List(index) = value
        End Set
    End Property

    Public ReadOnly Property SelectedButton() As ButtonBarButton
        Get
            If selectedIndex = -1 Then
                Return Nothing
            Else
                Return List(selectedIndex)
            End If
        End Get
    End Property

    Public Sub SetSelected(ByVal index As Integer)
        '
        ' allows an external caller to set the selected button
        ' but does not fire the selected button event
        '
        selectedIndex = index
        If Not List(index).RadioMode Then
            For Each b As ButtonBarButton In List
                If b.Enabled Then
                    If Not b.RadioMode Then
                        If b.Index = index Then
                            b.Selected = True
                        Else
                            b.Selected = False
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Public Function Add(ByVal value As ButtonBarButton) As Integer
        List.Add(value)
        Dim btnIndex As Integer = List.Count - 1
        value.Index = btnIndex
        value.TabIndex = btnIndex + 3
        '
        ' position button on screen and add to controls
        '
        value.Left = btnIndex * value.Width
        value.Top = 0
        Me.Controls.Add(value)
        Me.Width = value.Left + value.Width
        Me.Height = value.Height
        AddHandler value.Click, AddressOf buttonClick
        Return btnIndex
    End Function

    Public ReadOnly Property Buttons() As List(Of ButtonBarButton)
        Get
            Return List
        End Get
    End Property

    Private Sub buttonClick(ByVal index As Integer)
        '
        ' if the clicked button is in radiomode, dont touch other buttons
        ' otherwise set non-radio selected buttons to not selected
        '
        selectedIndex = index
        If Not List(index).RadioMode Then
            For Each b As ButtonBarButton In List
                If b.Enabled Then
                    If Not b.RadioMode Then
                        If b.Index <> index Then b.Selected = False
                    End If
                End If
            Next
        End If
        '
        ' ripple event to caller
        '
        RaiseEvent Click(Me, List(index))
    End Sub

End Class

