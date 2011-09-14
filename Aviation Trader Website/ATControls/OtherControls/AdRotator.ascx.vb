Option Strict On
Option Explicit On
Imports ATLib



'***************************************************************************************
'*
'* Ad Rotator
'*
'*  Chooses the next ad from the list in a random sequence and display its
'*
'*
'***************************************************************************************
Partial Class ATControls_AdRotator
    Inherits System.Web.UI.UserControl
    Private _category As RotatorAd.Categories
    Private _height As Integer


    Public Property category() As RotatorAd.Categories
        Get
            Return _category
        End Get
        Set(ByVal value As RotatorAd.Categories)
            _category = value
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.EnableViewState = False
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        '
        ' get the complete set of candidate ads for the category
        '
        Dim rotatorads As New RotatorAds
        rotatorads.retrieveSet(_category)
        '
        ' unsort the retrieved set into a randomised array
        '
        Dim count As Integer = rotatorads.Count - 1
        Dim indexList(count) As Integer
        CommonRoutines.RandomizeArray(indexList)
        ' 
        ' stack the ads into the available space, randomly
        '
        Dim spaceRemaining As Integer = _height
        Do While spaceRemaining > 0
            Dim nextAd As RotatorAd = getNextAd(rotatorads, indexList, spaceRemaining)
            If nextAd Is Nothing Then Exit Do 'nothing more to do
            Me.Controls.Add(New AdSlot(nextAd))
            spaceRemaining -= (nextAd.Height + nextAd.MarginTop + nextAd.MarginBottom)
            nextAd.UsageCount += 1                'bump usage count
            nextAd.Update()
        Loop
    End Sub

    Private Function getNextAd(ByVal ads As RotatorAds, ByVal indexList() As Integer, ByVal spaceRemaining As Integer) As RotatorAd
        '
        ' returns the first available ad from the randomized list, which has not been used, consistent with
        ' the remaining space
        '
        For i As Integer = 0 To indexList.Length - 1
            Dim idx As Integer = indexList(i)
            If idx > -1 Then                        'ad has not been used yet
                Dim ad As RotatorAd = ads(idx)
                '
                ' if the ad will fit, then use it
                '
                If (ad.Height + ad.MarginTop + ad.MarginBottom) <= spaceRemaining Then
                    indexList(i) = -1             'show this ad as used
                    Return ad
                End If
            End If
        Next
        '
        ' if we get to here, there are no more ads that are suitable
        ' either because the list is exhausted or they are all too big
        ' 
        Return Nothing

    End Function

  

End Class

Friend Class AdSlot
    Inherits Panel

    Public Sub New()
        '
        ' generate the nullad code
        ''

    End Sub


    Public Sub New(ByVal Ad As RotatorAd)
        '
        ' generate the right code for the ad type
        '
        Select Case Ad.Type
            Case RotatorAd.Types.Flash : addFlash(Ad)
            Case RotatorAd.Types.Image : addImage(Ad)
        End Select

    End Sub

    Private Sub addFlash(ByVal Ad As RotatorAd)
        '
        ' Note - there is no way that a flash object will propagate onclick.
        ' Need to  spec to the flash programmers to call an external javascript function:
        '
        '
        '//This is what your AS code should look like: 
        'import flash.external.ExternalInterface; 
        'import flash.events.MouseEvent; 

        'root.addEventListener( MouseEvent.CLICK, useExternal, true ); 

        'function useExternal( event:MouseEvent):void 
        '{ 
        '//swfClickFunction is defined in JavaScript 
        'ExternalInterface.call( "incrCount" ); 
        '} 
        'Unless you are able to do either of the above, Flash will "eat" your mouse events and there is nothing which can be done about that.
        '
        Dim marginSpec As String = "cursor:pointer;margin-top:" & Ad.MarginTop & "px;margin-bottom:" & Ad.MarginBottom & "px;margin-left:" & Ad.MarginLeft & "px;margin-right:" & Ad.MarginRight & "px"
        Dim bannerAdContainer As String
        Dim flashobject As New HtmlGenericControl
        Dim param As HtmlGenericControl

        Me.Attributes.Add("style", marginSpec)
        Me.Width = Ad.Width
        Me.Height = Ad.Height


        If (Ad.Width = 180) Then
            If (Ad.Height = 50) Then
                bannerAdContainer = "AdRotators/flash/banner_ad_container_180x50.swf"
            ElseIf (Ad.Height = 110) Then
                bannerAdContainer = "AdRotators/flash/banner_ad_container_180x110.swf"
            Else
                bannerAdContainer = "AdRotators/flash/banner_ad_container_180x220.swf"
            End If
        Else
            bannerAdContainer = "AdRotators/flash/banner_ad_container_450.swf"
        End If

        flashobject.TagName = "object"
        flashobject.Attributes.Add("width", Ad.Width.ToString)
        flashobject.Attributes.Add("height", Ad.Height.ToString)
        flashobject.Attributes.Add("type", "application/x-shockwave-flash")
        flashobject.Attributes.Add("data", CommonRoutines.GetApplicationPath & bannerAdContainer)


        param = New HtmlGenericControl
        param.TagName = "param"
        param.Attributes.Add("name", "movie")
        param.Attributes.Add("value", CommonRoutines.GetApplicationPath & bannerAdContainer)
        flashobject.Controls.Add(param)

        param = New HtmlGenericControl
        param.TagName = "param"
        param.Attributes.Add("name", "quality")
        param.Attributes.Add("value", "high")
        flashobject.Controls.Add(param)

        param = New HtmlGenericControl
        param.TagName = "param"
        param.Attributes.Add("name", "bgcolor")
        param.Attributes.Add("value", Ad.BGColorHTML)
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

        param = New HtmlGenericControl
        param.TagName = "param"
        param.Attributes.Add("name", "flashvars")
        param.Attributes.Add("value", "ad_name=" & Ad.Name & "&click_url=" & Ad.ClickURL & "&image_url=" & CommonRoutines.GetApplicationPath & Ad.ImageURL)
        flashobject.Controls.Add(param)

        Me.Controls.Add(flashobject)

    End Sub

    Private Sub addImage(ByVal Ad As RotatorAd)

        Dim marginSpec As String = "cursor:pointer;margin-top:" & Ad.MarginTop & "px;margin-bottom:" & Ad.MarginBottom & "px;margin-left:" & Ad.MarginLeft & "px;margin-right:" & Ad.MarginRight & "px"
        Me.Attributes.Add("style", marginSpec)
        Me.Width = Ad.Width
        Me.Height = Ad.Height

        Dim img As New HtmlImage
        img.Width = Ad.Width
        img.Height = Ad.Height
        img.Attributes.Add("onclick", "incrRotatorClickCount('" & Ad.hexID & "');popup('" & Ad.ClickURL & "','b')")
        img.Src = CommonRoutines.GetApplicationPath & Ad.ImageURL
        Me.Controls.Add(img)

    End Sub

End Class
