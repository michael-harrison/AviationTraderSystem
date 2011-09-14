Option Strict On
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.ComponentModel
Imports System



'***************************************************************************************
'*
'* Rate Table
'*
'* AUDIT TRAIL
'* 
'* V1.000   05-SEP-2007  BA  Original
'*
'*
'*
'***************************************************************************************

Public Class RateTable


    '
    ' define constants for extracting the correct rows and columns
    '
    Private _connectionstring As String

    Private _depthNames() As String
    Private _colNames() As String
    Private _MonoTable(,) As Integer
    Private _Spot1Table(,) As Integer
    Private _Spot2Table(,) As Integer
    Private _FPCTable(,) As Integer

    Private _ClassadTable(,) As Integer
  
    Private _rateName As String

    Private Const displayColCount As Integer = 6           'display table is 6 cols wide, aranged in groups of 4 24 cols in total
    Private Const classadColCount As Integer = 3                  'classad column count
    Private Const classadRowCount As Integer = 29                    'classad rowcount

    Public Enum DisplayColorTypes
        <Description("Mono")> Mono = 0
        <Description("1 Spot Colour")> Spot1 = 1
        <Description("2 Spot Colours")> Spot2 = 2
        <Description("Full Colour")> FPC = 3
    End Enum

    Public Enum DisplayWidths
        <Description("2 Cols")> Col2 = 2
        <Description("3 Cols")> Col3 = 3
        <Description("4 Cols")> Col4 = 4
        <Description("5 Cols")> Col5 = 5
        <Description("7 Cols")> Col7 = 7
        <Description("DPS")> Col8 = 8
    End Enum

    Public Enum DisplayHeights
        <Description("1 cm")> cm1 = 1
        <Description("2 cm")> cm2 = 2
        <Description("3 cm")> cm3 = 3
        <Description("4 cm")> cm4 = 4
        <Description("5 cm")> cm5 = 5
        <Description("6 cm")> cm6 = 6
        <Description("7 cm")> cm7 = 7
        <Description("8 cm")> cm8 = 8
        <Description("9 cm")> cm9 = 9
        <Description("10 cm")> cm10 = 10
        <Description("11 cm")> cm11 = 11
        <Description("12 cm")> cm12 = 12
        <Description("13 cm")> cm13 = 13
        <Description("14 cm")> cm14 = 14
        <Description("15 cm")> cm15 = 15
        <Description("16 cm")> cm16 = 16
        <Description("17 cm")> cm17 = 17
        <Description("18 cm")> cm18 = 18
        <Description("19 cm")> cm19 = 19
        <Description("20 cm")> cm20 = 20
        <Description("21 cm")> cm21 = 21
        <Description("22 cm")> cm22 = 22
        <Description("23 cm")> cm23 = 23
        <Description("24 cm")> cm24 = 24
        <Description("25 cm")> cm25 = 25
        <Description("26 cm")> cm26 = 26
        <Description("27 cm")> cm27 = 27
        <Description("28 cm")> cm28 = 28
        <Description("29 cm")> cm29 = 29
        <Description("30 cm")> cm30 = 30
        <Description("31 cm")> cm31 = 31
        <Description("32 cm")> cm32 = 32
        <Description("33 cm")> cm33 = 33
        <Description("34 cm")> cm34 = 34
        <Description("35 cm")> cm35 = 35
        <Description("36 cm")> cm36 = 36
        <Description("37 cm")> cm37 = 37
        <Description("38 cm")> cm38 = 38
    End Enum

    Public Enum ClassadHeights
        <Description("2 lines")> line2 = 2
        <Description("3 lines")> line3 = 3
        <Description("4 lines")> line4 = 4
        <Description("5 lines")> line5 = 5
        <Description("6 lines")> line6 = 6
        <Description("7 lines")> line7 = 7
        <Description("8 lines")> line8 = 8
        <Description("9 lines")> line9 = 9
        <Description("10 lines")> line10 = 10
        <Description("11 lines")> line11 = 11
        <Description("12 lines")> line12 = 12
        <Description("13 lines")> line13 = 13
        <Description("14 lines")> line14 = 14
        <Description("15 lines")> line15 = 15
        <Description("16 lines")> line16 = 16
        <Description("17 lines")> line17 = 17
        <Description("18 lines")> line18 = 18
        <Description("19 lines")> line19 = 19
        <Description("20 lines")> line20 = 20
        <Description("21 lines")> line21 = 21
        <Description("22 lines")> line22 = 22
        <Description("23 lines")> line23 = 23
        <Description("24 lines")> line24 = 24
        <Description("25 lines")> line25 = 25
        <Description("26 lines")> line26 = 26
        <Description("27 lines")> line27 = 27
        <Description("28 lines")> line28 = 28
        <Description("29 lines")> line29 = 29
        <Description("30 lines")> line30 = 30
    End Enum




    Public ReadOnly Property ClassadTable() As Integer(,)
        Get
            Return _ClassadTable
        End Get
    End Property


    Public ReadOnly Property MonoTable() As Integer(,)
        Get
            Return _MonoTable
        End Get
    End Property

    Public ReadOnly Property Spot1Table() As Integer(,)
        Get
            Return _Spot1Table
        End Get
    End Property

    Public ReadOnly Property Spot2table() As Integer(,)
        Get
            Return _Spot2Table
        End Get
    End Property

    Public ReadOnly Property FPCTable() As Integer(,)
        Get
            Return _FPCTable
        End Get
    End Property

    Public ReadOnly Property DisplayDepths() As String()
        Get
            Return _depthNames
        End Get
    End Property

    ''' <summary>
    ''' Imports the data from the supplied excel spreadsheet into memory based tables
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <param name="sheetname"></param>
    ''' <remarks></remarks>
    Public Sub ImportClassadRates(ByVal Filename As String, ByVal Sheetname As String)
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        '
        ' fetch excel data and create sql table
        ' Note - requires Office 2007 Dataconnectivity components which includes the ACE.OLEDB.12 driver
        ' get from http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=7554f536-8c28-4598-9b72-ef94e038c891
        '
        Cmd.CommandText = "SELECT * FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Excel 8.0;HDR=NO;IMEX=1;Database=" & Filename & "','SELECT * FROM [" & Sheetname & "$]')"
        Dim ds As New DataSet

        Try
            Cmd.Connection.Open()
            Dim SQLAdaptor As New SqlDataAdapter(Cmd)
            SQLAdaptor.Fill(ds)
            Dim dt As DataTable = ds.Tables(0)
            '
            ' read the sheet header to get offsets of where the data is found
            '
            _rateName = dt.Rows(1)(3).ToString
            Dim depthNameIndex As Integer = Asc(dt.Rows(2)(3).ToString.ToUpper) - Asc("A")
            Dim colNameIndex As Integer = Convert.ToInt32(dt.Rows(3)(3)) - 1
            Dim firstDataRow As Integer = Convert.ToInt32(dt.Rows(4)(3)) - 1
            Dim lastDataRow As Integer = Convert.ToInt32(dt.Rows(5)(3)) - 1
            Dim firstDataColumn As Integer = Asc(dt.Rows(6)(3).ToString.ToUpper) - Asc("A")
            Dim lastDataColumn As Integer = Asc(dt.Rows(7)(3).ToString.ToUpper) - Asc("A")
            '
            ' set up indexes
            '
            Dim rowCount As Integer = lastDataRow - firstDataRow
            Dim colCount As Integer = Convert.ToInt32(lastDataColumn - firstDataColumn)
            '
            ' set table sizes
            '
            ReDim _ClassadTable(rowCount, colCount)
            '
            ' extract the data into the tables
            '
            For r As Integer = 0 To rowCount
                Dim currRow As Integer = r + firstDataRow
                For c As Integer = 0 To colCount
                    Dim currCol As Integer = c + firstDataColumn
                    _ClassadTable(r, c) = getNumericValue(dt.Rows(currRow)(currCol))
                Next
            Next

        Finally
            Cmd.Connection.Dispose()
        End Try

    End Sub

    ''' <summary>
    ''' Imports the data from the supplied excel spreadsheet into memory based tables
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <param name="sheetname"></param>
    ''' <remarks></remarks>
    Public Sub ImportDisplayRates(ByVal Filename As String, ByVal Sheetname As String)
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        '
        ' fetch excel data and create sql table
        ' Note - requires Office 2007 Dataconnectivity components which includes the ACE.OLEDB.12 driver
        ' get from http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=7554f536-8c28-4598-9b72-ef94e038c891
        '
        Cmd.CommandText = "SELECT * FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0','Excel 8.0;HDR=NO;IMEX=1;Database=" & Filename & "','SELECT * FROM [" & Sheetname & "$]')"
        Dim ds As New DataSet


        Try
            Cmd.Connection.Open()
            Dim SQLAdaptor As New SqlDataAdapter(Cmd)
            SQLAdaptor.Fill(ds)
            Dim dt As DataTable = ds.Tables(0)
            '
            ' read the sheet header to get offsets of where the data is found
            '
            _rateName = dt.Rows(1)(3).ToString
            Dim depthNameIndex As Integer = Asc(dt.Rows(2)(3).ToString.ToUpper) - Asc("A")
            Dim colNameIndex As Integer = Convert.ToInt32(dt.Rows(3)(3)) - 1
            Dim firstDataRow As Integer = Convert.ToInt32(dt.Rows(4)(3)) - 1
            Dim lastDataRow As Integer = Convert.ToInt32(dt.Rows(5)(3)) - 1
            Dim firstDataColumn As Integer = Asc(dt.Rows(6)(3).ToString.ToUpper) - Asc("A")
            Dim lastDataColumn As Integer = Asc(dt.Rows(7)(3).ToString.ToUpper) - Asc("A")
            '
            ' set up indexes
            '

            Dim rowCount As Integer = lastDataRow - firstDataRow
            Dim colCount As Integer = Convert.ToInt32(lastDataColumn - firstDataColumn) + 1
            Dim colxCount As Integer = (colCount >> 2) - 1    'divide by 4
            '
            ' set table sizes
            '
            ReDim _MonoTable(rowCount, colxCount)
            ReDim _Spot1Table(rowCount, colxCount)
            ReDim _Spot2Table(rowCount, colxCount)
            ReDim _FPCTable(rowCount, colxCount)
            ReDim _colNames(colxCount)
            ReDim _depthNames(rowCount)
            '
            ' extract the data into the tables
            '

            For d As Integer = 0 To colCount - 4 Step 4
                _colNames(d >> 2) = dt(colNameIndex)(firstDataColumn + d).ToString
            Next

            For r As Integer = 0 To rowCount
                Dim currRow As Integer = r + firstDataRow
                _depthNames(r) = dt(currRow)(depthNameIndex).ToString
                For c As Integer = 0 To colxCount
                    Dim cx As Integer = c << 2          'multiply by 4
                    Dim currCol As Integer = cx + firstDataColumn
                    _MonoTable(r, c) = getNumericValue(dt.Rows(currRow)(currCol))
                    _Spot1Table(r, c) = getNumericValue(dt.Rows(currRow)(currCol + 1))
                    _Spot2Table(r, c) = getNumericValue(dt.Rows(currRow)(currCol + 2))
                    _FPCTable(r, c) = getNumericValue(dt.Rows(currRow)(currCol + 3))
                Next
            Next

        Finally
            Cmd.Connection.Dispose()
        End Try

    End Sub

    Private Function getNumericValue(ByVal obj As Object) As Integer
        '
        ' returns the integer value after testing nulls and spaces
        '
        Dim rtnval As Integer = ATSystem.SysConstants.nullValue
        Try
            rtnval = CommonRoutines.Dollars2Integer(obj.ToString)
        Catch ex As Exception
        End Try
        Return rtnval

    End Function

    ''' <summary>
    ''' Exports the classad table to a rectangular array of numeric cells in the SQL database
    ''' </summary>
    ''' <param name="Tablename"></param>
    ''' <remarks></remarks>
    Public Sub ExportClassadRates(ByVal Tablename As String)
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Try
            Cmd.Connection.Open()
            '
            ' drop the table if it currently exists
            '
            Cmd.CommandText = "SELECT Count(Name) from sysobjects where xtype='u' and name = '" & Tablename & "'"
            Dim count As Integer = Convert.ToInt32(Cmd.ExecuteScalar())
            If count > 0 Then
                Cmd.CommandText = "Drop TABLE dbo." & Tablename
                Cmd.ExecuteNonQuery()
            End If
            '
            ' create the table with first columnm
            '
            Cmd.CommandText = "CREATE TABLE dbo." & Tablename & "(ID int IDENTITY(0,1) primary key clustered)"
            Cmd.ExecuteNonQuery()
            '
            ' add in other columns
            '
            Dim colspecs As String = ""
            Dim c As Integer
            Dim r As Integer
          
            For c = 0 To classadColCount - 1
                Dim suffix As String = ""
                Select Case c
                    Case 0 : suffix = "textonly"
                    Case 1 : suffix = "monopic"
                    Case 2 : suffix = "colorpic"
                   
                End Select
                colspecs &= suffix & " int NOT NULL,"
            Next

            colspecs = colspecs.Substring(0, colspecs.Length - 1)          'stip last comma


            Cmd.CommandText = "ALTER TABLE dbo." & Tablename & " ADD " & colspecs
            Cmd.ExecuteNonQuery()
            '
            ' Add data to rows 
            '
            Dim newtoken As String = ""
            For r = 0 To classadRowCount - 1

                newtoken = _ClassadTable(r, 0).ToString & "," & _ClassadTable(r, 1).ToString & "," & _ClassadTable(r, 2).ToString


                Cmd.CommandText = "INSERT INTO dbo." & Tablename & " VALUES (" & newtoken & ")"
                Cmd.ExecuteNonQuery()
            Next

        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try

    End Sub

    ''' <summary>
    ''' Exports the display tables to a rectangular array of numeric cells in the SQL database
    ''' </summary>
    ''' <param name="Tablename"></param>
    ''' <remarks></remarks>
    Public Sub ExportDisplayRates(ByVal Tablename As String)
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        Try
            Cmd.Connection.Open()
            '
            ' drop the table if it currently exists
            '
            Cmd.CommandText = "SELECT Count(Name) from sysobjects where xtype='u' and name = '" & Tablename & "'"
            Dim count As Integer = Convert.ToInt32(Cmd.ExecuteScalar())
            If count > 0 Then
                Cmd.CommandText = "Drop TABLE dbo." & Tablename
                Cmd.ExecuteNonQuery()
            End If
            '
            ' create the table with first columnm
            '
            Cmd.CommandText = "CREATE TABLE dbo." & Tablename & "(ID int IDENTITY(0,1) primary key clustered, Depth varchar(255) NOT NULL)"
            Cmd.ExecuteNonQuery()
            '
            ' add in other columns
            '
            Dim colspecs As String = ""
            Dim c As Integer
            Dim r As Integer
            For j = 0 To 3
                Dim prefix As String = ""
                Select Case j
                    Case 0 : prefix = "Mono_"
                    Case 1 : prefix = "Spot1_"
                    Case 2 : prefix = "Spot2_"
                    Case 3 : prefix = "FPC_"
                End Select
                For c = 0 To displayColCount - 1
                    Dim suffix As String = ""
                    Select Case c
                        Case 0 : suffix = "2_Column"
                        Case 1 : suffix = "3_Column"
                        Case 2 : suffix = "4_Column"
                        Case 3 : suffix = "5_Column"
                        Case 4 : suffix = "7_Column"
                        Case 5 : suffix = "8_Column"
                    End Select
                    colspecs &= prefix & suffix & " int NOT NULL,"
                Next
            Next
            colspecs = colspecs.Substring(0, colspecs.Length - 1)          'stip last comma


            Cmd.CommandText = "ALTER TABLE dbo." & Tablename & " ADD " & colspecs
            Cmd.ExecuteNonQuery()
            '
            ' Add data to rows by recombining the four arrays horizontally
            '
            For r = 0 To _depthNames.Length - 1
                Dim colvalues As String = "'" & _depthNames(r) & "',"
                For c = 0 To displayColCount - 1
                    Dim newtoken As String = _MonoTable(r, c).ToString & "," & _Spot1Table(r, c).ToString & "," & _Spot2Table(r, c).ToString & "," & _FPCTable(r, c).ToString & ","
                    colvalues &= newtoken
                Next
                colvalues = colvalues.Substring(0, colvalues.Length - 1)       'stip last comma

                Cmd.CommandText = "INSERT INTO dbo." & Tablename & " VALUES (" & colvalues & ")"
                Cmd.ExecuteNonQuery()
            Next


        Finally
            Cmd.Connection.Dispose()            'close and dispose connection
        End Try

    End Sub



    ''' <summary>
    ''' Reads the entire display table and loads the 4 in memory arrays
    ''' </summary>
    ''' <param name="tableName"></param>
    ''' <remarks></remarks>
    Public Sub LoadClassadTable(ByVal tableName As String)
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        '
        ' fetch sql data
        '
        Cmd.CommandText = "SELECT * FROM dbo." & tableName & " ORDER BY ID"
        Dim ds As New DataSet

        Try
            Cmd.Connection.Open()
            Dim SQLAdaptor As New SqlDataAdapter(Cmd)
            SQLAdaptor.Fill(ds)
            Dim dt As DataTable = ds.Tables(0)

            Dim rowCount As Integer = dt.Rows.Count
            '
            ' set table sizes
            '
            ReDim _ClassadTable(rowCount - 1, classadColCount - 1)
            '
            ' extract the data into the tables
            '
            For r As Integer = 0 To rowCount - 1
                For c As Integer = 0 To classadColCount - 1
                    _ClassadTable(r, c) = Convert.ToInt32(dt.Rows(r)(c + 1))      'skip ID
                Next
            Next

        Finally
            Cmd.Connection.Dispose()
        End Try

    End Sub



    ''' <summary>
    ''' Reads the entire display table and loads the 4 in memory arrays
    ''' </summary>
    ''' <param name="tableName"></param>
    ''' <remarks></remarks>
    Public Sub LoadDisplayTable(ByVal tableName As String)
        Dim Cmd As New SqlCommand
        Cmd.Connection = getConnection()
        '
        ' fetch sql data
        '
        Cmd.CommandText = "SELECT * FROM dbo." & tableName & " ORDER BY ID"
        Dim ds As New DataSet

        Try
            Cmd.Connection.Open()
            Dim SQLAdaptor As New SqlDataAdapter(Cmd)
            SQLAdaptor.Fill(ds)
            Dim dt As DataTable = ds.Tables(0)

            Dim rowCount As Integer = dt.Rows.Count
            '
            ' set table sizes
            '
            ReDim _MonoTable(rowCount - 1, displayColCount - 1)
            ReDim _Spot1Table(rowCount - 1, displayColCount - 1)
            ReDim _Spot2Table(rowCount - 1, displayColCount - 1)
            ReDim _FPCTable(rowCount - 1, displayColCount - 1)
            ReDim _depthNames(rowCount - 1)
            '
            ' extract the data into the tables
            '
            For r As Integer = 0 To rowCount - 1
                _depthNames(r) = dt(r)(1).ToString
                For c As Integer = 0 To displayColCount - 1
                    Dim cx As Integer = 2 + (c << 2)          'multiply by 4
                    _MonoTable(r, c) = Convert.ToInt32(dt.Rows(r)(cx))
                    _Spot1Table(r, c) = Convert.ToInt32(dt.Rows(r)(cx + 1))
                    _Spot2Table(r, c) = Convert.ToInt32(dt.Rows(r)(cx + 2))
                    _FPCTable(r, c) = Convert.ToInt32(dt.Rows(r)(cx + 3))
                Next
            Next

        Finally
            Cmd.Connection.Dispose()
        End Try

    End Sub


    Public Function GetClassadRate(ByVal AdInstance As AdInstance) As Integer
        '
        ' map display width to column index
        '
        Dim columnIndex As Integer
        Select Case AdInstance.ProductType
            Case Product.Types.ClassadTextOnly : columnIndex = 0
            Case Product.Types.ClassadMonoPic : columnIndex = 1
            Case Product.Types.ClassadColorPic : columnIndex = 2
        End Select
        '
        ' depthindex is height -1
        '
        Dim depthIndex As Integer = AdInstance.Height - 2
        Return _ClassadTable(depthIndex, columnIndex)

    End Function

    Public Function GetDisplayRate(ByVal AdInstance As AdInstance) As Integer
        '
        ' map display width to column index
        '
        Dim columnIndex As Integer
        Select Case AdInstance.Width
            Case DisplayWidths.Col2 : columnIndex = 0
            Case DisplayWidths.Col3 : columnIndex = 1
            Case DisplayWidths.Col4 : columnIndex = 2
            Case DisplayWidths.Col5 : columnIndex = 3
            Case DisplayWidths.Col7 : columnIndex = 4
            Case DisplayWidths.Col8 : columnIndex = 5
        End Select
        '
        ' depthindex is height -1
        '
        Dim depthIndex As Integer = AdInstance.Height - 1

        Select Case AdInstance.Color
            Case DisplayColorTypes.Mono : Return _MonoTable(depthIndex, columnIndex)
            Case DisplayColorTypes.Spot1 : Return _Spot1Table(depthIndex, columnIndex)
            Case DisplayColorTypes.Spot2 : Return _Spot2Table(depthIndex, columnIndex)
            Case DisplayColorTypes.FPC : Return _FPCTable(depthIndex, columnIndex)
        End Select

    End Function

    Public Function GetWebRate(ByVal AdInstance As AdInstance) As Integer
        '
        ' web ads not charged for - return 0 all cases
        '
        Select Case AdInstance.ProductType
            Case Product.Types.WebBasic : Return 0
            Case Product.Types.WebPremium : Return 0
            Case Product.Types.WebPDF : Return 0
            Case Product.Types.WebPDFText : Return 0
            Case Product.Types.WebFeaturedAd : Return 0
            Case Else : Return 0
        End Select

    End Function



    Private Function getConnection() As SqlConnection
        If _connectionstring Is Nothing Then _connectionstring = ConfigurationManager.ConnectionStrings("ATConnectionString").ConnectionString
        Return New SqlConnection(_connectionstring)
    End Function


End Class
