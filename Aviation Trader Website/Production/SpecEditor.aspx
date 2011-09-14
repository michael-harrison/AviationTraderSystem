<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SpecEditor.aspx.vb" Inherits="Production_SpecEditor" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <uc9:BarberPole ID="barberpole" Msg="Please wait - building previews" Left="450px" Top="270px" runat="server" />
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
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
                                    <td style="padding-left: 10px">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td style="width: 520px">
                                                    <asp:Repeater ID="grouplist" runat="server">
                                                        <HeaderTemplate>
                                                            <table style="width: 510px; border: solid 1px #c0c0c0;" border="0px">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="Specgroup">
                                                                    <%#container.DataItem.name %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Repeater ID="speclist" runat="server">
                                                                        <ItemTemplate>
                                                                            <uc7:SpecBuilder ID="spec" Spec="<%#container.dataitem %>" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </td>
                                                <td class="contenttext" style=" vertical-align:top">
                                                    Specifications are optional, but if you include them, it will make it easier for buyers to search and find your ad. Enter specifications for your ad, by selecting each option on the list.
                                                    <br />
                                                    <br />
                                                    If you want a spec to appear in your ad, select the Include checkbox. If this box is not checked, the spec will not be included.
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
        <div id="footer">
            <uc4:Footerbar ID="footerbar" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
