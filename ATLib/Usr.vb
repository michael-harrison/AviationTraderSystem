Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.ComponentModel
Imports System


'***************************************************************************************
'*
'* Usrs
'*
'* AUDIT TRAIL
'* 
'* V1.000   31-AUG-2009  BA  Original
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The Usrs Collection is used to hold the set of Usr
''' objects which are retrieved from the database by any of the overloaded Retrieve
''' methods. The collection is normally accessed by using the Usrs property of the parent System object. This class manages the Usr database table. Any object in the collection can be modified and then written back to the database,
'''  by either the Update method of the collection class, or the Update method of each object within the collection.
''' </para>
''' </summary>
Public Class Usrs : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Usrs(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...usrs.count-1</param>
    ''' <value>Usr object from Usrs collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Usr
        Get
            Return CType(List(index), Usr)
        End Get
        Set(ByVal value As Usr)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a Usr object to the Usrs collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Usr object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As Usr) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Usr) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Usr)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Usr)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Usr) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.Usr ORDER BY AcctAlias"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Usr
        Return Retrieve(Hex2Int(HexID))
    End Function


    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="ID">ObjectID, represented as an integer</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal ID As Integer) As Usr
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Usr WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function

    Public Sub retrieveSet(ByVal systemID As Integer)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "SELECT * from dbo.Usr WHERE SystemID=@ID ORDER BY AcctAlias"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = systemID
        doRetrieveR(Cmd)

    End Sub

    Public Sub RetrieveFilteredSet(ByVal systemID As Integer, ByVal Selector As Usr.Selectors, ByVal userType As Usr.LoginLevels)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        '
        ' make up the selection criteria
        '
        Dim selectStr As String = ""
        Select Case Selector
            Case Usr.Selectors.NewUsr : selectStr = "AcctAlias=''"
            Case Usr.Selectors.AD : selectStr = "(AcctAlias like 'A%' or AcctAlias like 'B%' or AcctAlias like 'C%' or AcctAlias like 'D%')"
            Case Usr.Selectors.EH : selectStr = "(AcctAlias like 'E%' or AcctAlias like 'F%' or AcctAlias like 'G%' or AcctAlias like 'H%')"
            Case Usr.Selectors.IL : selectStr = "(AcctAlias like 'I%' or AcctAlias like 'J%' or AcctAlias like 'K%' or AcctAlias like 'L%')"
            Case Usr.Selectors.MP : selectStr = "(AcctAlias like 'M%' or AcctAlias like 'N%' or AcctAlias like 'O%' or AcctAlias like 'P%')"
            Case Usr.Selectors.QT : selectStr = "(AcctAlias like 'Q%' or AcctAlias like 'R%' or AcctAlias like 'S%' or AcctAlias like 'T%')"
            Case Usr.Selectors.UZ : selectStr = "(AcctAlias like 'U%' or AcctAlias like 'V%' or AcctAlias like 'W%' or AcctAlias like 'X%' or AcctAlias like 'Y%' or AcctAlias like 'Z%')"
        End Select

        If userType <> Usr.LoginLevels.Unspecified Then
            selectStr &= " AND LoginLevel=@LoginLevel"
            Cmd.Parameters.Add("@LoginLevel", SqlDbType.Int).Value = userType
        End If

        Cmd.CommandText = "SELECT * from dbo.Usr WHERE SystemID=@ID AND " & selectStr & " ORDER BY AcctAlias"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = systemID
        doRetrieveR(Cmd)

    End Sub

    ''' <summary>
    ''' returns the list of aliases
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <remarks></remarks>
    Public Function GetAliasList(ByVal Key As String) As String()
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "Select Distinct AcctAlias FROM dbo.Usr Where AcctAlias like @Key ORDER BY AcctAlias"
        Cmd.Parameters.Add("@Key", SqlDbType.VarChar).Value = Key & "%"
        Dim rtnval As New List(Of String)
        Dim dr As SqlDataReader = Nothing
        Cmd.Connection.Open()
        Try
            dr = Cmd.ExecuteReader()
            While dr.Read
                rtnval.Add(Convert.ToString(dr("AcctAlias")))
            End While
        Finally
            If Not dr Is Nothing Then
                dr.Close()
                Cmd.Connection.Dispose()
            End If
        End Try
        Return rtnval.ToArray
    End Function



    ''' <summary>
    ''' Retrieves the user specified by the login name. Login Name has system-wide scope
    ''' so only one user or maybe no user is retrieved by this call. The success of the call 
    ''' can be tested by the Usrs.Count property.
    ''' </summary>
    ''' <param name="EmailAddr">login name for user to retrieve.</param>
    Public Sub RetrieveByEmailAddr(ByVal EmailAddr As String)
        '
        ' Retrieves the unique user by Name address
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Usr WHERE EmailAddr=@EmailAddr"
        Cmd.Parameters.Add("@EmailAddr", SqlDbType.VarChar).Value = EmailAddr
        doRetrieveR(Cmd)
    End Sub


    ''' <summary>
    ''' Retrieves the user specified by the login name. Login Name has system-wide scope
    ''' so only one user or maybe no user is retrieved by this call. The success of the call 
    ''' can be tested by the Usrs.Count property.
    ''' </summary>
    ''' <param name="AcctAlias">Account Alias.</param>
    Public Sub RetrieveByAcctAlias(ByVal AcctAlias As String)
        '
        ' Retrieves the set of usrs (should be only one, or none) by AcctAllias fieldddress
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.Usr WHERE AcctAlias=@AcctAlias"
        Cmd.Parameters.Add("@AcctAlias", SqlDbType.VarChar).Value = AcctAlias
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
                Dim Usr As New Usr
                Add(Usr)
                Usr.ConnectionString = _connectionString
                Usr.DR2Object(dr)
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
        Dim deletedObjects As New List(Of Usr)

        For Each Usr As Usr In List
            Usr.Update()
            If Usr.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Usr)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Usr In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class



'***************************************************************************************
'*
'* Usr - Usr object
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
''' <para> The Usr object holds all data retrieved from a specified row of the Usr table.
'''</para>
''' <para>It is always accessed as part of the Usrs collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Usrs collection
'''  property of the parent. 
''' </para>
''' </summary>
Public Class Usr

 

    Private _connectionString As String
    Private _ID As Integer
    Private _systemID As Integer
    Private _EmailAddr As String

    Private _Company As String
    Private _ACN As String
    Private _AcctAlias As String
    Private _FName As String
    Private _LName As String
    Private _Addr1 As String
    Private _Addr2 As String
    Private _Suburb As String
    Private _Postcode As String
    Private _State As String
    Private _Countrycode As String
    Private _phone As String
    Private _AHPhone As String
    Private _Mobile As String
    Private _Fax As String
    Private _Skin As String
    Private _website As String
    Private _discount As Integer

    Private _IdentVisible As IdentBits

    Private _loginLevel As LoginLevels
    Private _editionVisibility As Edition.VisibleState
    Private _UAM As UAMs
    Private _password As String
    Private _navtarget As String
    Private _createTime As Date
    Private _modifyTime As Date
    Private _ObjectStatus As ATSystem.ObjectState
    Private _status As ATSystem.StatusBits
    Private _adCount As Integer
    '
    ' embedded collections
    '
    Private _system As ATSystem
    Private _slots As Slots
    Private _ads As Ads
    Private _productEditionInstances As AdInstances

    Private _adProdnStatus As Ad.ProdnState
    Private _instanceEditionID As Integer
    Private _instanceProductID As Integer

    ''' <summary> Defines the login authority level of the user. Note that
    ''' the precedence should follow the authority hierarchy, as the application code
    ''' inludes statements such as if usr.loginlevel &gt; loginlevels.company... which
    ''' would select dealer, super user and sysadmin</summary>
    ''' <includesource>yes</includesource>
    Public Enum LoginLevels
        '
        ' Defines which node in the navigation hierarchy the user comes in at
        '
        <Description("All Users")> Unspecified = 8
        <Description("System Administrator")> SysAdmin = 7
        <Description("Inhouse Production")> INHProdn = 6
        <Description("Advertiser with Subscription")> AdvSub = 5
        <Description("Advertiser")> Advertiser = 4
        <Description("Subscriber")> Subscriber = 3
        <Description("Registered Reader")> RegisteredReader = 2
        <Description("Guest")> Guest = 1
    End Enum

    Public Enum Selectors
        <Description("New User")> NewUsr = 0
        <Description("A-D")> AD = 1
        <Description("E-H")> EH = 2
        <Description("I-L")> IL = 3
        <Description("M-P")> MP = 4
        <Description("Q-T")> QT = 5
        <Description("U-Z")> UZ = 6
    End Enum



    ''' <summary>
    ''' UAMs (User Authority Mask) is a set of 32 bits which further qualifies the user's login level, to more granularly define what types of operations he can do. The UAM field in the user object is 
    ''' constracted as the OR of these bits. Individual bits can be tested within the UAM by ANDing the bit calue with the contents of the UAM.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum UAMs
        <Description("Can access Setup and create new users/Systems/companies/dealers")> Constructor = &H1
        <Description("Can access dealer setup tab")> SetupAccess = &H800
    End Enum


    Public Enum IdentBits
        Email = &H1
        FName = &H2
        LName = &H4
        Company = &H8
        Phone = &H10
        AHPhone = &H20
        Mobile = &H40
        Fax = &H80
        Addr = &H100
        ACN = &H200
        AcctAlias = &H400
        Website = &H800
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


    ''' <summary>This is a memory-only property which is not written to the database.
    ''' Its plugged by the caller afte the object is retrieved to prime it with the
    ''' target URL of the page to load if the object is clicked. This is done before
    ''' databinding, so when the object is databound to a control, the click URL is
    ''' exposed to the control as a bindable property of the object.</summary>
    ''' <returns>String value, as primed by caller</returns>
    Public Property NavTarget() As String
        Get
            Return _navtarget
        End Get
        Set(ByVal value As String)
            _navtarget = value

        End Set
    End Property

    ''' <summary>Object EmailAddr. When this property is set, the name is truncated to 255
    ''' characters and sanitized to remove invalid characters. See
    ''' CommonRoutines.Sanitize. If scope-specific uniqueness of Name is required on
    ''' input, test the proposed name first by calling IsNameUnique</summary>
    ''' <returns>Object Name</returns>
    Public Property EmailAddr() As String
        Get
            Return _EmailAddr
        End Get
        Set(ByVal value As String)
            _EmailAddr = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the user first name.
    ''' </summary>
    Public Property FName() As String
        Get
            Return _FName
        End Get
        Set(ByVal value As String)
            _FName = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the user last name.
    ''' </summary>
    Public Property LName() As String
        Get
            Return _LName
        End Get
        Set(ByVal value As String)
            _LName = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the user address line 1.
    ''' </summary>
    Public Property Addr1() As String
        Get
            Return _Addr1
        End Get
        Set(ByVal value As String)
            _Addr1 = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the user address line 2.
    ''' </summary>
    Public Property Addr2() As String
        Get
            Return _Addr2
        End Get
        Set(ByVal value As String)
            _Addr2 = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the user address line 2.
    ''' </summary>
    Public Property Suburb() As String
        Get
            Return _Suburb
        End Get
        Set(ByVal value As String)
            _Suburb = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the user address line 2.
    ''' </summary>
    Public Property State() As String
        Get
            Return _State
        End Get
        Set(ByVal value As String)
            _State = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the user address line 2.
    ''' </summary>
    Public Property Postcode() As String
        Get
            Return _Postcode
        End Get
        Set(ByVal value As String)
            _Postcode = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the user address line 2.
    ''' </summary>
    Public Property Countrycode() As String
        Get
            Return _Countrycode
        End Get
        Set(ByVal value As String)
            _Countrycode = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Returns the full country name via ea
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Country() As String
        Get
            Dim ea As New EnumAssistant(New ATSystem.countrycodes)
            For Each ei As EnumItem In ea
                If ei.Name = _Countrycode Then
                    Return ei.Description
                End If
            Next
            Return ""
        End Get
    End Property


    ''' <summary>
    ''' Company
    ''' </summary>
    Public Property Company() As String
        Get
            Return _Company
        End Get
        Set(ByVal value As String)
            _Company = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Company
    ''' </summary>
    Public Property ACN() As String
        Get
            Return _ACN
        End Get
        Set(ByVal value As String)
            _ACN = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Company
    ''' </summary>
    Public Property AcctAlias() As String
        Get
            Return _AcctAlias
        End Get
        Set(ByVal value As String)
            _AcctAlias = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Phone
    ''' </summary>
    Public Property Phone() As String
        Get
            Return _phone
        End Get
        Set(ByVal value As String)
            _phone = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' AHPhone
    ''' </summary>
    Public Property AHPhone() As String
        Get
            Return _AHPhone
        End Get
        Set(ByVal value As String)
            _AHPhone = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Mobile
    ''' </summary>
    Public Property Mobile() As String
        Get
            Return _Mobile
        End Get
        Set(ByVal value As String)
            _Mobile = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Company
    ''' </summary>
    Public Property Fax() As String
        Get
            Return _Fax
        End Get
        Set(ByVal value As String)
            _Fax = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Site skin, as chosen by this user
    ''' </summary>
    Public Property Skin() As String
        Get
            Return _Skin
        End Get
        Set(ByVal value As String)
            _Skin = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' web site
    ''' </summary>
    Public Property WebSite() As String
        Get
            Return _website
        End Get
        Set(ByVal value As String)
            _website = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' web site
    ''' </summary>
    Public Property Discount() As Integer
        Get
            Return _discount
        End Get
        Set(ByVal value As Integer)
            _discount = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Defines whether user is gst exempt.
    ''' </summary>
    Public Property IsGSTExempt() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsGSTExempt And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsGSTExempt
            Else
                _status = _status And Not ATSystem.StatusBits.IsGSTExempt
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    Public Property IsDisplayFName() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.FName And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.FName
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.FName
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayEmail() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.Email And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.Email
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.Email
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayWebsite() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.Website And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.Website
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.Website
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayLName() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.LName And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.LName
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.LName
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayPhone() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.Phone And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.Phone
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.Phone
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayAHPhone() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.AHPhone And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.AHPhone
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.AHPhone
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayMobile() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.Mobile And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.Mobile
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.Mobile
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayFax() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.Fax And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.Fax
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.Fax
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayCompany() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.Company And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.Company
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.Company
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayAcctAlias() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.AcctAlias And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.AcctAlias
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.AcctAlias
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayACN() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.ACN And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.ACN
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.ACN
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property IsDisplayAddr() As Boolean
        Get
            Return Convert.ToBoolean(IdentBits.Addr And _IdentVisible)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _IdentVisible = _IdentVisible Or IdentBits.Addr
            Else
                _IdentVisible = _IdentVisible And Not IdentBits.Addr
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Returns the full user name
    ''' that the user is marked as the reference user.
    ''' </summary>
    ''' <value>Name like Fred or Fred (Ref)</value>
    Public ReadOnly Property FullName() As String

        Get

            Return _FName & " " & _LName
        End Get
    End Property

    ''' <summary>
    ''' Returns the full user name, qualified by whether the fname and lname are  required
    ''' that the user is marked as the reference user.
    ''' </summary>
    ''' <value>Name like Fred or Fred (Ref)</value>
    Public ReadOnly Property VisibleFullName() As String

        Get
            Dim visibility As IdentBits = (IdentBits.FName Or IdentBits.LName) And _IdentVisible
            Select Case visibility
                Case IdentBits.FName : Return _FName
                Case IdentBits.LName : Return _LName
                Case IdentBits.FName Or IdentBits.LName : Return _FName & " " & _LName
                Case Else : Return ""
            End Select

        End Get
    End Property

    ''' <summary>
    ''' Returns the full user name formatted for HTML
    ''' that the user is marked as the reference user.
    ''' </summary>
    ''' <value>Name like Fred or Fred (Ref)</value>
    Public ReadOnly Property HTMLAddress() As String

        Get
            Dim r As String = "<br />"
            Dim sp As String = " "
            Dim x As String


            Dim s As String = ""
            If _Addr1.Length > 0 Then s &= _Addr1 & r
            If _Addr2.Length > 0 Then s &= _Addr2 & r
            x = _Suburb & sp & _State & sp & _Postcode
            x = x.Trim()
            If x.Length > 0 Then s &= x & r
            s &= Country
            Return s

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

    Public Property IdentVisible() As IdentBits
        Get
            Return _IdentVisible
        End Get

        Set(ByVal value As IdentBits)
            _IdentVisible = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>
    ''' Specifies the user's password.
    ''' </summary>
    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
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
                _ads.RetrieveSet(ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, _ID, ATSystem.SysConstants.nullValue, ProdnStatus, Ad.SortOrders.Keywords)
            End If
            Return _ads
        End Get
    End Property

    Public ReadOnly Property AdCount() As Integer
        Get
            If _adCount = -1 Then
                Dim Ads As New Ads
                Ads.ConnectionString = _connectionString
                _adCount = Ads.GetAdCount(ATSystem.ObjectTypes.Usr, _ID)
            End If
            Return _adCount
        End Get
    End Property

    Public ReadOnly Property AdInstances() As AdInstances
        Get
            Return AdInstances(ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue)
        End Get
    End Property


    Public ReadOnly Property AdInstances(ByVal ProductID As Integer) As AdInstances
        Get
            Return AdInstances(ProductID, ATSystem.SysConstants.nullValue)
        End Get
    End Property

    ''' <summary>
    ''' Embedded collection of Ad Instance objects, which are assoicated with this Ad
    ''' </summary>
    Public ReadOnly Property AdInstances(ByVal ProductID As Integer, ByVal EditionID As Integer) As AdInstances
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
                _productEditionInstances.RetrieveSet(_ID, ATSystem.SysConstants.nullValue, ATSystem.SysConstants.nullValue, EditionID, ProductID)

            End If
            Return _productEditionInstances
        End Get
    End Property

    ''' <summary>
    ''' The UAM is a 32 bit boolean mask implemented as an integer. This property
    ''' further qualifies the user's login level, by providing up to 32 separate tests that can be made
    ''' to determine what he can do.
    ''' </summary>
    Public Property UAM() As UAMs
        Get
            Return _UAM
        End Get
        Set(ByVal value As UAMs)
            _UAM = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the login authority of the user.
    ''' </summary>
    Public Property LoginLevel() As LoginLevels
        Get
            Return CType(_loginLevel, LoginLevels)
        End Get
        Set(ByVal value As LoginLevels)
            _loginLevel = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Specifies the login authority of the user.
    ''' </summary>
    Public Property EditionVisibility() As Edition.VisibleState
        Get
            Return CType(_editionVisibility, Edition.VisibleState)
        End Get
        Set(ByVal value As Edition.VisibleState)
            _editionVisibility = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Returns a human readable string description of the login level property in a form suitable for databinding.
    ''' </summary>
    Public ReadOnly Property LoginLevelDescr() As String
        Get
            Dim EA As New EnumAssistant(New Usr.LoginLevels, _loginLevel, _loginLevel)
            Return EA(0).Name
        End Get
    End Property

    ''' <summary>Parent SystemID to which this object is subordinate, which is the identity value of the database Primary Key, represented as a 32 bit integer</summary>
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

    ''' <summary>
    ''' The System object which owns this user.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property System() As ATSystem
        Get
            If _system Is Nothing Then
                _system = New ATSystem
                _system.ConnectionString = _connectionString
                _system.Retrieve()
            End If
            Return _system
        End Get
    End Property

    ''' <summary>
    ''' Embedded collection of slots, representing all of the (unpurged) login sessions that this user has executed.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property Slots() As Slots
        Get
            If _slots Is Nothing Then
                _slots = New Slots
                _slots.ConnectionString = _connectionString
                _slots.retrieveSet(_ID)
            End If
            Return _slots
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

    ''' <summary> Checks that the name is unique for a parent within the current
    ''' hierarchy scope. This can differ for different objects. See remarks below
    ''' Returns true if the name is unique, false if it exists wtihin the
    ''' scope</summary>
    ''' <remarks>The scope of uniqueness for the user object is system-wide. That is,
    ''' this test enforces the rule that each user must have a unique name, irrespective
    ''' of what dealer-System-company the user has been created for. From an
    ''' implementation perspective the easiest way of ensuring this is by requiring that
    ''' user names are email addresses, but this rule is not implemented here.</remarks>
    ''' <param name="EmailAddr">The name to check</param>
    ''' <returns>True if the name is unique, false otherwise</returns>
    Public Function IsEmailAddrUnique(ByVal EmailAddr As String) As Boolean

        Dim rtnval As Boolean = False           'assume not unique

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT COUNT(ID) From dbo.Usr WHERE EmailAddr=@EmailAddr AND ID<>@ID"
        Cmd.Parameters.Add("@EmailAddr", SqlDbType.VarChar).Value = EmailAddr
        Cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = _ID

        Try
            Cmd.Connection.Open()
            If Convert.ToInt32(Cmd.ExecuteScalar()) = 0 Then rtnval = True
        Finally
            Cmd.Connection.Dispose()          'close and dispose connection
        End Try
        Return rtnval

    End Function

    ''' <summary>
    ''' Deletes all the ads for the user
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DeleteAds()
        Dim ads As New Ads
        ads.DeleteSet(ATSystem.ObjectTypes.Usr, _ID)

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



    Private Function doUpdate() As Integer


        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.CommandText = "UPDATE dbo.Usr SET " & _
        "modifyTime=getdate()," & _
        "SystemID=@SystemID," & _
        "UAM=@UAM," & _
        "Discount=@Discount," & _
        "Status=@Status," & _
        "Skin=@Skin," & _
        "Website=@Website," & _
        "IdentVisible=@IdentVisible," & _
        "LoginLevel=@LoginLevel," & _
        "EditionVisibility=@EditionVisibility," & _
        "Addr1=@Addr1," & _
        "Addr2=@Addr2," & _
        "Suburb=@Suburb," & _
        "State=@State," & _
        "Postcode=@Postcode," & _
        "Countrycode=@Countrycode," & _
        "Company=@Company," & _
        "ACN=@ACN," & _
        "AcctAlias=@AcctAlias," & _
        "Phone=@Phone," & _
        "AHPhone=@AHPhone," & _
        "Mobile=@Mobile," & _
        "Fax=@Fax," & _
        "EmailAddr=@EmailAddr," & _
        "FName=@FName," & _
        "LName=@LName," & _
        "Password=@Password" & _
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
        Cmd.CommandText = "INSERT INTO dbo.Usr " & _
        "(SystemID,CreateTime,ModifyTime,Status,IdentVisible,UAM,Discount,LoginLevel,EditionVisibility,Skin,Website,EmailAddr,FName,LName,Password," & _
        "Addr1,Addr2,Suburb,State,Postcode,Countrycode," & _
        "Company,ACN,AcctAlias,Phone,AHPhone,Mobile,Fax)" & _
        " VALUES (@SystemID,getdate(),getdate(),@Status,@IdentVisible,@UAM,@Discount,@LoginLevel,@EditionVisibility,@Skin,@Website,@EmailAddr,@FName,@LName,@Password," & _
        "@Addr1,@Addr2,@Suburb,@State,@Postcode,@Countrycode," & _
        "@Company,@ACN,@AcctAlias,@Phone,@AHPhone,@Mobile,@Fax)" & _
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
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status
        Cmd.Parameters.Add("@IdentVisible", SqlDbType.Int).Value = _IdentVisible
        Cmd.Parameters.Add("@SystemID", SqlDbType.Int).Value = _systemID
        Cmd.Parameters.Add("@UAM", SqlDbType.Int, 0, "UAM").Value = _UAM
        Cmd.Parameters.Add("@Discount", SqlDbType.Int, 0, "Discount").Value = _discount
        Cmd.Parameters.Add("@Skin", SqlDbType.VarChar).Value = _Skin
        Cmd.Parameters.Add("@Website", SqlDbType.VarChar).Value = _website
        Cmd.Parameters.Add("@LoginLevel", SqlDbType.Int).Value = _loginLevel
        Cmd.Parameters.Add("@EditionVisibility", SqlDbType.Int).Value = _editionVisibility
        Cmd.Parameters.Add("@EmailAddr", SqlDbType.VarChar).Value = _EmailAddr
        Cmd.Parameters.Add("@Company", SqlDbType.VarChar).Value = _Company
        Cmd.Parameters.Add("@ACN", SqlDbType.VarChar).Value = _ACN
        Cmd.Parameters.Add("@AcctAlias", SqlDbType.VarChar).Value = _AcctAlias
        Cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = _phone
        Cmd.Parameters.Add("@AHPhone", SqlDbType.VarChar).Value = _AHPhone
        Cmd.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = _Mobile
        Cmd.Parameters.Add("@Fax", SqlDbType.VarChar).Value = _Fax
        Cmd.Parameters.Add("@FName", SqlDbType.VarChar).Value = _FName
        Cmd.Parameters.Add("@LName", SqlDbType.VarChar).Value = _LName
        Cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = _password
        Cmd.Parameters.Add("@Addr1", SqlDbType.VarChar).Value = _Addr1
        Cmd.Parameters.Add("@Addr2", SqlDbType.VarChar).Value = _Addr2
        Cmd.Parameters.Add("@Suburb", SqlDbType.VarChar).Value = _Suburb
        Cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = _State
        Cmd.Parameters.Add("@Postcode", SqlDbType.VarChar).Value = _Postcode
        Cmd.Parameters.Add("@Countrycode", SqlDbType.VarChar).Value = _Countrycode

    End Sub



    ''' <summary>Physically deletes the object and subordinate structure from the db
    '''</summary> 
    Public Function doDelete() As Integer

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        '
        ' delete embedded collections first
        '
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID
        Try
            Cmd.Connection.Open()

            Cmd.CommandText = "DELETE dbo.Ad WHERE usrID=@ID"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.Slot WHERE UsrID=@ID"
            Cmd.ExecuteNonQuery()

            '
            ' finally delete the user
            '
            Cmd.CommandText = "DELETE dbo.Usr WHERE ID=@ID"
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
        _loginLevel = CType(dr("LoginLevel"), LoginLevels)
        _editionVisibility = CType(dr("EditionVisibility"), Edition.VisibleState)
        _UAM = CType(dr("UAM"), UAMs)
        _discount = Convert.ToInt32(dr("Discount"))
        _Skin = Convert.ToString(dr("Skin"))
        _website = Convert.ToString(dr("Website"))
        _EmailAddr = Convert.ToString(dr("EmailAddr"))
        _Company = Convert.ToString(dr("Company"))
        _ACN = Convert.ToString(dr("ACN"))
        _AcctAlias = Convert.ToString(dr("AcctAlias"))
        _phone = Convert.ToString(dr("Phone"))
        _AHPhone = Convert.ToString(dr("AHPhone"))
        _Mobile = Convert.ToString(dr("Mobile"))
        _Fax = Convert.ToString(dr("Fax"))
        _FName = Convert.ToString(dr("FName"))
        _LName = Convert.ToString(dr("LName"))
        _Addr1 = Convert.ToString(dr("Addr1"))
        _Addr2 = Convert.ToString(dr("Addr2"))
        _Suburb = Convert.ToString(dr("Suburb"))
        _Postcode = Convert.ToString(dr("Postcode"))
        _State = Convert.ToString(dr("State"))
        _Countrycode = Convert.ToString(dr("Countrycode"))
        _password = Convert.ToString(dr("Password")) '
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _IdentVisible = CType(dr("Identvisible"), IdentBits)
    End Sub

End Class

