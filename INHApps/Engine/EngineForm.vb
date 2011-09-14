Option Explicit On
Option Strict On
Imports ATLib
Imports ATControls
Imports System.IO
Imports System.Runtime.Remoting

'***************************************************************************************
'*
'* Engine
'*
'* AUDIT TRAIL
'* 
'* V1.000   25-OCT-2007  BA  Original
'*
'* front end driver for InDesign Sevrver
'*
'*
'*
'*
'***************************************************************************************


Public Class EngineForm

    Private formName As String


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As system.EventArgs) Handles Me.Load
        '
        ' init facia and line up the control and the form
        '
        Facia1.DetailVisible = True
        Facia1.SetBounds(0, 0, Facia1.Width, Facia1.Height)
        Me.Width = Facia1.Width + 5
        setMyheight()

        Try
            GL.Sys = New ATSystem
            GL.Sys.Retrieve()
            formName = Me.Text & " " & GL.Sys.SWVersion
            Me.Text = formName
            '
            ' raise local object and publish for access by all clients
            '
            GL.Engine = Sys.MapEngine(ATSystem.EngineModes.Server)
            AddHandler Engine.EngineEvent, AddressOf EQItemAdded
            Facia1.Msg(Engine.Ident)
            '
            ' map to InDesign
            '
            Facia1.SetEnabled(ATControls.Facia.ButtonMask.Busy)
            Facia1.Msg("Launching InDesign...")
            Me.Show()
            System.Windows.Forms.Application.DoEvents()
            '
            ' ID launch may not work if we are starting too quickly
            '
            Try

                GL.INDService = New EngineLib.INDService
                GL.INDService.InitializeINDD()
  
            Catch ex As Exception

                Facia1.ClearEnabled(ATControls.Facia.ButtonMask.Busy)
                Facia1.SetEnabled(ATControls.Facia.ButtonMask.Err)
                Facia1.Msg("Could not start InDesign - wait a few seconds then try again")
            End Try

            If Not GL.INDService.INDApp Is Nothing Then
                Facia1.Msg("Successfully connected to " & GL.INDService.INDApp.Name & " " & GL.INDService.INDApp.Version)
                '
                ' minimise the app
                '
                GL.INDService.minimizeInDesign()
                
                Facia1.ClearEnabled(ATControls.Facia.ButtonMask.Busy)
            End If

        Catch ex As Exception
            Facia1.Msg("Failed to start because " & ex.Message)
            Facia1.SetEnabled(Facia.ButtonMask.Quit)
            Facia1.ClearEnabled(Facia.ButtonMask.Run Or Facia.ButtonMask.Pause)
        End Try

    End Sub



    Private Sub setMyheight()
        Me.Height = Facia1.Bottom + 25         ' resize my own app if the facia changes
    End Sub



    Private Sub runProcess()
        GL.Sys.Retrieve()             're-read the sys object to get changes
        Me.Text = formName & " connected to " & GL.Sys.Name
        Facia1.Msg("Engine running")
        Facia1.ClearEnabled(Facia.ButtonMask.Quit)
        Engine.Status = Engine.EngineStates.Running       ' showengineservices as running
    End Sub


    Private Sub PauseProcess()
        Me.Text = formName
        Facia1.Msg("Engine paused by operator")
        Facia1.SetEnabled(Facia.ButtonMask.Quit)
        Engine.Status = Engine.EngineStates.Paused          'show engineservices as paused
    End Sub

    Private Sub EQItemAdded()
        '
        ' called when a new item is added to the engine q
        ' starts a new thread to process all items in the q
        '
        Me.BeginInvoke(New processDelegate(AddressOf processQ), New Object() {Engine})

    End Sub

    Private Delegate Sub processDelegate(ByVal Engine As ATLib.Engine)

    Private Sub processQ(ByVal engine As ATLib.Engine)
        '
        ' set engine to busy and process entire contents of q
        '

        Dim Q As EQItem
        engine.Status = engine.EngineStates.Busy
        Facia1.SetEnabled(ATControls.Facia.ButtonMask.Busy)
        Facia1.ClearEnabled(Facia.ButtonMask.Pause Or Facia.ButtonMask.Quit)
        refreshMe()
        '
        ' put a non-fatal try-catch in here in case anything goes wrong with the dequeue
        '
        Try

            Do While engine.EngineQ.Count > 0
                Me.Show()
                Windows.Forms.Application.DoEvents()                'let the form update

                Q = engine.EngineQ(0)               'get top of queue (fifo)
                If Q.TestCommandBits(EQItem.CommandBits.Classad) Then ProcessJob.processClassad(Q)
                If Q.TestCommandBits(EQItem.CommandBits.JPGfromPDF) Then ProcessJob.processJPGfromPDF(Q)
                If Q.TestCommandBits(EQItem.CommandBits.TextfromPDF) Then ProcessJob.processtextfromPDF(Q)
                engine.EngineQ.Remove(Q)
            Loop

        Catch ex As Exception

            Facia1.Msg("Could not process user request because: " & ex.Message)

        End Try
        '
        ' set engine back to run
        '
        Facia1.SetEnabled(Facia.ButtonMask.Pause Or Facia.ButtonMask.Quit)
        Facia1.ClearEnabled(Facia.ButtonMask.Busy)
        engine.Status = engine.EngineStates.Running

    End Sub


    Friend Sub refreshMe()
        System.Windows.Forms.Application.DoEvents()
        Me.Refresh()
    End Sub

    Private Sub ExitMe()
        Try

            If Not Engine Is Nothing Then
                GL.INDService.FinalizeINDD()
                RemoveHandler Engine.EngineEvent, AddressOf EQItemAdded
                Sys.UnMapServices()
                '
                ' wait a bit for INDD to exit
                '
                Threading.Thread.Sleep(3000)
            End If
            Me.Close()
        Catch ex As Exception
            Me.Close()
        End Try
    End Sub


    Private Sub Facia1_ButtonEvent(ByVal Enabled As Facia.ButtonMask, ByVal Selected As Facia.ButtonMask, ByVal buttonID As Facia.ButtonMask) Handles Facia1.ButtonClick
        Select Case buttonID

            Case Facia.ButtonMask.Run
                runProcess()

            Case Facia.ButtonMask.Pause
                PauseProcess()

            Case Facia.ButtonMask.Quit
                ExitMe()

            Case Facia.ButtonMask.MoreorLess
                setMyheight()                    ' resize my own app if the facia changes

            Case Else
        End Select

    End Sub

End Class
