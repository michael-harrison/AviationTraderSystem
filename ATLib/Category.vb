Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System



'***************************************************************************************
'*
'* Categories
'*
'*
'*
'***************************************************************************************

''' <summary>
''' The Categorys class implements a collection of Category objects, which are directly subordinate
''' to the System object. System Colors provide a single system wide reference of all spot colors that can be defined
''' in inks, foils and paper. System Colors are maintained in the RGB color space.
''' </summary>
Public Class Categories : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Categorys(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Categorys.count-1</param>
    ''' <value>Category object from Categorys collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Category
        Get
            Return CType(List(index), Category)
        End Get
        Set(ByVal value As Category)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a Category object to the Categorys collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Category object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As Category) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Category) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Category)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Category)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Category) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.Category ORDER BY SortKey"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Category
        '
        ' retrieves a specific record
        '
        Return Retrieve(Hex2Int(HexID))
    End Function

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="ID">ObjectID, represented as an integer</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal ID As Integer) As Category
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Category WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function

    Friend Sub retrieveSet(ByVal systemID As Integer)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "SELECT * from dbo.Category WHERE SystemID=@ID ORDER BY SortKey"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = systemID
        doRetrieveR(Cmd)

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
                Dim Category As New Category
                Add(Category)
                Category.ConnectionString = _connectionString
                Category.DR2Object(dr)
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
        Dim deletedObjects As New List(Of Category)

        For Each Category As Category In List
            Category.Update()
            If Category.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Category)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Category In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class



'***************************************************************************************
'*
'* Category
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para> The Category object holds all data retrieved from a specified row of the Category table.
'''</para>
''' <para>It is always accessed as part of the Categorys collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Categorys collection
'''  property of the parent. 
''' </para>
''' </summary>
''' 
Public Class Category

    Private _connectionString As String
    Private _ID As Integer
    Private _systemID As Integer
    Private _name As String
    Private _navTarget As String
    Private _ObjectStatus As ATSystem.ObjectState
    Private _status As ATSystem.StatusBits
    Private _createTime As Date
    Private _modifyTime As Date
    Private _SortKey As String
    Private _classificationCount As Integer
    Private _adCount As Integer
    '
    ' embedded collections
    '
    Private _Sys As ATSystem
    Private _Classifications As Classifications

    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _ID = ATSystem.SysConstants.nullValue
        _status = 0
        _classificationCount = -1
        _adCount = -1
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

    ''' <summary>Timestamp, read from dbTime, that the database object was last modified</summary>
    ''' <value>Long timestamp, in the current culture</value>
    Public ReadOnly Property ModifyTime() As Date
        Get
            Return _modifyTime
        End Get
    End Property

    ''' <summary>This is a set of 32 boolean status flags, implemented as a 32 bit
    ''' integer. The bit values are defined in ATSystem.StatusBits. The status property can
    ''' be used to set/test/clear bits within the word. For operations on individual
    ''' bits, it may be more convenient to use the specifically named boolean properties
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
            _name = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Defines the sort sequence of colors in the collection. This is used because VComp requires that
    ''' foils in particular are enumerated in a user-definable order.
    ''' </summary>
    Public Property SortKey() As String
        Get
            Return _SortKey
        End Get
        Set(ByVal value As String)
            _SortKey = value.ToUpper
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Parent system object, to which the Categorys are subordinate
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


    ''' <summary>Embedded collection of Classification objects. Holds all classifications in the Category</summary>
    ''' <value>Collection of Ad objects in the Category</value>
    Public ReadOnly Property Classifications() As Classifications
        Get
            If _Classifications Is Nothing Then
                _Classifications = New Classifications
                _Classifications.ConnectionString = _connectionString
                _Classifications.retrieveSet(_ID)
            End If
            Return _Classifications
        End Get
    End Property

    ''' <summary>ID of the System, to which this object is directly subordinate</summary>
    ''' <value>Object ID as an integer</value>
    Public Property systemID() As Integer
        Get
            Return _systemID
        End Get
        Set(ByVal value As Integer)
            _systemID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Defines whether the ad is enabled or disabled.
    ''' </summary>
    ''' <value>True if the spec is active, false otherwise.</value>
    Public Property IncludeInBar() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IncludeInBar And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IncludeInBar
            Else
                _status = _status And Not ATSystem.StatusBits.IncludeInBar
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    Public ReadOnly Property ClassificationCount() As Integer
        Get
            If _classificationCount = -1 Then
                Dim Classifications As New Classifications
                Classifications.ConnectionString = _connectionString
                _classificationCount = Classifications.GetClassificationCount(ATSystem.ObjectTypes.Category, _ID)
            End If
            Return _ClassificationCount
        End Get
    End Property

    Public ReadOnly Property AdCount() As Integer
        Get
            If _adCount = -1 Then
                Dim Ads As New Ads
                Ads.ConnectionString = _connectionString
                _adcount = Ads.GetAdCount(ATSystem.ObjectTypes.Category, _ID)
            End If
            Return _adcount
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


    ''' <summary>
    ''' Checks that the supplied name is unique within the scope of the color type, paper, ink or foil.
    ''' </summary>
    ''' <param name="Name">Nzme to test for uniqueness</param>
    ''' <returns>True if name is unique within scope, false otherise.</returns>
    Public Function IsNameUnique(ByVal Name As String) As Boolean
        '
        ' checks that the name is unique for a parent
        ' returns true if the name is unique, false if it exists
        '
        Dim rtnval As Boolean = False           'assume not unique

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT COUNT(ID) From dbo.Category WHERE Name=@Name AND SystemID=@SystemID AND ID<>@ID"
        Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name
        Cmd.Parameters.Add("@SystemID", SqlDbType.Int).Value = _systemID
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID
        Try
            Cmd.Connection.Open()
            If Convert.ToInt32(Cmd.ExecuteScalar()) = 0 Then rtnval = True
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return rtnval

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
        Cmd.CommandText = "UPDATE dbo.Category SET " & _
        "modifyTime=getdate()," & _
        "SystemID=@SystemID," & _
        "Name=@Name," & _
        "Status=@Status," & _
        "SortKey=@SortKey " & _
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
        Cmd.CommandText = "INSERT INTO dbo.Category " & _
        "(SystemID,CreateTime,ModifyTime,Status,Name,SortKey) " & _
        "VALUES " & _
        "(@SystemID,getdate(),getdate(),@Status,@Name,@SortKey)" & _
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
        Cmd.Parameters.Add("@SystemID", SqlDbType.Int).Value = _systemID
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = _name
        Cmd.Parameters.Add("@SortKey", SqlDbType.VarChar).Value = _SortKey

    End Sub

    Private Function doDelete() As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Try
            Cmd.Connection.Open()
            Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID

            Cmd.CommandText = "DELETE dbo.Category WHERE ID=@ID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return ATSystem.SysConstants.nullValue

    End Function

    Friend Sub DR2Object(ByVal dr As IDataRecord)
        _ObjectStatus = ATSystem.ObjectState.Original
        _ID = Convert.ToInt32(dr("ID"))
        _systemID = Convert.ToInt32(dr("SystemID"))
        _name = Convert.ToString(dr("Name"))
        _SortKey = Convert.ToString(dr("SortKey"))
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
    End Sub

End Class

