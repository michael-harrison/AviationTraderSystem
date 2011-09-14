Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Configuration
Imports System


'***************************************************************************************
'*
'* RotatorAds
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The RotatorAds collection is used to hold the set of RotatorAd
''' objects. The collection is normally accessed by using the RotatorAds property of the parent System object.
'''  This class manages the RotatorAd database table. Any object in the collection can be modified and then written back to the database,
'''  by either the Update method of the collection class, or the Update method of each object within the collection.
''' </para>
''' </summary>
Public Class RotatorAds : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG RotatorAds(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...RotatorAds.count-1</param>
    ''' <value>RotatorAd object from RotatorAds collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As RotatorAd
        Get
            Return CType(List(index), RotatorAd)
        End Get
        Set(ByVal value As RotatorAd)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a RotatorAd object to the RotatorAds collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">RotatorAd object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As RotatorAd) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As RotatorAd) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As RotatorAd)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As RotatorAd)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As RotatorAd) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.RotatorAd ORDER BY Name"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As RotatorAd
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
    Public Function Retrieve(ByVal ID As Integer) As RotatorAd
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.RotatorAd WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function

    Public Sub retrieveSet(ByVal category As RotatorAd.Categories)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.RotatorAd WHERE Category=@Category ORDER BY Name"
        Cmd.Parameters.Add("@Category", SqlDbType.Int).Value = category

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
                Dim RotatorAd As New RotatorAd
                Add(RotatorAd)
                RotatorAd.ConnectionString = _connectionString
                RotatorAd.DR2Object(dr)
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
        Dim deletedObjects As New List(Of RotatorAd)

        For Each RotatorAd As RotatorAd In List
            RotatorAd.Update()
            If RotatorAd.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(RotatorAd)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As RotatorAd In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class



'***************************************************************************************
'*
'* RotatorAd - RotatorAd object
'*
'*
'*
'*
'*
'**************************************************************************************

''' <summary>
''' <para> The RotatorAd object holds all data retrieved from a specified row of the RotatorAd table.
'''</para> It is always accessed as part of the RotatorAds collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded RotatorAds collection
'''  property of the parent. 
''' </summary>
Public Class RotatorAd
    Private _connectionString As String
    Private _ID As Integer
    Private _systemID As Integer
    Private _name As String
    Private _width As Integer
    Private _height As Integer
    Private _marginTop As Integer
    Private _marginBottom As Integer
    Private _marginLeft As Integer
    Private _marginRight As Integer
    Private _bgcolor As Integer
    Private _usageCount As Integer
    Private _clickCount As Integer
    Private _category As Categories
    Private _type As Types
    Private _ImageURL As String
    Private _ClickURL As String
    Private _ObjectStatus As ATSystem.ObjectState
    Private _status As ATSystem.StatusBits
    Private _createTime As Date
    Private _modifyTime As Date
    Private _navTarget As String
    '
    ' embedded collections
    '
    Private _Sys As ATSystem

    Public Enum Categories
        <Description("InActive")> InActive = 1
        <Description("Left panel")> Left = 2
        <Description("Right panel")> Right = 3
        <Description("Home Page - Left")> Homeleft = 4
        <Description("Home Page - Right")> HomeRight = 5
        <Description("Masthead")> MastHead = 6
    End Enum

    Public Enum Modules
        <Description("1 module")> m1 = 1
        <Description("2 module")> m2 = 2
        <Description("3 module")> m3 = 3
    End Enum

    Public Enum Types
        <Description("Flash")> Flash = 1
        <Description("Image")> Image = 2
    End Enum

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

    ''' <summary>Category</summary>
    Public Property Category() As Categories
        Get
            Return _category
        End Get
        Set(ByVal value As Categories)
            _category = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Type</summary>
    Public Property Type() As Types
        Get
            Return _type
        End Get
        Set(ByVal value As Types)
            _type = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>Width</summary>
    ''' <returns>Object Name</returns>
    Public Property Width() As Integer
        Get
            Return _Width
        End Get
        Set(ByVal value As Integer)
            _Width = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Height</summary>
    ''' <returns>Object Name</returns>
    Public Property Height() As Integer
        Get
            Return _height
        End Get
        Set(ByVal value As Integer)
            _height = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>MarginTop</summary>
    ''' <returns>Object Name</returns>
    Public Property MarginTop() As Integer
        Get
            Return _marginTop
        End Get
        Set(ByVal value As Integer)
            _marginTop = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>MarginBottom</summary>
    ''' <returns>Object Name</returns>
    Public Property MarginBottom() As Integer
        Get
            Return _marginBottom
        End Get
        Set(ByVal value As Integer)
            _marginBottom = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>MarginLeft</summary>
    ''' <returns>Object Name</returns>
    Public Property MarginLeft() As Integer
        Get
            Return _marginLeft
        End Get
        Set(ByVal value As Integer)
            _marginLeft = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>MarginLeft</summary>
    ''' <returns>Object Name</returns>
    Public Property MarginRight() As Integer
        Get
            Return _marginRight
        End Get
        Set(ByVal value As Integer)
            _marginRight = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>BG Color</summary>
    ''' <returns>Object Name</returns>
    Public Property BGColor() As Integer
        Get
            Return _bgcolor
        End Get
        Set(ByVal value As Integer)
            _bgcolor = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Usage Count</summary>
    ''' <returns>Object Name</returns>
    Public Property UsageCount() As Integer
        Get
            Return _usageCount
        End Get
        Set(ByVal value As Integer)
            _usageCount = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>Usage Count</summary>
    ''' <returns>Object Name</returns>
    Public Property ClickCount() As Integer
        Get
            Return _clickCount
        End Get
        Set(ByVal value As Integer)
            _clickCount = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property ConversionRate() As Double
        Get
            If _usageCount = 0 Then
                Return 0
            Else
                Return _clickCount / _usageCount
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the BG color as a hex string prefixed by # - html format
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BGColorHTML() As String
        Get
            Return "#" & _bgcolor.ToString("X6")
        End Get
    End Property

    ''' <summary>URL of where the ad image is found</summary>
    Public Property ImageURL() As String
        Get
            Return _ImageURL
        End Get
        Set(ByVal value As String)
            _ImageURL = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property


    ''' <summary>URL of where the user goes when an ad is clicked</summary>
    Public Property ClickURL() As String
        Get
            Return _ClickURL
        End Get
        Set(ByVal value As String)
            _ClickURL = value
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
    ''' Embedded system object, to which this RotatorAd is subordinate.
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
            Cmd.CommandText = "Update dbo.RotatorAd SET ClickCount=@ClickCount WHERE ID=@ID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try
        Return _clickCount          'return new value

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
        Cmd.CommandText = "UPDATE dbo.RotatorAd SET " & _
        "modifyTime=getdate()," & _
        "SystemID=@SystemID," & _
        "Status=@Status," & _
        "Name=@Name," & _
        "Width=@Width," & _
        "Height=@Height," & _
        "MarginBottom=@MarginBottom," & _
        "MarginLeft=@MarginLeft," & _
        "Marginright=@MarginRight," & _
        "MarginTop=@MarginTop," & _
        "BGColor=@BGColor," & _
        "UsageCount=@UsageCount," & _
        "ClickCount=@ClickCount," & _
        "Category=@Category," & _
        "Type=@Type," & _
        "ImageURL=@ImageURL," & _
        "ClickURL=@ClickURL" & _
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
        Cmd.CommandText = "INSERT INTO dbo.RotatorAd " & _
        "(SystemID,CreateTime,ModifyTime,Status,Name,Width,Height," & _
        "MarginTop,MarginBottom,MarginLeft,MarginRight," & _
        "BGColor,UsageCount,ClickCount,Category,Type,ImageURL,ClickURL) " & _
        "VALUES " & _
        "(@SystemID,getdate(),getdate(),@Status,@Name,@Width,@Height," & _
        "@MarginTop,@MarginBottom,@MarginLeft,@MarginRight," & _
        "@BGColor,@UsageCount,@ClickCount,@Category,@Type,@ImageURL,@ClickURl)" & _
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
        Cmd.Parameters.Add("@Width", SqlDbType.Int).Value = _width
        Cmd.Parameters.Add("@Height", SqlDbType.Int).Value = _height
        Cmd.Parameters.Add("@MarginTop", SqlDbType.Int).Value = _marginTop
        Cmd.Parameters.Add("@MarginBottom", SqlDbType.Int).Value = _marginBottom
        Cmd.Parameters.Add("@MarginLeft", SqlDbType.Int).Value = _marginLeft
        Cmd.Parameters.Add("@MarginRight", SqlDbType.Int).Value = _marginRight
        Cmd.Parameters.Add("@BGColor", SqlDbType.Int).Value = _bgcolor
        Cmd.Parameters.Add("@UsageCount", SqlDbType.Int).Value = _usageCount
        Cmd.Parameters.Add("@ClickCount", SqlDbType.Int).Value = _clickCount
        Cmd.Parameters.Add("@Category", SqlDbType.Int).Value = _category
        Cmd.Parameters.Add("@Type", SqlDbType.Int).Value = _type
        Cmd.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = _ImageURL
        Cmd.Parameters.Add("@ClickURL", SqlDbType.VarChar).Value = _ClickURL

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

            ''Cmd.CommandText = "DELETE dbo.Contact2RotatorAd WHERE ParentID=@ID"
            ''Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE dbo.RotatorAd WHERE ID=@ID"
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
        _width = Convert.ToInt32(dr("Width"))
        _height = Convert.ToInt32(dr("Height"))
        _marginTop = Convert.ToInt32(dr("MarginTop"))
        _marginBottom = Convert.ToInt32(dr("MarginBottom"))
        _marginLeft = Convert.ToInt32(dr("MarginLeft"))
        _marginRight = Convert.ToInt32(dr("MarginRight"))
        _bgcolor = Convert.ToInt32(dr("BGColor"))
        _usageCount = Convert.ToInt32(dr("UsageCount"))
        _clickCount = Convert.ToInt32(dr("ClickCount"))
        _ImageURL = Convert.ToString(dr("ImageURL"))
        _ClickURL = Convert.ToString(dr("ClickURL"))
        _category = CType(dr("Category"), Categories)
        _type = CType(dr("Type"), Types)
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
    End Sub

End Class

