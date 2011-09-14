<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PriceEditor.aspx.vb" Inherits="PriceEditor" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <uc9:BarberPole ID="barberpole" Msg="Please wait - building previews" Left="450px" Top="270px" runat="server" />
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
                        <table style="margin-top: 10px; background: white; border: solid 1px #c0c0c0; border-collapse: collapse; width: 160px; margin-left: 10px">
                            <tr>
                                <td style="text-align: center">
                                    <img id="Img1" alt="Print this page" src="~/Graphics/printicon.gif" style="cursor: pointer" onclick="window.print();" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="contentpanel1">
                        <table border="0" width="100%" style="border-collapse: collapse">
                            <tr>
                                <td>
                                    <table width="100%" border="1" style="border: solid 0px #c0c0c0; border-collapse: collapse">
                                        <tr>
                                            <td colspan="4">
                                                <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="background:#eaeaea">
                                            <td>
                                                Current Prodn Status:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ProdnStatusDD" CssClass="contentfield width3" DataTextField="Description" DataValueField="Value" runat="server" />
                                            </td>
                                            <td align="right">
                                                Send To:
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="contentfield" ID="SendToDD" DataTextField="Name" DataValueField="HexID" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="adconstructor" style="border: solid 1px #c0c0c0; padding-top: 5px;">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <uc1:TopMenu ID="tabbar" IsPostBackMode="false" CSSClass="tabbar" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Repeater ID="InstanceList" runat="server">
                                                        <HeaderTemplate>
                                                            <table width="100%" border="1">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="info" style="background: #e7f3ff; margin-top: 5px;">
                                                                    <%#Container.DataItem.edition.publication.name%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="contentfield" style="margin-top: 20px; padding-left: 40px">
                                                                    <%#Container.DataItem.edition.name%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-bottom: 20px">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="width: 300px;">
                                                                                <asp:Image ID="Image1" ImageUrl="<%#container.dataitem.PreviewImageURL %>" Width="200px" Style="border: solid 1px #c0c0c0; padding: 10px;" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <uc21:PriceBlock ID="priceBlock" OnPriceChange="PriceChangeEvent" AdInstance="<%# Container.DataItem %>" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="info" style="background: #e7f3ff; margin-top: 5px;">
                                                    TOTAL PRICE
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table border="0" width="100%">
                                                        <tr>
                                                            <td style="width: 430px">
                                                            </td>
                                                            <td>
                                                                Subtotal:
                                                            </td>
                                                            <td style="text-align: right; padding-right: 20px">
                                                                <asp:Label ID="subtotalLabel" CssClass="contentfield" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr id="latestListingRow" runat="server">
                                                            <td style="width: 430px">
                                                            </td>
                                                            <td>
                                                                Latest Listing Loading:
                                                            </td>
                                                            <td style="text-align: right; padding-right: 20px">
                                                                <asp:Label ID="LatestListingLabel" CssClass="contentfield" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 430px">
                                                            </td>
                                                            <td>
                                                                Discount:
                                                            </td>
                                                            <td style="text-align: right; padding-right: 20px">
                                                                <asp:Label ID="DiscountLabel" CssClass="contentfield" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                GST:
                                                            </td>
                                                            <td style="text-align: right; padding-right: 20px">
                                                                <asp:Label ID="GSTLabel" CssClass="contentfield" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                TOTAL PRICE:
                                                            </td>
                                                            <td style="text-align: right; padding-right: 20px">
                                                                <asp:Label ID="TotalLabel" CssClass="contentfield" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="footer">
            <uc4:Footerbar ID="footerbar" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
