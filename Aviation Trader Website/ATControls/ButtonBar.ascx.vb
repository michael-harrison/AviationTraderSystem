Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* CategoryEditor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = 
'*
'***************************************************************************************


Partial Class ATControls_ButtonBar
    Inherits System.Web.UI.UserControl

    Public Event buttonBarEvent(ByVal buttonNumber As Integer)

    Public B0 As New BBButton
    Public B1 As New BBButton
    Public B2 As New BBButton

    Private _msg As String = ""
    Private _scriptmanager As System.Web.UI.ScriptManager
    Private _scriptmanagerName As String



    Public Property Msg() As String
        Get
            Return _msg
        End Get
        Set(ByVal value As String)
            _msg = value
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        B0.setUniqueID(a0.UniqueID)
        B1.setUniqueID(a1.UniqueID)
        B2.setUniqueID(a2.UniqueID)

    End Sub





    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        '
        ' setup scripmanager
        '
        If Not _scriptmanagerName Is Nothing Then
            Dim c As Control = Page.FindControl(_scriptmanagerName)
            If Not c Is Nothing Then
                _scriptmanager = CType(c, ScriptManager)
            End If
        End If
        '
        ' script manager may have been registered programatically
        '
        If Not _scriptmanager Is Nothing Then
            _scriptmanager.RegisterAsyncPostBackControl(a0)
            _scriptmanager.RegisterAsyncPostBackControl(a1)
            _scriptmanager.RegisterAsyncPostBackControl(a2)

        End If
        '
        ' prime the four buttons in the control
        '
        primeButton(a0, B0)
        primeButton(a1, B1)
        primeButton(a2, B2)


        msgbox.Text = _msg

    End Sub

    Private Sub primeButton(ByVal a As ATWebToolkit.VW2Btn, ByVal bb As BBButton)
        If Not String.IsNullOrEmpty(bb.Text) Then
            a.Visible = True
            a.Text = bb.Text
            If Not String.IsNullOrEmpty(bb.OnClientClick) Then a.OnClientClick = bb.OnClientClick
            bb.setUniqueID(a.UniqueID)
        End If
    End Sub


    Protected Sub Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles a0.Click, a1.Click, a2.Click

        Dim buttonNo As Integer = Convert.ToInt32(CType(sender, ATWebToolkit.VW2Btn).ID.Substring(1, 1))
        RaiseEvent buttonBarEvent(buttonNo)
    End Sub

End Class

Public Class BBButton

    Private _onClientClick As String = ""
    Private _Text As String = ""
    Private _uniqueID As String = ""

    Public Property Text() As String
        Get
            Return _Text
        End Get
        Set(ByVal value As String)
            _Text = value
        End Set
    End Property


    Public Property OnClientClick() As String
        Get
            Return _onclientClick
        End Get
        Set(ByVal value As String)
            _onclientClick = value
        End Set
    End Property

    Public ReadOnly Property UniqueID() As String
        Get
            Return _UniqueID
        End Get
    End Property

    Friend Sub setUniqueID(ByVal value As String)
        _uniqueID = value
    End Sub


End Class
