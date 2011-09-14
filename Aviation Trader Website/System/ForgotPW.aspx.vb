Option Strict On
Option Explicit On
Imports ATLib


'***************************************************************************************
'*
'* Forgot PW
'*
'* AUDIT TRAIL
'* 
'* V1.000   02-AUG-2009  BA  Original
'*
'*
'***************************************************************************************

Partial Class ForgotPW
    Inherits System.Web.UI.Page


    Private Loader As Loader
    Private Slot As ATLib.Slot
    Private sys As ATSystem

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        sys = New ATSystem
        sys.Retrieve()

        Loader = New Loader(Request.QueryString(0))
        Dim slots As New Slots
        Slot = slots.Retrieve(Loader.SlotID)

        Page.Theme = Slot.skin

        Page.EnableViewState = False
        Response.Expires = 0                      'force page to always reload

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        displayContent()
      
    End Sub

 
  

    Private Sub displayContent()

        BtnSendEmail.Visible = True


    End Sub

 
    Private Sub SendEmail()


        Dim IV As New InputValidator
        IV.MinStringLength = 1         'do not allow nullstring
        IV.MaxStringLength = ATSystem.SysConstants.charLength

        Dim EmailAddr As String = IV.ValidateEmail(EmailBox, emailerror)

        If IV.ErrorCount = 0 Then

            Dim usrs As New Usrs
            usrs.RetrieveByEmailAddr(EmailAddr)
            If usrs.Count = 0 Then
                emailerror.Text = Constants.UnknownUsr
                emailerror.Visible = True
            Else
                Dim EA As New EmailAssistant(sys)
                EA.SendPassword(usrs(0))
                emailerror.Text = Constants.EmailSent
                emailerror.Visible = True
                BtnSendEmail.Visible = False

            End If

        End If
    End Sub

    Protected Sub BtnSendEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSendEmail.Click
        SendEmail()
    End Sub

End Class
