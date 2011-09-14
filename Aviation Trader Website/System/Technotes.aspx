<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Technotes.aspx.vb" Inherits="Technotes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1"   runat="server">
        <Services>
           <asp:ServiceReference Path="~/System/Webservices.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container">
        <div id="header">
            <uc3:Headerbar ID="headerbar" runat="server" />
        </div>
        <div id="wrapper">
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                    <table border="0" width="100%" style="border-collapse: collapse">
                        <tr>
                            <td>
                                <uc22:Navbar ID="NavBar" runat="server" />
                            </td>
                        </tr>
                    <tr>
                        <td class="info">
                            <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="scriptmanager1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="TechnoteList" CellPadding="5" HeaderStyle-BackColor="#eaeaea" BorderStyle="None" CssClass="contenttextleft" DataKeyNames="ID" Width="100%" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Number">
                                        <ItemTemplate>
                                            <a href="<%# Container.DataItem.NavTarget %>">
                                                <%# Container.DataItem.ID %></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-CssClass="contentfield" HeaderText="Title" HeaderStyle-HorizontalAlign="Left" DataField="Name" />
                                    <asp:BoundField ItemStyle-CssClass="contentfield" HeaderText="By" HeaderStyle-HorizontalAlign="Left" DataField="ReportedBy" />
                                    <asp:BoundField ItemStyle-CssClass="contentfield" HeaderText="Date" HeaderStyle-HorizontalAlign="Left" DataField="CreateTime" DataFormatString="{0:d/M/yyyy}" />
                                    <asp:BoundField ItemStyle-CssClass="contentfield" HeaderText="Resolution" HeaderStyle-HorizontalAlign="Left" DataField="Resolution" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                </div>
            </div>
        </div>
        <div id="footer">
            <uc4:Footerbar ID="footerbar" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
