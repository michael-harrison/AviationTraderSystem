Option Explicit On
Option Strict On

'***************************************************************************************
'*
'* BarberPole
'*
'* AUDIT TRAIL
'* 
'* V1.000   02-JUL-2009  BA  Original
'*
'*
'***************************************************************************************

Partial Class ATControls_BarberPole
    Inherits System.Web.UI.UserControl

    Private _top As String
    Private _left As String
    Private _msg As String
    Private _visible As Boolean = False

    Protected poleClientID As String
    Protected msgClientID As String



    Public Property Top() As String
        Get
            Return _top
        End Get
        Set(ByVal value As String)
            _top = value

        End Set
    End Property

    Public Property Left() As String
        Get
            Return _left
        End Get
        Set(ByVal value As String)
            _left = value

        End Set
    End Property

    Public Property IsVisible() As Boolean
        Get
            Return _visible
        End Get
        Set(ByVal value As Boolean)
            _visible = value
        End Set
    End Property

    Public Property Msg() As String
        Get
            Return _msg
        End Get
        Set(ByVal value As String)
            _msg = value

        End Set
    End Property

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As system.EventArgs) Handles Me.PreRender
        MsgBox.Text = Msg
        pole.Style("top") = _top
        pole.Style("left") = _left
        If _visible Then pole.Style("visibility") = "visible"
        poleClientID = pole.ClientID
        msgClientID = MsgBox.ClientID
        '
        ' set the fixed positon to absolute for IE6 or less
        '
        If Request.Browser.Type = "IE6" Then pole.Style("position") = "absolute"

    End Sub
End Class
