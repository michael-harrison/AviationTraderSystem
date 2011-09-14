Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Text builder
'*
'* 
'*
'*
'***************************************************************************************
Partial Class ATControls_SpecBuilder
    Inherits System.Web.UI.UserControl

    Private _spec As Spec

    Public Property Spec() As Spec
        Get
            '
            ' on a postback recover object from viewstate id
            '
            If _spec Is Nothing Then
                Dim specID As Integer = CommonRoutines.Hex2Int(ViewState.Item("SpecID").ToString)
                Dim specs As New Specs
                _spec = specs.Retrieve(specID)
            End If
            Return _spec
        End Get
        Set(ByVal value As Spec)
            _spec = value
            '
            ' save spec ID in viewstate
            '
            ViewState.Add("SpecID", Spec.hexID)
        End Set
    End Property


    Private Sub displaySpec()

        nameLabel.Text = Spec.Name
        activecheck.Checked = Spec.IsActive

        texttype.Visible = False
        textareatype.Visible = False
        radioverticaltype.Visible = False
        radiohorizontaltype.Visible = False
        checkboxtype.Visible = False

        Select Case Spec.DisplayType

            Case SpecDefinition.DisplayTypes.Text
                texttype.Visible = True
                texttype.Text = Spec.Value

            Case SpecDefinition.DisplayTypes.TextArea
                textareatype.Visible = True
                textareatype.Text = Spec.Value

            Case SpecDefinition.DisplayTypes.RadioHorizontal
                radiohorizontaltype.Visible = True
                radiohorizontaltype.DataSource = getValueList()
                radiohorizontaltype.SelectedValue = getSelectedValue()
                radiohorizontaltype.DataBind()

            Case SpecDefinition.DisplayTypes.RadioVertical
                radioverticaltype.Visible = True
                radioverticaltype.DataSource = getValueList()
                radioverticaltype.SelectedValue = getSelectedValue()
                radioverticaltype.DataBind()

            Case SpecDefinition.DisplayTypes.CheckBox
                checkboxtype.Visible = True
                checkboxtype.DataSource = getValueList()
                checkboxtype.DataBind()

                Dim selectedValueList As String() = getSelectedValueList()
                For Each li As ListItem In checkboxtype.Items
                    If testSelected(li.Value, selectedValueList) Then li.Selected = True
                Next
        End Select

    End Sub

    Public Sub Update()
        '
        ' updates the spec chain with the current control value
        '
        Spec.IsActive = activecheck.Checked

        Select Case Spec.DisplayType

            Case SpecDefinition.DisplayTypes.Text
                Spec.Value = texttype.Text

            Case SpecDefinition.DisplayTypes.TextArea
                Spec.Value = textareatype.Text

            Case SpecDefinition.DisplayTypes.RadioHorizontal
                Spec.Value = radiohorizontaltype.SelectedValue


            Case SpecDefinition.DisplayTypes.RadioVertical
                Spec.Value = radioverticaltype.SelectedValue


            Case SpecDefinition.DisplayTypes.CheckBox
                Spec.Value = ""
                For Each li As ListItem In checkboxtype.Items
                    If li.Selected Then Spec.Value &= li.Value & vbLf
                Next

        End Select

        Spec.Update()

    End Sub

    Private Function getValueList() As String()
        '
        ' converts the specs value list into an array by splitting on the CR value
        '
        Dim lf() As String = {vbLf}
        Dim tokens As String() = Spec.ValueList.Split(lf, StringSplitOptions.None)
        Return tokens
    End Function

    Private Function getSelectedValueList() As String()
        '
        ' converts the specs value list into an array by splitting on the CR value
        '
        Dim lf() As String = {vbLf}
        Dim tokens As String() = Spec.Value.Split(lf, StringSplitOptions.None)
        Return tokens
    End Function

    Private Function getSelectedValue() As String
        '
        ' returns the selected value of the spec. If the value list in the SpecDefinitions has changed, the
        ' current selectedvalue may no longe be there
        '
        Dim tokens As String() = getValueList()
        Dim rtnval As String = tokens(0)
        For i As Integer = 0 To tokens.Length - 1
            If tokens(i) = Spec.Value Then
                rtnval = Spec.Value
                Exit For
            End If
        Next
        Return rtnval
    End Function

    Private Function testSelected(ByVal value As String, ByVal valuelist As String()) As Boolean
        '
        ' returns true if the supplied value is in the value list
        '
        Dim rtnval As Boolean = False
        For i As Integer = 0 To valuelist.Length - 1
            If value = valuelist(i) Then
                rtnval = True
                Exit For
            End If
        Next

        Return rtnval


    End Function



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''      bindList()
    End Sub


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        displaySpec()
    End Sub
End Class
