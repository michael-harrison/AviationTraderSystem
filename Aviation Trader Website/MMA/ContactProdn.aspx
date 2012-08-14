<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ContactProdn.aspx.vb" Inherits="ContactProdn" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
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
                <table style="margin-top: 10px; background: white; border: solid 1px #c0c0c0; border-collapse: collapse; width: 160px; margin-left: 10px">
                    <tr>
                        <td style="text-align: center">
                            <asp:Label ID="CatLabel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <asp:Label ID="ClsLabel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <asp:Label ID="statusLabel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; padding: 3px">
                            <asp:Image ID="Pic" Style="width: 152px;" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td colspan="2">
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 10px">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td style="width: 490px" class="contenttext">
                                                    <asp:TextBox ID="ProdnRequest" TextMode="MultiLine" class="textinput" Height="180" Width="450" runat="server" />
                                                </td>
                                                <td class="contenttext">
                                                    Please call Aviation Trader Production Staff on 02 6622 2133. Your ad number is displayed on the left.<br />
                                                    <br />
                                                    Alternatively you can enter your requests in the box on the left and click Save Changes. Your message will be forwarded to the production team.
                                                </td>
                                            </tr>
                                        </table>
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
