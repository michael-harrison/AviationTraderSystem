<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SubsRequest.aspx.vb" Inherits="SubsRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Ad Confirmation</title>
    <style type="text/css">
        .contenttext
        {
            font-size: 12px;
            padding-right: 18px;
            color: #999999;
            font-weight: normal;
        }
        .contentfield
        {
            width: 180px;
            text-align: left;
            font-family: Trebuchet MS;
            font-size: 12px;
            font-weight: normal;
            color: #404040;
        }
        .left
        {
            text-align: left;
        }
        .center
        {
            text-align: center;
        }
        .right
        {
            text-align: right;
        }
    </style>
</head>
<body style="font-size: 13px; font-family: Trebuchet MS; text-align: center; color: #707070;">
    <table border="0" width="500px">
        <tr>
            <td colspan="2" style="border: solid 1px #c0c0c0; background: #eaeaea; text-align: center; font-size: 24px; font-weight: bold">
                Aviation Trader Subscription Request
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left" class="contenttext">
                Received a request to modify the subscription (as shown in the subject line) which was originated by the following user:
            </td>
        </tr>
        <tr>
            <td width="200px" class="contenttext right">
                Account Alias :
            </td>
            <td class="contentfield">
                <%=Usr.acctalias%></span>
            </td>
        </tr>
        <tr>
            <td class="contenttext right">
                Email address :
            </td>
            <td class="contentfield">
                <%=Usr.emailaddr%></span>
            </td>
        </tr>
        <tr>
            <td class="contenttext right">
                Name :
            </td>
            <td class="contentfield">
                <%=Usr.FullName%></span>
            </td>
        </tr>
        <tr>
            <td class="contenttext right">
                Phone :
            </td>
            <td class="contentfield">
                <%=Usr.Phone%></span>
            </td>
        </tr>
    </table>
</body>
</html>
