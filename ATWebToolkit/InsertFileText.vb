Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports atlib


'***************************************************************************************
'*
'* Implements a server side click event for a table cell, not provided by standard asp.net
'*
'* AUDIT TRAIL
'* 
'* V1.000   18-AUG-2008  BA  Original
'*
'***************************************************************************************



<DefaultProperty("Text"), ToolboxData("<{0}:ClickableCell runat=server></{0}:ClickableCell>")> _
Public Class InsertFileText
    Inherits WebControls.WebControl
  

    Private _fileName As String


    Public Property Filename() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
        End Set
    End Property


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        '
        ' open the file and outut the text to  the htmlwriter
        '
       

        Dim sys As New ATSystem
        sys.Retrieve()
        Dim resolvedFilename As String = IO.Path.Combine(sys.PhysicalApplicationPath, Filename)
        Try

            Dim sr As IO.StreamReader = New IO.StreamReader(resolvedFilename)
            writer.Write(sr.ReadToEnd)
            sr.Close()
            sr.Dispose()

        Catch ex As Exception
            Dim s As String = "Could not find file " & _fileName & " at " & resolvedFilename
            writer.Write(s)
        End Try

    End Sub

End Class
