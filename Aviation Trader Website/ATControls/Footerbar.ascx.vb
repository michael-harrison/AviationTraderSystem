Option Strict On
Option Explicit On
Imports ATLib

'***************************************************************************************
'*
'* Footer bar
'*
'* ON ENTRY:
'*
'*  Loader: objectID = undefined
'*          
'*
'***************************************************************************************

Partial Class ATControls_Footerbar
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.EnableViewState = False
    End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' litCopyrightYear.Text = DateTime.Now.Year.ToString()
        ' initPopups()
	End Sub

    Private Sub initPopups()

        ' Dim tos As String = IO.Path.Combine(CommonRoutines.GetApplicationPath, "Popups/TermsOfUse.aspx")
        ' termsofuse.attributes.add("onclick", "popup('" & tos & "','a')")

        ' Dim privacy As String = IO.Path.Combine(CommonRoutines.GetApplicationPath, "Popups/PrivacyPolicy.aspx")
        ' privacypolicy.Attributes.Add("onclick", "popup('" & privacy & "','b')")

    End Sub
End Class
