Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Drop Ad
'*
'* AUDIT TRAIL
'* 
'* V1.000   01-Dec-2009  BA  Original
'*
'* This is a viewer  to drop an ad
'*
'*
'*
'*
'***************************************************************************************

Public Class DropAd

    Public Event DropAdEvent(ByVal Adinstance As AdInstance)

    Private _AdInstance As AdInstance

    Public ReadOnly Property AdInstance() As AdInstance
        Get
            Return _AdInstance
        End Get
    End Property

    Private Function getInstanceID(ByVal url As String) As Integer
        '
        ' decodes the supplied URL and returns the instance ID if its a valid ad intance
        ' otherwise nullvalue
        '
        Dim rtnval As Integer = ATLib.ATSystem.SysConstants.nullValue
        Dim sep() As String = {Constants.subsampledImagesAdInstance, Constants.InvalidPreviewImage}
        Try
            Dim s() As String = url.Split(sep, StringSplitOptions.None)

            If s.Length = 2 Then

                Dim s2 As String = s(1)
                If s2.StartsWith("?") Then
                    rtnval = CommonRoutines.Hex2Int(s2.Substring(1, 8))
                ElseIf s2.StartsWith("/") Then
                    If s2.EndsWith(".jpg") Then
                        rtnval = CommonRoutines.Hex2Int(s2.Substring(1, 8))
                    End If
                End If
            End If

        Finally
        End Try

        Return rtnval

    End Function

    Private Sub AdPage_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        Dim URL As String = ""

        Const unicodeURL As String = "UniformResourceLocatorW"
        Const asciiURL As String = "UniformResourceLocator"
        If DoesDDDataContainURL(e.Data, unicodeURL) Then
            URL = readURLfromData(e.Data, unicodeURL, System.Text.Encoding.Unicode)
        ElseIf DoesDDDataContainURL(e.Data, asciiURL) Then
            URL = readURLfromData(e.Data, asciiURL, System.Text.Encoding.ASCII)
        End If
        Me.BackColor = Color.Beige
        Dim instanceID As Integer = getInstanceID(URL)
        If instanceID = ATSystem.SysConstants.nullValue Then
            ErrorBox.Text = "Unrecognised ad instance URL" & URL
        Else
            Dim myInstances As New AdInstances
            _AdInstance = myInstances.Retrieve(instanceID)
            If _AdInstance Is Nothing Then
                ErrorBox.Text = "This ad instance " & instanceID & " does not exist in the database"
            Else
                '
                ' replace myself with the instanceviewer and sync the tree by a call to parent
                '
                RaiseEvent DropAdEvent(_AdInstance)
            End If
        End If

    End Sub

    Private Sub AdPage_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter


        Const unicodeURL As String = "UniformResourceLocatorW"
        Const asciiURL As String = "UniformResourceLocator"
        If DoesDDDataContainURL(e.Data, unicodeURL) Then
            Me.BackColor = Color.Coral
            e.Effect = DragDropEffects.Copy
        ElseIf DoesDDDataContainURL(e.Data, asciiURL) Then
            Me.BackColor = Color.Coral
            e.Effect = DragDropEffects.Copy

        End If
    End Sub

    Private Sub AdPage_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DragLeave
        Me.BackColor = Color.Beige
    End Sub


    Private Function DoesDDDataContainURL(ByVal data As IDataObject, ByVal dataFormatName As String) As Boolean
        Dim rtnval As Boolean = False
        If Not data Is Nothing Then
            If data.GetDataPresent(dataFormatName) Then rtnval = True
        End If
        Return rtnval

    End Function

    Private Function readURLfromData(ByVal data As IDataObject, ByVal dataFormat As String, ByVal encoding As System.Text.Encoding) As String
        Dim url As String = ""

        Using urlStream As IO.MemoryStream = CType(data.GetData(dataFormat), IO.MemoryStream)
            If Not urlStream Is Nothing Then

                Using tr As IO.TextReader = New IO.StreamReader(urlStream, encoding)
                    url = tr.ReadToEnd
                End Using
            End If
        End Using
        Return url.Substring(0, url.Length - 1)

    End Function

End Class
