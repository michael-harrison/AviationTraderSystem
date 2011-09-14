Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Configuration
Imports System


'***************************************************************************************
'*
'* Publications
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The Publications collection is used to hold the set of Publication
''' objects. The collection is normally accessed by using the Publications property of the parent System object.
'''  This class manages the Publication database table. Any object in the collection can be modified and then written back to the database,
'''  by either the Update method of the collection class, or the Update method of each object within the collection.
''' </para>
''' </summary>
Public Class Publications : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Publications(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Publications.count-1</param>
    ''' <value>Publication object from Publications collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Publication
        Get
            Return CType(List(index), Publication)
        End Get
        Set(ByVal value As Publication)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a Publication object to the Publications collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Publication object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As Publication) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Publication) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Publication)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Publication)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Publication) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.Publication ORDER BY SortKey"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Publication
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
    Public Function Retrieve(ByVal ID As Integer) As Publication
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Publication WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function


    Public Sub retrieveSet(ByVal SystemID As Integer, ByVal Type As Publication.Types)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        If Type = Publication.Types.Unspecified Then
            Cmd.CommandText = "SELECT * from dbo.Publication WHERE SystemID=@ID ORDER BY SortKey"
        Else
            Cmd.CommandText = "SELECT * from dbo.Publication WHERE SystemID=@ID AND Type=@Type ORDER BY SortKey"
            Cmd.Parameters.Add("@Type", SqlDbType.Int).Value = Type
        End If

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
                Dim Publication As New Publication
                Add(Publication)
                Publication.ConnectionString = _connectionString
                Publication.DR2Object(dr)
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
        Dim deletedObjects As New List(Of Publication)

        For Each Publication As Publication In List
            Publication.Update()
            If Publication.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Publication)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Publication In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class



'***************************************************************************************
'*
'* Publication - Publication object
'*
'*
'*
'*
'*
'**************************************************************************************

''' <summary>
''' <para> The Publication object holds all data retrieved from a specified row of the Publication table.
'''</para> It is always accessed as part of the Publications collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Publications collection
'''  property of the parent. 
''' </summary>
Public Class Publication
    Private _connectionString As String
    Private _ID As Integer
    Private _systemID As Integer
    Private _name As String
    Private _description As String
    Private _sortKey As String
    Private _navTarget As String
    Private _navTarget2 As String
    Private _ObjectStatus As ATSystem.ObjectState
    Private _status As ATSystem.StatusBits
    Private _createTime As Date
    Private _modifyTime As Date
    Private _type As Types
    Private _adCount As Integer
    Private _editionCount As Integer
    Private _productCount As Integer
    '
    ' embedded collections
    '
    Private _Sys As ATSystem
    Private _Products As Products
    Private _editions As Editions
    Private _featuredAds As AdInstances
    Private _firstOpenEdition As Edition
    Private _ProdnStatus As Edition.ProdnState

    Public Enum Types
        Unspecified = 0
        <Description("AT Display")> ATDisplay = 1
        <Description("AT Classifieds")> ATClassified = 2
        <Description("Web Site")> WebSite = 3
        <Description("Email Service")> EmailService = 4
        <Description("EMagazine")> EMagazine = 5
    End Enum

    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _ID = ATSystem.SysConstants.nullValue
        _status = 0
        _adCount = -1
        _editionCount = -1
        _productCount = -1
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


    ''' <summary>This is a memory-only property which is not written to the database.
    ''' Its plugged by the caller afte the object is retrieved to prime it with the
    ''' target URL of the page to load if the object is clicked. This is done before
    ''' databinding, so when the object is databound to a control, the click URL is
    ''' exposed to the control as a bindable property of the object.</summary>
    ''' <returns>String value, as primed by caller</returns>
    Public Property NavTarget2() As String
        Get
            Return _navTarget2
        End Get
        Set(ByVal value As String)
            _navTarget2 = value

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

    ''' <summary>Description</summary>
    ''' <returns>Object Name</returns>
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Sort Key</summary>
    ''' <returns>Sort Key</returns>
    Public Property SortKey() As String
        Get
            Return _sortKey
        End Get
        Set(ByVal value As String)
            _sortKey = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Defines the type of publication
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Type() As Types
        Get
            Return _type
        End Get
        Set(ByVal value As Types)
            _type = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property



    ''' <summary>
    ''' Embedded system object, to which this Publication is subordinate.
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

    ''' <summary>ID of the System, to which this object is directly subordinate</summary>
    ''' <value>Object ID as an integer</value>
    Public Property SystemID() As Integer
        Get
            Return _systemID
        End Get
        Set(ByVal value As Integer)
            _systemID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Embedded collection of product objects.</summary>
    ''' <returns>Returns collection of categories subordinate to the publication whose status is as requested</returns>
    Public ReadOnly Property Products() As Products
        '
        ' if there is no Products collection, then get a new collection
        '
        Get
            If (_Products Is Nothing) Then
                _Products = New Products()                      'get a new collection
                _Products.ConnectionString = _connectionString
                _Products.retrieveSet(_ID)
            End If
            Return _Products
        End Get
    End Property

    ''' <summary>Embedded collection of edition objects.</summary>
    ''' <returns>Returns collection of editions subordinate to the publication whose status is as requested</returns>
    Public ReadOnly Property Editions() As Editions
        Get
            Return Editions(Edition.ProdnState.Unspecified)
        End Get
    End Property


    ''' <summary>
    ''' Returns the first open edition of the publication
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FirstOpenEdition() As Edition
        Get
            If _firstOpenEdition Is Nothing Then
                Dim myEditions As New Editions
                myEditions.ConnectionString = _connectionString
                myEditions.retrieveSet(_ID, Edition.ProdnState.Open)
                If myEditions.Count > 0 Then _firstOpenEdition = myEditions(0)
            End If

            Return _firstOpenEdition
        End Get
    End Property


    ''' <summary>Embedded collection of edition objects.</summary>
    ''' <param name="ProdnStatus">edition status</param>
    ''' <returns>Returns collection of editions subordinate to the publication whose status is as requested</returns>
    Public ReadOnly Property Editions(ByVal ProdnStatus As Edition.ProdnState) As Editions
        '
        ' if there is no Editions collection or if the status has changed, then get a new collection
        '
        Get
            If (_editions Is Nothing) Or (_ProdnStatus <> ProdnStatus) Then
                _editions = New Editions()                      'get a new collection
                _ProdnStatus = ProdnStatus
                _editions.ConnectionString = _connectionString
                _editions.retrieveSet(_ID, ProdnStatus)
            End If
            Return _editions
        End Get
    End Property

    ''' <summary>
    ''' featured ads
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property FeaturedAds(ByVal Visibility As Edition.VisibleState) As AdInstances
        '
        ' if there is no Instances collection then get a new collection
        '
        Get
            If _featuredAds Is Nothing Then
                _featuredAds = New AdInstances
                _featuredAds.ConnectionString = _connectionString
                _featuredAds.RetrieveFeaturedSet(_ID, Visibility, Ad.ProdnState.Approved)
            End If
            Return _featuredAds
        End Get
    End Property

    Public ReadOnly Property AdCount() As Integer
        Get
            If _adCount = -1 Then
                Dim Ads As New Ads
                Ads.ConnectionString = _connectionString
                _adCount = Ads.GetAdCount(ATSystem.ObjectTypes.Publication, _ID)
            End If
            Return _adCount
        End Get
    End Property


    Public ReadOnly Property ProductCount() As Integer
        Get
            If _productCount = -1 Then
                Dim Products As New Products
                Products.ConnectionString = _connectionString
                _productCount = Products.GetProductCount(ATSystem.ObjectTypes.Publication, _ID)
            End If
            Return _productCount
        End Get
    End Property


    Public ReadOnly Property EditionCount() As Integer
        Get
            If _editionCount = -1 Then
                Dim Editions As New Editions
                Editions.ConnectionString = _connectionString
                _editionCount = Editions.GetEditionCount(ATSystem.ObjectTypes.Publication, _ID)
            End If
            Return _editionCount
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
        Cmd.CommandText = "UPDATE dbo.Publication SET " & _
        "modifyTime=getdate()," & _
        "SystemID=@SystemID," & _
        "Status=@Status," & _
        "Name=@Name," & _
        "Description=@Description," & _
        "Type=@Type," & _
        "SortKey=@SortKey" & _
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
        Cmd.CommandText = "INSERT INTO dbo.Publication " & _
        "(SystemID,CreateTime,ModifyTime,Status,Name,Description,Type,SortKey) " & _
        "VALUES " & _
        "(@SystemID,getdate(),getdate(),@Status,@Name,@Description,@Type,@SortKey)" & _
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
        Cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = _description
        Cmd.Parameters.Add("@Type", SqlDbType.Int).Value = _type
        Cmd.Parameters.Add("@SortKey", SqlDbType.VarChar).Value = _sortKey

    End Sub



    ''' <summary>Physically deletes the object and subordinate structure from the db
    '''</summary> 
    Private Function doDelete() As Integer

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        '
        ' delete embedded collections first
        '
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID
        Try
            Cmd.Connection.Open()

            ''Cmd.CommandText = "DELETE dbo.Contact2Publication WHERE ParentID=@ID"
            ''Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.Publication WHERE ID=@ID"
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
        _description = Convert.ToString(dr("Description"))
        _sortKey = Convert.ToString(dr("SortKey"))
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _type = CType(dr("Type"), Types)
    End Sub

End Class

