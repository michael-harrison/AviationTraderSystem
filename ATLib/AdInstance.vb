Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System



'***************************************************************************************
'*
'* AdInstances
'*
'*
'*
'***************************************************************************************

''' <summary>
''' Inmplements a M:M relationship between Ads and Products, to define which Ads can be accessed by which Products.
''' </summary>
Public Class AdInstances : Inherits CollectionBase

    Private _connectionString As String
    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG AdInstances(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...AdInstances.count-1</param>
    ''' <value>AdInstance object from AdInstances collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As AdInstance
        Get
            Return CType(List(index), AdInstance)
        End Get
        Set(ByVal value As AdInstance)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a AdInstance object to the AdInstances collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">AdInstance object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As AdInstance) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As AdInstance) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As AdInstance)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As AdInstance)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As AdInstance) As Boolean
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
        ' Retrieves the entire table, including deleted items
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT AdInstance.*,Product.Type as ProductType,Ad.AdNumber as AdNumber FROM dbo.AdInstance " & _
        "INNER JOIN dbo.Product on Product.ID=AdInstance.ProductID " & _
        "INNER JOIN dbo.Ad on Ad.ID=AdInstance.AdID "
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As AdInstance
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
    Public Function Retrieve(ByVal ID As Integer) As AdInstance
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT AdInstance.*,Product.Type as ProductType,Ad.AdNumber as AdNumber FROM dbo.AdInstance " & _
        "INNER JOIN dbo.Product on Product.ID=AdInstance.ProductID " & _
        "INNER JOIN dbo.Ad on Ad.ID=AdInstance.AdID " & _
        "WHERE ADInstance.ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function

    ''' <summary>This is a general puroose call which retrieves any number of ad instances
    ''' from the supplied IDS of parent ad, product and edition. If any of the three keys
    ''' are set to nullvalue it means 'dont care'
    '''</summary>
    ''' <param name="UsrID">Usr ID</param>
    ''' <param name="ADID">Ad ID</param>
    ''' <param name="PublicationID">Publication ID</param>
    ''' <param name="ProductID">Product ID</param>
    ''' <param name="EditionID">Edition ID</param>
    Public Sub RetrieveSet(ByVal UsrID As Integer, ByVal AdID As Integer, ByVal PublicationID As Integer, ByVal EditionID As Integer, ByVal ProductID As Integer)
    
        Dim actionmask As Integer = 0

        If UsrID <> ATSystem.SysConstants.nullValue Then actionmask = actionmask Or 1
        If AdID <> ATSystem.SysConstants.nullValue Then actionmask = actionmask Or 2
        If PublicationID <> ATSystem.SysConstants.nullValue Then actionmask = actionmask Or 4
        If ProductID <> ATSystem.SysConstants.nullValue Then actionmask = actionmask Or 8
        If EditionID <> ATSystem.SysConstants.nullValue Then actionmask = actionmask Or 16
        '
        ' Add in selector term
        '
        Dim s1 As String = " AND Ad.UsrID=@UsrID"
        Dim s2 As String = " AND AdID=@AdID"
        Dim s4 As String = " AND Product.PublicationID=@PublicationID"
        Dim s8 As String = " AND ProductID=@ProductID"
        Dim s16 As String = " AND EditionID=@EditionID"

        Dim Selector As String = ""

        If (actionmask And 1) <> 0 Then Selector &= s1
        If (actionmask And 2) <> 0 Then Selector &= s2
        If (actionmask And 4) <> 0 Then Selector &= s4
        If (actionmask And 8) <> 0 Then Selector &= s8
        If (actionmask And 16) <> 0 Then Selector &= s16
        '
        ' strip the first AND and replace by WHERE
        '
        Selector = "WHERE " & Selector.Substring(4, Selector.Length - 4)
        '
        ' add in sort term
        '
        Dim sort As String = " ORDER BY Product.PublicationID,AdInstance.EditionID,Product.ID"

        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@UsrID", SqlDbType.Int).Value = UsrID
        Cmd.Parameters.Add("@AdID", SqlDbType.Int).Value = AdID
        Cmd.Parameters.Add("@PublicationID", SqlDbType.Int).Value = PublicationID
        Cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID
        Cmd.Parameters.Add("@EditionID", SqlDbType.Int).Value = EditionID

        Dim commonSQL As String = "SELECT AdInstance.*,Product.Type as ProductType, Ad.Adnumber as Adnumber FROM dbo.AdInstance " & _
        "INNER JOIN dbo.Product on Product.ID=AdInstance.ProductID " & _
        "INNER JOIN dbo.Ad on Ad.ID=AdInstance.AdID "

        Cmd.CommandText = commonSQL & Selector & sort
        doRetrieveR(Cmd)

    End Sub

    ''' <summary>
    ''' Used by the classad dump. Retrieves all adinstances for the nominated edition, sorted by Ad Category:Classification:Sortkey
    ''' Where the ad is in the specified prodn state
    ''' </summary>
    ''' <param name="EditionID"></param>
    ''' <param name="ProdnStatus"></param>
    ''' <remarks></remarks>
    Public Sub RetrieveDumpSet(ByVal EditionID As Integer, ByVal ProdnStatus As Ad.ProdnState)
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Dim SQL As String = "SELECT AdInstance.*,Product.Type as ProductType, Ad.Adnumber as Adnumber FROM dbo.AdInstance " & _
        "INNER JOIN dbo.Product on Product.ID=AdInstance.ProductID " & _
        "INNER JOIN dbo.Ad on Ad.ID=AdInstance.AdID " & _
        "INNER JOIN dbo.Classification on Classification.ID=Ad.ClassificationID " & _
        "INNER JOIN dbo.Category on Category.ID=Classification.CategoryID " & _
        "WHERE AdInstance.EditionID=@EditionID "

        If ProdnStatus <> Ad.ProdnState.Unspecified Then SQL &= "AND Ad.ProdnStatus=@ProdnStatus "

        SQL &= "ORDER BY Category.Sortkey,Classification.Sortkey,Ad.SortKey"

        Cmd.CommandText = SQL

        Cmd.Parameters.Add("@EditionID", SqlDbType.Int).Value = EditionID
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = ProdnStatus

        doRetrieveR(Cmd)
    End Sub

    ''' <summary>Retrieves the featured ads for the edition
    '''</summary>
    ''' <param name="PublicationID">Publication ID</param>
    ''' <param name="Visibility">Edition Visibility</param>
    ''' <param name="ProdnStatus">Prodn Status</param>
    Public Sub RetrieveFeaturedSet(ByVal PublicationID As Integer, ByVal Visibility As Edition.VisibleState, ByVal ProdnStatus As Ad.ProdnState)

        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Dim SQL As String = "SELECT AdInstance.*,Product.Type as ProductType, Ad.Adnumber as Adnumber FROM dbo.AdInstance " & _
        "INNER JOIN dbo.Product on Product.ID=AdInstance.ProductID " & _
        "INNER JOIN dbo.Ad on Ad.ID=AdInstance.AdID " & _
        "INNER JOIN dbo.Edition on Edition.ID=AdInstance.EditionID " & _
        "WHERE AdInstance.EditionID=Edition.ID " & _
        "AND Edition.PublicationID=@PublicationID " & _
        "AND Ad.ProdnStatus=@ProdnStatus " & _
        "AND Product.Type=" & Product.Types.WebFeaturedAd
        '
        ' add in edition visibility clause if specified
        '
        Dim ednVisibility As String = ""
        If Visibility <> Edition.VisibleState.Unspecified Then
            ednVisibility = " AND Edition.Visibility=@EditionVisibility "
            Cmd.Parameters.Add("@EditionVisibility", SqlDbType.Int).Value = Visibility
        End If

        Cmd.CommandText = Sql & ednVisibility & " ORDER BY Ad.SortKey"

        Cmd.Parameters.Add("@PublicationID", SqlDbType.Int).Value = PublicationID
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = ProdnStatus

        doRetrieveR(Cmd)


    End Sub


    ''' <summary>
    ''' Searches the edition on first words and returns the found list
    ''' </summary>
    ''' <param name="PublicationID"></param>
    ''' <param name="ObjectType"></param>
    ''' <param name="ObjectID"></param>
    ''' <param name="Visibility">Edition Visibility</param>
    ''' <param name="ProdnStatus"></param>
    ''' <param name="Key"></param>
    ''' <remarks></remarks>
    Public Function GetKeywordList(ByVal PublicationID As Integer, ByVal ObjectType As ATSystem.ObjectTypes, ByVal ObjectID As Integer, ByVal Visibility As Edition.VisibleState, ByVal ProdnStatus As Ad.ProdnState, ByVal Key As String) As String()
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Dim SQL As String = ""
        Select Case ObjectType

            Case ATSystem.ObjectTypes.System
                SQL = "Select DISTINCT ad.KeyWords FROM dbo.Ad " & _
                                    "INNER JOIN dbo.AdInstance on AdInstance.AdID=Ad.ID " & _
                                    "INNER JOIN dbo.Edition on Edition.ID=AdInstance.EditionID " & _
                                    "WHERE AdInstance.EditionID=Edition.ID " & _
                                    "AND Edition.PublicationID=@PublicationID " & _
                                    "AND Ad.ProdnStatus=@ProdnStatus " & _
                                    "AND Ad.KeyWords like @Key"


            Case ATSystem.ObjectTypes.Category
                SQL = "Select DISTINCT ad.KeyWords FROM dbo.Ad " & _
                                    "INNER JOIN dbo.Classification on Classification.ID=Ad.ClassificationID " & _
                                    "INNER JOIN dbo.AdInstance on AdInstance.AdID=Ad.ID " & _
                                    "INNER JOIN dbo.Edition on Edition.ID=AdInstance.EditionID " & _
                                    "WHERE AdInstance.EditionID=Edition.ID " & _
                                    "AND Edition.PublicationID=@PublicationID " & _
                                    "AND Classification.CategoryID=@ObjectID " & _
                                    "AND Ad.ProdnStatus=@ProdnStatus " & _
                                    "AND Ad.KeyWords like @Key"

            Case ATSystem.ObjectTypes.Classification
                SQL = "Select DISTINCT ad.KeyWords FROM dbo.Ad " & _
                                 "INNER JOIN dbo.AdInstance on AdInstance.AdID=Ad.ID " & _
                                 "INNER JOIN dbo.Edition on Edition.ID=AdInstance.EditionID " & _
                                 "WHERE AdInstance.EditionID=Edition.ID " & _
                                 "AND Edition.PublicationID=@PublicationID " & _
                                 "AND Ad.ClassificationID=@ObjectID " & _
                                 "AND Ad.ProdnStatus=@ProdnStatus " & _
                                 "AND Ad.KeyWords like @Key"

        End Select

        '
        ' add in edition visibility clause if specified
        '
        Dim ednVisibility As String = " "
        If Visibility <> Edition.VisibleState.Unspecified Then
            ednVisibility = " AND Edition.Visibility=@EditionVisibility "
            Cmd.Parameters.Add("@EditionVisibility", SqlDbType.Int).Value = Visibility
        End If

        Cmd.CommandText = SQL & ednVisibility & " ORDER BY KeyWords"
        Cmd.Parameters.Add("@Key", SqlDbType.VarChar).Value = Key & "%"
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = ProdnStatus
        Cmd.Parameters.Add("@PublicationID", SqlDbType.Int).Value = PublicationID
        Cmd.Parameters.Add("@ObjectID", SqlDbType.Int).Value = ObjectID

        Dim rtnval As New List(Of String)

        Dim dr As SqlDataReader = Nothing
        Cmd.Connection.Open()
        Try
            dr = Cmd.ExecuteReader()
            While dr.Read
                rtnval.Add(Convert.ToString(dr("KeyWords")))
            End While
        Finally
            If Not dr Is Nothing Then
                dr.Close()
                Cmd.Connection.Dispose()
            End If
        End Try

        Return rtnval.ToArray

    End Function


    ''' <summary>This is a general puroose call which retrieves any number of ad instances
    ''' from the supplied IDS of parent ad, product and edition. If any of the three keys
    ''' are set to nullvalue it means 'dont care'
    '''</summary>
    ''' <param name="ClassificationID">Classification ID</param>
    ''' <param name="PublicationID">Publication ID</param>
    ''' <param name="Visibility">Edition Visibility</param>
    ''' <param name="ProdnStatus">Prodn Status</param>
    ''' <param name="StartIndex">Start Index</param>
    ''' <param name="Count">Count ID</param>
    Public Function RetrievePagedDisplaySet(ByVal ClassificationID As Integer, ByVal PublicationID As Integer, ByVal Visibility As Edition.VisibleState, ByVal ProdnStatus As Ad.ProdnState, ByVal StartIndex As Integer, ByVal Count As Integer) As Integer

        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Dim totalAdCount As Integer = 0
        Dim endIndex As Integer = StartIndex + Count - 1
        '
        ' do not retrieve product type selectedad
        '
        Dim sql As String = "SELECT AdInstance.*,Product.Type as ProductType, Ad.Adnumber as Adnumber FROM dbo.AdInstance " & _
        "INNER JOIN dbo.Product on Product.ID=AdInstance.ProductID " & _
        "INNER JOIN dbo.Ad on Ad.ID=AdInstance.AdID " & _
        "INNER JOIN dbo.Edition on Edition.ID=AdInstance.EditionID " & _
        "WHERE AdInstance.EditionID=Edition.ID " & _
        "AND Edition.PublicationID=@PublicationID " & _
        "AND Ad.ClassificationID=@ClassificationID " & _
        "AND Ad.ProdnStatus=@ProdnStatus " & _
        "AND Product.Type<>" & Product.Types.WebFeaturedAd
        '
        ' add in edition visibility clause if specified
        '
        Dim ednVisibility As String = ""
        If Visibility <> Edition.VisibleState.Unspecified Then
            ednVisibility = " AND Edition.Visibility=@EditionVisibility "
            Cmd.Parameters.Add("@EditionVisibility", SqlDbType.Int).Value = Visibility
        End If

        Cmd.CommandText = sql & ednVisibility & " ORDER BY Ad.LatestListing DESC,Ad.SortKey"

        Cmd.Parameters.Add("@ClassificationID", SqlDbType.Int).Value = ClassificationID
        Cmd.Parameters.Add("@PublicationID", SqlDbType.Int).Value = PublicationID
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = ProdnStatus


        List.Clear()
        Dim dr As SqlDataReader = Nothing
        Cmd.Connection.Open()
        Try
            dr = Cmd.ExecuteReader()


            While dr.Read
                '
                ' only add records to the list if they are within the required range
                '
                totalAdCount += 1
                If (totalAdCount >= StartIndex) And (totalAdCount <= endIndex) Then
                    Dim AdInstance As New AdInstance
                    Add(AdInstance)
                    AdInstance.ConnectionString = _connectionString
                    AdInstance.DR2Object(dr)
                End If
            End While
        Finally
            If Not dr Is Nothing Then
                dr.Close()
                Cmd.Connection.Dispose()
            End If
        End Try
        '
        ' return the total records in the set
        '
        Return totalAdCount

    End Function



 
    ''' <summary>This is a general purpose call which retrieves any number of ad instances
    ''' from the supplied IDS of parent ad, product and edition. If any of the three keys
    ''' are set to nullvalue it means 'dont care'
    '''</summary>
    ''' <param name="ObjectType">ObjectType</param>
    ''' <param name="ObjectID">Object ID</param>
    ''' <param name="PublicationID">Publication ID</param>
    ''' <param name="Visibility">Edition visibility</param>
    ''' <param name="ProdnStatus">Prodn Status</param>
    ''' <param name="Key">Key</param>
    ''' <param name="StartIndex">Start Index</param>
    ''' <param name="Count">Count ID</param>
    Public Function RetrievePagedSearchSet(ByVal ObjectType As ATSystem.ObjectTypes, ByVal ObjectID As Integer, ByVal PublicationID As Integer, ByVal Visibility As Edition.VisibleState, ByVal ProdnStatus As Ad.ProdnState, ByVal Key As String, ByVal StartIndex As Integer, ByVal Count As Integer) As Integer

        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Dim totalAdCount As Integer = 0
        Dim endIndex As Integer = StartIndex + Count - 1

        Dim SQL As String = ""
        '
        ' do not retrieve product type selectedad
        '
        Select Case ObjectType
            Case ATSystem.ObjectTypes.System
                SQL = "SELECT AdInstance.*, Product.Type as ProductType, Ad.Adnumber as Adnumber FROM dbo.AdInstance " & _
                          "INNER JOIN dbo.Ad on Ad.ID = AdInstance.AdID " & _
                          "INNER JOIN dbo.Product on Product.ID=AdInstance.ProductID " & _
                          "INNER JOIN dbo.Edition on Edition.ID=AdInstance.EditionID " & _
                          "WHERE AdInstance.EditionID=Edition.ID " & _
                          "AND Edition.PublicationID=@PublicationID " & _
                          "AND AdInstance.ProductID=Product.ID " & _
                          "AND AD.ProdnStatus=@ProdnStatus " & _
                          "AND Product.Type<>" & Product.Types.WebFeaturedAd

            Case ATSystem.ObjectTypes.Category
                SQL = "SELECT AdInstance.*, Product.Type as ProductType, Ad.Adnumber as Adnumber FROM dbo.AdInstance " & _
                        "INNER JOIN dbo.Ad on Ad.ID = AdInstance.AdID " & _
                        "INNER JOIN dbo.Classification on Classification.ID=Ad.ClassificationID " & _
                        "INNER JOIN dbo.Product on Product.ID=AdInstance.ProductID " & _
                        "INNER JOIN dbo.Edition on Edition.ID=AdInstance.EditionID " & _
                        "WHERE AdInstance.EditionID=Edition.ID " & _
                        "AND Edition.PublicationID=@PublicationID " & _
                        "AND AdInstance.ProductID=Product.ID " & _
                        "AND Classification.CategoryID=@ObjectID " & _
                        "AND AD.ProdnStatus=@ProdnStatus " & _
                        "AND Product.Type<>" & Product.Types.WebFeaturedAd

            Case ATSystem.ObjectTypes.Classification
                SQL = "SELECT AdInstance.*, Product.Type as ProductType, Ad.Adnumber as Adnumber FROM dbo.AdInstance " & _
                          "INNER JOIN dbo.Ad on Ad.ID = AdInstance.AdID " & _
                          "INNER JOIN dbo.Product on Product.ID=AdInstance.ProductID " & _
                          "INNER JOIN dbo.Edition on Edition.ID=AdInstance.EditionID " & _
                          "WHERE AdInstance.EditionID=Edition.ID " & _
                          "AND Edition.PublicationID=@PublicationID " & _
                          "AND AdInstance.ProductID=Product.ID " & _
                          "AND Ad.ClassificationID=@ObjectID " & _
                          "AND AD.ProdnStatus=@ProdnStatus " & _
                          "AND Product.Type<>" & Product.Types.WebFeaturedAd

        End Select
        '
        ' add in edition visibility clause if specified
        '
        Dim ednVisibility As String = ""
        If Visibility <> Edition.VisibleState.Unspecified Then
            ednVisibility = " AND Edition.Visibility=@EditionVisibility "
            Cmd.Parameters.Add("@EditionVisibility", SqlDbType.Int).Value = Visibility
        End If
        '
        ' add in the sort field spec only if the key is supplied
        '
        Dim searchspec As String = " ORDER BY Ad.LatestListing DESC,Ad.SortKey"
        If Key.Length > 0 Then
       




            Cmd.Parameters.Add("@Key", SqlDbType.VarChar).Value = getFTKeyNoStop(Key)
            searchspec = " AND CONTAINS(Ad.Text, @Key) ORDER BY Ad.LatestListing DESC,Ad.SortKey"
        End If
        Cmd.CommandText = SQL & ednVisibility & searchspec

        Cmd.Parameters.Add("@PublicationID", SqlDbType.Int).Value = PublicationID
        Cmd.Parameters.Add("@ObjectID", SqlDbType.Int).Value = ObjectID
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = ProdnStatus


        List.Clear()
        Dim dr As SqlDataReader = Nothing
        Cmd.Connection.Open()
        Try
            dr = Cmd.ExecuteReader()
            While dr.Read
                '
                ' only add records to the list if they are within the required range
                '
                totalAdCount += 1
                If (totalAdCount >= StartIndex) And (totalAdCount <= endIndex) Then
                    Dim AdInstance As New AdInstance
                    Add(AdInstance)
                    AdInstance.ConnectionString = _connectionString
                    AdInstance.DR2Object(dr)
                End If
            End While
        Finally
            If Not dr Is Nothing Then
                dr.Close()
                Cmd.Connection.Dispose()
            End If
        End Try
        '
        ' return the total records in the set
        '
        Return totalAdCount

    End Function

    ''' <summary>
    ''' works only for SQL Server 2008. Uses the parser to eliminate stop words from the search
    ''' key
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getFTKeyNoStop(ByVal key As String) As String
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT display_term FROM sys.dm_fts_parser(@key,1033,0,0) WHERE special_term='Exact Match'"
        Cmd.Parameters.Add("@Key", SqlDbType.VarChar).Value = """" & key & """"

        Dim rtnval As String = ""

        Dim dr As SqlDataReader = Nothing
        Cmd.Connection.Open()
        Try
            dr = Cmd.ExecuteReader()
            While dr.Read
                rtnval &= """" & Convert.ToString(dr("display_term")) & "*"" AND "
            End While
        Finally
            If Not dr Is Nothing Then
                dr.Close()
                Cmd.Connection.Dispose()
            End If
        End Try
        '
        ' remove the last AND
        '
        If rtnval.Length > 0 Then rtnval = rtnval.Substring(0, rtnval.Length - 5)
        Return rtnval
    End Function


    Private Function getFTKey(ByVal key As String) As String
        '
        ' makes up a full text search string of the form w1* AND w2* AND w3* ...
        '
        Dim rtnval As String = ""
        Dim sep As Char = Chr(&H20)
        Dim arr() As String = key.Split(sep)
        For i As Integer = 0 To arr.Length - 1
            Dim token As String = arr(i).Trim
            If token.Length > 0 Then
                rtnval &= """" & token & "*""" & " AND "
            End If
        Next
        '
        ' remove the last AND
        '
        If rtnval.Length > 0 Then rtnval = rtnval.Substring(0, rtnval.Length - 5)
        Return rtnval
    End Function


    ''' <summary>This is a general puroose call which deletes any number of ad instances
    ''' from the supplied IDS of parent ad, product and edition. If any of the three keys
    ''' are set to nullvalue it means 'dont care'
    '''</summary>
    ''' <param name="AdID">Ad ID</param>
    ''' <param name="ProductID">Product ID</param>
    ''' <param name="EditionID">Edition ID</param>
    Public Sub DeleteSet(ByVal AdID As Integer, ByVal ProductID As Integer, ByVal EditionID As Integer)
        '
        ' Retrieves the set of records for the Product
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Dim actionmask As Integer = 0

        If AdID <> ATSystem.SysConstants.nullValue Then actionmask = actionmask Or 1
        If ProductID <> ATSystem.SysConstants.nullValue Then actionmask = actionmask Or 2
        If EditionID <> ATSystem.SysConstants.nullValue Then actionmask = actionmask Or 4

        Cmd.Parameters.Add("@AdID", SqlDbType.Int).Value = AdID
        Cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID
        Cmd.Parameters.Add("@EditionID", SqlDbType.Int).Value = EditionID


        Select Case actionmask
            Case 0 : Cmd.CommandText = "DELETE dbo.AdInstance"
            Case 1 : Cmd.CommandText = "DELETE dbo.AdInstance WHERE AdID=@AdID"
            Case 2 : Cmd.CommandText = "DELETE dbo.AdInstance WHERE ProductID=@ProductID"
            Case 3 : Cmd.CommandText = "DELETE dbo.AdInstance WHERE AdID=@AdID AND ProductID=@ProductID"
            Case 4 : Cmd.CommandText = "DELETE dbo.AdInstance WHERE EditionID=@EditionID"
            Case 5 : Cmd.CommandText = "DELETE dbo.AdInstance WHERE AdID=@AdID AND EditionID=@EditionID"
            Case 6 : Cmd.CommandText = "DELETE dbo.AdInstance WHERE EditionID=@EditionID AND ProductID=@ProductID"
            Case 7 : Cmd.CommandText = "DELETE dbo.AdInstance WHERE AdID=@AdID AND ProductID=@ProductID AND EditionID=@EditionID"
        End Select

        Try
            Cmd.Connection.Open()
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try

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
                Dim AdInstance As New AdInstance
                Add(AdInstance)
                AdInstance.ConnectionString = _connectionString
                AdInstance.DR2Object(dr)
            End While
        Finally
            If Not dr Is Nothing Then
                dr.Close()
                Cmd.Connection.Dispose()
            End If
        End Try
    End Sub


    ''' <summary>Updates the AdInstances collection back to the database. Only those objects in the collection which have been
    ''' either modified, created from new, or marked for deletion are re-written.</summary>
    '''
    Public Sub Update()
        '
        ' Updates all objects in the list. The child object tests for dirty
        '
        Dim deletedObjects As New List(Of AdInstance)

        For Each AdInstance As AdInstance In List
            AdInstance.Update()
            If AdInstance.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(AdInstance)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As AdInstance In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class




'***************************************************************************************
'*
'* AdInstance
'*
'*
'*
'***************************************************************************************

''' <summary>
''' The AdInstance object is used to associated Product availaibility of Ads by imlementing a M:M relationship between
''' Ad and Product
''' </summary>
Public Class AdInstance

    Private _connectionString As String
    Private _ID As Integer
    Private _ProductID As Integer
    Private _EditionID As Integer
    Private _AdID As Integer
    Private _productType As Product.Types
    Private _adNumber As String
    Private _physicalApplicationPath As String
    Private _ProdnPDFFolder As String
    Private _navTarget As String
    Private _ObjectStatus As ATSystem.ObjectState
    Private _status As ATSystem.StatusBits
    Private _createTime As Date
    Private _modifyTime As Date

    Private _Price As Integer
    Private _PriceAdjust As Integer
    Private _Width As RateTable.DisplayWidths
    Private _Height As Integer
    Private _exactWidth As Integer  'mm * 1000 from INDD
    Private _exactHeight As Integer 'mm * 1000 from INDD
    Private _ProdnSurcharge As Integer
    Private _previewSequence As Integer
    Private _color As RateTable.DisplayColorTypes
    Private _INDDFilename As String

    '
    ' embedded collections
    '
    Private _Ad As Ad           'backpointer to parent ad
    Private _Product As Product 'backpointer to parent product
    Private _Edition As Edition  'backpointer to parent edition

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

    ''' <summary>Product ID, of the parent Product object</summary>
    ''' <value>ProductID as an integer</value>
    Public Property ProductID() As Integer
        Get
            Return _ProductID
        End Get
        Set(ByVal value As Integer)
            _ProductID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Edition ID, of the parent Edition object</summary>
    ''' <value>EditionID as an integer</value>
    Public Property EditionID() As Integer
        Get
            Return _EditionID
        End Get
        Set(ByVal value As Integer)
            _EditionID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Product ID in hex form, of the parent Product object</summary>
    ''' <value>ProductID as a hex string</value>
    Public ReadOnly Property ProductHexID() As String
        Get
            Return _ProductID.ToString("X8")
        End Get
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
    ''' Fully qualified path to the production folder
    ''' </summary>
    Public Property ProdnPDFFolder() As String
        Get
            Return _ProdnPDFFolder
        End Get
        Set(ByVal value As String)
            _ProdnPDFFolder = value
        End Set
    End Property

    ''' <summary>
    ''' INDD filename of display ad, or empty string otherwise
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property INDDFilename() As String
        Get
            Return _INDDFilename
        End Get
        Set(ByVal value As String)
            _INDDFilename = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' INDD short filename of display ad, or empty string otherwise
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property INDDShortFilename() As String
        Get
            If _INDDFilename = "" Then
                Return ""
            Else
                Dim fileInfo As System.IO.FileInfo
                fileInfo = My.Computer.FileSystem.GetFileInfo(_INDDFilename)
                Return fileInfo.Name
            End If
        End Get
    End Property

    ''' <summary>Preview Sequence - used to generate unique preview filenames</summary>
    ''' <value>PreviewSequence as an integer</value>
    Public Property PreviewSequence() As Integer
        Get
            Return _previewSequence
        End Get
        Set(ByVal value As Integer)
            _previewSequence = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    Public ReadOnly Property PreviewImageURL() As String
        '
        ' for web ads, return a standard image. For print ads, get the valid preview
        '
        Get

            Dim appPath As String = GetApplicationPath()

            Select Case _productType
                Case ATLib.Product.Types.WebBasic : Return appPath & Constants.webbasicpreviewimage
                Case ATLib.Product.Types.WebPremium : Return appPath & Constants.WebPremiumPreviewImage
                Case ATLib.Product.Types.WebFeaturedAd : Return appPath & Constants.webfeaturedadpreviewimage
                Case ATLib.Product.Types.WebPDF : Return appPath & Constants.WebPDFPreviewImage
                Case ATLib.Product.Types.WebPDFText : Return appPath & Constants.WebPDFTextPreviewImage

                Case Else

                    If IsPreviewValid Then
                        Dim filename As String = hexID & "-" & _previewSequence.ToString & ".jpg"
                        Return GetApplicationPath() & Constants.subsampledImagesAdInstance & "/" & filename
                    Else
                        Return appPath & Constants.InvalidPreviewImage
                    End If
            End Select

        End Get
    End Property



    Public ReadOnly Property PreviewPDFURL() As String
        '
        ' for normal web ads return the invalid pdf image
        ' for web ads with pdf display return the first print pdf image
        ' for print ads, get the pdf image
        Get

            Dim appPath As String = GetApplicationPath()

            Select Case _productType
                Case ATLib.Product.Types.WebBasic : Return appPath & Constants.InvalidPreviewPDF
                Case ATLib.Product.Types.WebPremium : Return appPath & Constants.InvalidPreviewPDF
                Case ATLib.Product.Types.WebFeaturedAd : Return appPath & Constants.InvalidPreviewPDF
                Case ATLib.Product.Types.WebPDF, ATLib.Product.Types.WebPDFText
                    Dim firstPrintInstance As AdInstance = getFirstPrintInstance()
                    If firstPrintInstance Is Nothing Then
                        Return appPath & Constants.InvalidPreviewPDF
                    Else
                        Return firstPrintInstance.PreviewPDFURL
                    End If

                Case Else

                    If IsPreviewValid Then
                        Dim filename As String = hexID & "-" & _previewSequence.ToString & ".pdf"
                        Return GetApplicationPath() & Constants.subsampledImagesAdInstance & "/" & filename
                    Else
                        Return appPath & Constants.InvalidPreviewPDF
                    End If
            End Select

        End Get
    End Property

    Public ReadOnly Property PreviewImageFilename() As String
        Get
            Dim filename As String = hexID & "-" & _previewSequence.ToString & ".jpg"
            Return IO.Path.Combine(_physicalApplicationPath, Constants.subsampledImagesAdInstance & "/" & filename)
        End Get
    End Property

    Public ReadOnly Property PreviewPDFFilename() As String
        Get
            Dim filename As String = hexID & "-" & _previewSequence.ToString & ".pdf"
            Return IO.Path.Combine(_physicalApplicationPath, Constants.subsampledImagesAdInstance & "/" & filename)
        End Get
    End Property

    Public ReadOnly Property ProdnPDFFilename() As String
        Get
            Dim filename As String = hexID & "-" & _previewSequence.ToString & ".pdf"
            Return IO.Path.Combine(_ProdnPDFFolder, filename)
        End Get
    End Property

    Private Function getFirstPrintInstance() As AdInstance
        '
        ' if I am a web instance, then this call gets the first print instance, in order to
        ' return the image and pdf previews
        '
        Dim rtnval As AdInstance = Nothing

        For Each AdInstance As AdInstance In Ad.Instances
            Select Case AdInstance.ProductType
                Case ATLib.Product.Types.ClassadColorPic, ATLib.Product.Types.ClassadMonoPic, ATLib.Product.Types.ClassadTextOnly, ATLib.Product.Types.DisplayComposite, ATLib.Product.Types.DisplayFinishedArt, ATLib.Product.Types.DisplayModule, ATLib.Product.Types.DisplayStandAlone
                    rtnval = AdInstance
                    Exit For
                Case Else           'continue looking
            End Select
        Next
        Return rtnval       'which may be null if there are no print instances

    End Function


    ''' <summary>
    ''' Ad ID, of the parent Ad object</summary>
    ''' <value>AdID as an integer</value>
    Public Property AdID() As Integer
        Get
            Return _AdID


        End Get
        Set(ByVal value As Integer)
            _AdID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Ad ID in hex form, of the parent Ad object</summary>
    ''' <value>AdID as a hex string</value>
    Public ReadOnly Property AdHexID() As String
        Get
            Return _AdID.ToString("X8")
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


    Friend Property ObjectStatus() As ATSystem.ObjectState
        Get
            Return _ObjectStatus
        End Get
        Set(ByVal value As ATSystem.ObjectState)
            _ObjectStatus = value
        End Set
    End Property

    ''' <summary>Price of this instance of the ad</summary>
    Public Property Price() As Integer
        Get
            Return _Price
        End Get
        Set(ByVal value As Integer)
            _Price = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Production Surcharge price</summary>
    Public Property ProdnSurcharge() As Integer
        Get
            Return _ProdnSurcharge
        End Get
        Set(ByVal value As Integer)
            _ProdnSurcharge = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Price Adjustment - -ve or +ve</summary>
    Public Property PriceAdjust() As Integer
        Get
            Return _Priceadjust
        End Get
        Set(ByVal value As Integer)
            _PriceAdjust = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property Subtotal() As Integer
        Get
            Return _Price + _ProdnSurcharge + _PriceAdjust
        End Get
    End Property

    ''' <summary>Width of ad in column enums</summary>
    Public Property Width() As RateTable.DisplayWidths
        Get
            Return _Width
        End Get
        Set(ByVal value As RateTable.DisplayWidths)
            _Width = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>Exact width of ad in mm * 1000 as determined by INDD</summary>
    Public Property ExactWidth() As Integer
        Get
            Return _exactWidth
        End Get
        Set(ByVal value As Integer)
            _exactWidth = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>height of ad in lines or cm excluding pic for classifieds</summary>
    Public Property Height() As Integer
        Get
            Return _Height
        End Get
        Set(ByVal value As Integer)
            _Height = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Exact height of ad in mm * 1000 as determined by INDD</summary>
    Public Property ExactHeight() As Integer
        Get
            Return _exactHeight
        End Get
        Set(ByVal value As Integer)
            _exactHeight = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Color of (display) ad</summary>
    Public Property Color() As RateTable.DisplayColorTypes
        Get
            Return _color
        End Get
        Set(ByVal value As RateTable.DisplayColorTypes)
            _color = value
            _ObjectStatus = ATSystem.ObjectState.Modified
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

    ''' <summary>
    ''' Returns the ad object for this instance
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
    ''' Returns the product object for this instance
    ''' </summary>
    Public ReadOnly Property Product() As Product
        Get
            If _Product Is Nothing Then
                Dim Products As New Products
                Products.ConnectionString = _connectionString
                _Product = Products.Retrieve(_ProductID)
            End If
            Return _Product
        End Get
    End Property

    Public ReadOnly Property ProductType() As Product.Types
        Get
            Return _productType
        End Get
    End Property

    Public ReadOnly Property AdNumber() As String
        Get
            Return _adNumber

        End Get
    End Property

    ''' <summary>
    ''' Returns the edition object for this instance
    ''' </summary>
    Public ReadOnly Property Edition() As Edition
        Get
            If _Edition Is Nothing Then
                Dim Editions As New Editions
                Editions.ConnectionString = _connectionString
                _Edition = Editions.Retrieve(_EditionID)
            End If
            Return _Edition
        End Get
    End Property


    ''' <summary>
    ''' Defines whether the proof preview is valid or not.
    ''' </summary>
    ''' <value>True if the spec is active, false otherwise.</value>
    Public Property IsPreviewValid() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsPreviewValid And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsPreviewValid
            Else
                _status = _status And Not ATSystem.StatusBits.IsPreviewValid
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Defines whether the object is checked out.
    ''' </summary>
    ''' <value>True if the spec is active, false otherwise.</value>
    Public Property IsCheckedOut() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsCheckedOut And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsCheckedOut
            Else
                _status = _status And Not ATSystem.StatusBits.IsCheckedOut
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
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

    Public Sub generateINDDName(ByVal DisplayAdFolder As String)
        Dim filename As String = _adNumber & " - " & _productType.ToString & ".INDD"
        INDDFilename = IO.Path.Combine(DisplayAdFolder, filename)
    End Sub

    Public Sub CalculateClassadSize()
        '
        ' guess the number of lines from the size - truncate works
        ' subtract the pic if its there
        '
        Dim mySys As New ATSystem
        mySys.Retrieve()

        Dim picheight As Integer = 0
        Select Case _productType
            Case Product.Types.ClassadColorPic : picheight = mySys.ClassadPicHeight + Convert.ToInt32(Constants.ClassadPicFudgefactor * 1000)
            Case Product.Types.ClassadMonoPic : picheight = mySys.ClassadPicHeight + Convert.ToInt32(Constants.ClassadPicFudgefactor * 1000)
        End Select

        Dim textsize As Integer = _exactHeight - picheight

        Dim computedheight As Integer = Convert.ToInt32(Math.Truncate(textsize / mySys.ClassadLineHeight))
        If computedheight < 2 Then computedheight = 2 'min two lines
        _Height = computedheight
      
    End Sub

    Public Sub CalculateDisplaySize()
        '
        ' works out the display size in column cm from the exact size
        '
        Dim mySys As New ATSystem
        mySys.Retrieve()

        Dim errorstatus As Integer = 0

        Dim h As Double = _exactHeight / 10000
        Dim hfloor As Double = Math.Floor(h)
        If hfloor <> h Then hfloor += 1 'hfloor in cm
        Dim maxpageheight As Double = mySys.DisplayColumnHeight / 10000       'in cm
        If hfloor > maxpageheight Then
            _Height = Convert.ToInt32(maxpageheight)
            errorstatus = 1
        Else
            _Height = Convert.ToInt32(hfloor)
         End If
        '
        ' ad width - allow a gutter width tolerance on the width
        '
        Dim cw As Integer = mySys.DisplayColumnWidth
        Dim gw As Integer = mySys.DisplayGutterWidth
        Dim w As Integer = _exactWidth
        If w <= cw + gw + cw + gw Then
            _Width = RateTable.DisplayWidths.Col2
        ElseIf w < cw + gw + cw + gw + cw + gw Then
            _Width = RateTable.DisplayWidths.Col3
        ElseIf w < cw + gw + cw + gw + cw + gw + cw + gw Then
            _Width = RateTable.DisplayWidths.Col4
        ElseIf w < cw + gw + cw + gw + cw + gw + cw + gw + cw + gw Then
            _Width = RateTable.DisplayWidths.Col5
        ElseIf w < cw + gw + cw + gw + cw + gw + cw + gw + cw + gw + cw + gw + cw Then
            _Width = RateTable.DisplayWidths.Col7
        Else
            _Width = RateTable.DisplayWidths.Col7
            errorstatus += 2
        End If

        Select Case errorstatus
            Case 1 : Throw New Exception(Constants.DisplayAdTooWide)
            Case 2 : Throw New Exception(Constants.DisplayAdTooHigh)
            Case 3 : Throw New Exception(Constants.DisplayAdTooHighAndWide)

        End Select

    End Sub

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
        Cmd.CommandText = "UPDATE dbo.AdInstance SET " & _
        "modifyTime=getdate()," & _
        "Status=@Status," & _
        "AdID=@AdID," & _
        "ProductID=@ProductID," & _
        "EditionID=@EditionID," & _
        "Price=@Price, " & _
        "INDDFilename=@INDDFilename, " & _
        "PreviewSequence=@PreviewSequence, " & _
        "PriceAdjust=@PriceAdjust," & _
        "ProdnSurcharge=@ProdnSurcharge," & _
        "Width=@Width," & _
        "Height=@Height," & _
        "ExactWidth=@ExactWidth," & _
        "ExactHeight=@ExactHeight," & _
        "Color=@Color " & _
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
        '
        ' must generate a AdInstance record and then a AdInstance2parent record
        '

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()

        Try
            Cmd.Connection.Open()

            Cmd.CommandText = "INSERT INTO dbo.AdInstance" & _
            "(AdID,ProductID,EditionID,CreateTime,ModifyTime,Status,PreviewSequence," & _
            "INDDFilename," & _
            "Price,PriceAdjust,ProdnSurcharge," & _
            "Width,Height,ExactWidth,ExactHeight,Color) " & _
            "VALUES " & _
            "(@AdID,@ProductID,@EditionID,getdate(),getdate(),@Status,@PreviewSequence," & _
            "@INDDFilename," & _
            "@Price,@PriceAdjust,@ProdnSurcharge," & _
            "@Width,@Height,@ExactWidth,@ExactHeight,@Color) " & _
            Global.Microsoft.VisualBasic.ChrW(13) & _
            Global.Microsoft.VisualBasic.ChrW(10) & "SELECT SCOPE_IDENTITY();"

            addParams(Cmd)

            _ID = CType(Cmd.ExecuteScalar(), Integer)

        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return _ID

    End Function

    Private Sub addParams(ByVal Cmd As SqlCommand)

        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID
        Cmd.Parameters.Add("@AdID", SqlDbType.Int).Value = _AdID
        Cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = _ProductID
        Cmd.Parameters.Add("@EditionID", SqlDbType.Int).Value = _EditionID
        Cmd.Parameters.Add("@PreviewSequence", SqlDbType.Int).Value = _previewSequence
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@INDDFilename", SqlDbType.VarChar).Value = _INDDFilename
        Cmd.Parameters.Add("@Price", SqlDbType.Int).Value = _Price
        Cmd.Parameters.Add("@PriceAdjust", SqlDbType.Int).Value = _PriceAdjust
        Cmd.Parameters.Add("@ProdnSurcharge", SqlDbType.Int).Value = _ProdnSurcharge
        Cmd.Parameters.Add("@Width", SqlDbType.Int).Value = _Width
        Cmd.Parameters.Add("@Height", SqlDbType.Int).Value = _Height
        Cmd.Parameters.Add("@ExactWidth", SqlDbType.Int).Value = _exactWidth
        Cmd.Parameters.Add("@ExactHeight", SqlDbType.Int).Value = _exactHeight
        Cmd.Parameters.Add("@Color", SqlDbType.Int).Value = _color
    End Sub

    Private Function doDelete() As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()

        Cmd.Parameters.Add("@ID", SqlDbType.Int, 0, "ID").Value = _ID
        Try
            Cmd.Connection.Open()

            Cmd.CommandText = "DELETE dbo.AdInstance WHERE ID=@ID"
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
        _ProductID = Convert.ToInt32(dr("ProductID"))
        _EditionID = Convert.ToInt32(dr("EditionID"))
        _previewSequence = Convert.ToInt32(dr("PreviewSequence"))
        _INDDFilename = Convert.ToString(dr("INDDFilename"))
        _Price = Convert.ToInt32(dr("Price"))
        _PriceAdjust = Convert.ToInt32(dr("PriceAdjust"))
        _ProdnSurcharge = Convert.ToInt32(dr("ProdnSurcharge"))
        _Width = CType(dr("Width"), RateTable.DisplayWidths)
        _Height = Convert.ToInt32(dr("Height"))
        _exactWidth = Convert.ToInt32(dr("ExactWidth"))
        _exactHeight = Convert.ToInt32(dr("ExactHeight"))
        _color = CType(dr("Color"), RateTable.DisplayColorTypes)
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        '
        ' readonly values from joined tables
        '
        _productType = CType(dr("ProductType"), Product.Types)
        _adNumber = Convert.ToString(dr("AdNumber"))

    End Sub

End Class

