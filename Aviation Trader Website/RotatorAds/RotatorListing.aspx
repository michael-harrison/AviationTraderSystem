<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RotatorListing.aspx.vb" Inherits="RotatorListing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
    <div id="container">
        <div id="header">
            <uc3:Headerbar ID="headerbar" runat="server" />
        </div>
        <div id="wrapper">
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <div style="border: solid 1px #c0c0c0; padding-top: 5px; padding-bottom: 5px;">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
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
                                <asp:GridView ID="RotatorList" CellPadding="5" HeaderStyle-BackColor="#eaeaea" BorderStyle="None" CssClass="contenttextleft" DataKeyNames="ID" Width="100%" runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Name">
                                            <ItemTemplate>
                                                <a href="<%# Container.DataItem.NavTarget %>" style="font-size: 12px">
                                                    <%#Container.DataItem.Name%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-CssClass="contentfield" HeaderText="Type" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Left" DataField="Type" />
                                        <asp:BoundField ItemStyle-CssClass="contentfield right" HeaderText="Width" HeaderStyle-Width="60" HeaderStyle-HorizontalAlign="Right" DataField="Width" />
                                        <asp:BoundField ItemStyle-CssClass="contentfield right" HeaderText="Height" HeaderStyle-Width="60" HeaderStyle-HorizontalAlign="Right" DataField="Height" />
                                        <asp:BoundField ItemStyle-CssClass="contentfield right" HeaderText="Inserts" HeaderStyle-Width="60" HeaderStyle-HorizontalAlign="Right" DataField="UsageCount" />
                                        <asp:BoundField ItemStyle-CssClass="contentfield right" HeaderText="Clicks" HeaderStyle-Width="60" HeaderStyle-HorizontalAlign="Right" DataField="ClickCount" />
                                        <asp:BoundField ItemStyle-CssClass="contentfield right" HeaderText="Conv Rate" HeaderStyle-Width="80" HeaderStyle-HorizontalAlign="Right" DataField="ConversionRate" DataFormatString="{0:p1}" />
                                        <asp:BoundField ItemStyle-CssClass="contentfield" HeaderText="Created" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Left" DataField="CreateTime" DataFormatString="{0:d/M/yyyy}" />
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
