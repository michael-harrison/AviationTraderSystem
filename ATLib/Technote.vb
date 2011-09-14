Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.ComponentModel
Imports System


'***************************************************************************************
'*
'* Technote
'*
'* This is used for maintaining a log of system changes
'*
'* AUDIT TRAIL
'* 
'* V1.000   21-DEC-2009  BA  Original
'*
'*
'***************************************************************************************

''' <summary>
''' The Technotes class implements a collection of Technotes, used for system development reporting purposes.
''' </summary>
Public Class Technotes : Inherits CollectionBase

    Private _connectionString As String
    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Technotes(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Technotes.count-1</param>
    ''' <value>Technote object from Technotes collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Technote
        Get
            Return CType(List(index), Technote)
        End Get
        Set(ByVal value As Technote)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a Technote object to the Technotes collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Technote object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As Technote) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Technote) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Technote)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Technote)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Technote) As Boolean
        Return (List.Contains(value))
    End Function

    Friend Property ConnectionString() As String
        Get
            Return _connectionString
        End Get
        Set(ByVal value As String)
            _connectionString = value
        End Set
    End Property

    Private Function getConnection() As SqlConnection
        If _connectionString Is Nothing Then _connectionString = ConfigurationManager.ConnectionStrings("ATConnectionString").ConnectionString
        Return New SqlConnection(_connectionString)
    End Function

    ''' <summary>Retrieves the entire set of Objects and populates the parent collection.</summary>
    Public Sub Retrieve()
        '
        ' Retrieves the entire table
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Technote ORDER BY ID DESC"
        doRetrieveR(Cmd)
    End Sub

    Friend Sub retrieveSet(ByVal systemID As Integer)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Technote WHERE SystemID=@SystemID ORDER BY ID Desc"
        Cmd.Parameters.Add("@SystemID", SqlDbType.Int).Value = systemID
        doRetrieveR(Cmd)
    End Sub

    ''' <summary>
    ''' Retrieves all Technotes which have the supplied status.
    ''' </summary>
    ''' <param name="status">Technote status</param>
    ''' <param name="SystemID">SystemID</param>
    Public Sub RetrieveSet(ByVal SystemID As Integer, ByVal Status As Technote.State)
        '
        ' Retrieves the entire table which matches the supplied status value
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Technote WHERE SystemID=@SystemID AND Status=@Status ORDER BY ID DESC"
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = Status
        Cmd.Parameters.Add("@SystemID", SqlDbType.Int).Value = SystemID

        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Technote
        Return Retrieve(Hex2Int(HexID))
    End Function



    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="ID">ObjectID, represented as an integer</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal ID As Integer) As Technote
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Technote WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function

    Private Sub doRetrieveR(ByVal Cmd As SqlCommand)
        '
        ' Retrieves records passed in the command object
        '
        List.Clear()
        Dim dr As SqlDataReader = Nothing
        Cmd.Connection.Open()
        Try
            dr = Cmd.ExecuteReader()
            While dr.Read
                Dim Technote As New Technote
                Add(Technote)
                Technote.ConnectionString = _connectionString
                Technote.DR2Object(dr)
            End While
        Finally
            If Not dr Is Nothing Then
                dr.Close()
                Cmd.Connection.Dispose()
            End If
        End Try
    End Sub


    ''' <summary>
    ''' Updates all objects in the collection which have been modified since the collection was
    ''' retrieved from the database. Individual objects may either have been updated, marked for deletion
    ''' (Delete property set to True) or newly created. Objects in the collection which have not been
    ''' modified by any of these three ways are not written back to the database.
    ''' </summary>
    Public Sub Update()
        '
        ' Updates all objects in the list. The child object tests for dirty
        '
        Dim deletedObjects As New List(Of Technote)

        For Each Technote As Technote In List
            Technote.Update()
            If Technote.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Technote)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Technote In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class



'***************************************************************************************
'*
'* Technote - Technote object
'*
'* AUDIT TRAIL
'* 
'* V1.000   22-DEC-2009  BA  Original
'*
'*
'*
'*
'**************************************************************************************

''' <summary>
''' <para> The Technote object holds all data retrieved from a specified row of the Technote table.
'''</para>
''' <para>It is always accessed as part of the Technotes collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Technotes collection
'''  property of the parent.
''' </para> 
''' </summary>
Public Class Technote

    Private _connectionString As String
    Private _ID As Integer
    Private _systemID As Integer
    Private _status As State
    Private _navTarget As String
    Private _name As String
    Private _ProblemDescription As String
    Private _ProblemFix As String
    Private _reportedBy As Reporters
    Private _fixedBy As Reporters
    Private _ObjectStatus As ATSystem.ObjectState
    Private _createTime As Date
    Private _fixTime As Date
    Private _resolution As Resolutions
    '
    ' embedded collections
    '
    Private _Sys As ATSystem
    Private _Loader As Loader

    ''' <summary>
    ''' Defines the state that a Technote can be in.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum State
        Open = 1
        Closed = 2
        Discuss = 3
        NewFeature = 4
        Warranty = 5
        WishList
    End Enum

    ''' <summary>
    ''' Defines what we decided to do to a Technote.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum Resolutions
        Active = 1
        Fixed = 2
        Discarded = 3
        Wishlist = 4
    End Enum

    ''' <summary>
    ''' Defines who can make up Technotes. Note the VB reserved word.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum Reporters
        BA = 1
        KG = 2
        TS = 3
        JD = 4
    End Enum

    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _ID = ATSystem.SysConstants.nullValue
        _status = 0
    End Sub

    ''' <summary>Object ID, which is the identity value of the database Primary Key, represented as a 32 bit integer</summary>
    ''' <value>Object ID as an integer</value>
    Public ReadOnly Property ID() As Integer
        Get
            Return _ID
        End Get
    End Property

    ''' <summary>Object ID, which is the identity value of the database Primary Key, represented as an 8 character hex string</summary>
    ''' <value>Object ID as an 8 character hex string</value>
    Public ReadOnly Property hexID() As String
        Get
            Return _ID.ToString("X8")
        End Get
    End Property

    ''' <summary>Timestamp, read from dbTime, that the database object was created</summary>
    ''' <value>Long timestamp, in the current culture</value>
    Public ReadOnly Property CreateTime() As Date
        Get
            Return _createTime
        End Get
    End Property

    Public ReadOnly Property FixTime() As Date
        Get
            Return _fixTime
        End Get
    End Property

    ''' <summary>Object Name. When this property is set, the name is truncated to 255
    ''' characters and sanitized to remove invalid characters. See
    ''' CommonRoutines.Sanitize. If scope-specific uniqueness of Name is required on
    ''' input, test the proposed name first by calling IsNameUnique</summary>
    ''' <returns>Object Name</returns>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = Sanitize(value)
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>This is a memory-only property which is not written to the database.
    ''' Its plugged by the caller afte the object is retrieved to prime it with the
    ''' target URL of the page to load if the object is clicked. This is done before
    ''' databinding, so when the object is databound to a control, the click URL is
    ''' exposed to the control as a bindable property of the object.</summary>
    ''' <returns>String value, as primed by caller</returns>
    Public Property NavTarget() As String
        Get
            Return _navTarget
        End Get
        Set(ByVal value As String)
            _navTarget = value

        End Set
    End Property



    ''' <summary>
    ''' Technote description field, truncated at 4096 characters.
    ''' </summary>
    Public Property ProblemDescription() As String
        Get
            Return _ProblemDescription
        End Get

        Set(ByVal value As String)
            _ProblemDescription = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Technote fix field, truncated at 4096 characters.
    ''' </summary>
    Public Property ProblemFix() As String
        Get
            Return _ProblemFix
        End Get

        Set(ByVal value As String)
            _ProblemFix = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Status of the Technote
    ''' </summary>
    ''' <value></value>
    Public Property Status() As State
        Get
            Return _status
        End Get

        Set(ByVal value As State)
            _status = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Determined resolution of the Technote
    ''' </summary>
    ''' <value></value>
    Public Property Resolution() As Resolutions
        Get
            Return _resolution
        End Get
        Set(ByVal value As Resolutions)
            _resolution = value
        End Set
    End Property

    ''' <summary>
    ''' Parent system object which owns the Technote.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property System() As ATSystem
        Get
            If _Sys Is Nothing Then
                _Sys = New ATSystem
                _Sys.ConnectionString = _connectionString
                _Sys.Retrieve()
            End If
            Return _Sys
        End Get
    End Property

    ''' <summary>
    ''' Person who reported the Technote
    ''' </summary>
    Public Property ReportedBy() As Reporters
        Get
            Return _reportedBy
        End Get
        Set(ByVal value As Reporters)
            _reportedBy = value
        End Set
    End Property


    ''' <summary>
    ''' Person who fixed the Technote
    ''' </summary>
    Public Property FixedBy() As Reporters
        Get
            Return _fixedBy
        End Get
        Set(ByVal value As Reporters)
            _fixedBy = value
        End Set
    End Property



    ''' <summary>
    ''' ID of owning system object
    ''' </summary>
    Public Property systemID() As Integer
        Get
            Return _systemID
        End Get
        Set(ByVal value As Integer)
            _systemID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Friend Property ObjectStatus() As ATSystem.ObjectState
        Get
            Return _ObjectStatus
        End Get
        Set(ByVal value As ATSystem.ObjectState)
            _ObjectStatus = value
        End Set
    End Property


    ''' <summary>Boolean value which defines if the object is marked for deletion. The
    ''' object is neither removed from the parent collection nor deleted from the
    ''' database when the property is set to true. Rather, the object will be physically
    ''' deleted from the db and removed from the collection on the next call to
    ''' object.Update or objectCollection.Update</summary>
    ''' <returns>true if object is marked for deletion, false otherwise</returns>
    Public Property Deleted() As Boolean
        Get
            If _ObjectStatus = ATSystem.ObjectState.Deleted Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            _ObjectStatus = ATSystem.ObjectState.Modified
            If value = True Then _ObjectStatus = ATSystem.ObjectState.Deleted
        End Set
    End Property



    Friend Property ConnectionString() As String
        Get
            Return _connectionString
        End Get
        Set(ByVal value As String)
            _connectionString = value
        End Set
    End Property

    Private Function getConnection() As SqlConnection
        If _connectionString Is Nothing Then _connectionString = ConfigurationManager.ConnectionStrings("ATConnectionString").ConnectionString
        Return New SqlConnection(_connectionString)
    End Function

    ''' <summary>Called to update the object back into the database.
    ''' <para></para>
    ''' <para>If the object has been modified since retrieval, then it is updated into
    ''' the db.</para>
    ''' <para>If the object has been marked for deletion (obj.deleted=true) then it is
    ''' deleted from the database.</para>
    ''' <para>If the object has been newly instantiated, and update has not been
    ''' previously called, then a new record is written to the database, and the ID of
    ''' the newly created record is returned </para></summary>
    ''' <remarks>If a newly substantiated object is to be written to the database,
    ''' ensure that all properties, especially parentID references have been correctly
    ''' plugged prior to calling Update</remarks>
    ''' <returns>Object ID, which is the identity value of the record's Primary
    ''' Key</returns>
    Public Function Update() As Integer

        If _ID = ATSystem.SysConstants.nullValue Then _ObjectStatus = ATSystem.ObjectState.Initial

        Select Case _ObjectStatus
            Case ATSystem.ObjectState.Modified
                _ID = doUpdate()
            Case ATSystem.ObjectState.Initial
                _ID = doInsert()
                _ObjectStatus = ATSystem.ObjectState.Original  ' show newly created object in sync with db
                Return _ID
            Case ATSystem.ObjectState.Deleted
                _ID = doDelete()
        End Select

        Return _ID
    End Function

    Private Function doUpdate() As Integer

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        '
        ' only update fixTime if the status is closed
        '
        Dim fixTime As String = ""
        If _status = State.Closed Then fixTime = "FixTime=getDate(),"

        Cmd.CommandText = "UPDATE dbo.Technote SET " & _
        "SystemID=@SystemID," & _
        "Name=@Name," & _
        "Status=@Status," & _
        "Resolution=@Resolution," & _
        fixTime & _
        "ProblemDescription=@ProblemDescription," & _
        "ProblemFix=@ProblemFix," & _
        "ReportedBy=@ReportedBy," & _
        "FixedBy=@FixedBy " & _
        "WHERE ID=@ID"

        addParams(Cmd)

        Try
            Cmd.Connection.Open()
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return _ID

    End Function

    Private Function doInsert() As Integer

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.CommandText = "INSERT INTO dbo.Technote " & _
        "(SystemID,CreateTime,FixTime,Status,Resolution,Name,ProblemDescription,ProblemFix,ReportedBy,FixedBy) " & _
        "VALUES " & _
        "(@SystemID,getdate(),getdate(),@Status,@Resolution,@Name,@ProblemDescription,@ProblemFix,@ReportedBy,@FixedBy)" & _
        Global.Microsoft.VisualBasic.ChrW(13) & _
        Global.Microsoft.VisualBasic.ChrW(10) & "SELECT SCOPE_IDENTITY();"

        addParams(Cmd)

        Try
            Cmd.Connection.Open()
            _ID = CType(Cmd.ExecuteScalar(), Integer)
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return _ID

    End Function


    Private Sub addParams(ByVal Cmd As SqlCommand)

        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@Resolution", SqlDbType.Int).Value = _resolution
        Cmd.Parameters.Add("@SystemID", SqlDbType.Int).Value = _systemID
        Cmd.Parameters.Add("@ReportedBy", SqlDbType.Int).Value = _reportedBy
        Cmd.Parameters.Add("@FixedBy", SqlDbType.Int).Value = _fixedBy
        Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = _name
        Cmd.Parameters.Add("@ProblemDescription", SqlDbType.VarChar).Value = _ProblemDescription
        Cmd.Parameters.Add("@ProblemFix", SqlDbType.VarChar).Value = _ProblemFix

    End Sub

    Private Function doDelete() As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        '
        ' delete embedded collections first
        '
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID
        Try
            Cmd.Connection.Open()
            Cmd.CommandText = "DELETE dbo.Technote WHERE ID=@ID"
            Cmd.ExecuteNonQuery()

        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return ATSystem.SysConstants.nullValue

    End Function

    Friend Sub DR2Object(ByVal dr As IDataRecord)
        _ObjectStatus = ATSystem.ObjectState.Original
        _systemID = Convert.ToInt32(dr("SystemID"))
        _name = Convert.ToString(dr("Name"))
        _ID = Convert.ToInt32(dr("ID"))
        _status = CType(dr("Status"), State)
        _reportedBy = CType(dr("ReportedBy"), Reporters)
        _resolution = CType(dr("Resolution"), Resolutions)
        _fixedBy = CType(dr("FixedBy"), Reporters)
        _ProblemDescription = Convert.ToString(dr("ProblemDescription"))
        _ProblemFix = Convert.ToString(dr("ProblemFix"))
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _fixTime = Convert.ToDateTime(dr("FixTime"))
    End Sub

End Class

