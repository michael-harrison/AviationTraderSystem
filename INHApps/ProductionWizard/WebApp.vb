

Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Built in web app
'*
'* AUDIT TRAIL
'* 
'* V1.000   01-Dec-2009  BA  Original
'*
'*
'*
'*
'*
'***************************************************************************************

Public Class WebApp

    Private _objecttype As ATSystem.ObjectTypes
    Private _objectID As Integer
    Private _Slot As Slot
    Private _inhouseURL As String


    Public Property Slot() As Slot
        Get
            Return _Slot
        End Get
        Set(ByVal value As Slot)
            _Slot = value
        End Set
    End Property

    Public Property InhouseURL() As String
        Get
            Return _inhouseURL
        End Get
        Set(ByVal value As String)
            _inhouseURL = value
        End Set
    End Property

    Public Property ObjectID() As Integer
        Get
            Return _ObjectID
        End Get
        Set(ByVal value As Integer)
            _ObjectID = value
        End Set
    End Property

    Public Sub Render(ByVal objectType As ATSystem.ObjectTypes, ByVal objectID As Integer)
        _objecttype = objectType
        _objectID = objectID

        Dim Loader As New Loader
        Loader.SlotID = Slot.ID
        Loader.ObjectID = objectID

        Select Case objectType
       
        End Select
        '
        ' go directly to the requested page
        '
        WB.ScrollBarsEnabled = True
        Dim s As String = _inhouseURL & "/" & Loader.Target
        WB.Url = New Uri(s)
    End Sub


End Class
