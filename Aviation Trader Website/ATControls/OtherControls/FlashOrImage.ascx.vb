Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* FlashOrImage
'*
'*  Inserts either a flash graphic or an image into the space, depending on the ImageType value
'*
'*
'***************************************************************************************
Partial Class ATControls_OtherControls_FlashOrImage
    Inherits System.Web.UI.UserControl
    Private _imageType As RotatorAd.Types
    Private _imageURL As String
    Private _height As Integer
    Private _width As Integer


    Public Property ImageType() As RotatorAd.Types
        Get
            Return _imageType
        End Get
        Set(ByVal value As RotatorAd.Types)
            _imageType = value
        End Set
    End Property

    Public Property ImageURL() As String
        Get
            Return _imageURL
        End Get
        Set(ByVal value As String)
            _imageURL = value
        End Set
    End Property

    Public Property Height() As Integer
        Get
            Return _height
        End Get
        Set(ByVal value As Integer)
            _height = value
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return _width
        End Get
        Set(ByVal value As Integer)
            _width = value
        End Set
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.EnableViewState = False
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.Controls.Add(New Imgx(_imageType, _imageURL, _width, _height))
    End Sub

End Class

Friend Class Imgx
    Inherits Panel


    Public Sub New(ByVal imageType As RotatorAd.Types, ByVal imageURL As String, ByVal width As Integer, ByVal height As Integer)
        '
        ' generate the right code for the ad type
        '
        Select Case imageType
            Case RotatorAd.Types.Flash : addFlash(imageURL, width, height)
            Case RotatorAd.Types.Image : addImage(imageURL, width, height)
        End Select

    End Sub

    Private Sub addFlash(ByVal imageURL As String, ByVal width As Integer, ByVal height As Integer)

        Me.Width = width
        Me.Height = height

        Dim flashobject As New HtmlGenericControl

        flashobject.TagName = "object"
        flashobject.Attributes.Add("width", width.ToString)
        flashobject.Attributes.Add("height", height.ToString)
        flashobject.Attributes.Add("type", "application/x-shockwave-flash")
        flashobject.Attributes.Add("data", imageURL)

        Dim param As HtmlGenericControl

        param = New HtmlGenericControl
        param.TagName = "param"
        param.Attributes.Add("name", "movie")
        param.Attributes.Add("value", imageURL)
        flashobject.Controls.Add(param)

        param = New HtmlGenericControl
        param.TagName = "param"
        param.Attributes.Add("name", "quality")
        param.Attributes.Add("value", "high")
        flashobject.Controls.Add(param)

        param = New HtmlGenericControl
        param.TagName = "param"
        param.Attributes.Add("name", "bgcolor")
        param.Attributes.Add("value", "#ffffff")
        flashobject.Controls.Add(param)

        param = New HtmlGenericControl
        param.TagName = "param"
        param.Attributes.Add("name", "wmode")
        param.Attributes.Add("value", "opaque")
        flashobject.Controls.Add(param)

        param = New HtmlGenericControl
        param.TagName = "param"
        param.Attributes.Add("name", "allowScriptAccess")
        param.Attributes.Add("value", "sameDomain")
        flashobject.Controls.Add(param)

        Me.Controls.Add(flashobject)

    End Sub

    Private Sub addImage(ByVal imageURL As String, ByVal width As Integer, ByVal height As Integer)

        Me.Width = width
        Me.Height = height

        Dim img As New HtmlImage
        img.Width = width
        img.Height = height
        img.Src = imageURL
        Me.Controls.Add(img)

    End Sub

End Class

