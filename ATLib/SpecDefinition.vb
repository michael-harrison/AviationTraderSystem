Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.ComponentModel
Imports System



'***************************************************************************************
'*
'* SpecDefinitions
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The spec dictionary contains definitions of all the specs on a per SpecGroup basis.
''' </para>
''' </summary>
Public Class SpecDefinitions : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Ads(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Ads.count-1</param>
    ''' <value>Ad object from Ads collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As SpecDefinition
        Get
            Return CType(List(index), SpecDefinition)
        End Get
        Set(ByVal value As SpecDefinition)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds an Ad object to the Ads collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Ad object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As SpecDefinition) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As SpecDefinition) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As SpecDefinition)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As SpecDefinition)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As SpecDefinition) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.SpecDefinition"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As SpecDefinition
        Return Retrieve(Hex2Int(HexID))
    End Function



    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="ID">ObjectID, represented as an integer</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal ID As Integer) As SpecDefinition
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.SpecDefinition WHERE ID=@ID"

        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function



    ''' <summary>
    ''' Retreives the set of specs for the spec group
    ''' </summary>
    ''' <param name="SpecGroupID">SpecGroupID</param>
    ''' <remarks></remarks>
    Friend Sub retrieveSet(ByVal SpecGroupID As Integer)
        '
        ' retrieves the set of specs for the spec group
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.Parameters.Add("@SpecGroupID", SqlDbType.Int).Value = SpecGroupID
        Cmd.CommandText = "Select * from dbo.SpecDefinition where SpecGroupID=@SpecGroupID ORDER BY SortKey"
        doRetrieveR(Cmd)

    End Sub

    ''' <summary>
    ''' Retreives the set of specs for all spec groups within the classification
    ''' </summary>
    ''' <param name="ClassificationID">ClassificationID</param>
    ''' <remarks></remarks>
    Friend Sub retrieveClassificationSet(ByVal ClassificationID As Integer)
        '
        ' retrieves the set of specs for the spec group
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.Parameters.Add("@ClassificationID", SqlDbType.Int).Value = ClassificationID
        Cmd.CommandText = "Select SpecDefinition.*,SpecGroup.sortkey as groupSortKey FROM dbo.SpecDefinition " & _
        "INNER JOIN dbo.SpecGroup on SpecGroup.ID=SpecDefinition.SpecGroupID " & _
        "WHERE SpecGroup.ClassificationID=@ClassificationID ORDER BY GroupSortKey,SpecDefinition.SortKey"
        doRetrieveR(Cmd)

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
        Dim deletedObjects As New List(Of SpecDefinition)

        For Each specdefn As SpecDefinition In List
            specdefn.Update()
            If specdefn.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(specdefn)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As SpecDefinition In deletedObjects
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
                Dim SpecDefinition As New SpecDefinition
                Add(SpecDefinition)
                SpecDefinition.ConnectionString = _connectionString
                SpecDefinition.DR2Object(dr)
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
'* SpecDefinition
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para> The spec definiton object holds the definition of a specification
''' </para>
''' </summary>
Public Class SpecDefinition
    ''' <summary>
    ''' Defines the display types for the spec builder
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum DisplayTypes
        Text = 1
        TextArea = 2
        RadioHorizontal = 3
        RadioVertical = 4
        CheckBox = 5

    End Enum


    Private _connectionString As String
    Private _ID As Integer
    Private _SpecGroupID As Integer
    Private _createTime As Date
    Private _modifyTime As Date
    Private _name As String
    Private _status As ATSystem.StatusBits
    Private _ObjectStatus As ATSystem.ObjectState
    Private _SortKey As String
    Private _ValueList As String
    Private _displayType As DisplayTypes
    Private _navTarget As String
    '
    ' embedded collections
    '
    Private _SpecGroup As SpecGroup         'backpointer to spec group

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
    ''' Returns the ID of the spec group
    ''' </summary>
    Public Property SpecGroupID() As Integer
        Get
            Return _SpecGroupID
        End Get
        Set(ByVal value As Integer)
            _SpecGroupID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Returns the spec group object that the spec definition is in
    ''' </summary>
    Public ReadOnly Property SpecGroup() As SpecGroup
        Get
            If _SpecGroup Is Nothing Then
                Dim SpecGroups As New SpecGroups
                SpecGroups.ConnectionString = _connectionString
                _SpecGroup = SpecGroups.Retrieve(_SpecGroupID)
            End If
            Return _SpecGroup
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
    ''' bits, it may be more convenient to use the specifically Valued boolean properties
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
    ''' Specifies the Production status of the Ad
    ''' </summary>
    Public Property SortKey() As String
        Get
            Return _SortKey
        End Get
        Set(ByVal value As String)
            _SortKey = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the Run status of the Ad
    ''' </summary>
    Public Property DisplayType() As DisplayTypes
        Get
            Return _displayType
        End Get
        Set(ByVal value As DisplayTypes)
            _displayType = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Name - name of the spec definition
    ''' </summary>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Value List - value options or default value
    ''' see this article - http://stackoverflow.com/questions/2104734/textbox-line-endings-on-the-web
    ''' So normalise line endings to be just OA by stripping 0D
    ''' </summary>
    Public Property ValueList() As String
        Get
            Return _ValueList
        End Get
        Set(ByVal value As String)
            Dim cr As Char = Chr(&HD)
            _ValueList = value.Replace(cr, "")
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
        Cmd.CommandText = "UPDATE dbo.SpecDefinition SET " & _
        "modifyTime=getdate()," & _
        "ValueList=@ValueList," & _
        "DisplayType=@DisplayType," & _
        "Name=@Name," & _
        "Status=@Status," & _
        "SortKey=@SortKey," & _
        "SpecGroupID=@SpecGroupID" & _
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
        Cmd.CommandText = "INSERT INTO dbo.SpecDefinition " & _
        "(Name,SpecGroupID,createTime,modifyTime," & _
        "ValueList,SortKey,DisplayType,Status) " & _
        "VALUES (@Name,@SpecGroupID,getdate(),getdate()," & _
        "@ValueList,@SortKey,@DisplayType,@Status)" & _
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
        Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = _name
        Cmd.Parameters.Add("@SortKey", SqlDbType.VarChar).Value = _SortKey
        Cmd.Parameters.Add("@ValueList", SqlDbType.VarChar).Value = _ValueList
        Cmd.Parameters.Add("@SpecGroupID", SqlDbType.Int).Value = _SpecGroupID
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@DisplayType", SqlDbType.Int).Value = _displayType

    End Sub

    ''' <summary>Physically deletes the object and subordinate structure from the db
    '''</summary> 
    Private Function doDelete() As Integer

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID

        Try
            Cmd.Connection.Open()
            '
            ' delete specs first
            ' 
            Cmd.CommandText = "DELETE dbo.Spec WHERE SpecDefinitionID=@ID"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.SpecDefinition WHERE ID=@ID"
            Cmd.ExecuteNonQuery()

        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return ATSystem.SysConstants.nullValue

    End Function



    Friend Sub DR2Object(ByVal dr As IDataRecord)
        _ObjectStatus = ATSystem.ObjectState.Original
        _ID = Convert.ToInt32(dr("ID"))
        _name = Convert.ToString(dr("Name"))
        _ValueList = Convert.ToString(dr("ValueList"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _SortKey = Convert.ToString(dr("SortKey"))
        _displayType = CType(dr("DisplayType"), DisplayTypes)
        _SpecGroupID = Convert.ToInt32(dr("SpecGroupID"))
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))

    End Sub

End Class



