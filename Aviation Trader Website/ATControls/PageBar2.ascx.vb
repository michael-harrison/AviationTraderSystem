Option Strict On
Option Explicit On
Imports ATLib
'***************************************************************************************
'*
'* Page Bar 2
'*
'* AUDIT TRAIL
'* 
'*
'*
'***************************************************************************************
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class ATControls_PageBar2
    Inherits System.Web.UI.UserControl

    Private _pageCount As Integer
    Private _loader As Loader
    Private Const cellsPerRow As Integer = 3

    Public Property PageCount() As Integer
        Get
            Return _pageCount
        End Get
        Set(ByVal value As Integer)
            _pageCount = value
        End Set
    End Property

    Public Property Loader() As Loader
        Get
            Return _loader
        End Get
        Set(ByVal value As Loader)
            _loader = value
        End Set
    End Property


    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        initialisePageBar()
    End Sub



    ''    <tr>
    ''    <td id="listitem" runat="server" class="<%#container.dataitem.cssclass %>" style="font-weight:bold">
    ''        <a href="<%#container.dataitem.navtarget %>">
    ''            <asp:Label ID="menuitem" Text="<%#container.dataitem.text %>" runat="server" />
    ''        </a>
    ''    </td>
    ''</tr>

    Private Sub initialisePageBar()
        Dim currentpage As Integer = _loader.Param1
        '
        ' add in cells to the table in the pagingbar div
        '
        Dim table As Table = New Table
        table.ID = "table"
        Dim row As TableRow
        row = New TableRow
        '
        ' prev
        '
        row.Cells.Add(newSeparator)
        _loader.Param1 = 0
        If currentpage > 0 Then _loader.Param1 = currentpage - 1
        row.Cells.Add(newCell("Prev", _loader, False))

         For i As Integer = 0 To _pageCount - 1
            Dim selected As Boolean = False
            If i = currentpage Then selected = True
            _loader.Param1 = i
            row.Cells.Add(newSeparator)
            row.Cells.Add(newCell((i + 1).ToString, _loader, selected))
        Next

        '
        ' next
        '
        row.Cells.Add(newSeparator)
        _loader.Param1 = _pageCount - 1
        If currentpage < _pageCount - 1 Then _loader.Param1 = currentpage + 1
        row.Cells.Add(newCell("Next", _loader, False))
        row.Cells.Add(newSeparator)
        table.Rows.Add(row)
        '
        ' add row to table and table to panel
        '

        Me.Controls.Add(table)

    End Sub

    Private Function newCell(ByVal text As String, ByVal loader As Loader, ByVal selected As Boolean) As TableCell
        Dim cssClass As String = "paging"
        If selected Then cssClass = "pagingselect"

        Dim c As New TableCell
        c.CssClass = cssClass
        If text.Length > 0 Then
            Dim a As New HyperLink
            a.NavigateUrl = loader.Target
            Dim lbl As New Label
            lbl.Text = text
            a.Controls.Add(lbl)
            c.Controls.Add(a)
        End If
        Return c

    End Function

    Private Function newSeparator() As TableCell
        Dim c As New TableCell
        c.Text = "|"
        c.CssClass = "pagingseparator"
        Return c
    End Function


End Class
