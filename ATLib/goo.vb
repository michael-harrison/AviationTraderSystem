Option Strict On
Option Explicit On
Imports System
Imports System.Net
Imports System.Text.RegularExpressions



'***************************************************************************************
'*
'* Enumerator assistant
'*
'*
'*
'***************************************************************************************


''' <summary>
''' Calls the google gl shortener to obtain a short url from a supplied url
''' </summary>
Public Class goo

    Private Const m_postFormat As String = "&user=toolbar@google.com&url={0}&auth_token={1}"

    Public Function Shorten(ByVal URL As String) As String
        Dim rtnval As String = ""
        Dim token As String = generateAuthToken(URL)
        Dim post As String = String.Format(m_postFormat, System.Web.HttpUtility.UrlEncode(URL), token)
        Dim request As HttpWebRequest = CType(HttpWebRequest.Create("http://goo.gl/api/url"), HttpWebRequest)
        request.Method = WebRequestMethods.Http.Post
        request.UserAgent = "toolbar"
        request.ContentLength = post.Length
        request.ContentType = "application/x-www-form-urlencoded"
        request.Headers.Add("Cache-Control", "no-cache")
        Using requestStream As IO.Stream = request.GetRequestStream()
            Dim postbuffer As Byte() = System.Text.Encoding.ASCII.GetBytes(post)
            requestStream.Write(postbuffer, 0, postbuffer.Length)
        End Using

        Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
            Using responseStream As IO.Stream = response.GetResponseStream()
                Using responseReader As IO.StreamReader = New IO.StreamReader(responseStream)
                    Dim json As String = responseReader.ReadToEnd
                    Dim sep As Char = Chr(&H22)
                    Dim arr As String() = json.Split(sep)
                    If arr.Length > 3 Then rtnval = arr(3)
                    
                End Using
            End Using
        End Using
        Return rtnval
    End Function

    Private Function generateAuthToken(ByVal b As String) As String
        Dim i As Long = _e(b)
        i = i >> 2 And 1073741823
        i = i >> 4 And 67108800 Or i And 63
        i = i >> 4 And 4193280 Or i And 1023
        i = i >> 4 And 245760 Or i And 16383

        Dim h As Long = _f(b)
        Dim k As Long = (i >> 2 And 15) << 4 Or h And 15
        k = k Or (i >> 6 And 15) << 12 Or (h >> 8 And 15) << 8
        k = k Or (i >> 10 And 15) << 20 Or (h >> 16 And 15) << 16
        k = k Or (i >> 14 And 15) << 28 Or (h >> 24 And 15) << 24

        Return "7" + _d(k)

    End Function

    Private Function _c(ByVal a As Long, ByVal b As Long, ByVal c As Long) As Long
        Dim l As Long = 0
        l += (a And 4294967295)
        l += (b And 4294967295)
        l += (c And 4294967295)
        Return l
    End Function

    Private Function _c(ByVal a As Long, ByVal b As Long, ByVal c As Long, ByVal d As Long) As Long
        Dim l As Long = 0
        l += (a And 4294967295)
        l += (b And 4294967295)
        l += (c And 4294967295)
        l += (d And 4294967295)
        Return l
    End Function


    Private Function _d(ByVal l As Long) As String
        Dim ll As String = l.ToString
        Dim m As String = IIf(l > 0, l, l + 4294967296).ToString()
        Dim n As Boolean = False
        Dim o As Long = 0

        For p As Integer = m.Length - 1 To 0 Step -1
            Dim q As Long = Int64.Parse(m(p).ToString())
            If n Then
                q *= 2
                o += CType(Math.Floor(q / 10) + q Mod 10, Long)
            Else
                o += q
            End If
            n = Not n
        Next

        Dim mm As Long = o Mod 10

        o = 0
        If mm <> 0 Then
            o = 10 - mm
            If ll.Length Mod 2 = 1 Then
                If o Mod 2 = 1 Then o += 9
                o = CType(0 / 2, Long)
            End If
        End If
        m = 0.ToString
        m &= ll
        Return m
    End Function

    Private Function _e(ByVal l As String) As Long
        Dim m As Long = 5381
        For o As Integer = 0 To l.Length - 1
            m = _c(m << 5, m, AscW(l(o)))
        Next
        Return m
    End Function

    Private Function _f(ByVal l As String) As Long
        Dim m As Long = 9
        For o As Integer = 0 To l.Length - 1
            m = _c(AscW(l(0)), m << 6, m << 16, -m)
        Next
        Return m
    End Function

End Class
