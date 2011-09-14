Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* News Rotator
'*
'* Displays set of live news items
'*
'*
'***************************************************************************************
Partial Class ATControls_NewsRotator
    Inherits System.Web.UI.UserControl

    Private _loader As Loader
    Private _currentNewsItemID As Integer


    Public Property Loader() As Loader
        Get
            Return _loader
        End Get
        Set(ByVal value As Loader)
            _loader = value
        End Set
    End Property

    Public Property CurrentNewsItemID() As Integer
        Get
            Return _currentNewsItemID
        End Get
        Set(ByVal value As Integer)
            _currentNewsItemID = value
        End Set
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.EnableViewState = False
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

         '
        ' set target urls
        '
        Dim newsItems As New NewsItems
        newsItems.retrieveSet(NewsItem.ProdnState.Latest)
        '
        ' get a private list where the current item (if any) is not displayed
        '
        Dim myList As New List(Of NewsItem)


        _loader.NextASPX = ATLib.Loader.ASPX.NewsReader
        For Each ni As NewsItem In newsItems
            _loader.ObjectID = ni.ID
            ni.NavTarget = _loader.Target
            If ni.ID <> _currentNewsItemID Then myList.Add(ni)
        Next

        RotatorList.DataSource = myList
        RotatorList.DataBind()


        Dim h As Control = RotatorList.Controls(0)      'header
        Dim btnAll As ATWebToolkit.VW2Btn = CType(h.FindControl("btnAllNews"), ATWebToolkit.VW2Btn)
        _loader.NextASPX = ATLib.Loader.ASPX.AllStories
        _loader.SelectedTab = 0
        btnAll.NavigateURL = _loader.Target


    End Sub
End Class
