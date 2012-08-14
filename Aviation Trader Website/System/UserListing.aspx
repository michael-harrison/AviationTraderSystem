<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserListing.aspx.vb" Inherits="UserListing" %>

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
            </div>
            <div id="contentpanel1">
                <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <uc22:Navbar ID="NavBar" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="scriptmanager1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Filter users by:
                                <asp:DropDownList CssClass="contentfield width4" ID="UserTypeDD"  AutoPostBack="true" DataTextField="Description" DataValueField="Value" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="UserList" CellPadding="5" ShowFooter="true" BorderStyle="None" HeaderStyle-BackColor="#eaeaea" FooterStyle-BackColor="#eaeaea" EnableViewState="false" CssClass="contentfield" Width="100%" runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <a style="color: #5A86B3; text-decoration: none" href="<%# Container.DataItem.NavTarget %>">
                                                    <%#Container.DataItem.FullName%></a>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                TOTAL
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="left" HeaderText="Acct Alias" ItemStyle-CssClass="contentfield left" DataField="AcctAlias" />
                                        <asp:BoundField HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="left" HeaderText="Company" ItemStyle-CssClass="contentfield left" DataField="Company" />
                                        <asp:BoundField HeaderStyle-Width="95px" HeaderStyle-HorizontalAlign="left" HeaderText="User Type" ItemStyle-CssClass="contentfield left" DataField="LoginLevel" />
                                        <asp:TemplateField HeaderText="Ads" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Right" ItemStyle-CssClass="contentfield right" FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%#Container.DataItem.AdCount%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <%#AdCountTotal%>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="70px" HtmlEncode="false" HeaderStyle-HorizontalAlign="center" HeaderText="Date Added" ItemStyle-CssClass="contentfield center" DataField="CreateTime" DataFormatString="{0:d/M/yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
