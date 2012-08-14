<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Editor2.aspx.vb" Inherits="Editor2" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1"   runat="server">
        <Services>
           <asp:ServiceReference Path="~/System/Webservices.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container_top">
        <div id="header">
            <uc3:Headerbar ID="headerbar" runat="server" />
        </div>
    </div>
    <div id="container">
        <div id="wrapper">
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td width="250px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="info">
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td class="contenttext right_text">
                                        SMTP Server :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="SMTPServerBox" runat="server" />
                                        <br />
                                        <asp:Label CssClass="contenttext error" ID="SMTPServerError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        SMTP Port :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="SMTPPortBox" runat="server" /><br />
                                        <asp:Label CssClass="contenttext error" ID="SMTPPortError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        SMTP User Name :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width3" ID="SMTPUserBox" runat="server" /><br />
                                        <asp:Label CssClass="contenttext error" ID="SMTPUserError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        SMTP Password :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width3" ID="SMTPPasswordBox" runat="server" /><br />
                                        <asp:Label CssClass="contenttext error" ID="SMTPPasswordError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="contenttext right_text">
                                        BCC Email Addr :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width4" ID="BCCEmailAddrBox" runat="server" /><br />
                                        <asp:Label CssClass="contenttext error" ID="BCCEmailAddrError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Test Email Addr activated :</td>
                                    <td align="left">
                                        <asp:CheckBox ID="TestEmailCheck" runat="server" /> Check this box to send all emails to the test address
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Test Email Addr :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width4" ID="TestEmailAddrBox" runat="server" /><br />
                                        <asp:Label CssClass="contenttext error" ID="TestEmailAddrError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Edition Close Days :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="EditionCloseDaysBox" runat="server" /> No. of days before ad deadline that emails will be sent for saved ads<br />
                                        <asp:Label CssClass="contenttext error" ID="EditionCLoseDaysError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
