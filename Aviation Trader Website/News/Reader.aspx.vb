Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Home page
'*
'* ON ENTRY:
'*
'*  Loader: objectID = NewsItemID
'* 
'*
'*
'***************************************************************************************

Partial Class Reader
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private newsItem As NewsItem

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)

        Page.Theme = Slot.skin
        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue + 1
        NewsRotator.Loader = Loader.Copy

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim newsitems As New NewsItems
        newsItem = newsitems.Retrieve(Loader.ObjectID)
        NewsRotator.currentnewsitemid = newsItem.ID

        displayLeftMenu()
        displayContent()

    End Sub

    Private Sub displayLeftMenu()
    End Sub


    Private Sub displayContent()

        storyhead.text = newsItem.Name
        StoryIntro.Text = newsItem.HTMLIntro
        StoryBody.Text = newsItem.HTMLBody
        picpanel.visible = False
        If newsItem.HasImage Then
            picpanel.visible = True
            storypiccaption.text = newsItem.PicCaption
            storypic.imageurl = newsItem.ImageURL

        End If



    End Sub

End Class
