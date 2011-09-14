Option Strict On
Option Explicit On
'***************************************************************************************
'*
'* Page Bar
'*
'* AUDIT TRAIL
'* 
'*
'*
'***************************************************************************************
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class ATControls_PageBar
    Inherits System.Web.UI.UserControl

    Public Event Click(ByVal sender As ATWebToolkit.ClickableCell, ByVal pagenumber As Integer)

    Private _pageCount As Integer
    Private _scriptmanager As System.Web.UI.ScriptManager
    Private _scriptmanagerName As String
    Private _pagenumber As Integer
   

    Public Property PageCount() As Integer
        Get
            Return _pageCount
        End Get
        Set(ByVal value As Integer)
            _pageCount = value
        End Set
    End Property

    Public Property PageNumber() As Integer
        Get
            Return _pagenumber
        End Get
        Set(ByVal value As Integer)
            _pagenumber = value
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



    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        initialisePageBar()
    End Sub


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
        ' if not postback, set the current page number to 0 in the hidden field
        '
        If Not IsPostBack Then
            currentpagenumber.value = "0"
        End If
    End Sub

    Private Sub initialisePageBar()
        '
        ' adds the static information to the paging bar. Called on init event and before load
        ' event since load will contain viewstate info
        '

        ' add in cells to the table in the pagingbar div
        '
        Dim table As Table = New Table
        table.ID = "table"

        Dim row As New TableRow
        Dim clickcell As ATWebToolkit.ClickableCell
        Dim cell As TableCell

        cell = New TableCell
        cell.Text = "|"
        cell.CssClass = "pagingseparator"
        row.Cells.Add(cell)


        clickcell = New ATWebToolkit.ClickableCell
        clickcell.Text = "Prev"
        clickcell.CssClass = "paging"
        clickcell.ID = "prev"
        clickcell.Value = "-1"
        If _scriptmanager IsNot Nothing Then _scriptmanager.RegisterAsyncPostBackControl(clickcell)
        AddHandler clickcell.Click, AddressOf ClickCell_click
        row.Cells.Add(clickcell)

        cell = New TableCell
        cell.Text = "|"
        cell.CssClass = "pagingseparator"
        row.Cells.Add(cell)

        Dim cellcount As Integer = 0
        For i As Integer = 0 To _pageCount - 1
            clickcell = New ATWebToolkit.ClickableCell
            clickcell.Text = (i + 1).ToString
            clickcell.CssClass = "paging"
            '
            ' on init put the first page down
            '
            If i = _pagenumber Then clickcell.CssClass = "pagingselect"
            clickcell.ID = "page" & i.ToString
            clickcell.Value = i.ToString

            If _scriptmanager IsNot Nothing Then _scriptmanager.RegisterAsyncPostBackControl(clickcell)
            AddHandler clickcell.Click, AddressOf ClickCell_click

            row.Cells.Add(clickcell)
            '
            ' more than 18 cells  - generate new row
            '
            cellcount += 1
            If cellcount > 18 Then

                cell = New TableCell
                cell.Text = "|"
                cell.CssClass = "pagingseparator"
                row.Cells.Add(cell)

                table.Rows.Add(row)
                row = New TableRow

                cellcount = 0
            End If

            cell = New TableCell
            cell.Text = "|"
            cell.CssClass = "pagingseparator"
            row.Cells.Add(cell)
        Next


        clickcell = New ATWebToolkit.ClickableCell
        clickcell.Text = "Next"
        clickcell.ID = "next"
        clickcell.CssClass = "paging"
        clickcell.Value = "3000000"

        If _scriptmanager IsNot Nothing Then _scriptmanager.RegisterAsyncPostBackControl(clickcell)
        AddHandler clickcell.Click, AddressOf ClickCell_click
        row.Cells.Add(clickcell)

        cell = New TableCell
        cell.Text = "|"
        cell.CssClass = "pagingseparator"
        row.Cells.Add(cell)
        '
        ' add row to table and table to panel
        '
        table.Rows.Add(row)
        Me.Controls.Add(table)


    End Sub

    Private Function updatePageBar(ByVal clickedCell As ATWebToolkit.ClickableCell) As Integer
        '
        ' updates the current page info into the paging bar
        ' called after load event or on click event to get at current viewstate
        '
        Dim clickCell As ATWebToolkit.ClickableCell
        Dim d As Table = DirectCast(Me.FindControl("table"), Table)

        Dim pageno As Integer = Convert.ToInt32(currentpagenumber.value)

        Select Case clickedCell.ID

            Case "prev"
                pageno += -1
                If pageno < 0 Then pageno = 0

            Case "next"
                pageno += 1
                If pageno > _pageCount - 1 Then pageno = _pageCount - 1

            Case Else
                pageno = Convert.ToInt32(clickedCell.Value)
        End Select
        '
        ' set the correct css for the current page
        '
        For Each r As Control In d.Controls
            For Each c As Control In r.Controls
                If c.GetType.Name = "ClickableCell" Then
                    clickCell = DirectCast(c, ATWebToolkit.ClickableCell)
                    clickCell.CssClass = "paging"
                    If Convert.ToInt32(clickCell.Value) = pageno Then
                        clickCell.CssClass = "pagingselect"
                    End If
                End If
            Next
        Next
        '
        ' save new page number in hidden field and return it to caller
        '
        currentpagenumber.value = pageno.ToString
        Return pageno

    End Function


    Private Sub ClickCell_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clickCell As ATWebToolkit.ClickableCell = DirectCast(sender, ATWebToolkit.ClickableCell)
        Dim newpageno As Integer = updatePageBar(clickCell)
        RaiseEvent Click(clickCell, newpageno)
    End Sub


End Class
