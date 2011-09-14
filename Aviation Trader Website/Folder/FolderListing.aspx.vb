Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* FolderEditor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = 
'*
'***************************************************************************************

Partial Class FolderListing
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Protected AdCountTotal As Integer


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.Skin

        headerbar.Slot = Slot
        headerbar.loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        displayLeftMenu()
        displayButtonBar()
        displayNavbar()
        displayFolders()

    End Sub

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.System
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.SystemEditor1
        leftmenu.Add("System Parameters", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.Technotes
        leftmenu.Add("Technotes", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.FolderListing
        Loader.ObjectID = sys.FirstFolderID
        leftmenu.Add("Folders", Loader.Target, True)

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

    End Sub


    Private Sub displayButtonBar()
        ButtonBar.Msg = ""
        ButtonBar.B2.Text = "ADD Folder"
    End Sub


    Private Sub displayFolders()

        '
        ' set up nav targets
        '
        Dim mysys As New ATSystem
        mysys.Retrieve()

        AdCountTotal = 0


        Dim Folders As Folders = mysys.Folders
        Loader.NextASPX = Loader.ASPX.FolderEditor
        For Each folder As Folder In Folders
            Loader.ObjectID = folder.ID
            folder.NavTarget = Loader.Target
            AdCountTotal += folder.AdCount
        Next

        FolderList.DataSource = Folders
        FolderList.DataBind()

    End Sub

    Private Sub addFolder()
        '
        ' add a new Folder
        '
        Dim folder As New Folder
        folder.SystemID = sys.ID
        folder.Name = "New Folder"
        Folder.Description = ""
        folder.Sortkey = "Z"
        '
        ' spooler params
        '
        folder.DoneFolderID = ATSystem.SysConstants.nullValue
        folder.ErrorFolderID = ATSystem.SysConstants.nullValue
        folder.SpoolerCommand = ATLib.Folder.SpoolerCommands.Unspecified

        Loader.ObjectID = Folder.Update()

        Loader.NextASPX = ATLib.Loader.ASPX.FolderEditor
        Response.Redirect(Loader.Target)

    End Sub

    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1
            Case 2 : addFolder()
        End Select
    End Sub


End Class
