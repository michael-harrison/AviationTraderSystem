<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductEditor.aspx.vb" Inherits="ProductEditor" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <uc9:BarberPole ID="barberpole" Msg="Please wait - building previews" Left="450px" Top="270px" runat="server" />
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
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
                                <td style="text-align: center">
                                    <asp:Label ID="FolderLabel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; padding: 3px">
                                    <asp:Image ID="Pic" Style="width: 152px;" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="AliasLabel" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="contentpanel1">
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td>
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="contenttext left" width="100px">
                                                                Category:
                                                            </td>
                                                            <td align="left" width="225px">
                                                                <asp:DropDownList CssClass="contentfield" ID="CatDD" DataTextField="Name" DataValueField="HexID" AutoPostBack="true" runat="server" />
                                                            </td>
                                                            <td class="contenttext left" width="100px">
                                                                Classification:
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList CssClass="contentfield" ID="ClsDD" DataTextField="Name" DataValueField="HexID" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="contenttext left">
                                                                Check here to include ad in latest listings:
                                                            </td>
                                                            <td colspan="2" align="left">
                                                            <asp:CheckBox ID="LatestListingCheck" runat="server" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="contenttext left">
                                                                Check here to show PDF form on tweets and featured ads:
                                                            </td>
                                                            <td colspan="2" align="left">
                                                                <asp:CheckBox ID="PDFHintCheck" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <!-- publication repeater -->
                                                    <asp:Repeater ID="PublicationList" runat="server">
                                                        <HeaderTemplate>
                                                            <table style="border-collapse: collapse">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="info" style="height: 30px; margin-top: 20px; color: white; background: #a0a0a0">
                                                                    <%#container.dataitem.name %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="errorMsg" class="error" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <!-- product repeater -->
                                                                    <asp:Repeater ID="ProductList" runat="server">
                                                                        <HeaderTemplate>
                                                                            <div style="border: solid 1px #c0c0c0; margin-left: 50px; margin-bottom: 15px; padding: 10px">
                                                                                <table border="0" width="100%" style="border-collapse: collapse">
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td class="contenttext">
                                                                                    <span>
                                                                                        <asp:HiddenField ID="productID" Value="<%#container.dataitem.hexid %>" runat="server" />
                                                                                        <%#Container.DataItem.name%>:&nbsp;</span>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:CheckBox ID="ProductCheck" Checked="<%#container.dataitem.checked %>" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table></div>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <!-- edition repeater -->
                                                                    <asp:Repeater ID="EditionList" runat="server">
                                                                        <HeaderTemplate>
                                                                            <div style="border: solid 1px #c0c0c0; margin-left: 50px; margin-bottom: 15px; padding: 10px">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div style="display: inline;">
                                                                                <span class="<%#container.dataitem.cssclass %>">
                                                                                    <asp:HiddenField ID="editionID" Value="<%#container.dataitem.hexid %>" runat="server" />
                                                                                    <%#container.dataitem.Shortname %>
                                                                                    <asp:CheckBox ID="EditionCheck" Width="20" Checked="<%#container.dataitem.checked %>" runat="server" /></span></div>
                                                                        </ItemTemplate>
                                                                        <SeparatorTemplate>
                                                                            <span style="font-size: 17px; color: red">&nbsp;</span></SeparatorTemplate>
                                                                        <FooterTemplate>
                                                                            </td></tr></table></div></FooterTemplate>
                                                                    </asp:Repeater>
                                                                </td>
                                                            </tr>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table></FooterTemplate>
                                                    </asp:Repeater>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc8:ButtonBar ID="ButtonBar2" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
