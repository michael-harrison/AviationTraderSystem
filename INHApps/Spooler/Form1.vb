Option Explicit On
Option Strict On
Imports ATLib
Imports ATControls
'***************************************************************************************
'*
'* Spooler
'*
'* AUDIT TRAIL
'* 
'* V1.000   13-APR-2010  BA  Original
'*
'*
'*
'***************************************************************************************

Public Class Form1


    Private Timer As New Timer
    Private Sys As New ATSystem
    Private EA As EmailAssistant

    Private formName As String



    Private Sub Form1_Load(ByVal sender As Object, ByVal e As system.EventArgs) Handles Me.Load
        formName = Me.Text & " " & Sys.SWVersion
        Me.Text = formName

        '
        ' init facia and line up the control and the form
        '
        Facia1.DetailVisible = False
        Facia1.SetBounds(0, 0, Facia1.Width, Facia1.Height)
        Me.Width = Facia1.Width + 5
        setMyheight()

        AddHandler Timer.MinuteEvent, AddressOf everyMinute
        Timer.SynchronizingObject = Me          'ensure timer events on this thread

    End Sub

    Private Sub setMyheight()
        Me.Height = Facia1.Bottom + 25         ' resize my own app if the facia changes
    End Sub



    Private Sub runProcess()

        Try
            Sys.Retrieve()
            Me.Text = formName & " Connected to " & Sys.Name
            '
            ' set up the email assistant
            '
            EA = New EmailAssistant(Sys)

            Facia1.ClearMsg()
            Facia1.Msg("Spooler running")
            Facia1.ClearEnabled(Facia.ButtonMask.Quit)
            Timer.StartTimer()


        Catch ex As Exception
            Me.Text = "Spooler "
            Facia1.ClearMsg()
            Facia1.Msg("Using connection string " & Sys.ConnectionString)
            Facia1.Msg("Could not start because " & ex.Message)
            Facia1.SetEnabled(Facia.ButtonMask.Err)
            Facia1.ClearEnabled(Facia.ButtonMask.Pause Or Facia.ButtonMask.Run)
        End Try


    End Sub

    Private Sub PauseProcess()
        Facia1.Msg("Spooler paused by operator")
        Facia1.SetEnabled(Facia.ButtonMask.Quit)
        Timer.StopTimer()
        Me.Text = formName
    End Sub

    Private Sub everyMinute(ByVal timestamp As DateTime)
        Facia1.SetEnabled(ATControls.Facia.ButtonMask.Busy)
        '
        ' poll the db and process all folders which are marked as spooler active
        '
        Dim activefolders As New Folders
        activefolders.retrieveSpoolerSet(Sys.ID)
        For Each Folder As Folder In activefolders
            '
            ' only process folders if they contain ads
            '
            If Folder.AdCount > 0 Then
                Select Case Folder.SpoolerCommand
                    Case ATLib.Folder.SpoolerCommands.ProofEmail : sendProofApprovalRQs(Folder)
                    Case ATLib.Folder.SpoolerCommands.DeleteAds : deleteAllAds(Folder)
                    Case Else : unrecognisedCommand(Folder)
                End Select

            End If
        Next
        Facia1.ClearEnabled(Facia.ButtonMask.Busy)
    End Sub

    Private Sub unrecognisedCommand(ByVal Folder As Folder)
        Facia1.Msg("Unrecognised command" & Folder.SpoolerCommand & " for folder " & Folder.Name)
    End Sub

    Private Sub sendProofApprovalRQs(ByVal Folder As Folder)
        Dim rtnval As Integer = 0
        

        For Each Ad As Ad In Folder.Ads
            Try
                '
                'check that the prodn status is Proofed. Send to error folder if not
                '
                If Ad.ProdnStatus <> ATLib.Ad.ProdnState.Proofed Then
                    Facia1.Msg("Proof Approval Request failure for ad " & Ad.Adnumber & " for " & Ad.Usr.EmailAddr & " because ad status is " & Ad.ProdnStatusDescr & " - must be Proofed")
                    Ad.FolderID = Folder.ErrorFolderID
                Else
                    EA.SendProofApprovalRQ(Ad)
                    Facia1.Msg("Proof Approval Request for ad " & Ad.Adnumber & " sent to " & Ad.Usr.EmailAddr)
                    Ad.FolderID = Folder.DoneFolderID
                End If
            Catch ex As Exception
                Facia1.Msg("Proof Approval Request email failure for " & Ad.Usr.EmailAddr & " because " & ex.Message)
                Ad.FolderID = Folder.ErrorFolderID
            End Try
        Next
        Folder.Ads.Update()
    End Sub

    Private Sub deleteAllAds(ByVal Folder As Folder)
        '
        ' the folder serves as a trash can. All ads in it will be deleted.
        '
        Dim adCount As Integer = Folder.AdCount
        For Each Ad As Ad In Folder.Ads
            Ad.Deleted = True
        Next
        Folder.Ads.Update()
        Facia1.Msg("Deleted " & adCount & " ads from " & Folder.Name)

    End Sub


    Private Sub ExitMe()
        Me.Close()
    End Sub




    Private Sub Facia1_ButtonEvent(ByVal Enabled As Facia.ButtonMask, ByVal Selected As Facia.ButtonMask, ByVal buttonID As Facia.ButtonMask) Handles Facia1.ButtonClick
        Select Case buttonID

            Case Facia.ButtonMask.Run
                runProcess()

            Case Facia.ButtonMask.Pause
                PauseProcess()

            Case Facia.ButtonMask.MoreorLess
                setMyheight()                       ' resize my own app if the facia changes

            Case Facia.ButtonMask.Quit
                ExitMe()

            Case Else
        End Select

    End Sub

End Class
