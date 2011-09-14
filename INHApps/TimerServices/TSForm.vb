Option Explicit On
Option Strict On
Imports ATLib
Imports ATControls
'***************************************************************************************
'*
'* Timer Services
'*
'* AUDIT TRAIL
'* 
'* V1.000   25-OCT-2007  BA  Original
'*
'* This task keeps the database clean by performing regular timer-related processes.
'*
'*
'*
'*
'***************************************************************************************

Public Class TSForm

    Private Timer As New Timer
    Private Sys As New atsystem

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

        Timer.Preset1 = Constants.FourPM

        AddHandler Timer.MinuteEvent, AddressOf everyMinute
        AddHandler Timer.HourEvent, AddressOf everyHour
        AddHandler Timer.MidnightEvent, AddressOf atMidnight
        AddHandler Timer.Preset1Event, AddressOf atFourPM
        Timer.SynchronizingObject = Me          'ensure timer events on this thread
    End Sub

    Private Sub setMyheight()
        Me.Height = Facia1.Bottom + 25         ' resize my own app if the facia changes
    End Sub



    Private Sub runProcess()

        Try
            Sys.Retrieve()
            Me.Text = formName & " Connected to " & Sys.Name
            Facia1.ClearMsg()
            Facia1.Msg("Timer Services running")
            Facia1.ClearEnabled(Facia.ButtonMask.Quit)
            Timer.StartTimer()

        Catch ex As Exception
            Me.Text = "Timer Services "
            Facia1.ClearMsg()
            Facia1.Msg("Using connection string " & Sys.ConnectionString)
            Facia1.Msg("Could not start because " & ex.Message)
            Facia1.SetEnabled(Facia.ButtonMask.Err)
            Facia1.ClearEnabled(Facia.ButtonMask.Pause Or Facia.ButtonMask.Run)
        End Try


    End Sub

    Private Sub PauseProcess()
        Facia1.Msg("Timer Services paused by operator")
        Facia1.SetEnabled(Facia.ButtonMask.Quit)
        Timer.StopTimer()
        Me.Text = formName
    End Sub

    Private Sub everyMinute(ByVal timestamp As DateTime)
        Facia1.SetEnabled(ATControls.Facia.ButtonMask.Busy)
        Dim Slots As New Slots
        Dim n As Integer

        ''     n = Slots.Timeout(Sys.SlotTimeout)

        If n > 0 Then Facia1.Msg(n & " sessions timed out")

        Facia1.ClearEnabled(Facia.ButtonMask.Busy)

    End Sub

    Private Sub everyHour(ByVal timestamp As DateTime)
        '
        ' ignore poll if remote db does not respond
        '
 
        Facia1.ClearEnabled(Facia.ButtonMask.Busy)
    End Sub

    Private Sub atFourPM(ByVal timestamp As DateTime)
        '
        ' rolls the next working day in the sys object
        '
    End Sub


    Private Sub atMidnight(ByVal timestamp As DateTime)
  
        Sys.TouchMonth()              'reset ad sequence at change of month
        Sys.Retrieve()            're-read to get lastest listing times

        purgeSlots()
        purgeAds()
        archiveAds()
        killLatestListings()
        Dim n As Integer
        n = remindUsersEditionClose()
        If n > 0 Then Facia1.Msg(n & " edition close emails sent")

   
     
        Facia1.Msg("All is well at Midnight")
    End Sub

    Private Function remindUsersEditionClose() As Integer        '
        ' if the user has ads that are scheduled for an edition that is about to close
        ' send a reminder email
        '

        Dim rtnval As Integer = 0

        Dim editionCloseDays As Integer = Sys.EditionCloseDays
        Dim now As Date = Sys.GetdbTime
        Dim EA As New EmailAssistant(Sys)

        Dim usrs As New Usrs
        usrs.Retrieve()               'get all users
        For Each Usr As Usr In usrs
            For Each Ad As Ad In Usr.Ads(Ad.ProdnState.Saved)           'get usrs saved ads
                '
                ' get all the ad instances and do the edition close check
                '
                Dim sendflag As Boolean = False
                For Each AI As AdInstance In Ad.Instances

                    If DateDiff(DateInterval.Day, now, AI.Edition.AdDeadline) = editionCloseDays Then
                        sendflag = True
                    End If
                Next
                If sendflag Then
                    Try
                        EA.SendEditionCLoseReminder(Usr, Ad)
                        rtnval += 1
                    Catch ex As Exception
                        Facia1.Msg("Edition close email failure because " & ex.Message)
                    End Try
                End If

            Next
        Next
        Return rtnval

    End Function

    Private Sub killLatestListings()
        '
        ' at this date only, clear the latest listing flag on all ads
        '
        Dim n As Integer
        If DateTime.Compare(Sys.GetdbTime, Sys.LatestListingKillTime) > 0 Then
            Dim ads As New Ads
            n = ads.KillLatestListings()
            If n > 0 Then Facia1.Msg(n & " lastest listings killed")
        End If


    End Sub
    Private Sub purgeSlots()
        '
        ' purge slots
        '
        Dim n As Integer
        Dim slots As New Slots
        n = slots.Purge(Sys.SlotPurge)
        If n > 0 Then Facia1.Msg(n & " sessions purged")
    End Sub

    Private Sub purgeAds()
        '
        ' purge all 
        '
        Dim n As Integer
        Dim ads As New Ads
        n = ads.Purge()
        If n > 0 Then Facia1.Msg(n & " ads purged")
    End Sub

    Private Sub archiveAds()
       
        Dim n As Integer
        Dim ads As New Ads
        n = ads.Archive()
        If n > 0 Then Facia1.Msg(n & " ads archived")
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
