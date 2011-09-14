
Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.ComponentModel
Imports System



'***************************************************************************************
'*
'* Ads
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The Ads collection contains a set of Ad objects.
''' </para>
''' </summary>
Public Class Ads : Inherits CollectionBase

    Private _connectionString As String
    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Ads(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Ads.count-1</param>
    ''' <value>Ad object from Ads collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Ad
        Get
            Return CType(List(index), Ad)
        End Get
        Set(ByVal value As Ad)
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
    Public Function Add(ByVal value As Ad) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Ad) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Ad)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Ad)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Ad) As Boolean
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
        Cmd.CommandText = commonSQL()
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Ad
        Return Retrieve(Hex2Int(HexID))
    End Function



    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="ID">ObjectID, represented as an integer</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal ID As Integer) As Ad
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = commonSQL() & " WHERE Ad.ID=@ID"

        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If

    End Function

    '''
    ''' <summary>Retrieves the ad with the supplied ad number</summary>
    '''
    Public Function RetrieveByNumber(ByVal AdNumber As String) As Ad

        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = commonSQL() & " WHERE Ad.AdNumber=@AdNumber"
        Cmd.Parameters.Add("@AdNumber", SqlDbType.VarChar).Value = AdNumber
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If

    End Function

    '''
    ''' <summary>Retrieves the ads with billing status=ready</summary>
    ''' 
    Public Sub RetrieveBillSet()
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = commonSQL() & " WHERE Ad.BillStatus=" & Ad.BillState.Ready
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Sets ad billing status to not ready where status=ready</summary>
    ''' 
    Public Function ClearBillSet() As Integer
        '
        ' Retrieves a specific record
        '
        Dim rowCount As Integer = 0
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Try
            Cmd.Connection.Open()
            Cmd.CommandText = "UPDATE dbo.Ad SET BillStatus=" & Ad.BillState.NotReady & " WHERE BillStatus=" & Ad.BillState.Ready & " ;SELECT @@rowCount;"
            rowCount = CType(Cmd.ExecuteScalar(), Integer)
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return rowCount
    End Function



    Public Sub RetrieveSet(ByVal CategoryID As Integer, ByVal ClassificationID As Integer, ByVal UsrID As Integer, ByVal FolderID As Integer, ByVal ProdnStatus As Ad.ProdnState, ByVal AdSortOrder As Ad.SortOrders)
        '
        ' general call to find ads depending on input params
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = CategoryID
        Cmd.Parameters.Add("@ClassificationID", SqlDbType.Int).Value = ClassificationID
        Cmd.Parameters.Add("@UsrID", SqlDbType.Int).Value = UsrID
        Cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = FolderID
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = ProdnStatus
        '
        ' define search tokens
        '
        Dim a As String = " AND "
        Dim w As String = " WHERE "
        Dim b1 As String = "CategoryID=@CategoryID"
        Dim b2 As String = "ClassificationID=@ClassificationID"
        Dim b4 As String = "UsrID=@UsrID"
        Dim b8 As String = "FolderID=@FolderID"
        Dim b16 As String = "ProdnStatus=@ProdnStatus"
        '
        ' define search bits
        '
        Dim selectflags As Integer = 0
        If CategoryID <> ATSystem.SysConstants.nullValue Then selectflags = 1
        If ClassificationID <> ATSystem.SysConstants.nullValue Then selectflags += 2
        If UsrID <> ATSystem.SysConstants.nullValue Then selectflags += 4
        If FolderID <> ATSystem.SysConstants.nullValue Then selectflags += 8
        If ProdnStatus <> Ad.ProdnState.Unspecified Then selectflags += 16
        '
        ' define search matrix
        '
        Dim selector As String = ""
        Select Case selectflags
            Case 0 : selector = ""
            Case 1 : selector = w & b1
            Case 2 : selector = w & b2
            Case 3 : selector = w & b2 & a & b1
            Case 4 : selector = w & b4
            Case 5 : selector = w & b4 & a & b1
            Case 6 : selector = w & b4 & a & b2
            Case 7 : selector = w & b4 & a & b2 & a & b1
            Case 8 : selector = w & b8
            Case 9 : selector = w & b8 & a & b1
            Case 10 : selector = w & b8 & a & b2
            Case 11 : selector = w & b8 & a & b2 & a & b1
            Case 12 : selector = w & b8 & a & b4
            Case 13 : selector = w & b8 & a & b4 & a & b1
            Case 14 : selector = w & b8 & a & b4 & a & b2
            Case 15 : selector = w & b8 & a & b4 & a & b2 & a & b1
            Case 16 : selector = w & b16
            Case 17 : selector = w & b16 & a & b1
            Case 18 : selector = w & b16 & a & b2
            Case 19 : selector = w & b16 & a & b2 & a & b1
            Case 20 : selector = w & b16 & a & b4
            Case 21 : selector = w & b16 & a & b4 & a & b1
            Case 22 : selector = w & b16 & a & b4 & a & b2
            Case 23 : selector = w & b16 & a & b4 & a & b2 & a & b1
            Case 24 : selector = w & b16 & a & b8
            Case 25 : selector = w & b16 & a & b8 & a & b1
            Case 26 : selector = w & b16 & a & b8 & a & b2
            Case 27 : selector = w & b16 & a & b8 & a & b2 & a & b1
            Case 28 : selector = w & b16 & a & b8 & a & b4
            Case 29 : selector = w & b16 & a & b8 & a & b4 & a & b1
            Case 30 : selector = w & b16 & a & b8 & a & b4 & a & b2
            Case 31 : selector = w & b16 & a & b8 & a & b4 & a & b2 & a & b1
        End Select
        '
        ' add in sort term
        '
        Dim sortOrder As String = ""
        Select Case AdSortOrder
            Case Ad.SortOrders.AdNumber : sortOrder = " ORDER BY Ad.AdNumber"
            Case Ad.SortOrders.Advertiser : sortOrder = " ORDER BY Ad.UsrID"
            Case Ad.SortOrders.Category : sortOrder = " ORDER BY CategoryName,ClassificationName"
            Case Ad.SortOrders.DateForward : sortOrder = " ORDER BY Ad.ID"
            Case Ad.SortOrders.DateReverse : sortOrder = " ORDER BY Ad.ID DESC"
            Case Ad.SortOrders.ItemPrice : sortOrder = " ORDER BY Ad.ItemPrice"
            Case Ad.SortOrders.Keywords : sortOrder = " ORDER BY Ad.SortKey"
        End Select
        '
        ' concatenate common sql with search and sort terms and execute
        '

        Cmd.CommandText = commonSQL() & selector & sortOrder
        doRetrieveR(Cmd)

    End Sub



    ''' <summary>
    ''' Returns the total number of ads according the the object type and ID passed
    ''' </summary>
    ''' <param name="ObjectType"></param>
    ''' <param name="ObjectID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAdCount(ByVal ObjectType As ATSystem.ObjectTypes, ByVal ObjectID As Integer) As Integer
        Dim SQL As String = ""
        Select Case ObjectType
            Case ATSystem.ObjectTypes.System
                SQL = "Select Count(ID) from dbo.Ad"

            Case ATSystem.ObjectTypes.Category
                SQL = "Select Count(Ad.ID) from dbo.ad " & _
                "INNER JOIN dbo.Classification on Ad.ClassificationID=Classification.ID " & _
                "WHERE Classification.CategoryID=@ObjectID"

            Case ATSystem.ObjectTypes.Classification
                SQL = "Select Count(Ad.ID) from dbo.ad " & _
               "WHERE Ad.ClassificationID=@ObjectID"

            Case ATSystem.ObjectTypes.Publication
                SQL = "Select Count(Ad.ID) from dbo.ad " & _
                 "INNER JOIN dbo.AdInstance on AdInstance.AdID=Ad.ID " & _
                 "INNER JOIN dbo.Product on AdInstance.ProductID=Product.ID " & _
                 "WHERE Product.PublicationID=@ObjectID"

            Case ATSystem.ObjectTypes.Product
                SQL = "Select Count(Ad.ID) from dbo.ad " & _
                 "INNER JOIN dbo.AdInstance on AdInstance.AdID=Ad.ID " & _
                 "WHERE AdInstance.ProductID=@ObjectID"

            Case ATSystem.ObjectTypes.Edition
                SQL = "Select Count(Ad.ID) from dbo.ad " & _
                 "INNER JOIN dbo.AdInstance on AdInstance.AdID=Ad.ID " & _
                 "WHERE AdInstance.EditionID=@ObjectID"

            Case ATSystem.ObjectTypes.Usr
                SQL = "Select Count(Ad.ID) from dbo.ad " & _
                "WHERE Ad.UsrID=@ObjectID"

            Case ATSystem.ObjectTypes.Folder
                SQL = "Select Count(Ad.ID) from dbo.ad " & _
                "WHERE Ad.FolderID=@ObjectID"

            Case Else
                SQL = "Select Count(ID) from dbo.Ad"
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



    Private Function commonSQL() As String
        Dim SQL As String = "Select ad.*, Classification.Name as ClassificationName,Category.Name as CategoryName FROM dbo.Ad " & _
        "INNER JOIN dbo.Classification on Classification.ID = Ad.ClassificationID " & _
        "INNER JOIN dbo.Category on Category.ID=Classification.CategoryID "
        Return SQL
    End Function

    ''' <summary>
    ''' Deletes all the ads associated with the object type
    ''' </summary>
    ''' <param name="ObjectType"></param>
    ''' <param name="ObjectID"></param>
    ''' <remarks></remarks>
    Friend Sub DeleteSet(ByVal ObjectType As ATSystem.ObjectTypes, ByVal ObjectID As Integer)
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ObjectID", SqlDbType.Int).Value = ObjectID

        Try
            Cmd.Connection.Open()
            Select Case ObjectType
                Case ATSystem.ObjectTypes.Usr

                    Cmd.CommandText = "DELETE dbo.AdInstance WHERE ID IN " & _
                    "(SELECT AdInstance.ID from dbo.AdInstance INNER JOIN dbo.Ad on AdInstance.AdID=Ad.ID " & _
                    "WHERE Ad.UsrID=@ObjectID)"
                    Cmd.ExecuteNonQuery()

                    Cmd.CommandText = "DELETE dbo.Image WHERE ID IN " & _
                    "(SELECT Image.ID from dbo.Image INNER JOIN dbo.Ad on Image.AdID=AdID " & _
                    "WHERE Ad.UsrID=@ObjectID)"
                    Cmd.ExecuteNonQuery()

                    Cmd.CommandText = "DELETE dbo.Spec WHERE ID IN " & _
                     "(SELECT Spec.ID from dbo.Spec INNER JOIN dbo.Ad on Spec.AdID=AdID " & _
                     "WHERE Ad.UsrID=@ObjectID)"

                    Cmd.CommandText = "DELETE dbo.AD WHERE Ad.UsrID=@ObjectID"
                    Cmd.ExecuteNonQuery()

            End Select

        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try

    End Sub

    ''' <summary>
    ''' Called by Timer Services. Deletes all ads which have a status of Initial - called at Midnight. 
    ''' We hope someone is not placing an ad at midnight.
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Purge() As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Dim rowCount As Integer = 0
        Try
            Cmd.Connection.Open()

            '
            ' delete ad instances
            '

            Cmd.CommandText = "DELETE dbo.AdInstance WHERE ID IN " & _
            "(SELECT AdInstance.ID from dbo.AdInstance " & _
            "INNER JOIN dbo.Ad on AdInstance.AdID=Ad.ID " & _
            "WHERE Ad.ProdnStatus=" & Ad.ProdnState.Initial & ")"
            Cmd.ExecuteNonQuery()
            '
            ' now delete images, and specs those ads that do not have any instances.
            '
            Cmd.CommandText = "DELETE dbo.Image WHERE ID IN " & _
             "(SELECT Image.ID from dbo.Image " & _
             "INNER JOIN dbo.Ad on Image.AdID=Ad.ID " & _
             "WHERE Ad.ProdnStatus=" & Ad.ProdnState.Initial & ")"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.Spec WHERE ID IN " & _
                      "(SELECT Spec.ID from dbo.Spec " & _
                      "INNER JOIN dbo.Ad on Spec.AdID=Ad.ID " & _
                      "WHERE Ad.ProdnStatus=" & Ad.ProdnState.Initial & ")"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.AD WHERE Ad.ProdnStatus=" & Ad.ProdnState.Initial & " ;SELECT @@rowCount;"

            rowCount = CType(Cmd.ExecuteScalar(), Integer)

        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return rowCount

    End Function


    ''' <summary>
    ''' Called by Timer Services. Sets the ad status to archive if all the instances are for past
    ''' editions - ie edn.visiblestate=past. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Archive() As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Dim rowCount As Integer = 0
        Try
            Cmd.Connection.Open()

            '
            ' update status only if all instances for the ad are in past editions
            '

            Cmd.CommandText = "Update dbo.Ad SET ProdnStatus=" & Ad.ProdnState.Archived & _
            " WHERE Ad.ID IN " & _
            "(select Ad.id   from dbo.ad " & _
            "inner join adinstance on adinstance.adid=ad.id " & _
            "inner join edition on edition.id=adinstance.editionid " & _
            " where edition.visibility=" & Edition.VisibleState.Past & ") " & _
            "AND Ad.ID NOT IN " & _
            "(select Ad.id   from dbo.ad " & _
            "inner join adinstance on adinstance.adid=ad.id " & _
            "inner join edition on edition.id=adinstance.editionid " & _
            " where edition.visibility=" & Edition.VisibleState.Active & ") " & _
            "AND Ad.ID NOT IN " & _
            "(select ad.id   from dbo.ad " & _
            "inner join adinstance on adinstance.adid=ad.id " & _
            "inner join edition on edition.id=adinstance.editionid " & _
            " where edition.visibility=" & Edition.VisibleState.Future & ") " & _
            " AND ProdnStatus <>" & Ad.ProdnState.Archived & _
            " ;SELECT @@rowCount;"

            rowCount = CType(Cmd.ExecuteScalar(), Integer)
  
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return rowCount

    End Function

    ''' <summary>
    ''' Called by timer services. resets the lastest listing flag on all ads
    ''' </summary>
    ''' <remarks></remarks>
    Public Function KillLatestListings() As Integer
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Dim rowCount As Integer = 0
        Try
            Cmd.Connection.Open()
            Cmd.CommandText = "Update dbo.Ad SET LatestListing=0 WHERE LatestListing=1" & _
            " ;SELECT @@rowCount;"
            rowCount = CType(Cmd.ExecuteScalar(), Integer)

        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return rowCount
    End Function




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
        Dim deletedObjects As New List(Of Ad)

        For Each Ad As Ad In List
            Ad.Update()
            If Ad.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Ad)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Ad In deletedObjects
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
                Dim Ad As New Ad
                Add(Ad)
                Ad.ConnectionString = _connectionString
                Ad.DR2Object(dr)
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
'* Ad
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para> The Ad object holds all data retrieved from a specified row of the Ad table.
'''</para>
''' <para>It is always accessed as part of the Ads collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Ads collection
'''  property of the parent. 
''' </para>
''' </summary>
Public Class Ad


    Private _connectionString As String
    Private _ID As Integer
    Private _UsrID As Integer
    Private _FolderID As Integer
    Private _clickCount As Integer
    Private _createTime As Date
    Private _modifyTime As Date
    Private _physicalApplicationPath As String
    Private _bindingparam1 As String
    Private _navTarget As String
    Private _navTarget2 As String
    Private _status As ATSystem.StatusBits
    Private _ObjectStatus As ATSystem.ObjectState
    '
    ' administrative data
    '
    Private _AdNumber As String
    Private _IslatestListing As Boolean

    Private _prodnstatus As ProdnState
    Private _billstatus As BillState
    Private _ProdnRequest As String
    Private _ProdnResponse As String
    Private _classificationID As Integer

    Private _schedule As Integer

    '
    ' publishable data
    '
    Private _KeyWords As String
    Private _youtubeVideoTag As String
    Private _sortKey As String
    Private _Itemprice As String
    Private _OriginalText As String
    Private _Text As String
    Private _Summary As String
    '
    ' embedded collections
    '
    Private _Usr As Usr
    Private _Folder As Folder
    Private _Classification As Classification
    Private _Images As Images
    Private _WebImages As Images
    Private _mainImage As Image
    Private _specs As Specs         'all specs
    Private _activeSpecs As Specs   'active specs
    Private _specgroupID As Integer
    Private _activeSpecgroupID As Integer
    '
    ' ad instance collections
    '
    Private _instances As AdInstances               'all instances
    Private _publicationInstances As AdInstances    'instances in specified publication
    Private _productInstances As AdInstances        'instances in specified product   Private _productInstances As AdInstances        'instances in specified product
    Private _editionInstances As AdInstances        'instances in specifed edition
    Private _productEditionInstances As AdInstances 'instances in specified product and edtion

    Private _instancePublicationID As Integer
    Private _instanceEditionID As Integer
    Private _instanceProductID As Integer
    Private _instanceProductEditionID As Integer
    '
    ' following values used for the paging algorithms
    '
    Private _prevID As Integer
    Private _nextID As Integer
    Private _curPage As Integer
    Private _prevPage As Integer
    Private _nextPage As Integer
    '
    ' fields from joined tables
    '
    Private _categoryName As String
    Private _classificationName As String

    ''' <summary>
    ''' Defines how the order is being paid for.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum PaymentTypes
        <Description("Select Payment Method")> Unspecified = 0
        <Description("American Express")> AMEX = 1
        <Description("Discover")> Discover = 2
        <Description("MasterCard")> MasterCard = 3
        <Description("VISA")> VISA = 4
        <Description("Terms")> Terms = 5
    End Enum

    ''' <summary>
    ''' Defines how ads are sorted (in proof editor)
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum SortOrders
        <Description("Ad Number")> AdNumber = 0
        <Description("Date - Reverse")> DateReverse = 1
        <Description("Date - Forward")> DateForward = 2
        <Description("Advertiser")> Advertiser = 3
        <Description("Category")> Category = 4
        <Description("Keywords")> Keywords = 5
        <Description("ItemPrice")> ItemPrice = 6
    End Enum

    ''' <summary>
    ''' Defines the production status that an Ad can be in
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum ProdnState
        Unspecified = 0
        Initial = 1
        Cancelled = 2
        Saved = 3
        Submitted = 4
        Proofed = 5
        Approved = 6
        Archived = 7
    End Enum


    ''' <summary>
    ''' Defines the billing state that an Ad can be in.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum BillState
        DoNotBill = 0           'exclude from billing processing.
        NotReady = 1            'not yet ready for billing agent to process the Ad
        Ready = 2               'setting to ready will cause Billing Agent to process the Ad
        Complete = 3            'when billing agent finishes processing, if successful, it will be set to complete
        Err = 4                 'set by billing agent if processing error is encountered
    End Enum

    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
    Public Sub New()
        _ID = ATSystem.SysConstants.nullValue
        _status = 0
        _prodnstatus = ProdnState.Initial
        _billstatus = BillState.NotReady
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


    Public Property CurPage() As Integer
        Get
            Return _curPage
        End Get
        Set(ByVal value As Integer)
            _curPage = value
        End Set
    End Property

    Public Property NextPage() As Integer
        Get
            Return _nextPage
        End Get
        Set(ByVal value As Integer)
            _nextPage = value
        End Set
    End Property

    Public Property PrevPage() As Integer
        Get
            Return _prevPage
        End Get
        Set(ByVal value As Integer)
            _prevPage = value
        End Set
    End Property

    Public Property NextID() As Integer
        Get
            Return _nextID
        End Get
        Set(ByVal value As Integer)
            _nextID = value
        End Set
    End Property

    Public Property PrevID() As Integer
        Get
            Return _prevID
        End Get
        Set(ByVal value As Integer)
            _prevID = value
        End Set
    End Property

    ''' <summary>
    ''' Ad number in the form YYYY.MM.NNNN.
    ''' </summary>
    ''' <value>Ad Adnumber (ID value)</value>
    Public Property Adnumber() As String
        Get
            Return _AdNumber
        End Get
        Set(ByVal value As String)
            _AdNumber = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set

    End Property

    Public Property IsLatestListing() As Boolean
        Get
            Return _IslatestListing
        End Get
        Set(ByVal value As Boolean)
            _IslatestListing = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Defines a pdf hint to show pdf form on tweeted ads and featured ads.
    ''' </summary>
    Public Property IsPDFHint() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsPDFHint And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsPDFHint
            Else
                _status = _status And Not ATSystem.StatusBits.IsPDFHint
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property



    ''' <summary>
    ''' Returns the ID of the user who placed the Ad
    ''' </summary>
    Public Property UsrID() As Integer
        Get
            Return _UsrID
        End Get
        Set(ByVal value As Integer)
            If value <> _UsrID Then
                _UsrID = value
                _Usr = Nothing
                _ObjectStatus = ATSystem.ObjectState.Modified
            End If
        End Set
    End Property

    ''' <summary>
    ''' Returns the usr object for the user who placed the Ad
    ''' </summary>
    Public ReadOnly Property Usr() As Usr
        Get
            If _Usr Is Nothing Then
                Dim Usrs As New Usrs
                Usrs.ConnectionString = _connectionString
                _Usr = Usrs.Retrieve(_UsrID)
            End If
            Return _Usr
        End Get
    End Property


    ''' <summary>
    ''' Returns the number of times that the ad has been clicked
    ''' </summary>
    Public Property ClickCount() As Integer
        Get
            Return _clickCount
        End Get
        Set(ByVal value As Integer)
            _clickCount = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Returns the ID of the folder that the ad is in
    ''' </summary>
    Public Property FolderID() As Integer
        Get
            Return _FolderID
        End Get
        Set(ByVal value As Integer)
            If value <> _FolderID Then
                _FolderID = value
                _Folder = Nothing
                _ObjectStatus = ATSystem.ObjectState.Modified
            End If
        End Set
    End Property

    ''' <summary>
    ''' Returns the folder object that the ad is in
    ''' </summary>
    Public ReadOnly Property Folder() As Folder
        Get
            If _Folder Is Nothing Then
                Dim Folders As New Folders
                Folders.ConnectionString = _connectionString
                _Folder = Folders.Retrieve(_FolderID)
            End If
            Return _Folder
        End Get
    End Property

    ''' <summary>
    ''' Returns the ID of the classification that the ad is in
    ''' </summary>
    Public Property ClassificationID() As Integer
        Get
            Return _classificationID
        End Get
        Set(ByVal value As Integer)
            If value <> _classificationID Then
                _classificationID = value
                _Classification = Nothing
                _ObjectStatus = ATSystem.ObjectState.Modified
            End If
        End Set
    End Property

    ''' <summary>
    ''' Returns the Classfication object that the ad is in
    ''' </summary>
    Public ReadOnly Property Classification() As Classification
        Get
            If _Classification Is Nothing Then
                Dim Classifications As New Classifications
                Classifications.ConnectionString = _connectionString
                _Classification = Classifications.Retrieve(_classificationID)
            End If
            Return _Classification
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
    ''' Specifies the Production status of the Ad
    ''' </summary>
    Public Property ProdnStatus() As ProdnState
        Get
            Return _prodnstatus
        End Get
        Set(ByVal value As ProdnState)
            _prodnstatus = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property BillMe() As Boolean
        Get
            Select Case _billstatus
                Case BillState.Complete, BillState.Err, BillState.Ready : Return True
                Case Else : Return False
            End Select
        End Get
        Set(ByVal value As Boolean)
            _billstatus = BillState.NotReady
            If value Then _billstatus = BillState.Ready
            _ObjectStatus = ATSystem.ObjectState.Modified

        End Set
    End Property

    ''' <summary>
    ''' Specifies the Billing status of the Ad
    ''' </summary>
    Public Property BillStatus() As BillState
        Get
            Return _billstatus
        End Get
        Set(ByVal value As BillState)
            _billstatus = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property



    ''' <summary>
    ''' Returns the production status, as a human readable string.
    ''' </summary>
    Public ReadOnly Property ProdnStatusDescr() As String
        Get
            Dim EA As New EnumAssistant(New Ad.ProdnState)
            Return EA(_prodnstatus).Description
        End Get
    End Property

    Public ReadOnly Property CategoryName() As String
        Get
            Return _categoryName
        End Get
    End Property

    Public ReadOnly Property ClassificationName() As String
        Get
            Return _classificationName
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

    ''' <summary>This is a memory-only property which is not written to the database.
    ''' Its plugged by the caller afte the object is retrieved to prime it with the
    ''' any string suitable for binding to controls. This is done before
    ''' databinding, so when the object is databound to a control, the click URL is
    ''' exposed to the control as a bindable property of the object.</summary>
    ''' <returns>String value, as primed by caller</returns>
    Public Property BindingParam1() As String
        Get
            Return _bindingparam1
        End Get
        Set(ByVal value As String)
            _bindingparam1 = value
        End Set
    End Property


    ''' <summary>
    ''' First few words of ad - for bold display in classifieds
    ''' </summary>
    Public Property KeyWords() As String
        Get
            Return _KeyWords
        End Get
        Set(ByVal value As String)
            _KeyWords = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Summary of ad - appears in listings
    ''' </summary>
    Public Property Summary() As String
        Get
            Return _Summary
        End Get
        Set(ByVal value As String)
            _Summary = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' sort key - normally derived from first word by GenerateSortKey but can be plugged manually
    ''' </summary>
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
    ''' Text of ad
    ''' </summary>
    Public Property Text() As String
        Get
            Return _Text
        End Get
        Set(ByVal value As String)
            _Text = CommonRoutines.TruncateText(value, ATSystem.SysConstants.textCharLength)
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Fully qualified path to the web application. This field
    ''' is normally plugged by a call to Server.MapPath(Request.ApplicationPath), and is used as a property in databinding
    ''' </summary>
    Public Property PhysicalApplicationPath() As String
        Get
            Return _physicalApplicationPath
        End Get
        Set(ByVal value As String)
            _physicalApplicationPath = value
        End Set
    End Property


    ''' <summary>
    ''' Searches for first or second separator character and if found returns the second and or third parts of the string
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TrimmedText() As String
        Get
            Dim barray() As Byte = String2ByteArray(_Text)



            Dim sep() As Char = {Constants.TextSeparator}
            Dim arr() As String = _Text.Split(sep, 3)

            Select Case arr.Length
                Case 1 : Return _Text
                Case 2 : Return arr(1)
                Case 3 : Return arr(1) & arr(2)
                Case Else : Return arr(3)
            End Select

        End Get
    End Property

    ''' <summary>
    ''' Text of ad
    ''' </summary>
    Public Property OriginalText() As String
        Get
            Return _OriginalText
        End Get
        Set(ByVal value As String)
            _OriginalText = CommonRoutines.TruncateText(value, ATSystem.SysConstants.textCharLength)
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Price of the item being advertised
    ''' </summary>
    Public Property ItemPrice() As String
        Get
            Return _Itemprice
        End Get
        Set(ByVal value As String)
            _Itemprice = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' You tube video tage
    ''' </summary>
    Public Property YoutubeVideoTag() As String
        Get
            Return _youtubeVideoTag
        End Get
        Set(ByVal value As String)
            _youtubeVideoTag = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Prodn Note - special production instructions
    ''' </summary>
    Public Property ProdnRequest() As String
        Get
            Return _ProdnRequest
        End Get
        Set(ByVal value As String)
            _ProdnRequest = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property ProdnRequestHTML() As String
        Get
            Dim lf As Char = Chr(&HA)
            Dim s As String = _ProdnRequest.Replace(lf, "<br />")
            Return s
        End Get
    End Property

    ''' <summary>
    ''' Prodn feedback - special production feedback
    '''     ''' </summary>
    Public Property ProdnResponse() As String
        Get
            Return _ProdnResponse
        End Get
        Set(ByVal value As String)
            _ProdnResponse = value
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

    ''' <summary>
    ''' Returns the thb url if a main image exists, otherwise the default url
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property THBURL() As String
        Get
            If MainImage Is Nothing Then
                Return GetApplicationPath() & Constants.ImageNotFound
            Else
                Return MainImage.THBURL
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the lores url if a main image exists, otherwise the default url
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LoResURL() As String
        Get
            If MainImage Is Nothing Then
                Return GetApplicationPath() & Constants.ImageNotFound
            Else
                Return MainImage.LoResURL
            End If
        End Get
    End Property


    ''' <summary>
    ''' Embedded collection of Image objects, which are assoicated with this Ad, and which are in the
    '''  supplied production status range.   
    '''  The collection is filtered on Imageument Adnumber by the value of the filter parameter.
    '''The collection is sorted by the supplied property in the supplied Ad 
    ''' </summary>
    Public ReadOnly Property Images() As Images
        '
        ' if there is no Images collection, or if there is, but the prodnstatus has changed,
        ' then get a new collection
        '
        Get
            If (_Images Is Nothing) Then
                _Images = New Images()                      'get a new collection
                _Images.ConnectionString = _connectionString
                _Images.RetrieveSet(_ID)
                '
                ' plant myself as the Ad parent in each returned Images
                ' and set the source path from the system
                '
                Dim mysys As New ATSystem
                mysys.Retrieve()

                For Each Image As Image In _Images
                    Image.WorkingSourcePath = mysys.SourceImageWorkingFolder
                    Image._Ad = Me
                Next

            End If
            Return _Images
        End Get
    End Property

    ''' <summary>
    ''' Embedded collection of Image objects, which are assoicated with this Ad, and which are in the
    '''  supplied production status range.   
    '''  The collection is filtered on Imageument Adnumber by the value of the filter parameter.
    '''The collection is sorted by the supplied property in the supplied Ad 
    ''' </summary>
    Public ReadOnly Property WebImages() As Images
        '
        ' if there is no WebImages collection, or if there is, but the prodnstatus has changed,
        ' then get a new collection
        '
        Get
            If (_webImages Is Nothing) Then
                _webImages = New Images()                      'get a new collection
                _webImages.ConnectionString = _connectionString
                _webImages.RetrievewebSet(_ID)
                '
                ' plant myself as the Ad parent in each returned Images
                ' and set the source path from the system
                '
                Dim mysys As New ATSystem
                mysys.Retrieve()

                For Each Image As Image In _webImages
                    Image.WorkingSourcePath = mysys.SourceImageWorkingFolder
                    Image._Ad = Me
                Next

            End If
            Return _webImages
        End Get
    End Property

    ''' <summary>
    ''' Embedded collection of Ad Instance objects, which are assoicated with this Ad
    ''' </summary>
    Public ReadOnly Property Instances() As AdInstances
        '
        ' if there is no Instances collection then get a new collection
        '
        Get
            If (_instances Is Nothing) Then
                _instances = New AdInstances()                      'get a new collection
                _instances.ConnectionString = _connectionString
                _instances.RetrieveSet(ATSystem.SysConstants.nullValue, _ID, ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue)

            End If
            Return _instances
        End Get
    End Property

    ''' <summary>
    ''' Embedded collection of Ad Instance objects, which are assoicated with this Ad
    ''' </summary>
    Public ReadOnly Property PublicationInstances(ByVal publicationID As Integer) As AdInstances
        '
        ' if there is no Instances collection then get a new collection
        '
        Get
            If (_publicationInstances Is Nothing) Or (_instancePublicationID <> publicationID) Then
                _publicationInstances = New AdInstances()                      'get a new collection
                _instancePublicationID = publicationID
                _publicationInstances.ConnectionString = _connectionString
                _publicationInstances.RetrieveSet(ATSystem.SysConstants.nullValue, _ID, publicationID, ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue)

            End If
            Return _publicationInstances
        End Get
    End Property

    ''' <summary>
    ''' Embedded collection of Ad Instance objects, which are assoicated with this Ad
    ''' </summary>
    Public ReadOnly Property EditionInstances(ByVal EditionID As Integer) As AdInstances
        '
        ' if there is no Instances collection then get a new collection
        '
        Get
            If (_editionInstances Is Nothing) Or (_instanceEditionID <> EditionID) Then
                _editionInstances = New AdInstances()                      'get a new collection
                _instanceEditionID = EditionID
                _editionInstances.ConnectionString = _connectionString
                _editionInstances.RetrieveSet(ATSystem.SysConstants.nullValue, _ID, ATSystem.SysConstants.nullValue, EditionID, ATSystem.SysConstants.nullValue)

            End If
            Return _editionInstances
        End Get
    End Property

    ''' <summary>
    ''' Embedded collection of Ad Instance objects, which are assoicated with this Ad
    ''' </summary>
    Public ReadOnly Property ProductInstances(ByVal ProductID As Integer) As AdInstances
        '
        ' if there is no Instances collection then get a new collection
        '
        Get
            If (_productInstances Is Nothing) Or (_instanceProductID <> ProductID) Then
                _productInstances = New AdInstances()                      'get a new collection
                _productInstances.ConnectionString = _connectionString
                _instanceProductID = ProductID
                _productInstances.RetrieveSet(ATSystem.SysConstants.nullValue, _ID, ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, ProductID)

            End If
            Return _editionInstances
        End Get
    End Property

    ''' <summary>
    ''' Embedded collection of Ad Instance objects, which are assoicated with this Ad
    ''' </summary>
    Public ReadOnly Property ProductEditionInstances(ByVal ProductID As Integer, ByVal EditionID As Integer) As AdInstances
        '
        ' if there is no Instances collection then get a new collection
        '
        Get
            If (_productEditionInstances Is Nothing) Or _
            (_instanceEditionID <> EditionID) Or _
            (_instanceProductID <> ProductID) Then
                _productEditionInstances = New AdInstances()                      'get a new collection
                _productEditionInstances.ConnectionString = _connectionString
                _instanceProductID = ProductID
                _instanceEditionID = EditionID
                _productEditionInstances.RetrieveSet(ATSystem.SysConstants.nullValue, _ID, ATSystem.SysConstants.nullValue, EditionID, ProductID)

            End If
            Return _productEditionInstances
        End Get
    End Property



    ''' <summary>
    ''' Main image object in the supplied Ad 
    ''' </summary>
    Public ReadOnly Property MainImage() As Image
        '
        ' if there is no Images collection, or if there is, but the prodnstatus has changed,
        ' then get a new collection
        '
        Get
            If (_mainImage Is Nothing) Then
                Dim myImages As New Images                      'get a new collection
                myImages.ConnectionString = _connectionString
                myImages.RetrieveMainImage(_ID)


                If myImages.Count > 0 Then
                    '
                    ' plant myself as the Ad parent in each returned Images
                    ' and set the source path from the system
                    '
                    Dim mysys As New ATSystem
                    mysys.Retrieve()
                    _mainImage = myImages(0)
                    _mainImage.WorkingSourcePath = mysys.SourceImageWorkingFolder
                    _mainImage._Ad = Me
                    ' plant myself as the Ad parent
                End If
            End If

            Return _mainImage
        End Get
    End Property

    ''' <summary>
    ''' returns all the specs for the ad
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Specs() As Specs
        Get
            Return Specs(ATSystem.SysConstants.nullValue)
        End Get
    End Property


    ''' <summary>
    ''' retrieves the set of specs for the ad which are in a particular group, if supplied,
    ''' otherwise retrieves all the specs for the ad
    ''' </summary>
    ''' <param name="SpecGroupID">groupid</param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Specs(ByVal SpecGroupID As Integer) As Specs
        '
        ' if there is no specs collection, or if there is, but the prodnstatus has changed,
        ' then get a new collection
        '
        Get
            If (_specs Is Nothing) Or (_specgroupID <> SpecGroupID) Then
                _specgroupID = SpecGroupID
                _specs = New Specs()                      'get a new collection
                _specs.ConnectionString = _connectionString
                _specs.retrieveSet(_ID, SpecGroupID, 0)
                '
                ' plant myself as the Ad parent in each returned Spec
                '
                For Each Spec As Spec In _specs
                    Spec._Ad = Me
                Next

            End If
            Return _specs
        End Get
    End Property


    ''' <summary>
    ''' returns all the specs for the ad
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ActiveSpecs() As Specs
        Get
            Return ActiveSpecs(ATSystem.SysConstants.nullValue)
        End Get
    End Property
    ''' <summary>
    ''' Embedded collection of spec objects, which are assoicated with this Ad
    ''' </summary>
    Public ReadOnly Property ActiveSpecs(ByVal SpecGroupID As Integer) As Specs
        '
        ' if there is no specs collection, or if there is, but the prodnstatus has changed,
        ' then get a new collection
        '
        Get
            If (_activeSpecs Is Nothing) Or (_activeSpecgroupID <> SpecGroupID) Then
                _activeSpecgroupID = SpecGroupID
                _activeSpecs = New Specs()                      'get a new collection
                _activeSpecs.ConnectionString = _connectionString
                _activeSpecs.retrieveSet(_ID, SpecGroupID, ATSystem.StatusBits.IsSpecActive)
                '
                ' plant myself as the Ad parent in each returned Spec
                '
                For Each Spec As Spec In _activeSpecs
                    Spec._Ad = Me
                Next

            End If
            Return _activeSpecs
        End Get
    End Property

    ''' <summary>
    ''' Checks the ad's spec list and adds in any new specs from the spec group
    ''' which are not already attached to the ad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AddSpecs()
        Dim SpecDefinitions As New SpecDefinitions
        SpecDefinitions.retrieveClassificationSet(_classificationID)
        For Each specdefn As SpecDefinition In SpecDefinitions
            '
            ' does this ad already have the spec defn?
            '
            Dim found As Boolean = False
            For Each currentSpec As Spec In Specs
                If currentSpec.SpecDefinitionID = specdefn.ID Then
                    found = True
                    Exit For
                End If
            Next

            If Not found Then
                Dim spec As New Spec
                spec.ConnectionString = _connectionString
                spec.AdID = _ID
                spec.SpecDefinitionID = specdefn.ID
                spec.Value = ""
                spec.Update()
            End If
        Next

    End Sub

    ''' <summary>
    ''' Deletes the set of specs for the ad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DeleteSpecs()
        Dim specs As New Specs
        specs.DeleteSet(_ID)
        _specs = Nothing          'show nothing left
    End Sub

    ''' <summary>
    ''' Clears the image list to forece a reload on the next call
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearImages()
        _Images = Nothing
    End Sub




    ''' <summary>
    ''' Generates the sort key from the first words of the ad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GenerateSortKey()
        _sortKey = _KeyWords.Replace(" ", "").ToUpper
    End Sub

    ''' <summary>
    ''' Deletes the ad instance for the supplied product id
    ''' </summary>
    ''' <param name="ProductID">ProductID</param>
    ''' <remarks></remarks>
    Public Sub RemoveInstances(ByVal ProductID As Integer, ByVal editionID As Integer)
        Dim adInstances As New AdInstances
        adInstances.DeleteSet(_ID, ProductID, editionID)
    End Sub


    ''' <summary>
    ''' Clears the instance list to cause a db re-read next call
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearInstances()
        _instances = Nothing
    End Sub


    ''' <summary>
    ''' Adds an ad instance to the ad for the specified product and returns it
    ''' </summary>
    ''' <param name="ProductID">ProductID</param>
    ''' <remarks></remarks>
    Public Function AddInstance(ByVal ProductID As Integer, ByVal EditionID As Integer) As AdInstance
        Dim adInstance As New AdInstance
        adInstance.ConnectionString = _connectionString
        adInstance.AdID = _ID
        adInstance.ProductID = ProductID
        adInstance.EditionID = EditionID
        adInstance.INDDFilename = ""
        adInstance.Price = 0
        adInstance.PriceAdjust = 0
        adInstance.ProdnSurcharge = 0
        adInstance.Width = RateTable.DisplayWidths.Col2
        adInstance.Color = RateTable.DisplayColorTypes.Mono
        adInstance.ExactHeight = 0
        adInstance.ExactWidth = 0
        adInstance.Height = 2
        adInstance.Update()
        '
        ' re-read the instance to get the product type
        '
        Dim adInstances As New AdInstances
        adInstances.ConnectionString = _connectionString
        adInstance = adInstances.Retrieve(adInstance.ID)
        Return adInstance
    End Function


    ''' <summary>
    ''' Returns the instance if it exists, otherise returns nothing. This function can therefore be used
    ''' to check if an instance exists
    ''' </summary>
    ''' <param name="ProductId"></param>
    ''' <param name="EditionID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInstance(ByVal ProductID As Integer, ByVal EditionID As Integer) As AdInstance
        Dim adInstances As New AdInstances
        adInstances.ConnectionString = _connectionString
        adInstances.RetrieveSet(ATSystem.SysConstants.nullValue, _ID, ATSystem.SysConstants.nullValue, EditionID, ProductId)
        If adInstances.Count > 0 Then
            Return adInstances(0)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Increments the ad click count
    ''' </summary>
    ''' <remarks></remarks>
    Public Function IncrClickCount() As Integer
        _clickCount += 1         'increment obj value

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ClickCount", SqlDbType.Int).Value = _clickCount
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID

        Try
            Cmd.Connection.Open()
            Cmd.CommandText = "Update dbo.Ad SET ClickCount=@ClickCount WHERE ID=@ID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return _clickCount          'return new value

    End Function


    ''' <summary>
    ''' Deletes the specified ad instance
    ''' </summary>
    ''' <param name="ProductID"></param>
    ''' <param name="EditionID"></param>
    ''' <remarks></remarks>
    Public Sub DeleteInstance(ByVal ProductID As Integer, ByVal EditionID As Integer)
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@AdID", SqlDbType.Int).Value = _ID
        Cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID
        Cmd.Parameters.Add("@EditionID", SqlDbType.Int).Value = EditionID

        Try
            Cmd.Connection.Open()
            Cmd.CommandText = "Delete dbo.AdInstance WHERE AdID=@AdID AND EditionID=@EditionID AND ProductID=@ProductID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try

    End Sub


    ''' <summary>
    ''' Sets the instance preview flag to invalid for all instances attached to this ad. Called as a result
    ''' of a text or image change.    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InvalidateInstancePreviews()

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ADID", SqlDbType.Int).Value = _ID
        Dim notPreviewValid As Integer = Not ATSystem.StatusBits.IsPreviewValid

        Try
            Cmd.Connection.Open()
            Cmd.CommandText = "Update dbo.AdInstance set Status=Status & " & notPreviewValid & " WHERE AdID=@AdID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try

    End Sub

    ''' <summary>
    ''' Tests if the ad is defined for the supplied product
    ''' </summary>
    ''' <param name="productID">ProductID</param>
    ''' <returns>True if ad is defined for product, false otherwise</returns>
    ''' <remarks></remarks>
    Public Function IsInProduct(ByVal productID As Integer) As Boolean
        Dim count As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ADID", SqlDbType.Int).Value = _ID
        Cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID

        Try
            Cmd.Connection.Open()
            Cmd.CommandText = "Select count(ID) FROM dbo.AdInstance WHERE AdID=@AdID and ProductID=@ProductID"
            count = CType(Cmd.ExecuteScalar(), Integer)
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try

        Return Convert.ToBoolean(count)
    End Function

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
        Cmd.CommandText = "UPDATE dbo.Ad SET " & _
        "modifyTime=getdate()," & _
        "Adnumber=@Adnumber," & _
        "LatestListing=@LatestListing," & _
        "KeyWords=@KeyWords," & _
        "SortKey=@SortKey," & _
        "Summary=@Summary," & _
        "ItemPrice=@ItemPrice," & _
        "YoutubeVideoTag=@YoutubeVideoTag," & _
        "OriginalText=@OriginalText," & _
        "Text=@Text," & _
        "ProdnRequest=@ProdnRequest," & _
        "ProdnResponse=@ProdnResponse," & _
        "Status=@Status," & _
        "ProdnStatus=@ProdnStatus," & _
        "BillStatus=@BillStatus," & _
        "ClassificationID=@ClassificationID," & _
        "FolderID=@FolderID," & _
        "ClickCount=@ClickCount," & _
        "UsrID=@UsrID" & _
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
        Cmd.CommandText = "INSERT INTO dbo.Ad " & _
        "(Adnumber,LatestListing,FolderID,UsrID,ClassificationID,createTime,modifyTime," & _
        "KeyWords,Summary,SortKey,ClickCount,ItemPrice,YoutubeVideoTag,OriginalText,Text," & _
        "ProdnRequest,ProdnResponse,ProdnStatus,BillStatus,Status) " & _
        "VALUES (@Adnumber,@LatestListing,@FolderID,@UsrID,@ClassificationID,getdate(),getdate()," & _
        "@KeyWords,@Summary,@SortKey,@ClickCount,@ItemPrice,@YoutubeVideoTag,@OriginalText,@Text," & _
        "@ProdnRequest,@ProdnResponse,@ProdnStatus,@BillStatus,@Status)" & _
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
        Cmd.Parameters.Add("@Adnumber", SqlDbType.VarChar).Value = _AdNumber
        Cmd.Parameters.Add("@LatestListing", SqlDbType.Int).Value = Convert.ToInt32(_IslatestListing)
        Cmd.Parameters.Add("@KeyWords", SqlDbType.VarChar).Value = _KeyWords
        Cmd.Parameters.Add("@Summary", SqlDbType.VarChar).Value = _Summary
        Cmd.Parameters.Add("@SortKey", SqlDbType.VarChar).Value = _sortKey
        Cmd.Parameters.Add("@ItemPrice", SqlDbType.VarChar).Value = _Itemprice
        Cmd.Parameters.Add("@YoutubeVideoTag", SqlDbType.VarChar).Value = _youtubeVideoTag
        Cmd.Parameters.Add("@OriginalText", SqlDbType.VarChar).Value = _OriginalText
        Cmd.Parameters.Add("@Text", SqlDbType.VarChar).Value = _Text
        Cmd.Parameters.Add("@ProdnRequest", SqlDbType.VarChar).Value = _ProdnRequest
        Cmd.Parameters.Add("@ProdnResponse", SqlDbType.VarChar).Value = _ProdnResponse
        Cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _FolderID
        Cmd.Parameters.Add("@ClickCount", SqlDbType.Int).Value = _clickCount
        Cmd.Parameters.Add("@UsrID", SqlDbType.Int).Value = _UsrID
        Cmd.Parameters.Add("@ClassificationID", SqlDbType.Int).Value = _classificationID
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = _prodnstatus
        Cmd.Parameters.Add("@BillStatus", SqlDbType.Int).Value = _billstatus

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
            ' delete Images
            ' 
            Cmd.CommandText = "DELETE dbo.Image WHERE AdID=@ID"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.AdInstance WHERE AdID=@ID"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.spec WHERE AdID=@ID"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.Ad WHERE ID=@ID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return ATSystem.SysConstants.nullValue

    End Function


    Friend Sub DR2Object(ByVal dr As IDataRecord)
        _ObjectStatus = ATSystem.ObjectState.Original
        _ID = Convert.ToInt32(dr("ID"))
        _AdNumber = Convert.ToString(dr("Adnumber"))
        _IslatestListing = Convert.ToBoolean(dr("LatestListing"))
        _KeyWords = Convert.ToString(dr("KeyWords"))
        _Summary = Convert.ToString(dr("Summary"))
        _sortKey = Convert.ToString(dr("SortKey"))
        _Itemprice = Convert.ToString(dr("ItemPrice"))
        _youtubeVideoTag = Convert.ToString(dr("YoutubeVideoTag"))
        _OriginalText = Convert.ToString(dr("OriginalText"))
        _Text = Convert.ToString(dr("Text"))
        _ProdnRequest = Convert.ToString(dr("ProdnRequest"))
        _ProdnResponse = Convert.ToString(dr("ProdnResponse"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _clickCount = Convert.ToInt32(dr("ClickCount"))
        _prodnstatus = CType(dr("ProdnStatus"), ProdnState)
        _billstatus = CType(dr("BillStatus"), BillState)
        _UsrID = Convert.ToInt32(dr("UsrID"))
        _FolderID = Convert.ToInt32(dr("FolderID"))
        _classificationID = Convert.ToInt32(dr("ClassificationID"))
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        '
        ' readonly fields from joined tables 
        '
        _categoryName = Convert.ToString(dr("CategoryName"))
        _classificationName = Convert.ToString(dr("ClassificationName"))

    End Sub

End Class



