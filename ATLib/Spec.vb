Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.ComponentModel
Imports System



'***************************************************************************************
'*
'* Specs
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The Specs collection contains a set of Spec objects.
''' </para>
''' </summary>
Public Class Specs : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Specs(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Specs.count-1</param>
    ''' <value>Spec object from Specs collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Spec
        Get
            Return CType(List(index), Spec)
        End Get
        Set(ByVal value As Spec)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds an Spec object to the Specs collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Spec object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As Spec) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Spec) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Spec)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Spec)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Spec) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.Spec"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Spec
        Return Retrieve(Hex2Int(HexID))
    End Function



    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="ID">ObjectID, represented as an integer</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal ID As Integer) As Spec
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = commonSQL() & " WHERE Spec.ID=@ID"

        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function

    Friend Sub retrieveSet(ByVal AdID As Integer, ByVal SpecGroupID As Integer, ByVal Status As ATSystem.StatusBits)
        '
        ' retrieves the set of Specs for the ad or specgroup or both with the status as requested
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        If SpecGroupID = ATSystem.SysConstants.nullValue Then
            Cmd.CommandText = commonSQL() & " WHERE AdID=@AdID"
        ElseIf AdID = ATSystem.SysConstants.nullValue Then
            Cmd.CommandText = commonSQL() & " WHERE SpecDefinition.SpecGroupID=@SpecgroupID"
        Else
            Cmd.CommandText = commonSQL() & " WHERE AdID=@AdID AND SpecDefinition.SpecGroupID=@SpecgroupID"
        End If

        Cmd.Parameters.Add("@SpecGroupID", SqlDbType.Int).Value = SpecGroupID
        Cmd.Parameters.Add("@AdID", SqlDbType.Int).Value = AdID

        If Status <> 0 Then
            Cmd.CommandText &= " AND (Spec.Status & @Status) = @Status"
            Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = Status
        End If

        Cmd.CommandText &= " ORDER BY SpecDefinition.SortKey"
        doRetrieveR(Cmd)

    End Sub

    Private Function commonSQL() As String
        Dim SQL As String = "Select Spec.*," & _
                "SpecDefinition.Name as Name," & _
                "SpecDefinition.DisplayType as DisplayType," & _
                "SpecDefinition.ValueList as Valuelist " & _
                "from dbo.Spec " & _
                "INNER JOIN dbo.SpecDefinition on Spec.SpecDefinitionID = SpecDefinition.ID"

        Return SQL
    End Function


    Friend Sub DeleteSet(ByVal AdID As Integer)

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@AdID", SqlDbType.Int).Value = AdID

        Try
            Cmd.Connection.Open()

            Cmd.CommandText = "DELETE dbo.Spec WHERE AdID=@AdID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
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
        Dim deletedObjects As New List(Of Spec)

        For Each Spec As Spec In List
            Spec.Update()
            If Spec.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Spec)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Spec In deletedObjects
            List.Remove(s)
        Next

    End Sub

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
                Dim Spec As New Spec
                Add(Spec)
                Spec.ConnectionString = _connectionString
                Spec.DR2Object(dr)
            End While
        Finally
            If Not dr Is Nothing Then
                dr.Close()
                Cmd.Connection.Dispose()
            End If
        End Try
    End Sub

End Class




'***************************************************************************************
'*
'* Spec
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para> The Spec object holds all data retrieved from a specified row of the Spec table.
'''</para>
''' <para>It is always accessed as part of the Specs collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Specs collection
'''  property of the parent. 
''' </para>
''' </summary>
Public Class Spec

    Private _connectionString As String
    Private _ID As Integer
    Private _AdID As Integer
    Private _specDefinitionID As Integer
    Private _createTime As Date
    Private _modifyTime As Date
    Private _value As String

    Private _status As ATSystem.StatusBits
    Private _ObjectStatus As ATSystem.ObjectState
    '
    ' joined data from SpecDefinitions
    '
    Private _name As String
    Private _displayType As SpecDefinition.DisplayTypes
    Private _valueList As String

    '
    ' embedded collections
    '
    Friend _Ad As Ad

    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
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


    ''' <summary>
    ''' Returns the ID of the user who placed the Spec
    ''' </summary>
    Public Property AdID() As Integer
        Get
            Return _AdID
        End Get
        Set(ByVal value As Integer)
            _AdID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property



    ''' <summary>
    ''' Returns the ID of the classification that the Spec is in
    ''' </summary>
    Public Property SpecDefinitionID() As Integer
        Get
            Return _specDefinitionID
        End Get
        Set(ByVal value As Integer)
            _specDefinitionID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Returns the Ad object that the Spec is in
    ''' </summary>
    Public ReadOnly Property Ad() As Ad
        Get
            If _Ad Is Nothing Then
                Dim Ads As New Ads
                Ads.ConnectionString = _connectionString
                _Ad = Ads.Retrieve(_AdID)
            End If
            Return _Ad
        End Get
    End Property

    ''' <summary>
    ''' Defines whether the spec is active for the ad.
    ''' </summary>
    ''' <value>True if the spec is active, false otherwise.</value>
    Public Property IsActive() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsSpecActive And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.isSpecActive
            Else
                _status = _status And Not ATSystem.StatusBits.isSpecActive
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Spec name, from data dictionary
    ''' </summary>
    Public ReadOnly Property Name() As String
        Get
            Return _name
        End Get
    End Property

    ''' <summary>
    ''' Spec value list, from data dictionary
    ''' </summary>
    Public ReadOnly Property ValueList() As String
        Get
            Return _valueList
        End Get
    End Property

    ''' <summary>
    ''' Spec display type, from data dictionary
    ''' </summary>
    Public ReadOnly Property DisplayType() As SpecDefinition.DisplayTypes
        Get
            Return _displayType
        End Get
    End Property

    Friend Property ObjectStatus() As ATSystem.ObjectState
        Get
            Return _ObjectStatus
        End Get
        Set(ByVal value As ATSystem.ObjectState)
            _ObjectStatus = value
        End Set
    End Property

    ''' <summary>This is a set of 32 boolean status flags, implemented as a 32 bit
    ''' integer. The bit values are defined in ATSystem.StatusBits. The status property can
    ''' be used to set/test/clear bits within the word. For operations on individual
    ''' bits, it may be more convenient to use the specifically Adnumberd boolean properties
    ''' of this object.</summary>
    ''' <returns>32 status bits, defined in ATSystem.StatusBits, and implemented as an
    ''' integer</returns>
    ''' <example>
    ''' <code>dim currentStatus as ATSystem.StatusBits = obj.Status
    ''' currentStatus = CurrentStatus or (ATSystem.StatusBits.isStressed or ATSystem.StatusBits.hasCustomStressedIcon) 'sets these bits
    ''' currentStatus = CurrsntStatus and not ATSystem.StatusBits.isOpenForEdit 'clear this bit
    ''' obj.Status = currentStatus 'modify object in memory
    ''' obj.Update 'and update to db</code></example>
    Public Property Status() As ATSystem.StatusBits
        Get
            Return _status
        End Get
        Set(ByVal value As ATSystem.StatusBits)
            _status = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Value of Spec
    ''' </summary>
    Public Property Value() As String
        Get
            Return _value
        End Get
        Set(ByVal value As String)
            _value = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Timestamp, read from dbTime, that the database object was created</summary>
    ''' <value>Long timestamp, in the current culture</value>
    Public ReadOnly Property CreateTime() As Date
        Get
            Return _createTime
        End Get
    End Property

    ''' <summary>Timestamp, read from dbTime, that the database object was last modified</summary>
    ''' <value>Long timestamp, in the current culture</value>
    Public ReadOnly Property ModifyTime() As Date
        Get
            Return _modifyTime
        End Get
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
        Cmd.CommandText = "UPDATE dbo.Spec SET " & _
        "modifyTime=getdate()," & _
        "AdID=@AdID," & _
        "SpecDefinitionID=@SpecDefinitionID," & _
        "Value=@Value," & _
        "Status=@Status" & _
        " WHERE ID=@ID"

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
        Cmd.CommandText = "INSERT INTO dbo.Spec " & _
        "(AdID,SpecDefinitionID,createTime,modifyTime," & _
        "Value,Status) " & _
        "VALUES (@AdID,@SpecDefinitionID,getdate(),getdate()," & _
        "@Value,@Status)" & _
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
        Cmd.Parameters.Add("@Value", SqlDbType.VarChar).Value = _value
        Cmd.Parameters.Add("@AdID", SqlDbType.Int).Value = _AdID
        Cmd.Parameters.Add("@SpecDefinitionID", SqlDbType.Int).Value = _specDefinitionID
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status

    End Sub

    ''' <summary>Physically deletes the object and subordinate structure from the db
    '''</summary> 
    Private Function doDelete() As Integer

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID

        Try
            Cmd.Connection.Open()

            Cmd.CommandText = "DELETE dbo.Spec WHERE ID=@ID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return ATSystem.SysConstants.nullValue

    End Function



    Friend Sub DR2Object(ByVal dr As IDataRecord)
        _ObjectStatus = ATSystem.ObjectState.Original
        _ID = Convert.ToInt32(dr("ID"))
        _AdID = Convert.ToInt32(dr("AdID"))
        _specDefinitionID = Convert.ToInt32(dr("SpecDefinitionID"))
        _value = Convert.ToString(dr("Value"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        '
        ' readonly values from joined SpecDefinitions table
        '
        _name = Convert.ToString(dr("Name"))
        _valueList = Convert.ToString(dr("ValueList"))
        _displayType = CType(dr("DisplayType"), SpecDefinition.DisplayTypes)

    End Sub

End Class



