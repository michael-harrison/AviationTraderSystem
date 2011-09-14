Option Strict On
Option Explicit On
Imports System.ComponentModel
Imports System.Configuration
Imports System
Imports AcrobatAccessLib
Imports Acrobat


'***************************************************************************************
'*
'* UNDService
'*
'*
'*
'***************************************************************************************
Public Class PDFAssistant

    Public Function ExtractTextFromPDF(ByVal filename As String) As String

        Dim pdfdoc As New Acrobat.AcroPDDoc
        If Not pdfdoc.Open(filename) Then
            Throw New Exception("Failed to open PDF file")
        End If

        Dim page As Acrobat.CAcroPDPage = CType(pdfdoc.AcquirePage(0), Acrobat.CAcroPDPage)
        Dim hl As New Acrobat.AcroHiliteList()
        hl.Add(0, 32767)                'offset, count = all words in doc
        Dim ts As Acrobat.AcroPDTextSelect = CType(page.CreatePageHilite(hl), Acrobat.AcroPDTextSelect)


        Dim wordCount As Integer = ts.GetNumText()
        Dim text As String = ""
        For i As Integer = 0 To wordCount - 1
            text &= ts.GetText(i)
        Next
        pdfdoc.Close()
        Return text
    End Function

End Class
