Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System
Imports System.Net
Imports System.Net.Mail


'***************************************************************************************
'*
'* Email Assistant
'*
'* AUDIT TRAIL
'* 
'*
'* V1.000   19-NOV-2009  BA  Original
'*
'*
'***************************************************************************************


''' <summary>
''' This class provides email services for the formatting and transmission of emails. In the current implementatation,
''' EmailAssistant works by scraping the requested page from the web application into memory as an HTML
''' string, and then transmits that string via the SMTP server.
''' </summary>
Public Class EmailAssistant

    Private _Sys As ATSystem

    Public Sub New(ByVal Sys As ATSystem)
        _Sys = Sys
    End Sub


    ''' <summary>
    ''' Sends a welcome registration page to user
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendRegistrationWelcome(ByVal Usr As Usr)
        Dim sendTo() As String = {Usr.EmailAddr}
        If _Sys.IsEmailTestMode Then sendTo(0) = _Sys.TestEmailAddr
        Dim CC() As String = {""}
        Dim bcc() As String = {_Sys.BCCEmailAddr}
        '
        ' Scrape web site to get content
        '
        Dim subject As String = "Welcome to Aviation Trader"
        Dim URL As String = BuildURL(Usr.ID, Loader.ASPX.RegistrationWelcomeEmail)
        Dim body As String = GetContent(URL)
        '
        ' send the mail
        '
        Send(body, Constants.ProdnEmail, sendTo, bcc, CC, subject)
    End Sub

    ''' <summary>
    ''' Sends a forgot password to the user with his password
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendPassword(ByVal Usr As Usr)
        Dim sendTo() As String = {Usr.EmailAddr}
        If _Sys.IsEmailTestMode Then sendTo(0) = _Sys.TestEmailAddr

        Dim CC() As String = {""}
        Dim bcc() As String = {_Sys.BCCEmailAddr}
        '
        ' Scrape web site to get content
        '
        Dim subject As String = "Your Aviation Trader password"
        Dim URL As String = BuildURL(Usr.ID, Loader.ASPX.ForgotPWEmail)
        Dim body As String = GetContent(URL)
        '
        ' send the mail
        '
        Send(body, Constants.ProdnEmail, sendTo, bcc, CC, subject)
    End Sub

    ''' <summary>
    ''' Sends an email to admin saying what the user wants
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendSubsRequest(ByVal Usr As Usr, ByVal subjectLine As String)
        Dim sendTo() As String = {"admin@aviationtrader.com.au"}
        Dim CC() As String = {""}
        Dim bcc() As String = {_Sys.BCCEmailAddr}
        '
        ' Scrape web site to get content
        '
        Dim subject As String = subjectLine
        Dim URL As String = BuildURL(Usr.ID, Loader.ASPX.SubsRequestEmail)
        Dim body As String = GetContent(URL)
        '
        ' send the mail
        '
        Send(body, Constants.ProdnEmail, sendTo, bcc, CC, subject)
    End Sub

    ''' <summary>
    ''' Sends an email to usr saying edition is about to close
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendEditionCLoseReminder(ByVal Usr As Usr, ByVal Ad As Ad)
        Dim sendto() As String = {Usr.EmailAddr}
        If _Sys.IsEmailTestMode Then sendto(0) = _Sys.TestEmailAddr
        Dim CC() As String = {""}
        Dim bcc() As String = {_Sys.BCCEmailAddr}
        '
        ' Scrape web site to get content
        '
        Dim subject As String = "Edition Close Reminder"
        Dim URL As String = BuildURL(Ad.ID, Loader.ASPX.EditionCloseReminderEmail)
        Dim body As String = GetContent(URL)
        '
        ' send the mail
        '
        Send(body, Constants.ProdnEmail, sendTo, bcc, CC, subject)
    End Sub


    ''' <summary>
    ''' Sends a confirmation of the ad booking
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendAdConfirmation(ByVal Ad As Ad)
        Dim sendTo() As String = {Ad.Usr.EmailAddr}
        If _Sys.IsEmailTestMode Then sendTo(0) = _Sys.TestEmailAddr
        Dim CC() As String = {""}
        Dim bcc() As String = {_Sys.BCCEmailAddr}
        '
        ' Scrape web site to get content
        '
        Dim subject As String = "Aviation Trader Booking Confirmation"
        Dim URL As String = BuildURL(Ad.ID, Loader.ASPX.AdConfirmationEmail)
        Dim body As String = GetContent(URL)
        '
        ' send the mail
        '
        Send(body, Constants.ProdnEmail, sendTo, bcc, CC, subject)
    End Sub

    ''' <summary>
    ''' Sends a proof approval request to the customer who placed the ad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendProofApprovalRQ(ByVal Ad As Ad)
        Dim sendTo() As String = {Ad.Usr.EmailAddr}
        If _Sys.IsEmailTestMode Then sendTo(0) = _Sys.TestEmailAddr
        Dim CC() As String = {""}
        Dim bcc() As String = {_Sys.BCCEmailAddr}
        '
        ' Scrape web site to get content
        '
        Dim subject As String = "Your proof is available for approval" & sendTo(0)
        Dim URL As String = BuildURL(Ad.ID, Loader.ASPX.ProofApprovalRQ)
        Dim body As String = GetContent(URL)
        '
        ' send the mail
        '
        Send(body, Constants.ProdnEmail, sendTo, bcc, CC, subject)

    End Sub

    ''' <summary>
    ''' Sends the prodn note to prodn staff at at
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendProdnNote(ByVal Ad As Ad)
        Dim sendTo() As String = {Constants.ProdnEmail}
        If _Sys.IsEmailTestMode Then sendTo(0) = _Sys.TestEmailAddr
        Dim CC() As String = {""}
        Dim bcc() As String = {_Sys.BCCEmailAddr}
        '
        ' Scrape web site to get content
        '
        Dim subject As String = "Prodn Request from " & Ad.Usr.AcctAlias & " for ad " & Ad.Adnumber
        Dim URL As String = BuildURL(Ad.ID, Loader.ASPX.ProdnNoteEmail)
        Dim body As String = GetContent(URL)
        '
        ' send the mail
        '
        Send(body, Ad.Usr.EmailAddr, sendTo, bcc, CC, subject)

    End Sub



    Public Function BuildURL(ByVal objectID As Integer, ByVal ASPX As Loader.ASPX) As String
        '
        ' makes up a URL to scrape from the web site. This will be the email message
        ' No need for a slot for email responses
        ' But the user UAM ( for can view price) is passed as param1
        '
        Dim loader As New Loader
        loader.SlotID = ATSystem.SysConstants.nullValue
        loader.NextASPX = ASPX
        loader.ApplicationPath = _Sys.InternalURL
        loader.ObjectID = objectID

        Return loader.Target
    End Function


    ''' <summary>
    ''' This function can be used to scrape a page from a web site. The page is returned as an HTML string.
    ''' </summary>
    ''' <param name="URL">fully qualified URL of the site from which the page will be scraped.</param>
    ''' <returns>HTML page data as as string</returns>
    Public Function GetContent(ByVal URL As String) As String
        '
        ' scrape a page from the web site for the email content
        '
        Dim WR As HttpWebRequest = CType(WebRequest.Create(URL), HttpWebRequest)
        '
        ' next two lines fix .net problem with persistent web requests
        '
        WR.KeepAlive = False
        WR.ProtocolVersion = HttpVersion.Version10
        Dim sr As New IO.StreamReader(WR.GetResponse().GetResponseStream())
        Dim rtnVal As String = sr.ReadToEnd
        sr.Close()
        Return rtnVal

    End Function
    ''' <summary>
    ''' Sends the email content defined in Body to the repients.
    ''' </summary>
    ''' <param name="Body">HTML string which forms the email body</param>
    ''' <param name="from">Email from field</param>
    ''' <param name="recipient">String array of email addresses which will form the TO field</param>
    ''' <param name="bcc">String array of email addresses which will form the BCC field</param>
    ''' <param name="cc">String array of email addresses which will form the CC field</param>
    ''' <param name="subject">Subject line</param>
    Public Sub Send(ByVal Body As String, ByVal from As String, ByVal recipient As String(), ByVal bcc As String(), ByVal cc As String(), ByVal subject As String)

        Dim MailMessage As New MailMessage()
        MailMessage.From = New MailAddress(from)

        For i As Integer = 0 To recipient.Length - 1
            If recipient(i) <> String.Empty Then MailMessage.To.Add(New MailAddress(recipient(i)))
        Next

        For i As Integer = 0 To cc.Length - 1
            If cc(i) <> String.Empty Then MailMessage.CC.Add(New MailAddress(cc(i)))
        Next

        For i As Integer = 0 To bcc.Length - 1
            If bcc(i) <> String.Empty Then MailMessage.Bcc.Add(New MailAddress(bcc(i)))
        Next


        MailMessage.Subject = subject
        MailMessage.Body = Body
        MailMessage.IsBodyHtml = True
        MailMessage.Priority = MailPriority.Normal
        '
        ' Instantiate a new instance of SmtpClient
        ' supported values for authtype are NTLM, Digest, Kerberos, Negotiate
        '
        Dim Smtp As New SmtpClient(_Sys.SMTPHost)
        Dim _SmtpDomain As String = ""
        Dim creds As Net.NetworkCredential = New Net.NetworkCredential(_Sys.SMTPUser, _Sys.SMTPPassword, _SMTPDomain)
        Smtp.Credentials = creds.GetCredential(_Sys.SMTPHost, _Sys.SMTPPort, "Negotiate")
        Smtp.Send(MailMessage)
    End Sub

End Class

