Option Explicit On
Option Strict On
Imports ATLib
Imports ATControls


'***************************************************************************************
'*
'* DisplayWizard
'*
'* AUDIT TRAIL
'* 
'* V1.000   25-NOV-2009  BA  Original
'*
'* Display ad wizard
'*
'*
'*
'*
'***************************************************************************************

Public Class Form1

    Private slot As Slot
    Private Sys As ATSystem
    Private currentObjectID As Integer
    Private currentObjectType As ATSystem.ObjectTypes
    Private Const controlPad As Integer = 10
    Private formName As String
    Private Tree As DataTree
    Private INDService As EngineLib.INDService

    Friend Const CheckoutText As String = "Check Out"
    Friend Const CheckinText As String = "Check In"

    Private Enum buttonID
        None = -1
        Ad
        Image
        WebApp
        Logout
    End Enum

    Private Sub TPForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ContentPanel.TabIndex = 0
        Dim splash As New Splash
        splash.ErrorBox.Visible = False

        showControl(splash)
        Sys = New ATSystem
        Try

            Sys.Retrieve()
            newTree()
            '
            ' get a reference to INDService
            '
            INDService = New EngineLib.INDService
            Me.Text = Me.Text & " " & Sys.SWVersion & " connected to " & Sys.Name

        Catch ex As Exception

            splash.Errorbox.Visible = True
            splash.Errorbox.Text = ex.Message

        End Try

    End Sub

    Private Sub newTree()
        Tree = New DataTree

        AddHandler Tree.NodeSelect, AddressOf Tree_NodeSelect
        Tree.Dock = DockStyle.Fill
        TreePanel.Controls.Clear()
        TreePanel.Controls.Add(Tree)
        Tree.Render()
    End Sub



    Private Sub Tree_NodeSelect(ByVal Node As TreeNode)
        '
        ' get the tree node params
        '
        Dim hexTag As String = Node.Tag.ToString
        Dim nodeType As ATSystem.ObjectTypes = CType(Hex2Int(hexTag.Substring(0, 2)), ATSystem.ObjectTypes)

        Select Case nodeType

            Case ATSystem.ObjectTypes.AdInstance
                Dim InstanceID As Integer = CommonRoutines.Hex2Int(hexTag.Substring(2, 8))
                Dim iv As New InstanceViewer()
                iv.InstanceID = InstanceID
                iv.physicalapplicationpath = Sys.PhysicalApplicationPath
                iv.prodnpdffolder = Sys.ProdnPDFFolder
                iv.INDService = INDService
                iv.THBHeight = Sys.THBImageHeight
                iv.LoresHeight = Sys.LRImageHeight
                iv.Render()
                showControl(iv)

            Case Else
                dropControl()

        End Select

    End Sub

    Private Sub showControl(ByVal c As Control)
        c.Dock = DockStyle.Fill
        ContentPanel.Controls.Clear()           'clear existing control
        ContentPanel.Controls.Add(c)            'show new control
    End Sub

    Private Sub clearControl()
        ContentPanel.Controls.Clear()
    End Sub

    Private Sub dropControl()
        ContentPanel.Controls.Clear()           'clear existing control
        Dim dropAd As New DropAd
        ContentPanel.Controls.Add(dropAd)            'show new control
        AddHandler DropAd.DropAdEvent, AddressOf dropad_adDropped

    End Sub


    Protected Sub dropad_adDropped(ByVal AdInstance As AdInstance)
        newTree()     'get a new tree and set to dropped context
        clearControl()        'clear current content panel

        Dim iv As New InstanceViewer()
        iv.InstanceID = AdInstance.ID
        iv.physicalapplicationpath = Sys.PhysicalApplicationPath
        iv.prodnpdffolder = Sys.ProdnPDFFolder
        iv.INDService = INDService
        iv.THBHeight = Sys.THBImageHeight
        iv.LoresHeight = Sys.LRImageHeight

        iv.Render()
        showControl(iv)
    End Sub

End Class
