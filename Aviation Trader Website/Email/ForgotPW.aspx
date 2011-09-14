<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ForgotPW.aspx.vb" Inherits="ForgotPW" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Email Password Reminder</title>
</head>
<body style="font-size: 13px; font-family: Trebuchet MS; text-align: center; color: #707070;">
    <table border="0" width="100%">
        <tr>
            <td style="border: solid 1px #c0c0c0; background: #eaeaea; text-align: center; font-size: 24px; font-weight: bold">
                Aviation Trader Password
            </td>
        </tr>
        <tr>
            <td>
                &nbsp
            </td>
        </tr>
        <tr>
            <td align="left" class="contenttext">
                Hello <%=usr.fullname%>
            </td>
        </tr>
        <tr>
            <td align="left" class="contenttext">
                Your Login name is <span style="color: #c70727;">
                <%=usr.emailaddr%> </span> and your password is 
                <span style="color: #c70727;">
                    <%=Usr.Password%></span>
            </td>
        </tr>
       
        <tr>
            <td align="left">
                <br />
                Regards,<br /><br />The team at 
                <b>Aviation Trader</b>
            </td>
        </tr>
    </table>
   </body>
</html>
