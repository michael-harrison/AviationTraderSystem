<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProdnNote.aspx.vb" Inherits="ProdnNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Ad Confirmation</title>
</head>
<body style="font-size: 13px; font-family: Trebuchet MS; text-align: center; color: #707070;">
    <table border="0" width="100%">
        <tr>
            <td style="border: solid 1px #c0c0c0; background: #eaeaea; text-align: center; font-size: 24px; font-weight: bold">
                Aviation Trader Prodn Note for ad
                <%=Ad.Adnumber%>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp
            </td>
        </tr>
        <tr>
            <td align="left" class="contenttext">
                                <%=Ad.Usr.AcctAlias%>
            </td>
        </tr>
        <tr>
            <td align="left" class="contenttext">
                Sends prodn request as follows: for ad <span style="color: #c70727;">
                    <%=Ad.Adnumber%></span>
            </td>
        </tr>
        <tr>
            <td align="left" class="contenttext">
                  <%=Ad.ProdnRequestHTML%>
            </td>
        </tr>
        
    </table>
</body>
</html>
