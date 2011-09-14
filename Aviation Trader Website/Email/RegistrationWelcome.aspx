<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RegistrationWelcome.aspx.vb" Inherits="RegistrationWelcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Email Password Reminder</title>
    
    <style type="text/css">
        .storyhead
        {
            font-size: 16px;
            font-weight: bold;
            color: #c70727;
            line-height: 55px;
        }
        .storyintro
        {
            font-size: 14px;
            font-weight: bold;
            color: #404040;
            text-align:left;
        }
        .storybody
        {
            font-size: 12px;
            color: #404040;
            text-align:left;
        }
        .storypicpanel
        {
            float: right;
            width: 250px;
            background: #eaeaea;
            margin-left: 10px;
        }
        .storypic
        {
            width: 250px;
        }
        .storypiccaption
        {
            font-size: 12px;
            color: #404040;
        }
    </style>
</head>
<body style="font-size: 13px; font-family: Trebuchet MS; color: #707070;">
    <div style="margin:auto;width:600px;border: solid 1px #c0c0c0; padding: 10px;">
        <table border="0" width="100%">
        
        <tr>
            <td style="padding: 0px; width: 310px; height: 89px; vertical-align: top">
                <asp:Image id="ATlogo" AlternateText="Aviation Trader" height="100" runat="server" />
            </td>
        </tr>
       
    
        <tr>
            <td>
                <span class="storyhead">Welcome
                    <%=usr.fullname%></span>
            </td>
        </tr>
        <tr>
            <td>
                
                
                <span class="storybody">Thank you for registering with Aviation Trader.<br /><br />
                Your login name is your email address - <%=usr.EmailAddr %>, and your password is <%=Usr.Password%>.<br /><br />
                Please record and keep these details in a safe place.<br /><br />
                <a href="http://120.151.17.241">Click here </a>to go to the Aviation Trader web site.
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
    </div>
</body>
</html>
