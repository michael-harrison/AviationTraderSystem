Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Configuration
Imports System


'***************************************************************************************
'*
'* Products
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The Products collection is used to hold the set of Product
''' objects. The collection is normally accessed by using the Products property of the parent System object.
'''  This class manages the Product database table. Any object in the collection can be modified and then written back to the database,
'''  by either the Update method of the collection class, or the Update method of each object within the collection.
''' </para>
''' </summary>
Public Class Products : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Products(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Products.count-1</param>
    ''' <value>Product object from Products collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Product
        Get
            Return CType(List(index), Product)
        End Get
        Set(ByVal value As Product)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a Product object to the Products collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Product object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As Product) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Product) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Product)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Product)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Product) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.Product ORDER BY SortKey"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Product
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
    Public Function Retrieve(ByVal ID As Integer) As Product
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Product WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function


    Public Sub retrieveSet(ByVal PublicationID As Integer)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "SELECT * from dbo.Product WHERE PublicationID=@ID ORDER BY SortKey"
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
    Public Function GetProductCount(ByVal ObjectType As ATSystem.ObjectTypes, ByVal ObjectID As Integer) As Integer
        Dim SQL As String = ""
        Select Case ObjectType
            Case ATSystem.ObjectTypes.System
                SQL = "Select Count(ID) from dbo.Product"

            Case ATSystem.ObjectTypes.Publication
                SQL = "Select Count(Product.ID) from dbo.Product " & _
                "WHERE Product.PublicationID=@ObjectID"

            Case Else
                SQL = "Select Count(ID) from dbo.Product"
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
                Dim Product As New Product
                Add(Product)
                Product.ConnectionString = _connectionString
                Product.DR2Object(dr)
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
        Dim deletedObjects As New List(Of Product)

        For Each Product As Product In List
            Product.Update()
            If Product.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Product)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Product In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class



'***************************************************************************************
'*
'* Product - Product object
'*
'*
'*
'*
'*
'**************************************************************************************

''' <summary>
''' <para> The Product object holds all data retrieved from a specified row of the Product table.
'''</para> It is always accessed as part of the Products collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Products collection
'''  property of the parent. 
''' </summary>
Public Class Product
    Private _connectionString As String
    Private _ID As Integer
    Private _PublicationID As Integer
    Private _instanceLoading As Integer
    Private _name As String
    Private _Description As String
    Private _sortKey As String
    Private _navTarget As String
    Private _ObjectStatus As ATSystem.ObjectState
    Private _status As ATSystem.StatusBits
    Private _createTime As Date
    Private _modifyTime As Date
    Private _Type As Types
    Private _adCount As Integer
    Private _checked As Boolean
    '
    ' embedded collections
    '
    Private _Publication As Publication
    Private _adInstances As AdInstances

    Public Enum Types
        Undefined = 0
        <Description("Classified Ad: Text only")> ClassadTextOnly = 1
        <Description("Classified Ad: Mono picture")> ClassadMonoPic = 2
        <Description("Classified Ad: Colour picture")> ClassadColorPic = 3
        <Description("Display Ad: Stand Alone")> DisplayStandAlone = 4
        <Description("Display Ad: Module")> DisplayModule = 5
        <Description("Display Ad: Composite")> DisplayComposite = 6
        <Description("Display Ad - Finished Art")> DisplayFinishedArt = 7
        <Description("Web Ad: Basic")> WebBasic = 8
        <Description("Web Ad: Premium")> WebPremium = 9
        <Description("Web Ad: PDF only")> WebPDF = 10
        <Description("Web Ad: PDF + Text")> WebPDFText = 11
        <Description("Web Ad: Featured Ad")> WebFeaturedAd = 12
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
    ''' <summary>Description</summary>
    ''' <returns>Product Type</returns>
    Public Property Type() As Types
        Get
            Return _Type
        End Get
        Set(ByVal value As Types)
            _Type = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property TypeDescr() As String
        Get
            Dim ea As New EnumAssistant(New Types, _Type, _Type)
            Return ea(0).Description
        End Get
    End Property

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
    ''' Embedded publication object, to which this Product is subordinate.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property Publication() As Publication
        Get
            If _Publication Is Nothing Then
                Dim publications As New Publications
                Publications.ConnectionString = _connectionString
                _Publication = publications.Retrieve(_PublicationID)

            End If
            Return _Publication
        End Get
    End Property

    ''' <summary>
    ''' Embedded publication object, to which this Product is subordinate.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property AdInstances() As AdInstances
        Get
            If _adInstances Is Nothing Then
                _adInstances = New AdInstances
                _AdInstances.ConnectionString = _connectionString
                _AdInstances.RetrieveSet(ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, _ID)
            End If
            Return _adInstances
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

    ''' <summary>Additional price for one instance of an ad in this product</summary>
    ''' <value>Object ID as an integer</value>
    Public Property InstanceLoading() As Integer
        Get
            Return _instanceLoading
        End Get
        Set(ByVal value As Integer)
            _instanceLoading = value
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

    Public ReadOnly Property AdCount() As Integer
        Get
            If _adCount = -1 Then
                Dim Ads As New Ads
                Ads.ConnectionString = _connectionString
                _adCount = Ads.GetAdCount(ATSystem.ObjectTypes.Product, _ID)
            End If
            Return _adCount
        End Get
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
    ''' Tests if the product has the ad as one of its instances
    ''' </summary>
    ''' <param name="AdID">ProductID</param>
    ''' <returns>True if ad is defined for product, false otherwise</returns>
    ''' <remarks></remarks>
    Public Function HasAd(ByVal AdID As Integer) As Boolean
        Dim count As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ADID", SqlDbType.Int).Value = AdID
        Cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = _ID

        Try
            Cmd.Connection.Open()
            Cmd.CommandText = "Select count(ID) FROM dbo.AdInstance WHERE AdID=@AdID and ProductID=@ProductID"
            count = CType(Cmd.ExecuteScalar(), Integer)
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try

        Return Convert.ToBoolean(count)
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
        Cmd.CommandText = "UPDATE dbo.Product SET " & _
        "modifyTime=getdate()," & _
        "PublicationID=@PublicationID," & _
        "Status=@Status," & _
        "Name=@Name," & _
        "Description=@Description," & _
        "InstanceLoading=@InstanceLoading," & _
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
        Cmd.CommandText = "INSERT INTO dbo.Product " & _
        "(PublicationID,CreateTime,ModifyTime,Status,Name,Description,Type,InstanceLoading,SortKey) " & _
        "VALUES " & _
        "(@PublicationID,getdate(),getdate(),@Status,@Name,@Description,@Type,@InstanceLoading,@SortKey)" & _
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
        Cmd.Parameters.Add("@PublicationID", SqlDbType.Int).Value = publicationiD
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@Type", SqlDbType.Int).Value = _Type
        Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = _name
        Cmd.Parameters.Add("@InstanceLoading", SqlDbType.Int).Value = _instanceLoading
        Cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = _Description
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

            ''Cmd.CommandText = "DELETE dbo.Contact2Product WHERE ParentID=@ID"
            ''Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.Product WHERE ID=@ID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return ATSystem.SysConstants.nullValue

    End Function



    Friend Sub DR2Object(ByVal dr As IDataRecord)
        _ObjectStatus = ATSystem.ObjectState.Original
        _ID = Convert.ToInt32(dr("ID"))
        _PublicationID = Convert.ToInt32(dr("PublicationID"))
        _name = Convert.ToString(dr("Name"))
        _Description = Convert.ToString(dr("Description"))
        _sortKey = Convert.ToString(dr("SortKey"))
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _instanceLoading = Convert.ToInt32(dr("InstanceLoading"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _Type = CType(dr("Type"), Types)
    End Sub

End Class

