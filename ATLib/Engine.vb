Option Strict On
Option Explicit On
Imports System.Net
Imports System.Runtime.Remoting


'***************************************************************************************
'*
'* Engine Service
'*
'* AUDIT TRAIL
'* 
'* V1.000   11-NOV-2009  BA  Original
'*
'* Implements a job queueing system using remoting
'*
'***************************************************************************************

''' <summary>
''' <para>EngineServices provides cross-network communication with the engine from a variety of
''' calling points. EngineServices uses Microsoft remoting technology to provide common instance of
''' a class that can be simulteanously instantiated and mapped from a variety of agents. EngineServices in turn embeds the EQItem class
'''  which is serializable and can therefore be transmitted over the network between the engine and the client.
'''  EngineServices is first instantiated by the Engine, and then this same copy of the object can be accessed
''' over the network by any client subscriber.
''' </para>
''' <para>The overall design of EngineServices is that it implements a queue of EQ items, where each EQ Item carries a job for the engine to perform, and also
''' provides a feedback for the caller to report on the results of the engine processing. The overall flow is that 1) a network client instantiates an EQ Item, and then
'''  enqueues it to EngineServices. The client supspends at this point.
'''  2) This fires an event which is serviced by the engine. The engine dequeues the item and procsses it.
''' 3) When the engine is ready to resume the client, it updates the EQ item with a resume status, and continues any post-resume processing.
'''  4) The client resumes, and decodes the returned results which are encoded into the EQItem.
'''  Note that this entire sequence is backstopped by a timer object which will resume the client after a timeout period(defined 
''' in the system object) it the engine does not complete processing in the required timeframe.
''' </para>
''' </summary>
''' 
Public Class Engine : Inherits MarshalByRefObject

    ''' <summary>
    ''' Defines the states that the engine can be in.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum EngineStates
        OffLine = 0
        Paused = 1
        Running = 2
        Busy = 4
        Err = 5
    End Enum

    Private _EngineQ As New List(Of EQItem)

    ''' <summary>
    ''' Fires when an EQItem is added to the queue. There are no parameters to the event
    ''' since the entire EQ can be inspected and processed via the EngineQ Collection.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event EngineEvent()

    Private _status As EngineStates
    Private _jobsOK As Integer
    Private _jobsBad As Integer
    Private _pollPeriod As Integer = 100
    Private _JobTimeout As Integer


    ''' <summary>
    ''' This allows the object to live foreever. See Microsoft Remoting documentation for more information.
    ''' </summary>
    Public Overrides Function InitializeLifetimeService() As Object
        '
        ' allow object to live forever
        '
        Return Nothing
    End Function

    ''' <summary>
    ''' This is the current status of the engine, see EngineServices.EngineStates which instantiated this copy of EngineServices.
    ''' </summary>
    Public Property Status() As EngineStates
        Get
            Return _status
        End Get
        Set(ByVal value As EngineStates)
            _status = value
        End Set
    End Property

    ''' <summary>
    '''Defines the time period in ms before the client will be resumed if the engine has 
    ''' not completed the job or resumed client before the timeout expires. 
    ''' </summary>
    Public Property JobTimeout() As Integer
        Get
            Return _JobTimeout
        End Get
        Set(ByVal value As Integer)
            _JobTimeout = value
            If value < 10 Then _JobTimeout = 10
        End Set
    End Property


    ''' <summary>
    '''Returns the total number of successfully processed jobs since engine start. Incremented by the engine for each successful job that is processed.
    ''' </summary>
    Public Property JobsOK() As Integer
        Get
            Return _jobsOK
        End Get
        Set(ByVal value As Integer)
            _jobsOK = value
        End Set
    End Property

    ''' <summary>
    '''Returns the total number of unsuccessfully processed jobs since engine start. Incremented by the engine for each job that fails to process. 
    ''' </summary>
    Public Property JobsBad() As Integer
        Get
            Return _jobsBad
        End Get
        Set(ByVal value As Integer)
            _jobsBad = value
        End Set
    End Property


    ''' <summary>
    '''This property is the embedded queue of EQItems, EQItems are created and enqueued by the client and dequeued and serviced by the engine. 
    ''' </summary>
    Public ReadOnly Property EngineQ() As List(Of EQItem)
        Get
            Return _EngineQ
        End Get
    End Property

    ''' <summary>
    ''' Returns a human readable statement identifying the host name and IP address of the machine that the engine is running on.
    ''' </summary>
    Public ReadOnly Property Ident() As String
        Get
            Return "Engine available on: " & HostName() & " - " & CommonRoutines.IPInt2String(HostIP)
        End Get
    End Property

    ''' <summary>
    ''' Returns the name of the machine that the engine is running on.
    ''' </summary>
    Public Function HostName() As String
        '
        ' returns the machine name as a string
        '
        Return My.Computer.Name
    End Function

    ''' <summary>
    ''' Returns the IP address of the machine that the engine is running on.
    ''' </summary>
    Public Function HostIP() As Integer
        '
        ' returns the machine IP address as a string
        '
        Dim hostNm As String = Dns.GetHostName()
        Dim host As IPHostEntry = Dns.GetHostEntry(HostName)
        Dim firstAddress As IPAddress = host.AddressList(0)
        Dim addrbyte() As Byte = firstAddress.GetAddressBytes
        Return CommonRoutines.IPBytes2Int(addrbyte)
    End Function



    ''' <summary>
    ''' <para>This method is called by the client to enqueue an EQItem to the engine queue. 
    ''' This method adds a new EQItem in the client's thread space
    ''' These options are specified in the Command word of the EQItem.
    '''  Items can only be successfully enqueued if the engine is currently running or busy (ie not paused or in error).
    '''  If the engine cannot accept new items, the EQItem Status word is updated and method immdediately returns.
    ''' </para>
    ''' <para>Otherwise, Enqueue is a suspension call. When the item is successfully enqueued, the client thread will suspend
    ''' until either the engine resumes the client, or one of the resumeTimeout or jobTimeout periods is exhausted.
    ''' In all cases, the EQItem Status word contains the results of the processing.
    ''' </para>
    ''' </summary>
    Public Function Enqueue(ByVal EQItem As EQItem) As EQItem
        '
        ' Only accept jobs if in running or busy states
        '
        If Not (_status = EngineStates.Running Or _status = EngineStates.Busy) Then
            EQItem.SetStatusBits(EQItem.StatusBits.NoStart)
        Else
            EngineQ.Add(EQItem)                         'add to engine q
            EQItem.SetStatusBits(EQItem.StatusBits.Queued)
            '
            ' engine must be single threaded - raise event only if not busy
            ' If engine is busy it will automatially dequeue next EQItem
            ' when it finishes current
            '
            If _status = EngineStates.Running Then RaiseEvent EngineEvent()
            '
            ' if client has asked for resume suspension, block thread and poll resume bit
            ' which will be set by engine at some point. Or timeout
            '
            EQItem.executionTime = 0
            '
            ' if client has asked for complete suspension, block thread and poll complete bit
            ' which will be set by engine at some point. Or timeout
            '
            If EQItem.TestCommandBits(EQItem.CommandBits.SuspendUntilComplete) Then

                Do While EQItem.executionTime < _JobTimeout
                    If EQItem.TestStatusBits(EQItem.StatusBits.Complete Or EQItem.StatusBits.Errored) Then Exit Do
                    Threading.Thread.Sleep(_pollPeriod)
                    EQItem.executionTime += _pollPeriod
                Loop
                If Not EQItem.TestStatusBits(EQItem.StatusBits.Complete Or EQItem.StatusBits.Errored) Then EQItem.SetStatusBits(EQItem.StatusBits.Timeout)

            End If

        End If

        Return EQItem                            'immediate return, no suspension

    End Function


    Private Function IndexOf(ByVal value As EQItem) As Integer
        Return (_EngineQ.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As EQItem)
        _EngineQ.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As EQItem)
        _EngineQ.Remove(value)
    End Sub
    Private Function Contains(ByVal value As EQItem) As Boolean
        Return (_EngineQ.Contains(value))
    End Function



End Class


'***************************************************************************************
'*
'* EQItem
'*
'* AUDIT TRAIL
'* 
'* V1.000   27-OCT-2007  BA  Original
'*
'* This is the item that gets queued to the engine
'*
'***************************************************************************************

''' <summary>
''' <para>
''' The EQItem class implements a serializable object that can be transmitted
''' across the wire, between the engine  and a client. The object has a simple
''' 4-integer construction - a Command word which contains a set of bits which tells
''' the engine what to do, a Status word which contains a set of bits which describe
''' what the engine did, an objectID word which is the ID of either a Template or
''' Doc object, and an integer which holds the execution time in ms.
''' </para>
''' </summary>

<Serializable()> Public Class EQItem


    ''' <summary>
    ''' Defines a set of ORable bits which in toto make up an integer value which defines the type of operation that the caller is requesting of the engine when the Q Item is processed.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum CommandBits
        Classad = &H1                   'request build of classad instance
        JPGfromPDF = &H2                'request build of jpg pic from pdf
        TextfromPDF = &H4               'request extract text from pdf
        SuspendUntilComplete = &H800    'Caller requests suspension until ob finishes
    End Enum

    ''' <summary>
    ''' Defines a set of ORable bits which represents the response of the engine to the caller when a Q Item is processed.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum StatusBits
        '
        ' Note that the output file responses are in the same bit places as the requests
        ' if this can be used to advantage
        '
        PDF = &H2                  'request production quality PDF output
        XML = &H8                  'request XML output
        JPG = &H10                  'request JPG output
        INDD = &H20                'request save of INDD file
        LoRes = &H40               'request low res image to web site
        ProofPDF = &H80            'request proof pdf to web site
        NoStart = &H100             'job did not start - no item added to q
        Queued = &H200             'item added to q
        ResumeClient = &H400       'engine sets this bit to resume client
        Complete = &H800           'engine sets this bit when transaction complete
        Timeout = &H1000            'Suspended caller times out
        Errored = &H2000           'job completed but with errors
    End Enum

    ''' <summary>
    ''' 32 bit command word, set by client from one of EQItem.CommandBits
    ''' </summary>
    Public Command As CommandBits


    ''' <summary>
    ''' 32 bit status word, set by engine from one of EQItem.StatusBits
    ''' </summary>
    Public Status As StatusBits


    ''' <summary>
    ''' Either the template ID or the document ID of the item to be processed
    ''' </summary>
    Public ObjectID As Integer


    ''' <summary>
    ''' Exectution time in ms
    ''' </summary>
    Public executionTime As Integer

    ''' <summary>
    ''' Test property used to determine which machine this code is executing on.
    ''' </summary>
    Public ReadOnly Property Ident() As String
        '
        ' which machine is this code executing on?
        '
        Get
            Return "Running on  " & HostName() & " - " & CommonRoutines.IPInt2String(HostIP)
        End Get
    End Property

    Private Function HostName() As String
        '
        ' returns the machine name as a string
        '
        Return My.Computer.Name
    End Function

    Private Function HostIP() As Integer
        '
        ' returns the machine IP address as a string
        '
        Dim hostNm As String = Dns.GetHostName()
        Dim host As IPHostEntry = Dns.GetHostEntry(HostName)
        Dim firstAddress As IPAddress = host.AddressList(0)
        Dim addrbyte() As Byte = firstAddress.GetAddressBytes
        Return CommonRoutines.IPBytes2Int(addrbyte)
    End Function


    ''' <summary>
    ''' Sets command bits into the EQ Command word
    ''' </summary>
    ''' <param name="newbits">Bitmap to set, from EQItem.CommandBits</param>
    Public Sub SetCommandBits(ByVal newbits As EQItem.CommandBits)
        Command = Command Or newbits
    End Sub

    ''' <summary>
    ''' Clears command bits from the EQ Command word
    ''' </summary>
    ''' <param name="newbits">Bitmap to set, from EQItem.CommandBits</param>
    Public Sub ClearCommandBits(ByVal newbits As EQItem.CommandBits)
        Command = Command And Not newbits
    End Sub

    ''' <summary>
    ''' Tests bits bits in the EQ Command word
    ''' </summary>
    ''' <param name="newbits">Bitmap to set, from EQItem.CommandBits</param>
    '''<returns>True if any bits are set, false if all bits are clear.</returns>
    Public Function TestCommandBits(ByVal newbits As EQItem.CommandBits) As Boolean
        Return Convert.ToBoolean(Command And newbits)
    End Function

    ''' <summary>
    ''' Sets status bits into the EQ Status word
    ''' </summary>
    ''' <param name="newbits">Bitmap to set, from EQItem.StatusBits</param>
    Public Sub SetStatusBits(ByVal newbits As EQItem.StatusBits)
        Status = Status Or newbits
    End Sub

    ''' <summary>
    ''' Clears status bits from the EQ Status word
    ''' </summary>
    ''' <param name="newbits">Bitmap to set, from EQItem.CommandBits</param>
    Public Sub ClearStatusBits(ByVal newbits As EQItem.StatusBits)
        Status = Status And Not newbits
    End Sub

    Public Function TestStatusBits(ByVal newbits As EQItem.StatusBits) As Boolean
        Return Convert.ToBoolean(Status And newbits)
    End Function

End Class

