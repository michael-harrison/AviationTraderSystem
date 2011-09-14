Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Configuration
Imports System


'***************************************************************************************
'*
'* Folders
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The Folders collection is used to hold the set of Folder
''' objects. The collection is normally accessed by using the Folders property of the parent System object.
'''  This class manages the Folder database table. Any object in the collection can be modified and then written back to the database,
'''  by either the Update method of the collection class, or the Update method of each object within the collection.
''' </para>
''' </summary>
Public Class Folders : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Folders(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Folders.count-1</param>
    ''' <value>Folder object from Folders collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As folder
        Get
            Return CType(List(index), folder)
        End Get
        Set(ByVal value As folder)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a Folder object to the Folders collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Folder object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As folder) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As folder) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As folder)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As folder)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As folder) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.Folder ORDER BY SortKey"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Folder
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
    Public Function Retrieve(ByVal ID As Integer) As Folder
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Folder WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function


    Public Sub RetrieveSet(ByVal SystemID As Integer)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "SELECT * from dbo.Folder WHERE SystemID=@ID ORDER BY SortKey"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = SystemID
        doRetrieveR(Cmd)

    End Sub

    Public Sub RetrieveSpoolerSet(ByVal SystemID As Integer)
        '
        ' Retrieves the set of records where spooling is defined as active and enabled
        '
        Dim spoolbits As Integer = ATSystem.StatusBits.IsSpooled Or ATSystem.StatusBits.IsSpoolerActive
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "SELECT * from dbo.Folder WHERE SystemID=@ID AND (Status & " & spoolbits & "=" & spoolbits & ") ORDER BY SortKey"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = SystemID
        doRetrieveR(Cmd)

    End Sub

    Public Sub RetrieveProdnSet(ByVal SystemID As Integer)
        '
        ' Retrieves the set of records where spooling is defined as active and enabled
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "SELECT * from dbo.Folder WHERE SystemID=@ID AND (Status & " & ATSystem.StatusBits.IsProdnFolder & "<>0) ORDER BY SortKey"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = SystemID
        doRetrieveR(Cmd)

    End Sub

    ''' <summary>
    ''' Returns the ID of the first proof folder
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFirstFolderID() As Integer
        Dim rtnval As Integer = ATSystem.SysConstants.nullValue
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT TOP 1 ID From dbo.Folder ORDER BY SortKey"

        Try
            Cmd.Connection.Open()
            rtnval = CType(Cmd.ExecuteScalar(), Integer)
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return rtnval
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
                Dim Folder As New Folder
                Add(Folder)
                Folder.ConnectionString = _connectionString
                Folder.DR2Object(dr)
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
        Dim deletedObjects As New List(Of Folder)

        For Each Folder As Folder In List
            Folder.Update()
            If Folder.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Folder)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Folder In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class



'***************************************************************************************
'*
'* Folder - Folder object
'*
'*
'*
'*
'*
'**************************************************************************************

''' <summary>
''' <para> The Folder object holds all data retrieved from a specified row of the Folder table.
'''</para> It is always accessed as part of the Folders collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Folders collection
'''  property of the parent. 
''' </summary>
Public Class Folder
    Private _connectionString As String
    Private _ID As Integer
    Private _systemID As Integer
    Private _name As String
    Private _description As String
    Private _sortkey As String
    Private _navTarget As String
    Private _ObjectStatus As ATSystem.ObjectState
    Private _status As ATSystem.StatusBits
    Private _createTime As Date
    Private _modifyTime As Date
    Private _adCount As Integer
    Private _spoolerCommand As SpoolerCommands
    Private _doneFolderID As Integer
    Private _errorFolderID As Integer
    '
    ' embedded collections
    '
    Private _Sys As ATSystem
    Private _ads As Ads
    Private _adProdnStatus As Ad.ProdnState

    Public Enum SpoolerCommands
        <Description("")> Unspecified = 0
        <Description("Send Proof Email")> ProofEmail = 1
        <Description("Delete All Ads")> DeleteAds = 2
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

    Public ReadOnly Property SpooledName() As String
        Get
            Dim spooledflag As String = ""
            If IsSpooled Then spooledflag = " (Spooled)"
            Return _name & spooledflag
        End Get
    End Property

    ''' <summary>Description</summary>
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Sortkey</summary>
    Public Property Sortkey() As String
        Get
            Return _sortkey
        End Get
        Set(ByVal value As String)
            _sortkey = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Sortkey</summary>
    Public Property SpoolerCommand() As SpoolerCommands
        Get
            Return _spoolerCommand
        End Get
        Set(ByVal value As SpoolerCommands)
            _spoolerCommand = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Defines whether the folder is to be displayed in prodn wizard.
    ''' </summary>
    ''' <value>True if the spec is active, false otherwise.</value>
    Public Property IsProdnFolder() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsProdnFolder And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsProdnFolder
            Else
                _status = _status And Not ATSystem.StatusBits.IsProdnFolder
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Defines whether the folder is spooled.
    ''' </summary>
    ''' <value>True if the spec is active, false otherwise.</value>
    Public Property IsSpooled() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsSpooled And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsSpooled
            Else
                _status = _status And Not ATSystem.StatusBits.IsSpooled
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Defines whether the folder is spooled.
    ''' </summary>
    ''' <value>True if the spec is active, false otherwise.</value>
    Public Property IsSpoolerActive() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsSpoolerActive And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsSpoolerActive
            Else
                _status = _status And Not ATSystem.StatusBits.IsSpoolerActive
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Embedded system object, to which this Folder is subordinate.
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

    ''' <summary>ID of the done folder</summary>
    ''' <value>Object ID as an integer</value>
    Public Property DoneFolderID() As Integer
        Get
            Return _doneFolderID
        End Get
        Set(ByVal value As Integer)
            _doneFolderID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>ID of the done folder</summary>
    ''' <value>Object ID as an integer</value>
    Public Property ErrorFolderID() As Integer
        Get
            Return _errorFolderID
        End Get
        Set(ByVal value As Integer)
            _errorFolderID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Returns a collection of ad objects, representing all of the ads
    ''' which have been placed by this user.
    ''' </summary>
    ''' <value>Collection of ad objects subordinate to the parent User</value>
    Public ReadOnly Property Ads() As Ads
        Get
            Return Ads(Ad.ProdnState.Unspecified)
        End Get
    End Property

    ''' <summary>
    ''' Returns a collection of order objects, representing all of the orders
    ''' which have been placed FOR this user, which are in the prodnstatus range, and qualified by the filter parameter.
    ''' </summary>
    ''' <value>Collection of Order objects subordinate to the parent User,
    ''' qualified by the Filter param</value>
    ''' <param name="ProdnStatus">specifies the production status</param>
    Public ReadOnly Property Ads(ByVal ProdnStatus As Ad.ProdnState) As Ads
        '
        ' if there is no Ads collection, or if there is, but the prodnstatus has changed,
        ' then get a new collection
        '
        Get
            If (_ads Is Nothing) Or (ProdnStatus <> _adProdnStatus) Then
                _adProdnStatus = ProdnStatus             'save the new status
                _ads = New Ads()                      'get a new collection
                _ads.ConnectionString = _connectionString
                _ads.RetrieveSet(ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, _ID, ProdnStatus, Ad.SortOrders.Keywords)
            End If
            Return _ads
        End Get
    End Property


    Public ReadOnly Property AdCount() As Integer
        Get
            If _adCount = -1 Then
                Dim Ads As New Ads
                Ads.ConnectionString = _connectionString
                _adCount = Ads.GetAdCount(ATSystem.ObjectTypes.Folder, _ID)
            End If
            Return _adCount
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
        Cmd.CommandText = "UPDATE dbo.Folder SET " & _
        "modifyTime=getdate()," & _
        "SystemID=@SystemID," & _
        "DoneFolderID=@DoneFolderID," & _
        "ErrorFolderID=@ErrorFolderID," & _
        "Status=@Status," & _
        "Name=@Name," & _
        "Description=@Description," & _
        "SpoolerCommand=@SpoolerCommand," & _
        "Sortkey=@Sortkey" & _
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
        Cmd.CommandText = "INSERT INTO dbo.Folder " & _
        "(SystemID,DoneFolderID,ErrorFolderID,CreateTime,ModifyTime,Status,Name,Description,SpoolerCommand,Sortkey) " & _
        "VALUES " & _
        "(@SystemID,@DoneFolderID,@ErrorFolderID,getdate(),getdate(),@Status,@Name,@Description,@SpoolerCommand,@Sortkey)" & _
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
        Cmd.Parameters.Add("@SpoolerCommand", SqlDbType.Int).Value = _spoolerCommand
        Cmd.Parameters.Add("@Sortkey", SqlDbType.VarChar).Value = _sortkey

        If _doneFolderID = ATSystem.SysConstants.nullValue Then
            Cmd.Parameters.Add("@DoneFolderID", SqlDbType.Int).Value = DBNull.Value
        Else
            Cmd.Parameters.Add("@DoneFolderID", SqlDbType.Int).Value = _doneFolderID
        End If

        If _errorFolderID = ATSystem.SysConstants.nullValue Then
            Cmd.Parameters.Add("@ErrorFolderID", SqlDbType.Int).Value = DBNull.Value
        Else
            Cmd.Parameters.Add("@ErrorFolderID", SqlDbType.Int).Value = _errorFolderID
        End If

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
            '
            ' update all folders which may have this folder as as the done or error folder
            '
            Dim clearbits As Integer = ATSystem.StatusBits.IsSpoolerActive Or ATSystem.StatusBits.IsSpooled
            Cmd.CommandText = "UPDATE Folder Set ErrorFolderID=NULL,Status=Status & ~" & clearbits & " WHERE ErrorFolderID=@ID"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "UPDATE Folder Set DoneFolderID=NULL,Status=Status & ~" & clearbits & " WHERE DoneFolderID=@ID"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.Folder WHERE ID=@ID"
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
        _spoolerCommand = CType(dr("SpoolerCommand"), SpoolerCommands)
        _sortkey = Convert.ToString(dr("Sortkey"))
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        '
        ' done and error folder ids can be null - convert to nullvalue
        '
        If IsDBNull(dr("DoneFolderID")) Then
            _doneFolderID = ATSystem.SysConstants.nullValue
        Else
            _doneFolderID = Convert.ToInt32(dr("DoneFolderID"))
        End If
        If IsDBNull(dr("ErrorFolderID")) Then
            _errorFolderID = ATSystem.SysConstants.nullValue
        Else
            _errorFolderID = Convert.ToInt32(dr("ErrorFolderID"))
        End If
    End Sub

End Class

