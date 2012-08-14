Option Strict On
Option Explicit On
Imports ATLib
Imports System.Web.Services
Imports System.Web.Services.Protocols


'***************************************************************************************
'*
'* Headerbar
'*
'* AUDIT TRAIL
'* 
'* V1.000   02-AUG-2009  BA  Original
'*
'*
'***************************************************************************************

Partial Class ATControls_Headerbar
    Inherits System.Web.UI.UserControl

    Private _selectedCatID As Integer
    Private _slot As Slot
    Private _loader As Loader
    Private _selected As Integer
    Private sys As ATSystem
    Private _showCategories As Boolean = True

    Private _scriptmanager As System.Web.UI.ScriptManager
    Private _scriptmanagerName As String


    Public Property Loader() As Loader
        Get
            Return _loader
        End Get
        Set(ByVal value As Loader)
            _loader = value

        End Set
    End Property

    Public Property Slot() As Slot
        Get
            Return _slot
        End Get
        Set(ByVal value As Slot)
            _slot = value

        End Set
    End Property


    Public Property SelectedCatID() As Integer
        Get
            Return _selectedCatID
        End Get
        Set(ByVal value As Integer)
            _selectedCatID = value

        End Set
    End Property

    Public Property Selected() As Integer
        Get
            Return _selected
        End Get
        Set(ByVal value As Integer)
            _selected = value
        End Set
    End Property

    Public Property ShowSearchBar() As Boolean
        Get
            Return searchBar.Visible
        End Get
        Set(ByVal value As Boolean)
            searchBar.Visible = value
            nullSearchBar.Visible = Not value
        End Set
    End Property

    Public Property ShowCategories() As Boolean
        Get
            Return _showCategories
        End Get
        Set(ByVal value As Boolean)
            _showCategories = value
        End Set
    End Property

    Public Property ScriptManager() As ScriptManager
        Get
            Return _scriptmanager
        End Get
        Set(ByVal value As ScriptManager)
            _scriptmanager = value
        End Set
    End Property

    Public Property ScriptManagerName() As String
        Get
            Return _scriptmanagerName
        End Get
        Set(ByVal value As String)
            _scriptmanagerName = value
        End Set
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
        ' setup scripmanager
        '
        If Not _scriptmanagerName Is Nothing Then
            Dim c As Control = Page.FindControl(_scriptmanagerName)
            If Not c Is Nothing Then
                _scriptmanager = CType(c, ScriptManager)
            End If
        End If
        '
        ' script manager may have been registered programatically
        '
        If Not _scriptmanager Is Nothing Then
            _scriptmanager.RegisterAsyncPostBackControl(btnSearch)
            _scriptmanager.RegisterAsyncPostBackControl(CategoryDD)
        End If

        sys = New ATSystem
        sys.Retrieve()

        If Not IsPostBack Then
            If searchBar.Visible Then
                displaySearchBar()
            End If
        End If

        UpdateMyAT()
        displayMenu()

    End Sub

    Private Sub displaySearchBar()
        '
        ' if in search mode, remember search criteria from slot
        '
        Dim clslist As List(Of clsItem) = sys.GetCatClassList
        CategoryDD.DataSource = clslist
        CategoryDD.DataBind()
        If _slot.SearchMode = ATLib.Slot.SearchModes.Search Then
            CategoryDD.SelectedValue = CommonRoutines.Int2ShortHex(_slot.SearchObjectType) & CommonRoutines.Int2Hex(_slot.SearchObjectID)
            SearchBox.Text = _slot.SearchKey
        Else
            CategoryDD.SelectedValue = clslist(0).Value
            SearchBox.Text = ""

        End If
        autoComplete1.ContextKey = sys.GetFirstWebPublication.hexID & CommonRoutines.Int2ShortHex(_slot.EditionVisibility) & CategoryDD.SelectedValue

    End Sub


    Private Sub displayMenu()
        '
        ' if the selectedcat is null then then nothing is down.
        ' if the selectedcat is null+1 then the home page is down. Otherwise set the 
        ' if the selectedcat is null+2 then the view all page is down. Otherwise set the 
        ' correct button down

        Dim topNode As MenuNode
        Dim subNode As MenuNode
        '
        ' go to the correct home page as a function of login level
        '
        Select Case _slot.LoginLevel
            Case Usr.LoginLevels.INHProdn, Usr.LoginLevels.SysAdmin : _loader.NextASPX = ATLib.Loader.ASPX.HomeINH
            Case Usr.LoginLevels.Guest : _loader.NextASPX = ATLib.Loader.ASPX.HomeGuest
            Case Else : _loader.NextASPX = ATLib.Loader.ASPX.HomeRegistered
        End Select

		membersHomeLink.NavigateUrl = _loader.Target


        topNode = New MenuNode()
        topNode.ID = CommonRoutines.Int2Hex(ATSystem.SysConstants.nullValue)
        topNode.Text = "HOME"
        topNode.NavTarget = _loader.Target
        topNode.Selected = False
        If SelectedCatID = ATSystem.SysConstants.nullValue + 1 Then topNode.Selected = True
        topmenu.Nodes.Add(topNode)

        If _showCategories Then
            _loader.NextASPX = ATLib.Loader.ASPX.BrowseList
            _loader.Param1 = 0            'default to page 0

            For Each cat As Category In sys.Categories
                '
                ' only include categroy in bar if requested
                '
                If cat.IncludeInBar Then
                    topNode = New MenuNode()

                    For Each cls As Classification In cat.Classifications
                        _loader.ObjectID = cls.ID
                        subNode = New MenuNode(cls.hexID, cls.Name, _loader.Target, False)
                        topNode.Add(subNode)
                    Next
                    '
                    ' select the first classification if its there for insertion into the default top menu item
                    '
                    topNode.NavTarget = ""           'disable if no classifications
                    topNode.Text = cat.Name
                    topNode.Selected = False
                    If cat.Classifications.Count > 0 Then
                        _loader.ObjectID = cat.Classifications(0).ID
                        topNode.NavTarget = _loader.Target
                    End If
                    If SelectedCatID = cat.ID Then topNode.Selected = True
                    topmenu.Nodes.Add(topNode)
                End If
            Next
            '
            ' add a view all button as the last item - goes to new page
            '
            topNode = New MenuNode()
            topNode.ID = CommonRoutines.Int2Hex(ATSystem.SysConstants.nullValue)
            topNode.Text = "VIEW ALL"
            _loader.NextASPX = ATLib.Loader.ASPX.BrowseAllClassifications
            topNode.NavTarget = _loader.Target
            topNode.Selected = False
            If SelectedCatID = ATSystem.SysConstants.nullValue + 2 Then topNode.Selected = True
            topmenu.Nodes.Add(topNode)
        End If


    End Sub

    Public Sub UpdateMyAT()
        '
        ' display the login link or login name as appropriate
        '

        If _slot.LoginLevel = Usr.LoginLevels.Guest Then
            _loader.NextASPX = ATLib.Loader.ASPX.Login
			loginLink.Text = "Sign in or Register"
            loginLink.NavigateUrl = _loader.Target
            LoginWelcome.Text = ""
        Else
            _loader.NextASPX = ATLib.Loader.ASPX.Logout
			loginLink.Text = "Sign out"
            loginLink.NavigateUrl = _loader.Target
            LoginWelcome.Text = "Welcome " & _slot.FullName
            '
            ' if impersonation is on, put out the target users name as well
            '
            If Slot.UsrID <> Slot.ImpersonateUsrID Then
                Dim usrs As New Usrs
                Dim usr As Usr = usrs.Retrieve(Slot.ImpersonateUsrID)
                LoginWelcome.Text &= vbCr & "Impersonating " & usr.FullName

            End If
        End If


    End Sub


    Protected Sub CategoryDD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CategoryDD.SelectedIndexChanged
        autoComplete1.ContextKey = sys.GetFirstWebPublication.hexID & CommonRoutines.Int2ShortHex(_slot.EditionVisibility) & CategoryDD.SelectedValue
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        '
        ' update the slot with the current search params and raise event to caller who
        ' will use the slot to do the search
        '
       
        _slot.SearchMode = ATLib.Slot.SearchModes.Search
        _slot.SearchKey = SearchBox.Text
        _slot.SearchObjectType = CType(CategoryDD.SelectedValue.Substring(0, 2), ATSystem.ObjectTypes)
        _slot.SearchObjectID = CommonRoutines.Hex2Int(CategoryDD.SelectedValue.Substring(2, 8))
        _slot.Update()
        '
        'go directly to search page
        '
        _loader.NextASPX = ATLib.Loader.ASPX.SearchList
        Loader.Param1 = 0     'start on first page
        Response.Redirect(Loader.Target)

    End Sub
End Class


