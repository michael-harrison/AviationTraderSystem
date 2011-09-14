Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Folder Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Folder ID
'*
'***************************************************************************************

Partial Class FolderEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Folder As Folder


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.Skin

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Folders As New Folders
        Folder = Folders.Retrieve(Loader.ObjectID)

        displayLeftMenu()
        displayButtonBar()
        displayNavbar()

        If Not IsPostBack Then
            displayFolder()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Delete Folder"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.Folder
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.NextASPX = ATLib.Loader.ASPX.FolderListing
        leftmenu.Add("Folders", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.FolderEditor
        leftmenu.Add(Folder.Name, Loader.Target, True)

    End Sub

    Private Sub displayFolder()

        NameBox.Text = Folder.Name
        SortKeyBox.Text = Folder.Sortkey
        descriptionbox.text = Folder.Description
        AdCountLabel.Text = Folder.AdCount.ToString
        ProdnFolderCheck.Checked = Folder.IsProdnFolder
        SpooledCheck.Checked = Folder.IsSpooled
        SpoolerActiveCheck.Checked = Folder.IsSpoolerActive
        spoolerinfo.Visible = Folder.IsSpooled

        Dim ea As New EnumAssistant(New Folder.SpoolerCommands)

        spoolercommanddd.datasource = ea
        SpoolerCommanddd.databind()
        SpoolerCommanddd.selectedvalue = Convert.ToString(Folder.SpoolerCommand)

        DoneFolderDD.DataSource = sys.Folders
        DoneFolderDD.DataBind()
        If Folder.DoneFolderID <> ATSystem.SysConstants.nullValue Then
            DoneFolderDD.SelectedValue = CommonRoutines.Int2Hex(Folder.DoneFolderID)
        End If

        ErrorFolderDD.DataSource = sys.Folders
        ErrorFolderDD.DataBind()
        If Folder.ErrorFolderID <> ATSystem.SysConstants.nullValue Then
            ErrorFolderDD.SelectedValue = CommonRoutines.Int2Hex(Folder.ErrorFolderID)
        End If
        '
        ' set Folder dd
        '
        FolderDD.DataSource = sys.Folders
        FolderDD.DataBind()
        FolderDD.SelectedValue = Folder.hexID

    End Sub

    Private Sub updateFolder()

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        Folder.Name = IV.ValidateText(NameBox, NameError)
        Folder.Sortkey = IV.ValidateText(SortKeyBox, SortKeyError)

        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.textCharLength
        Folder.Description = IV.ValidateText(DescriptionBox, DescriptionError)
        '
        ' spooler params
        '
        Folder.IsSpoolerActive = False
        Folder.SpoolerCommand = ATLib.Folder.SpoolerCommands.Unspecified
        Folder.DoneFolderID = ATSystem.SysConstants.nullValue
        Folder.ErrorFolderID = ATSystem.SysConstants.nullValue
        Folder.IsProdnFolder = ProdnFolderCheck.Checked
        Folder.IsSpooled = SpooledCheck.Checked
        If Folder.IsSpooled Then
            Folder.IsSpoolerActive = SpoolerActiveCheck.Checked
            Folder.DoneFolderID = CommonRoutines.Hex2Int(DoneFolderDD.SelectedValue)
            Folder.ErrorFolderID = CommonRoutines.Hex2Int(ErrorFolderDD.SelectedValue)
            Folder.SpoolerCommand = CType(spoolercommanddd.selectedvalue, Folder.SpoolerCommands)
        End If

     

        If IV.ErrorCount = 0 Then
            '
            ' move ads
            '
            Dim newFolderID As Integer = CommonRoutines.Hex2Int(FolderDD.SelectedValue)
            If newFolderID <> Folder.ID Then
                For Each Ad As Ad In Folder.Ads
                    Ad.FolderID = newFolderID
                    Ad.Update()
                Next
            End If
            Folder.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.FolderEditor
            Loader.ObjectID = Folder.ID
            Response.Redirect(Loader.Target)         'refresh entire page
        End If

    End Sub

    Private Sub deleteFolder()
        If Folder.AdCount = 0 Then
            Folder.Deleted = True
            Folder.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.FolderListing
            Loader.ObjectID = sys.ID
            Response.Redirect(Loader.Target)
        Else
            ButtonBar.Msg = Constants.NoFolderDelete
        End If
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : deleteFolder()
            Case 2 : updateFolder()
        End Select
    End Sub

    Protected Sub SpooledCheck_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SpooledCheck.CheckedChanged
        spoolerinfo.Visible = SpooledCheck.Checked
    End Sub
End Class
