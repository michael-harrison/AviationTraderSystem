<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProofApprovalRQ.aspx.vb" Inherits="ProofApprovalRQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Email Proof Approval Request</title>
</head>
<body style="font-size: 13px; font-family: Trebuchet MS; text-align: center; color: #707070;">
    <table border="0" width="100%">
        <tr>
            <td style="border: solid 1px #c0c0c0; background: #eaeaea; text-align: center; font-size: 24px; font-weight: bold">
                Aviation Trader Proof
            </td>
        </tr>
        <tr>
            <td>
                &nbsp
            </td>
        </tr>
        <tr>
            <td align="left" class="contenttext">
                <asp:Label ID="GreetingLabel" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="left" class="contenttext">
                <asp:Label ID="AdDescrLabel" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <br />
                <br />
                <asp:HyperLink ID="prooflink" Font-Bold="true" Font-Underline="true" runat="server">Click here to view proof </asp:HyperLink>.
            </td>
        </tr>
        <tr>
            <td align="left">
                <br />
                <b>Aviation Trader</b>
            </td>
        </tr>
    </table>
</body>
</html>
