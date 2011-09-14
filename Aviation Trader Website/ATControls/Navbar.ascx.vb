Option Strict On
Option Explicit On
Imports ATLib
'***************************************************************************************
'*
'* Navbar
'*
'* AUDIT TRAIL
'* 
'*
'*
'***************************************************************************************
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class ATControls_Navbar
    Inherits System.Web.UI.UserControl

    Private _objectType As ATSystem.ObjectTypes
    Private _objectID As Integer
    Private _loginlevel As Usr.LoginLevels

    Private _Loader As Loader

    Private cellList As New List(Of CellDef)

    Public Property ObjectType() As ATSystem.ObjectTypes
        Get
            Return _objectType
        End Get
        Set(ByVal value As ATSystem.ObjectTypes)
            _objectType = value
        End Set
    End Property

    Public Property Loader() As Loader
        Get
            Return _Loader
        End Get
        Set(ByVal value As Loader)
            _Loader = value
        End Set
    End Property


    Public Property ObjectID() As Integer
        Get
            Return _objectID
        End Get
        Set(ByVal value As Integer)
            _objectID = value
        End Set
    End Property


    Public Property LoginLevel() As Usr.LoginLevels
        Get
            Return _loginlevel
        End Get
        Set(ByVal value As Usr.LoginLevels)
            _loginlevel = value
        End Set
    End Property



    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        '
        ' build paging bar right to left
        '
        Select Case _objectType
            Case ATSystem.ObjectTypes.CategoryBrowse
                Dim categories As New Categories
                categories.Retrieve(_objectID)
                addCategoryBrowse(categories(0))

            Case ATSystem.ObjectTypes.ClassificationBrowse
                Dim classifications As New Classifications
                classifications.Retrieve(_objectID)
                addClassificationBrowse(classifications(0))

            Case ATSystem.ObjectTypes.AdBrowse
                Dim ads As New Ads
                ads.Retrieve(_objectID)
                addAdBrowse(ads(0))

            Case ATSystem.ObjectTypes.Edition
                Dim Editions As New Editions
                Editions.Retrieve(_objectID)
                addEdition(Editions(0))

            Case ATSystem.ObjectTypes.Product
                Dim Products As New Products
                Products.Retrieve(_objectID)
                addProduct(Products(0))

            Case ATSystem.ObjectTypes.Publication
                Dim publications As New Publications
                publications.Retrieve(_objectID)
                addPublication(publications(0))

            Case ATSystem.ObjectTypes.SpecDefinition
                Dim specdefns As New SpecDefinitions
                specdefns.Retrieve(_objectID)
                addSpecDefn(specdefns(0))

            Case ATSystem.ObjectTypes.SpecGroup
                Dim Specgroups As New SpecGroups
                Specgroups.Retrieve(_objectID)
                addSpecgroup(Specgroups(0))

            Case ATSystem.ObjectTypes.Classification
                Dim classifications As New Classifications
                classifications.Retrieve(_objectID)
                addClassification(classifications(0))


            Case ATSystem.ObjectTypes.Category
                Dim categories As New Categories
                categories.Retrieve(_objectID)
                addCategory(categories(0))

            Case ATSystem.ObjectTypes.Usr
                Dim usrs As New Usrs
                usrs.Retrieve(_objectID)
                addUsr(usrs(0))

            Case ATSystem.ObjectTypes.Technote
                Dim technotes As New Technotes
                technotes.Retrieve(_objectID)
                addtechnote(technotes(0))

            Case ATSystem.ObjectTypes.System
                Dim sys As New ATSystem
                sys.Retrieve()
                addSystem(sys)
        End Select
        '
        ' build control dynamically by iterating thru cellList in reverse order
        '

        ' add in cells to the table in the pagingbar div
        '
        Dim table As Table = New Table
        table.ID = "table"

        ''   table.Attributes.Add("border", "1")

        Dim row As New TableRow
        table.Rows.Add(row)
        Dim cell As TableCell

        For i As Integer = cellList.Count - 1 To 0 Step -1
            Dim cd As CellDef = cellList(i)
            cell = New TableCell
            Loader.ObjectID = cd.ObjectID
            Loader.ObjectType = cd.ObjectType
            Loader.NextASPX = cd.NextASPX
            cell.Text = cd.Name
            cell.Attributes.Add("onclick", "document.location.href='" & Loader.Target & "'")
            cell.CssClass = "nav"
            row.Cells.Add(cell)
            '
            ' add separator unless at and
            '
            If i > 0 Then
                cell = New TableCell
                cell.CssClass = "navseparator"
                row.Cells.Add(cell)

            End If
        Next

        Me.Controls.Add(table)

    End Sub

    Private Sub addCategoryBrowse(ByVal Category As Category)
        '
        ' setup first avail class and page 0
        '
        Dim firstClassID As Integer = ATSystem.SysConstants.nullValue
        If Category.Classifications.Count > 0 Then firstClassID = Category.Classifications(0).ID
        cellList.Add(New CellDef(Category.Name, ATLib.Loader.ASPX.BrowseList, ATSystem.ObjectTypes.Classification, firstClassID, 0))
    End Sub

    Private Sub addClassificationBrowse(ByVal Classification As Classification)
        cellList.Add(New CellDef(Classification.Name, ATLib.Loader.ASPX.BrowseList, ATSystem.ObjectTypes.Classification, Classification.ID, _Loader.Param1))
        addCategoryBrowse(Classification.Category)
    End Sub

    Private Sub addAdBrowse(ByVal Ad As Ad)
        Dim tag As String = Ad.KeyWords
        If tag.Length = 0 Then tag = Ad.Adnumber
        cellList.Add(New CellDef(tag, ATLib.Loader.ASPX.TextReader, ATSystem.ObjectTypes.AdBrowse, Ad.ID, _Loader.Param1))
        addClassificationBrowse(Ad.Classification)
    End Sub


    Private Sub addEdition(ByVal Edition As Edition)
        cellList.Add(New CellDef(Edition.Name, ATLib.Loader.ASPX.EditionEditor, ATSystem.ObjectTypes.Edition, Edition.ID, _Loader.Param1))
        addPublication(Edition.Publication)
    End Sub

    Private Sub addProduct(ByVal Product As Product)
        cellList.Add(New CellDef(Product.Name, ATLib.Loader.ASPX.ProductEditor, ATSystem.ObjectTypes.Product, Product.ID, _Loader.Param1))
        addPublication(Product.Publication)
    End Sub

    Private Sub addPublication(ByVal Publication As Publication)
        cellList.Add(New CellDef(Publication.Name, ATLib.Loader.ASPX.PublicationEditor, ATSystem.ObjectTypes.Publication, Publication.ID, _Loader.Param1))
        addSystem(Publication.System)
    End Sub

    Private Sub addSpecDefn(ByVal SpecDefn As SpecDefinition)
        cellList.Add(New CellDef(SpecDefn.Name, ATLib.Loader.ASPX.SpecDefinitionEditor, ATSystem.ObjectTypes.SpecDefinition, SpecDefn.ID, _Loader.Param1))
        addSpecgroup(SpecDefn.SpecGroup)
    End Sub


    Private Sub addSpecgroup(ByVal Specgroup As SpecGroup)
        cellList.Add(New CellDef(Specgroup.Name, ATLib.Loader.ASPX.SpecGroupEditor, ATSystem.ObjectTypes.SpecGroup, Specgroup.ID, _Loader.Param1))
        addClassification(Specgroup.Classification)
    End Sub

    Private Sub addClassification(ByVal Classification As Classification)
        cellList.Add(New CellDef(Classification.Name, ATLib.Loader.ASPX.ClassificationEditor, ATSystem.ObjectTypes.Classification, Classification.ID, _Loader.Param1))
        addCategory(Classification.Category)
    End Sub

    Private Sub addCategory(ByVal Category As Category)
        cellList.Add(New CellDef(Category.Name, ATLib.Loader.ASPX.CategoryEditor, ATSystem.ObjectTypes.Category, Category.ID, _Loader.Param1))
        addSystem(Category.System)
    End Sub

    Private Sub addUsr(ByVal Usr As Usr)
        cellList.Add(New CellDef(Usr.FullName, ATLib.Loader.ASPX.UserEditor1, ATSystem.ObjectTypes.Usr, Usr.ID, _Loader.Param1))
        addSystem(Usr.System)
    End Sub

    Private Sub addTechnote(ByVal Technote As Technote)
        cellList.Add(New CellDef(Technote.Name, ATLib.Loader.ASPX.TechnoteEditor, ATSystem.ObjectTypes.Technote, Technote.ID, _Loader.Param1))
        addSystem(Technote.System)
    End Sub


    Private Sub addSystem(ByVal Sys As ATSystem)
        cellList.Add(New CellDef(Sys.Name, ATLib.Loader.ASPX.HomeINH, ATSystem.ObjectTypes.System, Sys.ID, _Loader.Param1))
    End Sub

    Friend Class CellDef
        Friend Name As String
        Friend ObjectID As Integer
        Friend Param1 As Integer
        Friend ObjectType As ATSystem.ObjectTypes
        Friend NextASPX As Loader.ASPX

        Friend Sub New(ByVal pName As String, ByVal pNextASPX As Loader.ASPX, ByVal pObjectType As ATSystem.ObjectTypes, ByVal pObjectID As Integer, ByVal pParam1 As Integer)
            Name = pName
            ObjectID = pObjectID
            ObjectType = pObjectType
            NextASPX = pNextASPX
            Param1 = pParam1
        End Sub

    End Class
End Class


