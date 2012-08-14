<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Impersonate.aspx.vb" Inherits="Impersonate" %>

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
                <asp:Repeater ID="usrlist" runat="server">
                    <HeaderTemplate>
                        <table cellpadding="5px" width="100%" border="0" style="border: solid 1px #c0c0c0; border-collapse: collapse">
                            <tr>
                                <td style="width:150px;border: solid 1px #c0c0c0;background: #eaeaea">
                                    Name
                                </td>
                                <td style="width: 120px; border: solid 1px #c0c0c0; background: #eaeaea">
                                    Acct Alias
                                </td>
                                <td style="width: 270px; border: solid 1px #c0c0c0; background: #eaeaea">
                                    Email Address
                                </td>
                                <td style="border: solid 1px #c0c0c0; background: #eaeaea">
                                    Address
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <cc1:ClickableCell ID="clickCell" style="border: solid 1px #c0c0c0;" OnClick="impersonate_Click" runat="server" CssClass="<%#container.dataitem.navtarget %>" Value="<%#container.dataitem.hexid %>" Text="<%#container.dataitem.fullname %>">
                                     
                        </cc1:ClickableCell>
                        <td style="border: solid 1px #c0c0c0;">
                            <%#container.dataitem.acctalias %>
                        </td>
                        <td style="border: solid 1px #c0c0c0;">
                            <%#container.dataitem.emailaddr %>
                        </td>
                        <td style="border: solid 1px #c0c0c0;">
                            <%#container.dataitem.addr1 %>&nbsp;
                            <%#container.dataitem.suburb %>&nbsp;
                            <%#container.dataitem.postcode %>&nbsp;
                            <%#container.dataitem.state %>&nbsp;
                        </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
