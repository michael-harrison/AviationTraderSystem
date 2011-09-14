Imports System


'***************************************************************************************
'*
'* Common routines
'*
'*
'* Provides commonly used small routines accessible to all without having to raise an object 
'*
'***************************************************************************************


''' <summary>
''' The CommonRoutines module provides a set of static methods which can be used as required. This centralizes
''' code to a common point for frequently used one line routines.
''' </summary>
Public Module CommonRoutines

    ''' <summary>
    ''' Relative path of the web application. This field
    ''' is normally plugged from Request.ApplicationPath, and is used as a property in databinding
    ''' </summary>
    Public Function GetApplicationPath() As String
        Dim appPath As String = System.Web.HttpRuntime.AppDomainAppVirtualPath
        If Not (appPath.EndsWith("/")) Then appPath &= "/"
        Return appPath
    End Function


    ''' <summary>
    ''' returns a random integer in the range 1 to 2,147,483,647. 
    ''' This is used as a query string for image output
    ''' to ensure that each image name is unique and is not cached by the browser
    ''' </summary>
    Public Function RandomInteger() As Integer
        Static rand As New Random
        Return rand.Next()
    End Function

    ''' <summary>
    ''' returns a random integer in the range 1 to 2,147,483,647. 
    ''' This is used as a query string for image output
    ''' to ensure that each image name is unique and is not cached by the browser
    ''' </summary>
    Public Function RandomInteger(ByVal lo As Integer, ByVal hi As Integer) As Integer
        Static rand As New Random()
        Return rand.Next(lo, hi)
    End Function

    ''' <summary>
    ''' randomises an integer array. Use for unsorting collection of retrieved objects into random order
    ''' </summary>
    ''' <param name="items"></param>
    ''' <remarks></remarks>
    Public Sub RandomizeArray(ByVal items() As Integer)
        '
        ' turn the supplied items() into a randomised array
        '
        Dim max_index As Integer = items.Length - 1

        For i = 0 To max_index
            items(i) = i
        Next

        Dim rnd As New Random
        For i As Integer = 0 To max_index - 1
            ' Pick an item for position i.
            Dim j As Integer = rnd.Next(i, max_index + 1)

            ' Swap them.
            Dim temp As Integer = items(i)
            items(i) = items(j)
            items(j) = temp
        Next i
    End Sub

    ''' <summary>
    ''' Diagnostic routine to display text string as byte values in the VS Watch window
    ''' </summary>
    ''' <param name="s">Sting to display</param>
    ''' <returns>Byte array of character values</returns>
    Public Function String2ByteArray(ByVal s As String) As Byte()
        Dim enc As Text.Encoding = New Text.ASCIIEncoding()
        Return enc.GetBytes(s)
    End Function

    ''' <summary>
    ''' HTML Conditional output. Conditionally outputs the first parameter only if it is non-blank, and 
    ''' if so, follows this with the second parameter, which can be a literal like "<BR/>"
    ''' This function therefore provides a basic HTML conditional formatter at the markup level
    ''' </summary>
    ''' <param name="param1">parameter to conditionally outuput</param>
    ''' <param name="param2">second parameter, follows first only if first output</param>
    ''' <returns>String to render</returns>
    Public Function Cout(ByVal param1 As String, ByVal param2 As String) As String
        If Not String.IsNullOrEmpty(param1) Then
            Return param1 & param2
        Else
            Return ""
        End If

    End Function

    ''' <summary>
    ''' Converts a string containing one or more CRLF line end characters as supplied in textarea control into a LF character.
    '''  This is used so that InDesign will produce a hard line end rather than a Para / Line end sequence
    ''' </summary>
    ''' <param name="s">the string field containing crlf sequences</param>
    ''' <returns>The string converted as specified</returns>
    Public Function ConvertCRLF2LF(ByVal s As String) As String
        '
        ' convert user-supplied CRLF to just LF. This means that
        ' when the textarea option is used, InDesign produces a non-para hard line end.
        '
        Dim bCRLF As Byte() = {&HD, &HA}
        Dim bLF As Byte() = {&HA}

        Dim enc As New Text.ASCIIEncoding
        Dim CRLF As String = enc.GetString(bCRLF)
        Dim LF As String = enc.GetString(bLF)
        Return s.Replace(CRLF, LF)
    End Function

    Public Function ConvertLFLF2LF(ByVal s As String) As String
        '
        ' Strips one LF from an LF LF LF sequence. Used to remove blank lines in textarea
        ' option
        '
        Dim bLFLF As Byte() = {&HA, &HA}
        Dim bLF As Byte() = {&HA}

        Dim enc As New Text.ASCIIEncoding
        Dim LFLF As String = enc.GetString(bLFLF)
        Dim LF As String = enc.GetString(bLF)
        Return s.Replace(LFLF, LF)
    End Function


    ''' <summary>
    ''' Validates a string to be a valid price in dollars and cents. Exception thrown if validation fails
    ''' </summary>
    ''' <param name="fieldname">the field name to include in the exception message</param>
    ''' <param name="value">String to validate</param>
    ''' <param name="min">minimum range in cents</param>
    ''' <param name="max">maximum range in cents</param>
    ''' <returns>an integer value of the string, converted to cents</returns>
    Public Function validateDollars(ByVal fieldname As String, ByVal value As String, ByVal min As Integer, ByVal max As Integer) As Integer
        '
        ' checks that the supplied field is in correct format 
        '
        Try
            Dim n As Integer = Dollars2Integer(value)
            If n < min Then Throw New Exception
            If n > max Then Throw New Exception
            Return n

        Catch ex As Exception
            Dim exmsg As String = "Invalid Field - " & fieldname & " must be between " & Integer2Dollars(min) & " and " & Integer2Dollars(max)
            Throw New Exception(exmsg)
            Return 0
        End Try

    End Function


    ''' <summary>
    ''' Converts a 32 bit integer to an eight character hex string value
    ''' </summary>
    ''' <param name="value">integer value to convert</param>
    ''' <returns>fixed length hex string of 8 characters</returns>
    Public Function Int2Hex(ByVal value As Integer) As String
        Return value.ToString("X8")
    End Function

    ''' <summary>
    ''' Converts a 32 bit integer to an eight character hex string value
    ''' </summary>
    ''' <param name="value">integer value to convert</param>
    ''' <returns>fixed length hex string of 8 characters</returns>
    Public Function Int2ShortHex(ByVal value As Integer) As String
        Return value.ToString("X2")
    End Function

    ''' <summary>
    ''' Converts a fixed length hex string of eight characters to a 32 bit integer
    ''' </summary>
    ''' <param name="value">Hex string to convert</param>
    ''' <returns>integer value</returns>
    Public Function Hex2Int(ByVal value As String) As Integer
        Return Int32.Parse(value, Globalization.NumberStyles.HexNumber)
    End Function

    ''' <summary>
    ''' Converts an integer value in cents to a dollars and cents string in the form nn.dd
    ''' </summary>
    ''' <param name="value">number of cents</param>
    ''' <returns>dollar value</returns>
    Public Function Integer2Dollars(ByVal value As Integer) As String
        '
        ' converts integer cents to nn.cc
        '
        Dim d As Double = value / 100
        Return FormatNumber(d, 2)

    End Function

    ''' <summary>
    ''' Converts a dollar value as a string into cents
    ''' </summary>
    ''' <param name="value">Dollar value in the form nn.dd</param>
    ''' <returns>Number of cents</returns>
    Public Function Dollars2Integer(ByVal value As String) As Integer
        '
        ' converts nn.cc to cents
        '
        Dim d As Double = Convert.ToDouble(value)
        Return Convert.ToInt32(d * 100)
    End Function

    ''' <summary>
    ''' Converts an integer value into mm, according to the place count. For example
    ''' 4350 with a place count of 3 would return 4.350
    ''' </summary>
    ''' <param name="value">value consistent with place count</param>
    ''' <param name="places">number of places</param>
    ''' <returns>String value, converted consistent with placecount</returns>
    Public Function Integer2mm(ByVal value As Integer, ByVal places As Integer) As String
        Dim d As Double = value / (10 ^ places)
        Return FormatNumber(d, places)
    End Function

    ''' <summary>
    ''' Converts a string value as a string into integer * 1000
    ''' </summary>
    ''' <param name="value">Dollar value in the form nn.dd</param>
    ''' <returns>Number of cents</returns>
    Public Function mm2Integer(ByVal value As String) As Integer

        Dim d As Double = Convert.ToDouble(value)
        Return Convert.ToInt32(d * 1000)
    End Function

    ''' <summary>
    ''' Converts an IPV4 address of four bytes into a 32 bit integer
    ''' </summary>
    ''' <param name="IP">Byte array of four bytes</param>
    ''' <returns>Integer value</returns>
    Public Function IPBytes2Int(ByVal IP() As Byte) As Integer

        Dim a1 As Integer = IP(0)
        Dim a2 As Integer = IP(1)
        Dim a3 As Integer = IP(2)
        Dim a4 As Integer = IP(3)
        Return (a1 << 24) Or (a2 << 16) Or (a3 << 8) Or a4
    End Function

    ''' <summary>
    ''' Converts an IP address expressed as an integer into a string, eg 192.168.0.1
    ''' </summary>
    ''' <param name="IP">IP address as integer</param>
    ''' <returns>String representation of address</returns>
    Public Function IPInt2String(ByVal IP As Integer) As String

        Dim a1 As Integer = &HFF And (IP >> 24)
        Dim a2 As Integer = &HFF And (IP >> 16)
        Dim a3 As Integer = &HFF And (IP >> 8)
        Dim a4 As Integer = &HFF And IP

        Return a1.ToString & "." & a2.ToString & "." & a3.ToString & "." & a4.ToString
    End Function

    ''' <summary>
    ''' Sanitizes the supplied string by triming left and right spaces, stripping illegal characters, and truncates it to 
    ''' ATSystem.SysConstants.CharLength in length if necessary.
    ''' </summary>
    ''' <param name="s">string value</param>
    ''' <returns>stripped and truncated string</returns>
    Public Function Sanitize(ByVal s As String) As String
        Return Sanitize(s, ATSystem.SysConstants.charLength)
    End Function

    ''' <summary>
    ''' Sanitizes the supplied string by triming left and right spaces, stripping illegal characters, and truncates it to 
    ''' the supplied maxLength param if necessary.
    ''' </summary>
    ''' <param name="s">string value</param>
    ''' <param name="maxlength">maximum length of returned string</param>
    ''' <returns>stripped and truncated string</returns>
    Public Function Sanitize(ByVal s As String, ByVal maxlength As Integer) As String
        '
        ' All text user input should be wrapped by this function to clean text before it enters the db
        '
        ' 1. trims left and right spaces
        ' 2. sanitizes it by removing unacceptable characters
        ' 3. truncates to the supplied maxlength.
        '
        Dim rtnval As String = s
        '
        ' trim
        '
        rtnval = s.Trim()
        '
        ' sanitize
        '
        ''     rtnval = rtnval.Replace("&", "")
        rtnval = rtnval.Replace("<", "")
        rtnval = rtnval.Replace(">", "")
        ''    rtnval = rtnval.Replace("/", "")
        rtnval = rtnval.Replace("\", "")
        ''       rtnval = rtnval.Replace(",", "")
        '
        ' truncate
        '
        Return TruncateText(rtnval, maxlength)
    End Function

    Public Function TruncateText(ByVal s As String, ByVal maxLength As Integer) As String
        '
        ' truncates supplied text to the supplied length
        '
        If s.Length > maxLength Then
            Return s.Substring(0, maxLength)
        Else
            Return s
        End If
    End Function

    ''' <summary>
    ''' Maps a standard image extension eg .bmp, .eps, .gif etc to the Image.Imagetypes values
    ''' </summary>
    ''' <param name="xtn">string value of extension</param>
    ''' <returns>enum value of extension</returns>
    Public Function Xtn2Type(ByVal xtn As String) As Image.ImageTypes
        '
        ' maps the file extension to the image type
        '
        Select Case xtn.ToLower
            Case ".bmp" : Return Image.ImageTypes.BMP
            Case ".eps" : Return Image.ImageTypes.EPS
            Case ".gif" : Return Image.ImageTypes.GIF
            Case ".jpg" : Return Image.ImageTypes.JPG
            Case ".jpeg" : Return Image.ImageTypes.JPG
            Case ".pdf" : Return Image.ImageTypes.PDF
            Case ".png" : Return Image.ImageTypes.PNG
            Case ".tif" : Return Image.ImageTypes.TIF
            Case ".swf" : Return Image.ImageTypes.SWF
            Case Else : Return Image.ImageTypes.Unknown
        End Select

    End Function

    ''' <summary>
    ''' Maps an enum from Image.Image types to  a standard image extension eg .bmp, .eps, .gif etc
    ''' </summary>
    ''' <param name="imagetype">Imagetype from Image.ImageTypes enum</param>
    ''' <returns>string extension</returns>
    Public Function Type2Xtn(ByVal imagetype As Image.ImageTypes) As String
        '
        ' maps the image type to return the file extension
        '
        Select Case imagetype
            Case Image.ImageTypes.BMP : Return ".bmp"
            Case Image.ImageTypes.EPS : Return ".eps"
            Case Image.ImageTypes.GIF : Return ".gif"
            Case Image.ImageTypes.JPG : Return ".jpg"
            Case Image.ImageTypes.PDF : Return ".pdf"
            Case Image.ImageTypes.PNG : Return ".png"
            Case Image.ImageTypes.TIF : Return ".tif"
            Case Image.ImageTypes.SWF : Return ".swf"
            Case Else : Return ".xxx"
        End Select

    End Function

    ''' <summary>
    ''' Checks if a file is in use - that is if it is open anywhere
    ''' </summary>
    ''' <param name="filename">fully qualified filename to check</param>
    ''' <returns>True if the file is open , false otherwise</returns>
    Public Function IsFileInUse(ByVal filename As String) As Boolean
        '
        ' checks if the file is open somewhere else
        '
        Try
            Dim file As IO.FileStream = IO.File.Open(filename, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.None)
            file.Close()
            Return False
        Catch ex As Exception
            Return True
        End Try

    End Function

End Module

