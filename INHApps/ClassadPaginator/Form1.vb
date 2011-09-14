Option Explicit On
Option Strict On
Imports ATLib
Imports EngineLib
Imports System


'***************************************************************************************
'*
'* DisplayWizard
'*
'* AUDIT TRAIL
'* 
'* V1.000   25-NOV-2009  BA  Original
'*
'* Display ad wizard
'*
'*
'*
'*
'***************************************************************************************
Public Class Form1

    Private sys As ATSystem
    Private INDDTemplateName As String

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        sys = New ATSystem
        sys.Retrieve()

        Me.Text = Me.Text & " " & sys.SWVersion & " connected to " & sys.Name
        showDD()
        btnRun.Enabled = False

    End Sub

    Private Sub runProcess()
        '
        ' check that INDD file exists and if OK, start dump
        '
        btnRun.Enabled = False        'avoid hitting more than once
        StatusBox.Items.Clear()

        Status("Run started")
        '
        ' take a copy of the template file and open it in indd
        '
        Try
            Dim fileinfo As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(INDDTemplateName)

            Dim sourcepath As String = fileinfo.DirectoryName
            INDDFilenameBox.Text = fileinfo.Name

            Dim pub As Publication = CType(PubDD.SelectedItem, Publication)
            Dim edition As Edition = CType(EditionDD.SelectedItem, Edition)
            Dim EI As EnumItem = CType(StatusDD.SelectedItem, EnumItem)
            Dim prodnStatus As Ad.ProdnState = CType(EI.Value, Ad.ProdnState)
            Dim INDDfilename As String = pub.Name & " - " & edition.Name & " - " & EI.Name & ".INDD"
            Dim billfilename As String = pub.Name & " - " & edition.Name & " - " & EI.Name & ".txt"
            INDDfilename = IO.Path.Combine(sys.ClassAdFolder, INDDfilename)
            billfilename = IO.Path.Combine(sys.ClassAdFolder, billfilename)
            IO.File.Copy(INDDTemplateName, INDDfilename, True)
            Status("Writing: " & INDDfilename)

            Dump.Dump(pub, edition, prodnStatus, INDDfilename, billfilename)
            Status("Run completed")


        Catch ex As Exception
            Status(ex.Message)
            Status("Run aborted")
        End Try

        btnRun.Enabled = True

    End Sub

    Public Sub Status(ByVal value As String)
        '
        ' if the line contains a newline sequence, break it at the first one.
        '
        Dim n As Integer = value.IndexOf(ControlChars.Lf)
        If n >= 0 Then
            Dim yy As String = value.Substring(0, n)
            Dim xx As String = value.Substring(n + 1, value.Length - (n + 1))

            Status(value.Substring(0, n))
            Status(value.Substring(n + 1, value.Length - (n + 1)))
        Else
            '
            ' if value is too long for one line, break into two or more lines
            '
            Dim maxlinelength As Integer = 60
            If value.Length > maxlinelength Then
                Status(value.Substring(0, maxlinelength))
                Status(value.Substring(maxlinelength, value.Length - maxlinelength))
            Else
                StatusBox.Items.Add(value)
                StatusBox.TopIndex = StatusBox.Items.Count - 1        'go to bottom of list
            End If
        End If
        Application.DoEvents()
        Me.Show()
    End Sub


   

    Private Sub showDD()
        Dim pubs As Publications = sys.Publications(Publication.Types.ATClassified)
        PubDD.ValueMember = "hexid"
        PubDD.DisplayMember = "Name"
        PubDD.DataSource = pubs

        Dim EA As New EnumAssistant(New Ad.ProdnState, Ad.ProdnState.Unspecified, Ad.ProdnState.Approved)
        StatusDD.ValueMember = "value"
        StatusDD.DisplayMember = "name"
        StatusDD.DataSource = EA
        StatusDD.SelectedValue = Convert.ToInt32(Ad.ProdnState.Approved)



        If pubs.Count > 0 Then

            Dim ednList As New List(Of Edition)
            For Each edn As Edition In pubs(0).Editions
                If edn.IsVisibleInWizard Then ednList.Add(edn)
            Next
            If ednList.Count = 0 Then
                EditionDD.Visible = False
                StatusDD.Visible = False
            Else
                EditionDD.ValueMember = "hexid"
                EditionDD.DisplayMember = "Name"
                EditionDD.Visible = True
                EditionDD.DataSource = ednList
                StatusDD.Visible = True
            End If
        End If

   

    End Sub


    Private Sub PubDD_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PubDD.SelectedIndexChanged
        Dim pubs As New Publications
        pubs.Retrieve(PubDD.SelectedValue.ToString)
        If pubs.Count > 0 Then

            Dim ednList As New List(Of Edition)
            For Each edn As Edition In pubs(0).Editions
                If edn.IsVisibleInWizard Then ednList.Add(edn)
            Next
            If ednList.Count = 0 Then
                EditionDD.Visible = False
                StatusDD.Visible = False
            Else
                EditionDD.ValueMember = "hexid"
                EditionDD.DisplayMember = "Name"
                EditionDD.Visible = True
                EditionDD.DataSource = ednList
                StatusDD.Visible = True
            End If
        End If
    End Sub


    Private Sub btnChooseINDD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseINDD.Click
        '
        ' assigns or re-assigns indd file to ad instance
        '
        INDDFileDialog.Filter = "Indesign files|*.INDD"

        If INDDFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            inddtemplatename = INDDFileDialog.FileName
            Dim fileinfo As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(INDDTemplateName)
            INDDFilenameBox.Text = fileinfo.Name
            btnRun.Enabled = True
        End If
    End Sub

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
        runProcess()
    End Sub
End Class
