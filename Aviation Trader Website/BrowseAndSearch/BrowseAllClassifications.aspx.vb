Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols

'***************************************************************************************
'*
'* Browse All Classifications
'*
'* Provides a total list of all classes which can be clicked
'*
'*
'***************************************************************************************

Partial Class BrowseAllClassifications
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()
  
        Loader = New Loader(Request.QueryString(0))
        Dim slots As New Slots
        Slot = slots.Retrieve(Loader.SlotID)

        Page.Theme = Slot.skin
        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue + 2

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        displayLeftMenu()
        displayContent()

    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Select Case Slot.LoginLevel
            Case Usr.LoginLevels.SysAdmin
                Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
                leftmenu.Add("System Parameters", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.Technotes
                leftmenu.Add("Technotes", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.FolderListing
                Loader.ObjectID = sys.FirstFolderID
                leftmenu.Add("Folders", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.PublicationListing
                leftmenu.Add("Publications", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.CategoryListing
                leftmenu.Add("Categories", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.RotatorListing
                leftmenu.Add("Rotator Ads", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.NewsListing
                leftmenu.Add("News", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserListing
                leftmenu.Add("Users", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.ProofList
                 leftmenu.Add("Proof Reader", Loader.Target, False)

                Loader.NextASPX = ATLib.Loader.ASPX.UserImpersonate
                leftmenu.Add("Impersonate User...", Loader.Target, False)

            Case Usr.LoginLevels.Advertiser, Usr.LoginLevels.Subscriber
                Loader.ObjectID = Slot.ImpersonateUsrID
                Loader.NextASPX = ATLib.Loader.ASPX.UserEditor1
                leftmenu.Add("Manage My Profile", Loader.Target, False)
        End Select

    End Sub

    Private Sub displayContent()
        '
        ' spin thru all categories and classifications and create three even columns
        '
        Dim x As Double = Math.Ceiling((sys.Categories.Count + sys.ClassificationCount) / 3)
        Dim itemspercolumn As Integer = Convert.ToInt32(x)

        Dim currentItemCount As Integer = -(ATSystem.SysConstants.nullValue + 10)
        Dim currentColumnNumber As Integer = -1

        Loader.NextASPX = ATLib.Loader.ASPX.BrowseList
        Dim UL As HtmlGenericControl = Nothing

        For Each cat As Category In sys.Categories
            currentItemCount += 1
            If currentItemCount >= itemspercolumn Then
                '
                ' start a new column
                '
                currentColumnNumber += 1
                currentItemCount = 0
                UL = newUL(currentColumnNumber)
            End If
            '
            ' only show cat if it has classifications
            '
            If cat.Classifications.Count > 0 Then
                Loader.Param1 = 0                 'page 0
                Loader.ObjectID = cat.Classifications(0).ID   'first class
                addItem(UL, cat.Name, Loader.Target, "color:#5A86B3;border-bottom:1px solid #5A86B3")
                For Each cls As Classification In cat.Classifications
                    currentItemCount += 1
                    If currentItemCount >= itemspercolumn Then
                        currentColumnNumber += 1
                        currentItemCount = 0
                        UL = newUL(currentColumnNumber)
                    End If
                    Loader.Param1 = 0                 'page 0
                    Loader.ObjectID = cls.ID
                    addItem(UL, cls.Name, Loader.Target, "")

                Next
            End If
        Next

    End Sub


    Private Function newUL(ByVal columnNumber As Integer) As HtmlGenericControl
        '
        ' adds a ul to the current column
        '
        Dim UL As New HtmlGenericControl
        UL.TagName = "ul"
        UL.Attributes.Add("class", "allcatmenu")
        table.Rows(0).Cells(columnNumber).Controls.Add(UL)
        Return UL
    End Function



    Private Sub addItem(ByVal UL As HtmlGenericControl, ByVal text As String, ByVal navTarget As String, ByVal styleModifier As String)
        '
        ' adds a new list item to the supplied ul
        '
        Dim LI As New HtmlControls.HtmlGenericControl
        LI.TagName = "li"
        UL.Controls.Add(LI)
        Dim A As New HtmlControls.HtmlGenericControl
        A.TagName = "a"
        A.Attributes.Add("href", navTarget)
        If styleModifier.Length > 0 Then A.Attributes.Add("Style", styleModifier)
        A.InnerText = text
        LI.Controls.Add(A)
    End Sub


End Class
