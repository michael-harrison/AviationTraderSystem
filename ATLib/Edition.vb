Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Configuration
Imports System


'***************************************************************************************
'*
'* Editions
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The Editions collection is used to hold the set of Edition
''' objects. The collection is normally accessed by using the Editions property of the parent System object.
'''  This class manages the Edition database table. Any object in the collection can be modified and then written back to the database,
'''  by either the Update method of the collection class, or the Update method of each object within the collection.
''' </para>
''' </summary>
Public Class Editions : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Editions(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Editions.count-1</param>
    ''' <value>Edition object from Editions collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Edition
        Get
            Return CType(List(index), Edition)
        End Get
        Set(ByVal value As Edition)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a Edition object to the Editions collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Edition object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As Edition) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Edition) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Edition)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Edition)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Edition) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.Edition ORDER BY SortKey"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Edition
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
    Public Function Retrieve(ByVal ID As Integer) As Edition
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Edition WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function


    Public Sub retrieveSet(ByVal PublicationID As Integer, ByVal ProdnStatus As Edition.ProdnState)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        If ProdnStatus = Edition.ProdnState.Unspecified Then
            Cmd.CommandText = "SELECT * from dbo.Edition WHERE PublicationID=@ID ORDER BY SortKey"
        Else
            Cmd.CommandText = "SELECT * from dbo.Edition WHERE PublicationID=@ID AND ProdnStatus=@ProdnStatus ORDER BY SortKey"
            Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = ProdnStatus
        End If

        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = PublicationID
        doRetrieveR(Cmd)

    End Sub

    ''' <summary>
    ''' Returns the total number of Products according the the object type and ID passed
    ''' </summary>
    ''' <param name="ObjectType"></param>
    ''' <param name="ObjectID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEditionCount(ByVal ObjectType As ATSystem.ObjectTypes, ByVal ObjectID As Integer) As Integer
        Dim SQL As String = ""
        Select Case ObjectType
            Case ATSystem.ObjectTypes.System
                SQL = "Select Count(ID) from dbo.Edition"

            Case ATSystem.ObjectTypes.Publication
                SQL = "Select Count(Edition.ID) from dbo.Edition " & _
                "WHERE Edition.PublicationID=@ObjectID"

            Case Else
                SQL = "Select Count(ID) from dbo.Edition"
        End Select

        Dim count As Integer = 0
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ObjectID", SqlDbType.Int).Value = ObjectID

        Try
            Cmd.Connection.Open()
            Cmd.CommandText = SQL
            count = CType(Cmd.ExecuteScalar(), Integer)
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return count
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
                Dim Edition As New Edition
                Add(Edition)
                Edition.ConnectionString = _connectionString
                Edition.DR2Object(dr)
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
        Dim deletedObjects As New List(Of Edition)

        For Each Edition As Edition In List
            Edition.Update()
            If Edition.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Edition)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Edition In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class



'***************************************************************************************
'*
'* Edition - Edition object
'*
'*
'*
'*
'*
'**************************************************************************************

''' <summary>
''' <para> The Edition object holds all data retrieved from a specified row of the Edition table.
'''</para> It is always accessed as part of the Editions collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Editions collection
'''  property of the parent. 
''' </summary>
Public Class Edition
    Private _connectionString As String
    Private _ID As Integer
    Private _PublicationID As Integer
    Private _name As String
    Private _shortname As String
    Private _Description As String
    Private _navTarget As String
    Private _ObjectStatus As ATSystem.ObjectState
    Private _status As ATSystem.StatusBits
    Private _createTime As Date
    Private _modifyTime As Date
    Private _adDeadline As Date
    Private _prodnDeadline As Date
    Private _onsaleDate As Date
    Private _ProdnStatus As ProdnState
    Private _Visibility As VisibleState
    Private _adCount As Integer
    Private _sortKey As String
    Private _checked As Boolean
    Private _cssClass As String
    '
    ' embedded collections
    '
    Private _Publication As Publication
    Private _adInstances As AdInstances


    Public Enum ProdnState
        Unspecified = 0
        Open = 2
        Closed = 4
    End Enum

    Public Enum VisibleState
        Unspecified = 0
        Past = 1
        Active = 2
        Future = 3
    End Enum


    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _ID = ATSystem.SysConstants.nullValue
        _status = 0
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

    ''' <summary>
    ''' Defines a pdf hint to show pdf form on tweeted ads and featured ads.
    ''' </summary>
    Public Property IsVisibleInWizard() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsVisibleInWizard And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsVisibleInWizard
            Else
                _status = _status And Not ATSystem.StatusBits.IsVisibleInWizard
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property IsVisibleInWizardText() As String
        Get
            If IsVisibleInWizard Then
                Return "Yes"
            Else
                Return ""
            End If
        End Get
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

    ''' <summary>Object short Name. Used for display in the edition/product selector
    '''</summary>
    ''' <returns>Object Name</returns>
    Public Property ShortName() As String
        Get
            Return _shortname
        End Get
        Set(ByVal value As String)
            _shortname = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>SortKey</summary>
    ''' <returns>SortKey</returns>
    Public Property SortKey() As String
        Get
            Return _sortKey
        End Get
        Set(ByVal value As String)
            _sortKey = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Description</summary>
    ''' <returns>Object Name</returns>
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' memory-only property to display highlight on page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CSSClass() As String
        Get
            Return _cssClass
        End Get
        Set(ByVal value As String)
            _cssClass = value
        End Set
    End Property

    ''' <summary>
    ''' Ad deadline
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AdDeadline() As Date
        Get
            Return _adDeadline
        End Get
        Set(ByVal value As Date)
            _adDeadline = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Prodn deadline
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProdnDeadline() As Date
        Get
            Return _prodnDeadline
        End Get
        Set(ByVal value As Date)
            _prodnDeadline = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' On sale date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OnsaleDate() As Date
        Get
            Return _onsaleDate
        End Get
        Set(ByVal value As Date)
            _onsaleDate = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property AdCount() As Integer
        Get
            If _adCount = -1 Then
                Dim Ads As New Ads
                Ads.ConnectionString = _connectionString
                _adCount = Ads.GetAdCount(ATSystem.ObjectTypes.Edition, _ID)
            End If
            Return _adCount
        End Get
    End Property


    Public Property ProdnStatus() As ProdnState
        Get
            Return _ProdnStatus
        End Get
        Set(ByVal value As ProdnState)
            _ProdnStatus = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    Public Property Visibility() As VisibleState
        Get
            Return _Visibility
        End Get
        Set(ByVal value As VisibleState)
            _Visibility = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Embedded publication object, to which this Edition is subordinate.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property Publication() As Publication
        Get
            If _Publication Is Nothing Then
                Dim publications As New Publications
                publications.ConnectionString = _connectionString
                _Publication = publications.Retrieve(_PublicationID)

            End If
            Return _Publication
        End Get
    End Property

    ''' <summary>ID of the System, to which this object is directly subordinate</summary>
    ''' <value>Object ID as an integer</value>
    Public Property publicationID() As Integer
        Get
            Return _PublicationID
        End Get
        Set(ByVal value As Integer)
            _PublicationID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Embedded publication object, to which this Product is subordinate.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property AdInstances() As AdInstances
        Get
            If _adInstances Is Nothing Then
                _adInstances = New AdInstances
                _adInstances.ConnectionString = _connectionString
                _adInstances.RetrieveSet(ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, _ID, ATSystem.SysConstants.nullValue)
            End If
            Return _adInstances
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
    ''' Tests if the Edition has the ad as one of its instances
    ''' </summary>
    ''' <param name="AdID">EditionID</param>
    ''' <returns>True if ad is defined for Edition, false otherwise</returns>
    ''' <remarks></remarks>
    Public Function HasAd(ByVal AdID As Integer) As Boolean
        Dim count As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ADID", SqlDbType.Int).Value = AdID
        Cmd.Parameters.Add("@EditionID", SqlDbType.Int).Value = _ID

        Try
            Cmd.Connection.Open()
            Cmd.CommandText = "Select count(ID) FROM dbo.AdInstance WHERE AdID=@AdID and EditionID=@EditionID"
            count = CType(Cmd.ExecuteScalar(), Integer)
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try

        Return Convert.ToBoolean(count)
    End Function

    ''' <summary>
    ''' Memory-only value not written to db. Used to set bound check boxes
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value
        End Set
    End Property


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
        Cmd.CommandText = "UPDATE dbo.Edition SET " & _
        "modifyTime=getdate()," & _
        "PublicationID=@PublicationID," & _
        "Status=@Status," & _
        "ProdnStatus=@ProdnStatus," & _
        "Visibility=@Visibility," & _
        "Name=@Name," & _
        "Shortname=@Shortname," & _
        "SortKey=@SortKey," & _
        "Description=@Description," & _
        "AdDeadline=@AdDeadline," & _
        "ProdnDeadline=@ProdnDeadline," & _
        "OnsaleDate=@OnsaleDate" & _
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
        Cmd.CommandText = "INSERT INTO dbo.Edition " & _
        "(PublicationID,CreateTime,ModifyTime,Status,Name,Shortname,SortKey,Description,ProdnStatus,Visibility," & _
        "AdDeadline,ProdnDeadline,OnsaleDate) " & _
        "VALUES " & _
        "(@PublicationID,getdate(),getdate(),@Status,@Name,@Shortname,@SortKey,@Description,@ProdnStatus,@Visibility," & _
        "@AdDeadline,@ProdnDeadline,@OnsaleDate)" & _
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
        Cmd.Parameters.Add("@PublicationID", SqlDbType.Int).Value = publicationID
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = _ProdnStatus
        Cmd.Parameters.Add("@Visibility", SqlDbType.Int).Value = _Visibility
        Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = _name
        Cmd.Parameters.Add("@Shortname", SqlDbType.VarChar).Value = _shortname
        Cmd.Parameters.Add("@SortKey", SqlDbType.VarChar).Value = _sortKey
        Cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = _Description
        Cmd.Parameters.Add("@AdDeadline", SqlDbType.DateTime).Value = _adDeadline
        Cmd.Parameters.Add("@ProdnDeadline", SqlDbType.DateTime).Value = _prodnDeadline
        Cmd.Parameters.Add("@OnsaleDate", SqlDbType.DateTime).Value = _onsaleDate

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

            ''Cmd.CommandText = "DELETE dbo.Contact2Edition WHERE ParentID=@ID"
            ''Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.Edition WHERE ID=@ID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return ATSystem.SysConstants.nullValue

    End Function



    Friend Sub DR2Object(ByVal dr As IDataRecord)
        _ObjectStatus = ATSystem.ObjectState.Original
        _ID = Convert.ToInt32(dr("ID"))
        publicationID = Convert.ToInt32(dr("PublicationID"))
        _name = Convert.ToString(dr("Name"))
        _shortname = Convert.ToString(dr("Shortname"))
        _sortKey = Convert.ToString(dr("SortKey"))
        _Description = Convert.ToString(dr("Description"))
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _adDeadline = Convert.ToDateTime(dr("AdDeadline"))
        _prodnDeadline = Convert.ToDateTime(dr("ProdnDeadline"))
        _onsaleDate = Convert.ToDateTime(dr("OnsaleDate"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _ProdnStatus = CType(dr("ProdnStatus"), ProdnState)
        _Visibility = CType(dr("Visibility"), VisibleState)
    End Sub

End Class

