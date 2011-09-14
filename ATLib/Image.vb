Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.ComponentModel
Imports System
Imports System.IO
Imports System.Drawing


'***************************************************************************************
'*
'* Images
'*
'* AUDIT TRAIL
'* 
'* V1.000   05-SEP-2007  BA  Original
'*
'*
'***************************************************************************************

''' <summary>
''' <para>The Images collection contains a set of Image objects. Each Image object represents
''' an invocation of a Ad wihin a particular order, It therefore implemens a M:M relationship
'''  between the Order object and the Ad object.
''' </para>
''' </summary>
Public Class Images : Inherits CollectionBase

    Private _connectionString As String
    Private _Ad As Ad

    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _Ad = Nothing
    End Sub

    Public Sub New(ByVal parent As Ad)
        _Ad = parent
    End Sub

    '''
    ''' ''' <summary>
    ''' Default Item property, used for indexing into the returned collection. EG Images(i)
    ''' </summary>
    ''' <param name="index">index in the range 0...Images.count-1</param>
    ''' <value>Image object from Images collection, at the indexed postion.</value>
    '''
    Default Public Property Item(ByVal index As Integer) As Image
        Get
            Return CType(List(index), Image)
        End Get
        Set(ByVal value As Image)
            List(index) = value
        End Set
    End Property


    '''
    ''' <summary>
    ''' Adds a Image object to the Images collection. This however does not update the database.
    ''' To write the newly added object to the database, call the Update Method of the parent collection or the Update method of the added object.
    ''' </summary>
    ''' <param name="value">Image object</param>
    ''' <returns>Index of added object</returns>
    '''
    Public Function Add(ByVal value As Image) As Integer
        Return (List.Add(value))
    End Function
    Private Function IndexOf(ByVal value As Image) As Integer
        Return (List.IndexOf(value))
    End Function
    Private Sub Insert(ByVal index As Integer, ByVal value As Image)
        List.Insert(index, value)
    End Sub
    Private Sub Remove(ByVal value As Image)
        List.Remove(value)
    End Sub
    Private Function Contains(ByVal value As Image) As Boolean
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
        Cmd.CommandText = "Select * from dbo.Image ORDER BY ID"
        doRetrieveR(Cmd)
    End Sub

    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="HexID">ObjectID, represented as an 8 character hex string</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal HexID As String) As Image
        Return Retrieve(Hex2Int(HexID))
    End Function


    '''
    ''' <summary>Retrieves a specific object, identified by the value of its ID.
    ''' The parent collection is populated with the retrieved object, and the object is also explicitly returned to the caller.
    ''' If the object is not found, Nothing is returned and the collection will be empty</summary>
    ''' <param name="ID">ObjectID, represented as an integer</param>
    ''' <returns>Object if found, otherwise Nothing</returns>
    ''' 
    Public Function Retrieve(ByVal ID As Integer) As Image
        '
        ' Retrieves a specific record
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.CommandText = "Select * from dbo.Image WHERE Image.ID=@ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        doRetrieveR(Cmd)
        If List.Count = 0 Then
            Return Nothing
        Else
            Return Item(0)
        End If
    End Function


    Public Sub RetrieveSet(ByVal AdID As Integer)
        '
        ' Retrieves all images for a specific Ad
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "Select * from dbo.Image WHERE AdID=@ID ORDER BY ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = AdID
        doRetrieveR(Cmd)
    End Sub


    Public Sub RetrieveWebSet(ByVal AdID As Integer)
        '
        ' Retrieves all images for a specific Ad which are enabled for web display
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "Select * from dbo.Image WHERE AdID=@ID AND " & _
        "(Status & " & ATSystem.StatusBits.IsWebEnabled & "=" & ATSystem.StatusBits.IsWebEnabled & ")" & _
        " ORDER BY ID"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = AdID
        doRetrieveR(Cmd)
    End Sub

  
    Friend Sub RetrieveMainImage(ByVal AdID As Integer)
        '
        ' Retrieves the main image for a specific Ad
        '
        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()

        Cmd.CommandText = "Select * from dbo.Image WHERE AdID=@ID AND ISMAINIMAGE=1"
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = AdID
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
                Dim Image As New Image
                Add(Image)
                Image.ConnectionString = _connectionString
                Image.DR2Object(dr)
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
        Dim deletedObjects As New List(Of Image)

        For Each Image As Image In List
            Image.Update()
            If Image.ObjectStatus = ATSystem.ObjectState.Deleted Then deletedObjects.Add(Image)
        Next
        '
        ' remove deleted objects from list
        '
        For Each s As Image In deletedObjects
            List.Remove(s)
        Next

    End Sub



End Class



'***************************************************************************************
'*
'* Image - Image object
'*
'* AUDIT TRAIL
'* 
'* V1.000   05-SEP-2007  BA  Original
'*
'*
'*
'*
'**************************************************************************************

''' <summary>
''' <para> The Image object holds all data retrieved from a specified row of the Image table.
'''</para>
''' <para> It is always accessed as part of the Dcos collection, which is either
'''  explicitly read from the database by one of the Retrieve methods, or implicitly populated
'''  through an embedded Images collection
'''  property of the parent. 
''' </para>
''' </summary>
Public Class Image

    Private _connectionString As String
    Private _ID As Integer
    Private _AdID As Integer
    Private _type As ImageTypes
    Private _status As ATSystem.StatusBits
    Private _prodnStatus As ProdnState
    Private _createTime As Date
    Private _modifyTime As Date
    Private _physicalApplicationPath As String
    Private _navTarget As String
    Private _isMainImage As Boolean
    Private _pixelWidth As Integer
    Private _pixelHeight As Integer
    Private _resolution As Integer
    Private _previewSequence As Integer

    Private _OriginalSourcePath As String
    Private _WorkingSourcePath As String
    Private _lowresPath As String

    Private _ObjectStatus As ATSystem.ObjectState
    '
    ' embedded objects
    '
    Friend _Ad As Ad
    Private mySys As ATSystem

    ''' <summary>
    ''' Defines the production status that a Image can be in.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum ProdnState
        Unspecified = 0
        <Description("Initial")> Initial = 1
        <Description("In Production")> Prodn = 2
        <Description("Production Hold")> ProdnHold = 3
        <Description("Ready")> Ready = 4
    End Enum

    ''' <summary>
    ''' Defines the uploadable image types that are acceptable to the system.
    ''' </summary>
    ''' <includesource>yes</includesource>
    Public Enum ImageTypes As Integer
        Unknown = 0
        JPG = 1
        TIF = 2
        EPS = 3
        BMP = 4
        PNG = 5
        PDF = 6
        GIF = 7
        SWF = 8
    End Enum


    ''' <summary>
    ''' Instantiates the object. When instantiated, the ID property will be set to ATSystem.SysConstants.nullValue and the Status word will be zero.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _ID = ATSystem.SysConstants.nullValue
        _status = 0
        _prodnStatus = ProdnState.Initial
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
    ''' Absolute path of source image. Not written to  db, but used to derive sourceFileName
    ''' </summary>
    Public Property OriginalSourcePath() As String
        Get
            If _OriginalSourcePath Is Nothing Then
                If mysys Is Nothing Then
                    mySys = New ATSystem
                    mysys.Retrieve()
                End If
                _OriginalSourcePath = mySys.SourceImageOriginalFolder
            End If
            Return _OriginalSourcePath
        End Get
        Set(ByVal value As String)
            _OriginalSourcePath = value
        End Set
    End Property

    ''' <summary>
    ''' Absolute path of source image. Not written to  db, but used to derive sourceFileName
    ''' </summary>
    Public Property WorkingSourcePath() As String
        Get
            If _WorkingSourcePath Is Nothing Then
                If mySys Is Nothing Then
                    mySys = New ATSystem
                    mySys.Retrieve()
                End If
                _WorkingSourcePath = mySys.SourceImageWorkingFolder
            End If
            Return _WorkingSourcePath
        End Get
        Set(ByVal value As String)
            _WorkingSourcePath = value
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

    Public ReadOnly Property OriginalSourceFileName() As String
        Get
            Dim filename As String = hexID & CommonRoutines.Type2Xtn(_type)
            Return System.IO.Path.Combine(OriginalSourcePath, filename)
        End Get
    End Property

    Public ReadOnly Property WorkingSourceFileName() As String
        Get
            Dim filename As String = hexID & CommonRoutines.Type2Xtn(_type)
            Return System.IO.Path.Combine(WorkingSourcePath, filename)
        End Get
    End Property

    Public ReadOnly Property ShortFileName() As String
        Get
            Return hexID & CommonRoutines.Type2Xtn(_type)
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

    Public ReadOnly Property THBFilename() As String
        Get
            Dim filename As String = hexID & "-" & PreviewSequence.ToString & ".jpg"
            Return IO.Path.Combine(_physicalApplicationPath, Constants.subsampledImagesTHB & "/" & filename)
        End Get
    End Property

    Public ReadOnly Property LoResFilename() As String
        Get
            Dim filename As String = hexID & "-" & PreviewSequence.ToString & ".jpg"
            Return IO.Path.Combine(_physicalApplicationPath, Constants.subsampledImagesLores & "/" & filename)
        End Get
    End Property

    Public ReadOnly Property THBURL() As String
        Get
            Dim filename As String = hexID & "-" & PreviewSequence.ToString & ".jpg"
            Return GetApplicationPath() & Constants.subsampledImagesTHB & "/" & filename
        End Get
    End Property

    Public ReadOnly Property LoResURL() As String
        Get
            Dim filename As String = hexID & "-" & PreviewSequence.ToString & ".jpg"
            Return GetApplicationPath() & Constants.subsampledImagesLores & "/" & filename
        End Get
    End Property

    Public Property ProdnStatus() As ProdnState
        Get
            Return _prodnStatus
        End Get
        Set(ByVal value As ProdnState)
            _prodnStatus = value
        End Set
    End Property

    Public ReadOnly Property ProdnStatusDescr() As String
        Get
            Dim EA As New EnumAssistant(New Image.ProdnState)
            Return EA(_prodnStatus).Name
        End Get
    End Property

    Public Property PixelWidth() As Integer
        Get
            Return _pixelWidth
        End Get
        Set(ByVal value As Integer)
            _pixelWidth = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property PixelHeight() As Integer
        Get
            Return _pixelHeight
        End Get
        Set(ByVal value As Integer)
            _pixelHeight = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property Resolution() As Integer
        Get
            Return _resolution
        End Get
        Set(ByVal value As Integer)
            _resolution = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property AdID() As Integer
        Get
            Return _AdID
        End Get
        Set(ByVal value As Integer)
            _AdID = value
            _ObjectStatus = ATSystem.ObjectState.Modified
        End Set
    End Property

    Public Property Type() As ImageTypes
        Get
            Return _type
        End Get
        Set(ByVal value As ImageTypes)
            _type = value
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
    ''' Defines whether the object can be viewed on the web site.
    ''' </summary>
    ''' <value>True if the spec is active, false otherwise.</value>
    Public Property IsWebEnabled() As Boolean
        Get
            Return Convert.ToBoolean(ATSystem.StatusBits.IsWebEnabled And _status)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _status = _status Or ATSystem.StatusBits.IsWebEnabled
            Else
                _status = _status And Not ATSystem.StatusBits.IsWebEnabled
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

    Public Property IsMainImage() As Boolean
        Get
            Return _isMainImage
        End Get
        Set(ByVal value As Boolean)
            _isMainImage = value
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


    Private Function getConnection() As SqlConnection
        If _connectionString Is Nothing Then _connectionString = ConfigurationManager.ConnectionStrings("ATConnectionString").ConnectionString
        Return New SqlConnection(_connectionString)
    End Function

    ''' <summary>
    ''' makes this image the main image for the ad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetMainImage()

        Dim Cmd As New SqlCommand()
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@AdID", SqlDbType.Int).Value = _AdID
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID

        Try
            Cmd.Connection.Open()

            Cmd.CommandText = "Update dbo.Image SET IsMainImage=0 WHERE Image.AdID=@AdID"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Update dbo.Image SET IsMainImage=1 WHERE Image.ID=@ID"
            Cmd.ExecuteNonQuery()
        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try


    End Sub

    Public Sub GenerateSubsample(ByVal Filename As String, ByVal Height As Integer)

        Dim str As Stream = Nothing
        Select Case _type
            Case ImageTypes.EPS
                str = getTifPreview(WorkingSourceFileName)
                If str Is Nothing Then
                    Dim defaultEPSFilename As String = System.IO.Path.Combine(_WorkingSourcePath, Constants.DefaultEPS)
                    str = New FileStream(defaultEPSFilename, FileMode.Open, FileAccess.Read)
                End If
                Try
                    GenerateSubsample(str, Filename, Height)
                Catch ex As Exception
                    Dim defaultEPSFilename As String = System.IO.Path.Combine(_WorkingSourcePath, Constants.DefaultEPS)
                    str = New FileStream(defaultEPSFilename, FileMode.Open, FileAccess.Read)
                    GenerateSubsample(str, Filename, Height)
                End Try


            Case ImageTypes.PDF
                Dim defaultPDFFilename As String = System.IO.Path.Combine(_WorkingSourcePath, Constants.DefaultPDF)
                str = New FileStream(defaultPDFFilename, FileMode.Open, FileAccess.Read)
                GenerateSubsample(str, Filename, Height)

            Case Else
                str = New FileStream(WorkingSourceFileName, FileMode.Open, FileAccess.Read)
                Try
                    GenerateSubsample(str, Filename, Height)
                Catch ex As Exception
                    Dim defaultImageFilename As String = System.IO.Path.Combine(_WorkingSourcePath, Constants.DefaultImage)
                    str = New FileStream(defaultImageFilename, FileMode.Open, FileAccess.Read)
                    GenerateSubsample(str, Filename, Height)
                End Try
        End Select

    

        str.Dispose()
       
    End Sub



    ''' <summary>
    ''' Generates a low res subsample with the supplied name into a fixed height, variable width 
    ''' </summary>
    ''' <param name="Height"></param>
    ''' <remarks>http://dotnetslackers.com/articles/aspnet/Generating-Image-Thumbnails-in-ASP-NET.aspx</remarks>
    Public Sub GenerateSubsample(ByVal SourceFilename As String, ByVal DestnFilename As String, ByVal Height As Integer)

        Using fs As New FileStream(SourceFilename, FileMode.Open, FileAccess.Read)

            Dim targetImage As Bitmap = Nothing

            Try
                Using bitmap As New Bitmap(fs)
                    Dim size As SizeF = bitmap.PhysicalDimension

                    Dim imageAspectRatio As Double = size.Width / size.Height
                    Dim scaledWidth As Integer = Convert.ToInt32(Height * imageAspectRatio)

                    targetImage = New Bitmap(scaledWidth, Height)

                    Using G As Graphics = Graphics.FromImage(targetImage)
                        G.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
                        G.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                        G.CompositingMode = Drawing2D.CompositingMode.SourceCopy
                        G.DrawImage(bitmap, 0, 0, scaledWidth, Height)
                    End Using

                    Using ms As New MemoryStream()
                        targetImage.Save(ms, Imaging.ImageFormat.Jpeg)
                        Using fsout As New FileStream(DestnFilename, FileMode.OpenOrCreate, FileAccess.Write)
                            ms.WriteTo(fsout)
                        End Using
                    End Using
                End Using

            Finally
                If Not targetImage Is Nothing Then
                    targetImage.Dispose()
                End If
            End Try
            fs.Dispose()
        End Using

    End Sub

    Public Sub generateSubSample(ByVal stream As Stream, ByVal destnFileName As String, ByVal Height As Integer)
        Dim targetImage As Bitmap = Nothing

        Try
            Using bitmap As New Bitmap(stream)
                Dim size As SizeF = bitmap.PhysicalDimension

                Dim imageAspectRatio As Double = size.Width / size.Height
                Dim scaledWidth As Integer = Convert.ToInt32(Height * imageAspectRatio)

                targetImage = New Bitmap(scaledWidth, Height)

                Using G As Graphics = Graphics.FromImage(targetImage)
                    G.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
                    G.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    G.CompositingMode = Drawing2D.CompositingMode.SourceCopy
                    G.DrawImage(bitmap, 0, 0, scaledWidth, Height)
                End Using

                Using ms As New MemoryStream()
                    targetImage.Save(ms, Imaging.ImageFormat.Jpeg)
                    Using fsout As New FileStream(destnFileName, FileMode.OpenOrCreate, FileAccess.Write)
                        ms.WriteTo(fsout)
                    End Using
                End Using
            End Using


        Finally
            If Not targetImage Is Nothing Then
                targetImage.Dispose()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Attempts to generate a valid TIFF preview from the supplied EPS filename. The preview is extracted from 
    ''' the TIFF preview within the EPS file. If no preview exists, a proforma EPS preview is returned instead.
    ''' </summary>
    ''' <param name="EPSfile"></param>
    ''' <returns>TIF preview in system bitmap format</returns>
    Public Function getTifPreview(ByVal EPSfile As String) As MemoryStream
        '
        ' Tries to extract a tif preview from the EPS file.
        ' if not possible returns a default image
        ' the EPS file can be big - eg 100MB. But the streamreader.seek call means it does
        ' not have to be read entirely into memory
        '
        Const TIFFIdent As Integer = &HC6D3D0C5
        Dim buffer(7) As Byte
        Dim preview() As Byte

        Dim tifFlag As Integer
        Dim ms As MemoryStream = Nothing

        Dim fs As New FileStream(EPSfile, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
        Dim streamreader As New StreamReader(fs)
        '
        ' read the first 4 bytes to see if there's tif preview
        '
        streamreader.BaseStream.Read(buffer, 0, 4)
        tifFlag = BitConverter.ToInt32(buffer, 0)
        If tifFlag = TIFFIdent Then

            Dim start As Integer                'offset into file of tif preview
            Dim length As Integer               'length of tif preview
            '
            ' skip next 16 bytes then read start and length
            '
            Try
                streamreader.BaseStream.Seek(16, IO.SeekOrigin.Current)
                streamreader.BaseStream.Read(buffer, 0, 8)
                start = BitConverter.ToInt32(buffer, 0)
                length = BitConverter.ToInt32(buffer, 4)
                '
                ' get some space for the preview and read it in
                '
                ReDim preview(length - 1)
                streamreader.BaseStream.Seek(start, IO.SeekOrigin.Begin)
                streamreader.BaseStream.Read(preview, 0, length)
                ms = New IO.MemoryStream(preview, 0, preview.Length)
                '
                ' testing - enable next line to write preview to file
                '
                ''            ByteArray2File(preview, "c:\edrive\test.tif")
    
            Finally
                fs.Close()
            End Try

        End If

        Return ms
    End Function

    Public Sub ByteArray2File(ByVal b() As Byte, ByVal filename As String)

        Dim fs As New FileStream(filename, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.Read)

        fs.Write(b, 0, b.Length)
        fs.Close()
        fs.Dispose()
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
        Cmd.CommandText = "UPDATE dbo.Image SET " & _
        "modifyTime=getdate()," & _
        "AdID=@AdID," & _
        "Type=@Type," & _
        "PreviewSequence=@PreviewSequence," & _
        "Resolution=@Resolution," & _
        "PixelWidth=@PixelWidth," & _
        "PixelHeight=@PixelHeight," & _
        "Status=@Status," & _
        "IsMainImage=@IsMainImage," & _
        "ProdnStatus=@ProdnStatus" & _
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
        Cmd.CommandText = "INSERT INTO dbo.Image " & _
        "(AdID,PreviewSequence,Resolution,PixelWidth,PixelHeight,Type,createTime,modifyTime,IsMainImage,Status,ProdnStatus) " & _
        "VALUES (@AdID,@PreviewSequence,@Resolution,@PixelWidth,@PixelHeight,@Type,getdate(),getdate(),@IsMainImage,@Status,@ProdnStatus)" & _
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
        Cmd.Parameters.Add("@AdID", SqlDbType.Int).Value = _AdID
        Cmd.Parameters.Add("@PreviewSequence", SqlDbType.Int).Value = _previewSequence
        Cmd.Parameters.Add("@Resolution", SqlDbType.Int).Value = _resolution
        Cmd.Parameters.Add("@PixelWidth", SqlDbType.Int).Value = _pixelWidth
        Cmd.Parameters.Add("@PixelHeight", SqlDbType.Int).Value = _pixelHeight
        Cmd.Parameters.Add("@Type", SqlDbType.Int).Value = _type
        Cmd.Parameters.Add("@IsMainImage", SqlDbType.Int).Value = _isMainImage
        Cmd.Parameters.Add("@ProdnStatus", SqlDbType.Int).Value = _prodnStatus
        Cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _status

    End Sub


    ''' <summary>Physically deletes the object and subordinate structure from the db
    '''</summary> 
    Private Function doDelete() As Integer

        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Cmd.Parameters.Add("@ID", SqlDbType.Int).Value = _ID

        Try
            Cmd.Connection.Open()

            Cmd.CommandText = "DELETE dbo.Image WHERE ID=@ID"
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
        _type = CType(dr("Type"), ImageTypes)
        _AdID = Convert.ToInt32(dr("AdID"))
        _previewSequence = Convert.ToInt32(dr("PreviewSequence"))
        _resolution = Convert.ToInt32(dr("Resolution"))
        _pixelWidth = Convert.ToInt32(dr("PixelWidth"))
        _pixelHeight = Convert.ToInt32(dr("PixelHeight"))
        _isMainImage = Convert.ToBoolean(dr("IsMainImage"))
        _prodnStatus = CType(dr("ProdnStatus"), ProdnState)
        _createTime = Convert.ToDateTime(dr("CreateTime"))
        _modifyTime = Convert.ToDateTime(dr("ModifyTime"))
    End Sub

End Class


