﻿Option Strict On
Option Explicit On



'***************************************************************************************
'*
'* TIMER
'*
'* AUDIT TRAIL
'* 
'*
'* V1.000   19-NOV-2009  BA  Original
'*
'*
'***************************************************************************************


''' <summary>
''' The Timer class generates Timer events at various points. A basic one second
''' loop  is split out into several different events. This class uses the
''' system.timers.timer object to generate  a basic 1 second tick, and looks at the
''' curent system time (now) to determine time intervals etc Note that this does not
''' call System.dbtime, since thats an IO call. Therefore the time generated by
''' this  class may may differ from the db time. 
''' <para></para>
''' <para>Timer publishes events through 4 events - any process can subscribe to
''' these events by declaring an event handler.</para>
''' </summary>
Public Class Timer
    Private _running As Boolean
    Private _preset1 As Date
    Private _preset2 As Date
    Private _interval1 As Integer
    Private _interval2 As Integer

    Private gotPreset1 As Boolean = False
    Private gotPreset2 As Boolean = False
    Private gotMinute As Boolean = False
    Private interval1count As Integer = 0
    Private interval2count As Integer = 0
    Private minuteCount As Integer = 0

    Private _syncobject As ComponentModel.ISynchronizeInvoke

    Private _timer As New Timers.Timer

    ''' <summary>
    ''' Fires every minute, on the minute
    ''' </summary>
    ''' <param name="Timestamp">Exact date-time of event firing</param>
    Public Event MinuteEvent(ByVal Timestamp As Date)


    ''' <summary>
    ''' Fires at midnight
    ''' </summary>
    ''' <param name="Timestamp">Exact date-time of event firing</param>
    Public Event MidnightEvent(ByVal Timestamp As Date)


    ''' <summary>
    ''' Fires every hour, on the hour
    ''' </summary>
    ''' <param name="Timestamp">Exact date-time of event firing</param>
    Public Event HourEvent(ByVal Timestamp As Date)


    ''' <summary>
    ''' Fires every day at the preset 1 time
    ''' </summary>
    ''' <param name="Timestamp">Exact date-time of event firing</param>
    Public Event Preset1Event(ByVal Timestamp As Date)


    ''' <summary>
    ''' Fires every day at the preset 2 time
    ''' </summary>
    ''' <param name="Timestamp">Exact date-time of event firing</param>
    Public Event Preset2Event(ByVal Timestamp As Date)


    ''' <summary>
    ''' Fires when each interval 1 is met
    ''' </summary>
    ''' <param name="Timestamp">Exact date-time of event firing</param>
    Public Event Interval1Event(ByVal Timestamp As Date)


    ''' <summary>
    ''' Fires when each interval 2 is met
    ''' </summary>
    ''' <param name="Timestamp">Exact date-time of event firing</param>
    Public Event Interval2Event(ByVal Timestamp As Date)



    ''' <summary>
    ''' Instantiates a new timer object but does not start it ticking
    ''' </summary>
    Public Sub New()
        AddHandler _timer.Elapsed, AddressOf processTick
    End Sub

    ''' <summary>
    ''' This property allows a synchronizing object to be declared to the timer class so that
    ''' events will fire on the sync object's thread rather than a separate timer thread.
    ''' </summary>
    Public Property SynchronizingObject() As ComponentModel.ISynchronizeInvoke
        '
        ' this is the object that calls the timer. it ensures that timer events 
        ' are processed by the thread that invoked the object.
        '
        Get
            Return _syncobject
        End Get
        Set(ByVal value As ComponentModel.ISynchronizeInvoke)
            _syncobject = value
            _timer.SynchronizingObject = value
        End Set
    End Property

    ''' <summary>
    ''' Supplies a preset date target.
    '''  Only hh:mm are used, so this property is accurate only to one minute. 
    ''' The Preset1 event will be fired at
    ''' the specified time every day when the preset time is met
    ''' </summary>
    Public Property Preset1() As Date
        Get
            Return _preset1
        End Get
        Set(ByVal value As Date)
            _preset1 = value
        End Set
    End Property

    ''' <summary>
    ''' Supplies a preset date target.
    '''  Only hh:mm are used, so this property is accurate only to one minute. 
    ''' The Preset2 event will be fired at
    ''' the specified time every day when the preset time is met
    ''' </summary>
    Public Property Preset2() As Date
        Get
            Return _preset2
        End Get
        Set(ByVal value As Date)
            _preset2 = value
        End Set
    End Property

    ''' <summary>
    ''' Supplies a preset interval in seconds.
    ''' The Interval1 event will be fired repeatedly each time the interval is met.
    ''' </summary>
    Public Property Interval1() As Integer
        '
        ' value is number of seconds. This fires the Interval1 event every nn seconds from now
        '
        Get
            Return _interval1

        End Get
        Set(ByVal value As Integer)
            _interval1 = value
        End Set
    End Property

    ''' <summary>
    ''' Supplies a preset interval in seconds.
    ''' The Interval2 event will be fired repeatedly each time the interval is met.
    ''' </summary>
    Public Property Interval2() As Integer
        '
        ' value is number of seconds. This fires the Interval2 event every nn seconds from now
        '
        Get
            Return _interval2

        End Get
        Set(ByVal value As Integer)
            _interval2 = value
        End Set
    End Property

    ''' <summary>
    ''' Starts the timer running
    ''' </summary>
    Public Sub StartTimer()
        _timer.Interval = 1000
        _timer.Enabled = True

    End Sub

    ''' <summary>
    ''' Stops the timer
    ''' </summary>
    Public Sub StopTimer()
        _timer.Enabled = False
    End Sub


    Private Sub processTick(ByVal source As Object, ByVal e As Timers.ElapsedEventArgs)

        '
        ' check the various conditions to see if event can be triggered
        '
        Dim currentTime As DateTime = Now                   'get current local system time
        '
        ' preset1 accurate only to a minute. fires event only first time that its seen each minute
        ' only if it has a value. IE if ticks is not zero
        '
        If _preset1.Ticks > 0 Then
            If currentTime.Hour = Preset1.Hour Then
                If currentTime.Minute = Preset1.Minute Then
                    If Not gotPreset1 Then
                        gotPreset1 = True
                        RaiseEvent Preset1Event(currentTime)
                    End If
                Else
                    gotPreset1 = False          'reset for next time
                End If
            End If
        End If
        '
        ' preset2 accurate only to a minute. fires event only first time that its seen each minute
        ' only if it has a value. IE if ticks is not zero
        '
        If _preset2.Ticks > 0 Then
            If currentTime.Hour = Preset2.Hour Then
                If currentTime.Minute = Preset2.Minute Then
                    If Not gotPreset2 Then
                        gotPreset2 = True
                        RaiseEvent Preset2Event(currentTime)
                    End If
                Else
                    gotPreset2 = False          'reset for next time
                End If
            End If
        End If
        '
        ' interval1 - only if enabled
        '
        If _interval1 > 0 Then
            interval1count += 1
            If interval1count = _interval1 Then
                interval1count = 0
                RaiseEvent Interval1Event(currentTime)
            End If
        End If
        '
        ' interval2 - only if enabled
        '
        If _interval2 > 0 Then
            interval2count += 1
            If interval2count = _interval2 Then
                interval2count = 0
                RaiseEvent Interval2Event(currentTime)
            End If
        End If
        '
        ' minute event - should be within first two seconds of the minute
        '
        If currentTime.Second < 2 Then
            If Not gotMinute Then
                gotMinute = True
                RaiseEvent MinuteEvent(currentTime)
                '
                ' hour event - minutes are zero
                '
                If currentTime.Minute = 0 Then
                    RaiseEvent HourEvent(currentTime)
                    '
                    ' midnight event - hours = 0
                    '
                    If currentTime.Hour = 0 Then
                        RaiseEvent MidnightEvent(currentTime)
                    End If

                End If
            End If
        Else
            gotMinute = False
        End If

    End Sub

End Class

