<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ImageUploader.aspx.vb" Inherits="Production_ImageUploader" %>

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
                                        <uc10:UploadBar ID="UploadBar" Text="Upload Image" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px">
                                        <table width="100%" border="0">
                                            
                                            <tr>
                                                <td>
                                                    <div style="width: 740px; border solid 1px #c0c0c0;">
                                                        <asp:Repeater ID="ImageList" runat="server">
                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" style="border-collapse: collapse">
                                                                    <tr>
                                                                        <td align="center" style="width: 65px; background: #eaeaea">
                                                                            Set<br />
                                                                            Main<br />
                                                                            Image
                                                                        </td>
                                                                        <td align="center" style="width: 60px; background: #eaeaea">
                                                                            Web<br />
                                                                            Enabled
                                                                        </td>
                                                                        <td style="background: #eaeaea">
                                                                        </td>
                                                                        <td align="center" style="width: 60px; background: #eaeaea">
                                                                            Delete<br />
                                                                            Image
                                                                        </td>
                                                                        <td style="text-align: center; width: 165px">
                                                                            Source Image Stats
                                                                        </td>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td align="center" style="background: #eaeaea">
                                                                        <cc1:GroupRadioButton ID="ismainimageradio" GroupName="groupradio" Checked="<%#container.dataitem.IsMainImage %>" runat="server" />
                                                                    </td>
                                                                    <td align="center" style="background: #eaeaea">
                                                                   <asp:CheckBox ID="WebEnabledCheck" Checked="<%#container.dataitem.iswebenabled %>" runat="server" />
                                                                   
                                                                     </td>
                                                                     
                                                                        <td style="border: 1px solid #c0c0c0; padding: 10px">
                                                                        <asp:Image ID="image" ImageUrl="<%#container.dataitem.loresURL %>" Width="340" runat="server" />
                                                                    </td>
                                                                    <td align="center" style="background: #eaeaea">
                                                                        <asp:CheckBox ID="deletecheck" runat="server" />
                                                                    </td>
                                                                    <td style="padding: 5px">
                                                                        <table border="0" width="100%">
                                                                            <tr>
                                                                                <td style="width: 65px">
                                                                                    Name:
                                                                                </td>
                                                                                <td>
                                                                                    <%#container.DataItem.shortfilename %>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Res (ppi):
                                                                                </td>
                                                                                <td>
                                                                                    <%#container.DataItem.resolution %>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Pixels:
                                                                                </td>
                                                                                <td>
                                                                                    <%#container.DataItem.pixelwidth %>&nbsp;x&nbsp;
                                                                                    <%#container.DataItem.pixelheight %>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    mm:
                                                                                </td>
                                                                                <td>
                                                                                    <%#convert.toint32(25.4 * container.DataItem.pixelwidth / container.DataItem.resolution) %>&nbsp;x&nbsp;
                                                                                    <%#convert.toint32(25.4 * container.DataItem.pixelheight / container.DataItem.resolution) %>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Status:
                                                                                </td>
                                                                                <td>
                                                                                    <%#container.DataItem.prodnstatus.tostring %>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr>
                                                                    <td colspan="4" style="height: 10px; background: #eaeaea">
                                                                    </td>
                                                                </tr>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    </div>
                    <div id="footer">
                        <uc4:Footerbar ID="footerbar" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
