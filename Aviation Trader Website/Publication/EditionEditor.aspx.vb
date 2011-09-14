Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Edition Editor
'*
'* ON ENTRY:
'*
'*  Loader: objectID = Edition ID
'*
'***************************************************************************************

Partial Class EditionEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Edition As Edition


    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Dim slots As New Slots
        Loader = New Loader(Request.QueryString(0))
        Slot = slots.Retrieve(Loader.SlotID)
        Page.Theme = Slot.skin

        headerbar.Slot = Slot
        headerbar.Loader = Loader.Copy
        headerbar.SelectedCatID = ATSystem.SysConstants.nullValue

        Page.EnableViewState = True
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Editions As New Editions
        Edition = Editions.Retrieve(Loader.ObjectID)

        displayLeftMenu()
        displayButtonBar()
        displayNavbar()

        If Not IsPostBack Then
            displayEdition()
        End If

    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "Delete Edition"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayNavbar()
        navbar.objecttype = ATSystem.ObjectTypes.Edition
        navbar.objectid = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub


    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.SelectedTab = 0

        Loader.ObjectID = Edition.publicationID
        Loader.NextASPX = ATLib.Loader.ASPX.Editionlisting
        leftmenu.Add("Editions", Loader.Target, False)

        Loader.ObjectID = Edition.ID
        Loader.NextASPX = ATLib.Loader.ASPX.EditionEditor
        leftmenu.Add(Edition.Name, Loader.Target, True)

    End Sub

    Private Sub displayEdition()

        NameBox.Text = Edition.Name
        ShortnameBox.Text = Edition.ShortName
        AdCountLabel.Text = Edition.AdCount.ToString
        SortKeyBox.Text = Edition.SortKey
        OnsaleBox.Text = Edition.OnsaleDate.ToShortDateString
        AdDeadlineBox.Text = Edition.AdDeadline.ToShortDateString & " " & Edition.AdDeadline.ToShortTimeString
        prodndeadlineBox.Text = Edition.ProdnDeadline.ToShortDateString & " " & Edition.ProdnDeadline.ToShortTimeString
        DescriptionBox.Text = Edition.Description
        wizardcheck.Checked = Edition.IsVisibleInWizard

        Dim EA As EnumAssistant

        EA = New EnumAssistant(New Edition.ProdnState, Edition.ProdnState.Open, Edition.ProdnState.Closed)
        ProdnStatusDD.DataSource = EA
        ProdnStatusDD.DataBind()
        ProdnStatusDD.SelectedValue = Convert.ToString(Edition.ProdnStatus)


        EA = New EnumAssistant(New Edition.VisibleState, Edition.VisibleState.Past, Edition.VisibleState.Future)
        visibilityDD.DataSource = EA
        VisibilityDD.DataBind()
        VisibilityDD.SelectedValue = Convert.ToString(Edition.Visibility)

        PublicationDD.DataSource = sys.Publications
        PublicationDD.DataBind()
        PublicationDD.SelectedValue = CommonRoutines.Int2Hex(Edition.publicationID)
        '
        ' set Edition dd
        '
        EditionDD.DataSource = Edition.Publication.Editions
        EditionDD.DataBind()
        EditionDD.SelectedValue = Edition.hexID

    End Sub

    Private Sub updateEdition()

        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        Edition.Name = IV.ValidateText(NameBox, NameError)
        Edition.ShortName = IV.ValidateText(ShortnameBox, ShortnameError)
        Edition.SortKey = IV.ValidateText(SortKeyBox, SortKeyError)
        Edition.ProdnStatus = CType(ProdnStatusDD.SelectedValue, Edition.ProdnState)
        Edition.Visibility = CType(VisibilityDD.SelectedValue, Edition.VisibleState)
        IV.MaxStringLength = ATSystem.SysConstants.textCharLength
        Edition.Description = IV.ValidateText(DescriptionBox, DescriptionError)
        Edition.AdDeadline = IV.ValidateDateTime(AdDeadlineBox, AdDeadlineError)
        Edition.OnsaleDate = IV.ValidateDateTime(OnsaleBox, OnsaleError)
        Edition.ProdnDeadline = IV.ValidateDateTime(ProdnDeadlineBox, ProdnDeadlineError)
        Edition.IsVisibleInWizard = wizardcheck.Checked
        '
        ' move ads if necessary
        '
        Dim newEditionID As Integer = CommonRoutines.Hex2Int(EditionDD.SelectedValue)
        If newEditionID <> Edition.ID Then
            For Each Adinstance As AdInstance In Edition.AdInstances
                Adinstance.EditionID = newEditionID
                Adinstance.Update()
            Next
        End If
        '
        ' only allow sending of this editon to another pub if it has no ads
        '
        Dim newPublicationID As Integer = CommonRoutines.Hex2Int(PublicationDD.SelectedValue)
        If (Edition.AdCount > 0) And (newPublicationID <> Edition.publicationID) Then
            ButtonBar.Msg = Constants.NoEditionSend
        Else
            Edition.publicationID = newPublicationID

            If IV.ErrorCount = 0 Then
                Edition.Update()
                Loader.NextASPX = ATLib.Loader.ASPX.EditionEditor
                Loader.ObjectID = Edition.ID
                Response.Redirect(Loader.Target)         'refresh entire page
            End If
        End If

    End Sub

    Private Sub deleteEdition()
        If Edition.AdCount = 0 Then
            Loader.ObjectID = Edition.publicationID
            Edition.Deleted = True
            Edition.Update()
            Loader.NextASPX = ATLib.Loader.ASPX.Editionlisting
            Response.Redirect(Loader.Target)
        Else
            ButtonBar.Msg = Constants.NoEditionDelete
        End If
    End Sub

    Protected Sub ButtonBar_buttonBarEvent(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent
        Select Case buttonNumber
            Case 0
            Case 1 : deleteEdition()
            Case 2 : updateEdition()
        End Select
    End Sub

End Class
