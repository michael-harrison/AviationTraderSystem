Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System


'***************************************************************************************
'*
'* Slots
'*
'*
'*
'***************************************************************************************

''' <summary>
''' The Slots class implements a collection of Slot objects. A new slot object is
''' created for each user login  and is used to track session state by an associated
''' database record. The SlotID is one of the primary properties that is
''' incorporated in the Loader object, and is passed as part of the loader data
''' structure between host and client via the Querystring. 
''' <para></para>
''' <para>The slot object for a session is normally accessed directly once the
''' loader is known. The collectio of slots for any user can also be found through
''' the Usr. Slots property of the Usr object. The following code shows how the slot
''' is retrieved from the request.querystring, via the loader object</para>
''' <para></para>
''' </summary>

Public Class Slots : Inherits CollectionBase
    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Slots(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...slots.count-1</param>
    ''' <value>Slot object from Slots collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Slot
        Get
            Return CType(List(index), Slot)
        End Get
        Set(ByVal value As Slot)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a Slot object to the Slots collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Slot object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As Slot) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Slot) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Slot)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Slot)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Slot) As Boolean
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
        Cmd.CommandText = commonSQL() & " ORDER by Slot.ID DESC"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Slot
        Return Retrieve(Hex2Int(HexID))
    End Function



    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="ID">ObjectID, represented as an integer</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal ID As Integer) As Slot
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = commonSQL() & " WHERE Slot.ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function


    Friend Sub retrieveSet(ByVal UsrID As Integer)
        '
        ' Retrieves the set of records for the parent
        '
        Dim connection As SqlConnection = getConnection()
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = commonSQL() & " WHERE Slot.UsrID=@UsrID ORDER BY Slot.ID DESC"
        Cmd.Parameters.Add("@UsrID", SqlDbType.Int).Value = UsrID
        doRetrieveR(Cmd)

    End Sub

    Friend Sub retrieveSet(ByVal loginStatus As Slot.LoginStates)
        '
        ' Retrieves the set of records which have the supplied loginStatus
        '
        Dim connection As SqlConnection = getConnection()
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        If loginStatus = Slot.LoginStates.Unspecified Then
            Cmd.CommandText = commonSQL() & " ORDER by Slot.ID DESC"
        Else
            Cmd.CommandText = commonSQL() & " WHERE Slot.loginStatus=@loginStatus ORDER by Slot.ID DESC"
            Cmd.Parameters.Add("@loginStatus", SqlDbType.Int).Value = loginStatus
        End If
        doRetrieveR(Cmd)

    End Sub

    Private Function commonSQL() As String
        Return "SELECT Slot.*," & _
                "Usr.EmailAddr as EmailAddr," & _
                "Usr.FName as FName," & _
                "Usr.LName as LName," & _
                "Usr.EditionVisibility as EditionVisibility," & _
                "Usr.UAM as UAM," & _
                "Usr.LoginLevel as LoginLevel" & _
                " FROM dbo.Slot " & _
                "INNER JOIN dbo.Usr on Slot.UsrID=Usr.ID "
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
                Dim Slot As New Slot
                Add(Slot)
                Slot.ConnectionString = _connectionString
                Slot.DR2Object(dr)
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
        Dim deletedObjects As New List(Of Slot)

        For Each Slot As Slot In List
            Slot.Update()
            If Slot.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Slot)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Slot In deletedObjects
            List.Remove(s)
        Next

    End Sub



    ''' <summary>
    ''' Deletes slots whose last activity is less than the supplied purge days
    ''' returns the number of slots affected
    ''' </summary>
    ''' <param name="days">Number of days to retain slots before purging</param>
    ''' <returns>Number of slots that have been purged.</returns>
    Public Function Purge(ByVal days As Integer) As Integer
        '
        '
        Dim rowCount As Integer
        If days > 0 Then

            Dim Cmd As New SqlCommand
            Cmd.Connection = getConnection()

            Try
                Cmd.Connection.Open()

                Cmd.CommandText = "DELETE dbo.Slot" & _
                " WHERE ModifyTime<getdate()-" & days & ";SELECT @@rowCount;"

                rowCount = CType(Cmd.ExecuteScalar(), Integer)

            Finally
                Cmd.Connection.Dispose()            'close and dispose connection
            End Try
        Else
            rowCount = 0
        End If

        Return rowCount
    End Function
End Class



'***************************************************************************************
'*
'* Slot - Slot object
'*
'* AUDIT TRAIL
'* 
'* V1.000   31-AUG-2009  BA  Original
'*
'*
'*
'*
'**************************************************************************************

''' <summary>
''' <para> The Slot object holds all data retrieved from a specified row of the Slot table.
'''</para>
''' <para> It is always accessed as part of the Slots collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Slots collection
'''  property of the parent Usr object. 
''' </para>
''' </summary>
Public Class Slot


    ''' <summary>
    ''' Defines the state that a slot can be in.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum LoginStates
        Undefined = 0
        Active = 1
        Killed = 2
        Timeout = 3
        Loggedout = 4
        InvalidUserName = 6
        InvalidPassword = 7
        Unspecified = 8
    End Enum

    Public Enum SearchModes
        Browse = 1
        Search = 2
        Manage = 3
        Proof = 4
        SingleShot = 5
    End Enum


    Private _connectionString As String
    Private _ID As Integer
    Private _UsrID As Integer
    Private _ImpersonateUsrID As Integer
    Private _AliasIDFilter As Integer
    Private _userTypeFilter As Usr.LoginLevels
    Private _AdSortOrder As Ad.SortOrders
    Private _EmailAddr As String
    Private _FName As String
    Private _LName As String
    Private _status As ATSystem.StatusBits
    Private _loginStatus As LoginStates
    Private _editionVisibility As Edition.VisibleState
    Private _createTime As Date
    Private _modifyTime As Date
    Private _IPAddr As String
    Private _sessionID As String
    Private _UAM As Usr.UAMs
    Private _loginlevel As Usr.LoginLevels
    Private _strParam1 As String
    Private _skin As String
    Private _searchMode As SearchModes
    Private _searchObjectType As ATSystem.ObjectTypes
    Private _searchObjectID As Integer
    Private _proofObjectType As ATSystem.ObjectTypes
    Private _proofObjectID As Integer
    Private _searchKey As String

    Private _ObjectStatus As ATSystem.ObjectState
    '
    ' embedded objects and collections
    '
    Private _Usr As Usr
    Private _ImpersonatedUsr As Usr

    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _ID = ATSystem.SysConstants.nullValue
        _loginStatus = LoginStates.Unspecified
        _editionVisibility = Edition.VisibleState.Active
        _status = 0
        _sessionID = ""
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
    ''' User's first name
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property FName() As String
        Get
            Return _FName
        End Get
    End Property


    ''' <summary>
    ''' User's last name
    ''' </summary>
    Public ReadOnly Property LName() As String
        Get
            Return _LName
        End Get
    End Property

    Public ReadOnly Property FullName() As String
        Get
            Return _FName & " " & _LName
        End Get
    End Property

    ''' <summary>
    ''' User's email addr
    ''' </summary>
    Public ReadOnly Property EmailAddr() As String
        Get
            Return _EmailAddr
        End Get
    End Property

    Public ReadOnly Property AdsPerPage() As Integer
        Get
            Return 10
        End Get
    End Property


    ''' <summary>
    ''' Uses the user login level to return a small TreeTop object which identifies startup
    ''' parameters for the system immediately after login. This property is used by the system start pages
    ''' to slot the user into the home page tree structure according to his login level.
    ''' </summary>
    ''' <value>Loader object which specifies the start conditions for the supplied frame.</value>
    Public ReadOnly Property TreeTop() As Loader
        '
        ' returns the initial start conditions for each frame as a function of login level
        '
        Get
            Dim t As Loader = New Loader
            t.SlotID = _ID
            t.SelectedTab = 0
            t.Param1 = ATSystem.SysConstants.nullValue

            Select Case Me.Usr.LoginLevel

                Case Usr.LoginLevels.Guest
                    t.ObjectID = _UsrID
                    ''             t.ObjectType = ATSystem.ObjectTypes.Usr
                    ''       t.NextASPX = Loader.ASPX.TemplateSelector


                Case Usr.LoginLevels.INHProdn
                    ''                   t.ObjectID = Me.Usr.Division.Company.Dealer.SystemID
                    t.ObjectType = ATSystem.ObjectTypes.System
                    ''      t.NextASPX = Loader.ASPX.DealerListing

                Case Usr.LoginLevels.SysAdmin
                    ''                   t.ObjectID = Me.Usr.Division.Company.Dealer.SystemID
                    t.ObjectType = ATSystem.ObjectTypes.System
                    ''      t.NextASPX = Loader.ASPX.DealerListing

            End Select
            Return t
        End Get
    End Property


    ''' <summary>
    ''' SesisonID, from .Net. This is used to detect if the user is trying to re-enter
    ''' a session from a bookmark
    ''' </summary>
    ''' <value>SessionID</value>
    Public Property SessionID() As String
        Get
            Return _sessionID
        End Get

        Set(ByVal value As String)
            _sessionID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Skin - current site skin as perceived by this slot
    ''' </summary>
    ''' <value>SessionID</value>
    Public Property Skin() As String
        Get
            Return _skin
        End Get

        Set(ByVal value As String)
            _skin = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' StrParam1 - provides a session based mechanism to pass wtring fields between pages
    ''' a session from a bookmark
    ''' </summary>
    ''' <value>SessionID</value>
    Public Property StrParam1() As String
        Get
            Return _strParam1
        End Get

        Set(ByVal value As String)
            _strParam1 = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' SearchMode - defines whether the slot is browsing, keyword searching or full text searching
    ''' </summary>
    ''' <value>SearchMode</value>
    Public Property SearchMode() As SearchModes
        Get
            Return _searchMode
        End Get

        Set(ByVal value As SearchModes)
            _searchMode = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' SearchObjectType - defines whether the search is for categories or classifications
    ''' </summary>
    ''' <value>SearchObjectType</value>
    Public Property SearchObjectType() As ATSystem.ObjectTypes
        Get
            Return _searchObjectType
        End Get

        Set(ByVal value As ATSystem.ObjectTypes)
            _searchObjectType = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' SearchObjectID - either a class or category id, consistentent with searchObjectType
    ''' </summary>
    ''' <value>SearchObjectID</value>
    Public Property SearchObjectID() As Integer
        Get
            Return _searchObjectID
        End Get

        Set(ByVal value As Integer)
            _searchObjectID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' ProofObjectType - Used for filtering results in Proof Reader
    ''' </summary>
    Public Property ProofObjectType() As ATSystem.ObjectTypes
        Get
            Return _proofObjectType
        End Get

        Set(ByVal value As ATSystem.ObjectTypes)
            _proofObjectType = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' ProofObjectID - Used for filtering results in Proof Reader
    ''' </summary>
    Public Property ProofObjectID() As Integer
        Get
            Return _proofObjectID
        End Get

        Set(ByVal value As Integer)
            _proofObjectID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>SearhKey - what the user asks to search from
    ''' </summary>
    ''' <value>SearchKey</value>
    Public Property SearchKey() As String
        Get
            Return _searchKey
        End Get

        Set(ByVal value As String)
            _searchKey = value
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

    ''' <summary>
    ''' The UAM is a 32 bit boolean mask implemented as an integer. The bit definitions are identified in the Usr.UAMS. This property
    ''' further qualifies the user's login level, by providing up to 32 separate tests that can be made
    ''' to determine what he can do. The UAM is defined in the Usr object and is returned in the slot object
    ''' as a joined field from the user.
    ''' </summary>
    Public ReadOnly Property UAM() As Usr.UAMs
        Get
            Return _UAM
        End Get
    End Property

    ''' <summary>
    ''' User login level, returned as a joined field from the user.
    ''' </summary>
    Public ReadOnly Property LoginLevel() As Usr.LoginLevels
        Get
            Return _loginlevel
        End Get
    End Property

    ''' <summary>
    ''' Slot loginStatus, which identifies whether the slot is operational, logged out, or has been timed out.
    ''' </summary>
    ''' <value></value>
    Public Property LoginStatus() As LoginStates
        Get
            Return _loginStatus
        End Get
        Set(ByVal value As LoginStates)
            _loginStatus = value
            _ObjectStatus = ATSystem.ObjectState.Modified

        End Set
    End Property

    ''' <summary>
    ''' Slot Edition Visibility, copied from Usr object.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property EditionVisibility() As Edition.VisibleState
        Get
            Return _editionVisibility
        End Get
    End Property

    ''' <summary>
    ''' IP address of the browser client from where the user logged in
    ''' </summary>
    Public Property IPAddr() As String
        Get
            Return _IPAddr
        End Get
        Set(ByVal value As String)
            _IPAddr = value
            _ObjectStatus = ATSystem.ObjectState.Modified

        End Set
    End Property

    ''' <summary>
    ''' ID of the Usr object which owns this slot.
    ''' </summary>
    ''' <value></value>
    Public Property UsrID() As Integer
        Get
            Return _UsrID
        End Get
        Set(ByVal value As Integer)
            _UsrID = value
            _ObjectStatus = ATSystem.ObjectState.Modified

        End Set
    End Property

    ''' <summary>
    ''' ID of the Usr object which is being impersonated
    ''' </summary>
    ''' <value></value>
    Public Property ImpersonateUsrID() As Integer
        Get
            Return _ImpersonateUsrId
        End Get
        Set(ByVal value As Integer)
            _ImpersonateUsrID = value
            _ImpersonatedUsr = Nothing            'clear user for lazy reload
            _ObjectStatus = ATSystem.ObjectState.Modified

        End Set
    End Property

    ''' <summary>
    ''' ID of usr alias for proof filtering of ads.
    ''' </summary>
    ''' <value></value>
    Public Property AliasIDFilter() As Integer
        Get
            Return _AliasIDFilter
        End Get
        Set(ByVal value As Integer)
            _AliasIDFilter = value
            _ObjectStatus = ATSystem.ObjectState.Modified

        End Set
    End Property

    ''' <summary>
    ''' Determines order of retrieved ads for proof reader.
    ''' </summary>
    ''' <value></value>
    Public Property AdSortOrder() As Ad.SortOrders
        Get
            Return _AdSortOrder
        End Get
        Set(ByVal value As Ad.SortOrders)
            _AdSortOrder = value
            _ObjectStatus = ATSystem.ObjectState.Modified

        End Set
    End Property

    ''' <summary>
    ''' Determines user type displayed in user listing.
    ''' </summary>
    ''' <value></value>
    Public Property UserTypeFilter() As Usr.LoginLevels
        Get
            Return _userTypeFilter
        End Get
        Set(ByVal value As Usr.LoginLevels)
            _userTypeFilter = value
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
    ''' Usr Object which owns this slot.
    ''' </summary>
    ''' <value></value>
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
    ''' Usr Object which owns this slot.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property ImpersonatedUsr() As Usr
        Get
            If _ImpersonatedUsr Is Nothing Then
                Dim Usrs As New Usrs
                Usrs.ConnectionString = _connectionString
                _ImpersonatedUsr = Usrs.Retrieve(_ImpersonateUsrID)
            End If
            Return _ImpersonatedUsr
        End Get

    End Property


    Private Function getConnection() As SqlConnection
        If _connectionString Is Nothing Then _connectionString = ConfigurationManager.ConnectionStrings("ATConnectionString").ConnectionString
        Return New SqlConnection(_connectionString)
    End Function


    ''' <summary>
    ''' This function performs user login validation and if successful creates a new slot object.
    ''' The function return value indicates whether a slot record was written, and if so, its ID will be
    ''' supplied through the ID property.
    ''' </summary>
    ''' <param name="EmailAddr">User login name</param>
    ''' <param name="Password">User password</param>
    ''' <returns>Result of login attempt.</returns>
    Public Function Login(ByVal EmailAddr As String, ByVal Password As String) As LoginStates
        '
        ' performs the login function
        ' precheck that a login name is supplied
        '
        Dim myEmailAddr As String = EmailAddr.Trim
        If myEmailAddr = "" Then Return LoginStates.InvalidUserName


        Dim rtnval As LoginStates = LoginStates.Undefined
        Dim usrs As New Usrs
        Dim usr As Usr
        usrs.RetrieveByEmailAddr(myEmailAddr)
        '
        ' validate login parameters
        '
        If usrs.Count = 0 Then
            rtnval = LoginStates.InvalidUserName
        Else
            usr = usrs(0)
            If usr.Password <> Password Then
                rtnval = LoginStates.InvalidPassword
            Else
                _loginStatus = LoginStates.Active  'show slot as active
                _Usr = usr            'plug usr object
                _UsrID = usr.ID
                _ImpersonateUsrId = usr.ID      'no impersonation yet
                _AliasIDFilter = ATSystem.SysConstants.nullValue  'no alias filtering yet
                _AdSortOrder = Ad.SortOrders.AdNumber
                _userTypeFilter = ATLib.Usr.LoginLevels.Unspecified
                _strParam1 = ""
                _skin = _Usr.Skin
                '
                ' plug initial user info to new slot
                '
                _FName = usr.FName
                _LName = usr.LName
                _EmailAddr = usr.EmailAddr
                _loginlevel = usr.LoginLevel
                _editionVisibility = usr.EditionVisibility
                _UAM = usr.UAM
                _searchMode = SearchModes.Browse
                _searchKey = ""
                _searchObjectType = ATSystem.ObjectTypes.Category
                _searchObjectID = ATSystem.SysConstants.nullValue
                _proofObjectType = ATSystem.ObjectTypes.System
                _proofObjectID = ATSystem.SysConstants.nullValue
                
                Update()               'create new slot and return

                rtnval = LoginStates.Active
            End If
        End If
        Return rtnval
    End Function

    ''' <summary>
    ''' Logs the user out by setting the slot state to logout.
    ''' </summary>
    Public Sub Logout()
        LoginStatus = LoginStates.Loggedout
        Update()
    End Sub

    ''' <summary>
    ''' This method is available to forceably terminate the slot.
    ''' </summary>
    ''' <param name="value"></param>
    Public Sub Terminate(ByVal value As LoginStates)
        LoginStatus = value
        Update()
    End Sub

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

    ''' <summary>
    ''' Touches the slot modify timestamp, used to control slot timeout
    ''' </summary>
    Public Sub Touch()
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.CommandText = "UPDATE dbo.Slot SET modifyTime=getdate() WHERE ID=@ID"
        addParams(Cmd)

        Try
            Cmd.Connection.Open()
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
    End Sub

    Private Function doUpdate() As Integer

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.CommandText = "UPDATE dbo.Slot SET " & _
        "modifyTime=getdate()," & _
        "Status=@Status," & _
        "UsrID=@UsrID," & _
        "ImpersonateUsrID=@ImpersonateUsrID," & _
        "AliasIDFilter=@AliasIDFilter," & _
        "AdSortOrder=@AdSortOrder," & _
        "UserTypeFilter=@UserTypeFilter," & _
        "SessionID=@SessionID," & _
        "Skin=@Skin," & _
        "StrParam1=@StrParam1," & _
        "SearchMode=@SearchMode," & _
        "SearchKey=@SearchKey," & _
        "SearchObjectID=@SearchObjectID," & _
        "SearchObjectType=@SearchObjectType," & _
        "ProofObjectID=@ProofObjectID," & _
        "ProofObjectType=@ProofObjectType," & _
        "IPAddr=@IPAddr," & _
        "loginStatus=@loginStatus" & _
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
        Cmd.CommandText = "INSERT INTO dbo.Slot " & _
        "(UsrID,ImpersonateUsrID,AliasIDFilter,AdSortOrder,UserTypeFilter,Status,createTime,modifyTime,loginStatus,SessionID," & _
        "SearchMode,SearchObjectType,SearchObjectID,SearchKey,ProofObjectType,ProofObjectID," & _
        "Skin,StrParam1,IPAddr) " & _
        "VALUES (@UsrID,@ImpersonateUsrID,@AliasIDFilter,@AdSortOrder,@UserTypeFilter,@Status,getdate(),getdate(),@loginStatus,@SessionID," & _
        "@SearchMode,@SearchObjectType,@SearchObjectID,@SearchKey,@ProofObjectType,@ProofObjectID," & _
        "@Skin,@StrParam1,@IPAddr)" & _
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
        Cmd.Parameters.Add("@UsrID", SqlDbType.Int).Value = _UsrID
        Cmd.Parameters.Add("@ImpersonateUsrID", SqlDbType.Int).Value = _ImpersonateUsrId
        Cmd.Parameters.Add("@AliasIDFilter", SqlDbType.Int).Value = _AliasIDFilter
        Cmd.Parameters.Add("@AdSortOrder", SqlDbType.Int).Value = _AdSortOrder
        Cmd.Parameters.Add("@UserTypeFilter", SqlDbType.Int).Value = _userTypeFilter
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@SearchMode", SqlDbType.Int).Value = _searchMode
        Cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar).Value = _searchKey
        Cmd.Parameters.Add("@SearchObjectType", SqlDbType.Int).Value = _searchObjectType
        Cmd.Parameters.Add("@SearchObjectID", SqlDbType.Int).Value = _searchObjectID
        Cmd.Parameters.Add("@ProofObjectType", SqlDbType.Int).Value = _proofObjectType
        Cmd.Parameters.Add("@ProofObjectID", SqlDbType.Int).Value = _proofObjectID
         Cmd.Parameters.Add("@SessionID", SqlDbType.VarChar).Value = _sessionID
        Cmd.Parameters.Add("@Skin", SqlDbType.VarChar).Value = _skin
        Cmd.Parameters.Add("@StrParam1", SqlDbType.VarChar).Value = _strParam1
        Cmd.Parameters.Add("@IPAddr", SqlDbType.VarChar).Value = _IPAddr
        Cmd.Parameters.Add("@loginStatus", SqlDbType.Int, 0).Value = _loginStatus
    End Sub


    Private Function doDelete() As Integer

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.CommandText = "DELETE dbo.Slot WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID

        Try
            Cmd.Connection.Open()
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return ATSystem.SysConstants.nullValue

    End Function

    Friend Sub DR2Object(ByVal dr As IDataRecord)
        _ObjectStatus = ATSystem.ObjectState.Original
        _ID = Convert.ToInt32(dr("ID"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _loginStatus = CType(dr("loginStatus"), LoginStates)
        _UsrID = Convert.ToInt32(dr("UsrID"))
        _ImpersonateUsrId = Convert.ToInt32(dr("ImpersonateUsrID"))
        _AliasIDFilter = Convert.ToInt32(dr("AliasIDFilter"))
        _AdSortOrder = CType(dr("AdSortOrder"), Ad.SortOrders)
        _userTypeFilter = CType(dr("UserTypeFilter"), Usr.LoginLevels)
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _sessionID = Convert.ToString(dr("SessionID"))
        _skin = Convert.ToString(dr("Skin"))
        _strParam1 = Convert.ToString(dr("Strparam1"))
        _IPAddr = Convert.ToString(dr("IPAddr"))
        _searchMode = CType(dr("SearchMode"), SearchModes)
        _searchObjectType = CType(dr("SearchObjectType"), ATSystem.ObjectTypes)
        _searchObjectID = Convert.ToInt32(dr("SearchObjectID"))
        _searchKey = Convert.ToString(dr("SearchKey"))
        _proofObjectType = CType(dr("ProofObjectType"), ATSystem.ObjectTypes)
        _proofObjectID = Convert.ToInt32(dr("ProofObjectID"))
        '
        ' readonly properties derived from joined usr table
        '
        _FName = Convert.ToString(dr("FName"))
        _LName = Convert.ToString(dr("LName"))
        _EmailAddr = Convert.ToString(dr("EmailAddr"))
        _loginlevel = CType(dr("LoginLevel"), Usr.LoginLevels)
        _UAM = CType(dr("UAM"), Usr.UAMs)
        _editionVisibility = CType(dr("EditionVisibility"), Edition.VisibleState)

    End Sub


End Class



