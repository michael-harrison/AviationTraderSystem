Imports System


'***************************************************************************************
'*
'* Input Validator
'*
'*
'* Provides a common set of validation routines for all user input 
'*
'***************************************************************************************

Public Class InputValidator

    Private _success As Boolean
    Private _errorCount As Integer
    Private _minValue As Integer
    Private _maxValue As Integer
    Private _minStringLength As Integer
    Private _maxStringLength As Integer

    Public Sub New()
        _success = True           'assume success
        _errorCount = 0               'and no errors
    End Sub

    Public Property MinValue() As Integer
        Get
            Return _minValue
        End Get
        Set(ByVal value As Integer)
            _minValue = value
        End Set
    End Property

    Public Property MaxValue() As Integer
        Get
            Return _maxValue
        End Get
        Set(ByVal value As Integer)
            _maxValue = value
        End Set
    End Property

    Public Property MinStringLength() As Integer
        Get
            Return _minStringLength
        End Get
        Set(ByVal value As Integer)
            _minStringLength = value
        End Set
    End Property

    Public Property MaxStringLength() As Integer
        Get
            Return _maxStringLength
        End Get
        Set(ByVal value As Integer)
            _maxStringLength = value
        End Set
    End Property


    Public ReadOnly Property Success() As Boolean
        Get
            Return _success
        End Get
    End Property

    Public ReadOnly Property ErrorCount() As Integer
        Get
            Return _errorCount
        End Get
    End Property

    Public Function ValidateText(ByVal InputBox As System.Web.UI.WebControls.TextBox, ByVal ErrorLabel As System.Web.UI.WebControls.Label) As String

        ErrorLabel.Visible = False
   
        Dim s As String = InputBox.Text.Trim()

        If s.Length = 0 And _minStringLength > 0 Then
            ErrorLabel.Text = "This value must be supplied"
            ErrorLabel.Visible = True
            _errorCount += 1
            _success = False

        ElseIf s.Length < _minStringLength Then
            ErrorLabel.Text = "This value must contain least " & _minStringLength & " characters"
            ErrorLabel.Visible = True
            _errorCount += 1
            _success = False

        ElseIf s.Length > _maxStringLength Then
            ErrorLabel.Text = "This value cannot be longer than " & _minStringLength & " characters"
            ErrorLabel.Visible = True
            _errorCount += 1
            _success = False

        Else

            Dim myflag As Boolean = False
            If s.Contains(">") Then myflag = True
            If s.Contains("<") Then myflag = True
            If s.Contains("\") Then myflag = True

            If myflag Then
                ErrorLabel.Text = "Value cannot contain <>\"
                ErrorLabel.Visible = True
                _errorCount += 1
                _success = False
            Else
                Return s
            End If
        End If
        Return ""

    End Function

    Public Function ValidateInteger(ByVal InputBox As System.Web.UI.WebControls.TextBox, ByVal Errorlabel As System.Web.UI.WebControls.Label) As Integer

        Errorlabel.Visible = False
  
        Dim s As String = InputBox.Text.Trim()
        Dim rtnval As Integer

        Try
            rtnval = Convert.ToInt32(s)

            If (rtnval < _minValue) Or (rtnval > _maxValue) Then
                Errorlabel.Text = "Expecting value between " & _minValue & " and " & _maxValue
                Errorlabel.Visible = True
                _errorCount += 1
                _success = False
                rtnval = ATSystem.SysConstants.nullValue
            End If

        Catch ex As Exception
            Errorlabel.Text = "Expecting an integer value"
            Errorlabel.Visible = True
            _errorCount += 1
            _success = False

        End Try

        Return rtnval
    End Function

    Public Function ValidateDateTime(ByVal InputBox As System.Web.UI.WebControls.TextBox, ByVal Errorlabel As System.Web.UI.WebControls.Label) As DateTime

        Errorlabel.Visible = False

        Dim s As String = InputBox.Text.Trim()
        Dim rtnval As DateTime

        Try
            rtnval = Convert.ToDateTime(s)

        Catch ex As Exception
            Errorlabel.Text = "Invalid date - time format"
            Errorlabel.Visible = True
            _errorCount += 1
            _success = False

        End Try

        Return rtnval
    End Function

    Public Function ValidateDollars(ByVal InputBox As System.Web.UI.WebControls.TextBox, ByVal Errorlabel As System.Web.UI.WebControls.Label) As Integer

        Errorlabel.Visible = False
        Dim neg As Boolean = False

        Dim s As String = InputBox.Text.Trim()
        If s.Length = 0 Then Return 0
        '
        ' strip leading $ if its there and watch for sign
        '
        If s.StartsWith("-$") Then
            s = s.Substring(2, s.Length - 2)
            neg = True
        End If

        If s.StartsWith("$-") Then
            s = s.Substring(2, s.Length - 2)
            neg = True
        End If

        If s.StartsWith("$") Then
            s = s.Substring(1, s.Length - 1)
        End If

        Dim rtnval As Integer

        Try
            rtnval = CommonRoutines.Dollars2Integer(s)

        Catch ex As Exception
            Errorlabel.Text = "Invalid Price"
            Errorlabel.Visible = True
            _errorCount += 1
            _success = False

        End Try
        If neg Then rtnval = -rtnval
        Return rtnval
    End Function

    ''' <summary>
    ''' Validates input value in form nnn.nnn and returns as value * 1000
    ''' </summary>
    ''' <param name="InputBox"></param>
    ''' <param name="Errorlabel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Validatemm(ByVal InputBox As System.Web.UI.WebControls.TextBox, ByVal Errorlabel As System.Web.UI.WebControls.Label) As Integer

        Errorlabel.Visible = False
        Dim neg As Boolean = False

        Dim s As String = InputBox.Text.Trim()
        If s.Length = 0 Then Return 0

        Dim rtnval As Integer

        Try
            rtnval = CommonRoutines.mm2Integer(s)

        Catch ex As Exception
            Errorlabel.Text = "Invalid value - expecting nnn.nnn"
            Errorlabel.Visible = True
            _errorCount += 1
            _success = False

        End Try
        If neg Then rtnval = -rtnval
        Return rtnval
    End Function

    Public Function ValidateEmail(ByVal InputBox As System.Web.UI.WebControls.TextBox, ByVal ErrorLabel As System.Web.UI.WebControls.Label) As String

        ErrorLabel.Visible = False
   
        Dim s As String = InputBox.Text.Trim()

        If s.Length = 0 And _minStringLength > 0 Then
            ErrorLabel.Text = "This value must be supplied"
            ErrorLabel.Visible = True
            _errorCount += 1
            _success = False

        ElseIf s.Length > _maxStringLength Then
            ErrorLabel.Text = "This value cannot be longer than " & _minStringLength & " characters"
            ErrorLabel.Visible = True
            _errorCount += 1
            _success = False

        ElseIf s.Length > 0 And Not EmailAddressCheck(s) Then
            ErrorLabel.Text = "Email address is not in the correct form"
            ErrorLabel.Visible = True
            _errorCount += 1
            _success = False
        Else
            Return s
        End If

        Return ""
    End Function

    Public Function ValidateURL(ByVal InputBox As System.Web.UI.WebControls.TextBox, ByVal ErrorLabel As System.Web.UI.WebControls.Label) As String

        ErrorLabel.Visible = False
 
        Dim s As String = InputBox.Text.Trim()

        If s.Length = 0 And _minStringLength > 0 Then
            ErrorLabel.Text = "This value must be supplied"
            ErrorLabel.Visible = True
            _errorCount += 1
            _success = False

        ElseIf s.Length < _minStringLength Then
            ErrorLabel.Text = "This value must contain least " & _minStringLength & " characters"
            ErrorLabel.Visible = True
            _errorCount += 1
            _success = False

        ElseIf s.Length > _maxStringLength Then
            ErrorLabel.Text = "This value cannot be longer than " & _minStringLength & " characters"
            ErrorLabel.Visible = True
            _errorCount += 1
            _success = False

        Else

            Dim myflag As Boolean = False
            If s.Contains(">") Then myflag = True
            If s.Contains("<") Then myflag = True
            If s.Contains("*") Then myflag = True

            If myflag Then
                ErrorLabel.Text = "URL cannot contain <>*"
                ErrorLabel.Visible = True
                _errorCount += 1
                _success = False
            Else
                Return s
            End If
        End If
        Return ""

    End Function

    Public Function ValidatePath(ByVal InputBox As System.Web.UI.WebControls.TextBox, ByVal Errorlabel As System.Web.UI.WebControls.Label) As String

        Errorlabel.Visible = False
       
        Dim s As String = InputBox.Text.Trim()

        If s.Length = 0 And _minStringLength > 0 Then
            Errorlabel.Text = "This value must be supplied"
            Errorlabel.Visible = True
            _errorCount += 1
            _success = False

        ElseIf s.Length < _minStringLength Then
            Errorlabel.Text = "This value must contain least " & _minStringLength & " characters"
            Errorlabel.Visible = True
            _errorCount += 1
            _success = False

        ElseIf s.Length > _maxStringLength Then
            Errorlabel.Text = "This value cannot be longer than " & _minStringLength & " characters"
            Errorlabel.Visible = True
            _errorCount += 1
            _success = False

        Else

            Dim myflag As Boolean = False
            If s.Contains(">") Then myflag = True
            If s.Contains("<") Then myflag = True
            If s.Contains("*") Then myflag = True
            If s.Contains("?") Then myflag = True

            If myflag Then
                Errorlabel.Text = "Path cannot contain <>?*"
                Errorlabel.Visible = True
                _errorCount += 1
                _success = False
            Else
                Return s
            End If
        End If
        Return ""

    End Function

    Public Function ValidateHex(ByVal InputBox As System.Web.UI.WebControls.TextBox, ByVal Errorlabel As System.Web.UI.WebControls.Label) As Integer

        Errorlabel.Visible = False

        Dim s As String = InputBox.Text.Trim()
        Dim rtnval As Integer
        '
        ' the field may be preceed by a #. If so strip it
        '
        If s.Length > 1 Then
            If s.StartsWith("#") Then s = s.Substring(1, s.Length - 1)
        End If

        Try
            rtnval = Hex2Int(s)

        Catch ex As Exception
            Errorlabel.Text = "Expecting an integer value"
            Errorlabel.Visible = True
            _errorCount += 1
            _success = False

        End Try

        Return rtnval
    End Function

    Private Function EmailAddressCheck(ByVal emailAddress As String) As Boolean
        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim emailAddressMatch As Text.RegularExpressions.Match = Text.RegularExpressions.Regex.Match(emailAddress, pattern)
        If emailAddressMatch.Success Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
