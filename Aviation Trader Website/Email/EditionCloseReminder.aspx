<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditionCloseReminder.aspx.vb" Inherits="EditionCloseReminder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Ad Confirmation</title>
</head>
<body style="font-size: 13px; font-family: Trebuchet MS; text-align: center; color: #707070;">
    <table border="0" width="100%">
        <tr>
            <td style="border: solid 1px #c0c0c0; background: #eaeaea; text-align: center; font-size: 24px; font-weight: bold">
                Aviation Trader Edition Close Reminder
            </td>
        </tr>
        <tr>
            <td>
                &nbsp
            </td>
        </tr>
        <tr>
            <td align="left" class="contenttext">
                Hello
                <%=Ad.Usr.FullName%>
            </td>
        </tr>
        <tr>
            <td align="left" class="contenttext">
                You have an unsubmitted ad
                <%=Ad.Adnumber%>
                which has been scheduled to run in an edition which will close in
                <%=Sys.EditionCloseDays%>
                days from now. Don't miss the deadline!<br />
                <br />
                Please 
                <asp:HyperLink ID="MMALink" Text="login to Manage My Ads" runat="server" />
                , click the saved tab, and submit ad number
                <%=Ad.Adnumber%> to ensure that your ad will be included in the next edition.
            </td>
        </tr>
        <tr>
            <td align="left">
                <br />
                Regards,<br />
                <br />
                The team at <b>Aviation Trader</b>
            </td>
        </tr>
    </table>
</body>
</html>
