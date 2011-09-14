Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Edit System Parameters 1
'*
'* ON ENTRY:
'*
'*  Loader: objectID = technoteID
'*
'***************************************************************************************

Partial Class TechnoteEditor
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem
    Private Technote As Technote


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

        Dim technotes As New ATLib.Technotes
        Technote = technotes.Retrieve(Loader.ObjectID)
        displayNavbar()
        displayLeftMenu()
        displayButtonBar()

        If Not IsPostBack Then
            displayNote()
        End If

    End Sub

    Private Sub displayNavbar()
        NavBar.ObjectType = ATSystem.ObjectTypes.technote
        NavBar.ObjectID = Loader.ObjectID
        NavBar.LoginLevel = Slot.LoginLevel
        NavBar.Loader = Loader.Copy
    End Sub

    Private Sub displayButtonBar()
        ButtonBar.B1.Text = "RETURN TO LIST"
        ButtonBar.B2.Text = "Save Changes"
    End Sub

    Private Sub displayLeftMenu()
        '
        ' set menu control
        '
        Loader.NextASPX = ATLib.Loader.ASPX.Technotes
        leftmenu.Add("Technotes", Loader.Target, False)

        Loader.NextASPX = ATLib.Loader.ASPX.TechnoteEditor
        leftmenu.Add("Technote " & Technote.ID, Loader.Target, True)

    End Sub


    Private Sub displayNote()
        titleLabel.Text = "Note " & Technote.ID & ":"
        Namebox.Text = Technote.Name

        Dim EA As EnumAssistant

        EA = New EnumAssistant(New Technote.State)
        StatusDropDown.DataSource = EA
        StatusDropDown.SelectedValue = Convert.ToString(Technote.Status)
        StatusDropDown.DataBind()

        EA = New EnumAssistant(New Technote.Reporters)
        FixedByDropDown.DataSource = EA
        FixedByDropDown.SelectedValue = Convert.ToString(Technote.FixedBy)
        FixedByDropDown.DataBind()

        EA = New EnumAssistant(New Technote.Reporters)
        ReportedByDropDown.DataSource = EA
        ReportedByDropDown.SelectedValue = Convert.ToString(Technote.ReportedBy)
        ReportedByDropDown.DataBind()

        EA = New EnumAssistant(New Technote.Resolutions)
        ResolutiondropDown.DataSource = EA
        ResolutiondropDown.SelectedValue = Convert.ToString(Technote.Resolution)
        ResolutiondropDown.DataBind()

        ProblemDescriptionBox.Text = Server.HtmlDecode(Technote.ProblemDescription)
        ProblemFixBox.Text = Server.HtmlDecode(Technote.ProblemFix)
        ReportedBox.Text = Technote.CreateTime.ToString
        '
        ' only show fixed date if the note is closed
        '
        FixedBox.Text = ""
        If Technote.Status = Technote.State.Closed Then FixedBox.Text = Technote.FixTime.ToString


    End Sub

    Private Sub updateNote()

        Technote.Status = CType(StatusDropDown.SelectedValue, Technote.State)
        Technote.ReportedBy = CType(ReportedByDropDown.SelectedValue, Technote.Reporters)
        Technote.FixedBy = CType(FixedByDropDown.SelectedValue, Technote.Reporters)
        Technote.Resolution = CType(ResolutiondropDown.SelectedValue, Technote.Resolutions)


        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength
        Technote.Name = IV.ValidateText(Namebox, NameError)

        IV.MinStringLength = 0         'allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.textCharLength
        Technote.ProblemDescription = IV.ValidateText(ProblemDescriptionBox, ProblemDescriptionError)
        Technote.ProblemFix = IV.ValidateText(ProblemFixBox, ProblemFixError)

        If IV.ErrorCount = 0 Then
            Technote.Update()
            ButtonBar.Msg = Constants.Saved
        End If
    End Sub

    Private Sub return2List()
        '
        ' go back to the Technote list
        '
        Loader.NextASPX = Loader.ASPX.Technotes
        Response.Redirect(Loader.Target)

    End Sub

    Private Function mapStatus2Tab(ByVal Status As Technote.State) As Integer

        Select Case Status
            Case ATLib.Technote.State.Open : Return 0
            Case ATLib.Technote.State.Closed : Return 1
            Case ATLib.Technote.State.Discuss : Return 2
            Case ATLib.Technote.State.NewFeature : Return 3
            Case ATLib.Technote.State.Warranty : Return 4
            Case ATLib.Technote.State.WishList : Return 5
        End Select

    End Function




    Protected Sub ButtonBar_buttonBarEvnt(ByVal buttonNumber As Integer) Handles ButtonBar.buttonBarEvent

        Select Case buttonNumber
            Case 0
            Case 1 : return2List()
            Case 2 : updateNote()
        End Select
    End Sub

End Class
