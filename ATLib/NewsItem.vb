Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System



'***************************************************************************************
'*
'* NewsItems
'*
'*
'*
'***************************************************************************************

''' <summary>
''' The NewsItems class implements a collection of NewsItem objects, which are directly subordinate
''' to the System object. System Colors provide a single system wide reference of all spot colors that can be defined
''' in inks, foils and paper. System Colors are maintained in the RGB color space.
''' </summary>
Public Class NewsItems : Inherits CollectionBase

    Private _connectionString As String

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG NewsItems(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...NewsItems.count-1</param>
    ''' <value>NewsItem object from NewsItems collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As NewsItem
        Get
            Return CType(List(index), NewsItem)
        End Get
        Set(ByVal value As NewsItem)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a NewsItem object to the NewsItems collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">NewsItem object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As NewsItem) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As NewsItem) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As NewsItem)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As NewsItem)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As NewsItem) As Boolean
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
        Cmd.CommandText = "SELECT * from dbo.NewsItem ORDER BY ID DESC"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As NewsItem
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
    Public Function Retrieve(ByVal ID As Integer) As NewsItem
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "SELECT * from dbo.NewsItem WHERE ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function

    Public Sub retrieveSet(ByVal ProdnStatus As NewsItem.ProdnState)
        '
        ' Retrieves the set of records for the parent
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "SELECT * from dbo.NewsItem WHERE ProdnStatus=@ProdnStatus ORDER BY ID DESC"
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = ProdnStatus
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
                Dim NewsItem As New NewsItem
                Add(NewsItem)
                NewsItem.ConnectionString = _connectionString
                NewsItem.DR2Object(dr)
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
        Dim deletedObjects As New List(Of NewsItem)

        For Each NewsItem As NewsItem In List
            NewsItem.Update()
            If NewsItem.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(NewsItem)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As NewsItem In deletedObjects
            List.Remove(s)
        Next

    End Sub
End Class



'***************************************************************************************
'*
'* NewsItem
'*
'*
'*
'***************************************************************************************

''' <summary>
''' <para> The NewsItem object holds all data retrieved from a specified row of the NewsItem table.
'''</para>
''' <para>It is always accessed as part of the NewsItems collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded NewsItems collection
'''  property of the parent. 
''' </para>
''' </summary>
''' 
Public Class NewsItem

    Private _connectionString As String
    Private _ID As Integer
    Private _systemID As Integer
    Private _name As String
    Private _navTarget As String
    Private _ObjectStatus As ATSystem.ObjectState
    Private _status As ATSystem.StatusBits
    Private _createTime As Date
    Private _modifyTime As Date
    Private _Head As String
    Private _Intro As String
    Private _Body As String
    Private _PicCaption As String
    Private _ProdnStatus As ProdnState
    Private _physicalApplicationPath As String
    '
    ' embedded collections
    '
    Private _Sys As ATSystem

    ''' <summary>
    ''' Defines the state that a Technote can be in.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum ProdnState
        Incomplete = 1
        Latest = 2
        Current = 3
        Archived = 4
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


    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property Intro() As String
        Get
            Return _Intro
        End Get
        Set(ByVal value As String)
            _Intro = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property HTMLIntro() As String
        Get
            Return _Intro.Replace(vbCr, "<br />")
        End Get
    End Property

    Public Property Body() As String
        Get
            Return _Body
        End Get
        Set(ByVal value As String)
            _Body = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property HTMLBody() As String
        Get
            Return _Body.Replace(vbCr, "<br />")
        End Get
    End Property

    Public Property PicCaption() As String
        Get
            Return _PicCaption
        End Get
        Set(ByVal value As String)
            _PicCaption = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    ''' <summary>
    ''' Parent system object, to which the NewsItems are subordinate
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

    ''' <summary>
    ''' Defines whether this news item has a pic.
    ''' </summary>
    Public Property HasImage() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.HasImage And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.HasImage
            Else
                _status = _status And Not ATSystem.StatusBits.HasImage
            End If
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public ReadOnly Property ImageFilename() As String
        Get
            Dim filename As String = hexID & ".jpg"
            Return IO.Path.Combine(_physicalApplicationPath, Constants.subsampledImagesNewsPics & "/" & filename)
        End Get
    End Property


    Public ReadOnly Property ImageURL() As String
        Get
            Dim filename As String = hexID & ".jpg"
            Return GetApplicationPath() & Constants.subsampledImagesNewsPics & "/" & filename
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
        Cmd.CommandText = "UPDATE dbo.NewsItem SET " & _
        "modifyTime=getdate()," & _
        "SystemID=@SystemID," & _
        "Name=@Name," & _
        "Status=@Status," & _
        "ProdnStatus=@ProdnStatus," & _
        "Intro=@Intro," & _
        "Body=@Body," & _
        "PicCaption=@PicCaption " & _
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
        Cmd.CommandText = "INSERT INTO dbo.NewsItem " & _
        "(SystemID,CreateTime,ModifyTime,Status," & _
        "Name,Intro,Body,PicCaption,ProdnStatus) " & _
        "VALUES " & _
        "(@SystemID,getdate(),getdate(),@Status," & _
        "@Name,@Intro,@Body,@PicCaption,@ProdnStatus) " & _
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
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = _ProdnStatus
        Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = _name
        Cmd.Parameters.Add("@Intro", SqlDbType.VarChar).Value = _Intro
        Cmd.Parameters.Add("@Body", SqlDbType.VarChar).Value = _Body
        Cmd.Parameters.Add("@PicCaption", SqlDbType.VarChar).Value = _PicCaption

    End Sub

    Private Function doDelete() As Integer
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Try
            Cmd.Connection.Open()
            Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID

            Cmd.CommandText = "DELETE dbo.NewsItem WHERE ID=@ID"
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
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
        _status = CType(dr("Status"), ATSystem.StatusBits)
        _ProdnStatus = CType(dr("ProdnStatus"), ProdnState)
        _name = Convert.ToString(dr("Name"))
        _Intro = Convert.ToString(dr("Intro"))
        _Body = Convert.ToString(dr("Body"))
        _PicCaption = Convert.ToString(dr("PicCaption"))
    End Sub

End Class

