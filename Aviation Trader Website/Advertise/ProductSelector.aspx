<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProductSelector.aspx.vb" Inherits="ProductSelector" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
            <div id="leftpanel" style="background: #eaeaea">
                <img src="../Graphics/AdThermo1-5.png" alt="step 3" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table border="0" width="100%" style="border-collapse: collapse">
                            
                            <tr>
                                <td colspan="2" class="info">
                                    Step 1: Select your product and booking schedule
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Errormsg1" CssClass="error" runat="server" />
                                </td>
                                <td align="right">
                                    <cc1:VW2Btn ID="BtnContentEditor2" CssClass="vwbright" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Step 2 - select ad category" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <!-- publication repeater -->
                                    <asp:Repeater ID="PublicationList" runat="server">
                                        <HeaderTemplate>
                                            <table style="border-collapse: collapse">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="height: 30px" />
                                            </tr>
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
                                                    <asp:Repeater ID="ProductList"  runat="server">
                                                        <HeaderTemplate>
                                                            <div style="border: solid 1px #c0c0c0; margin-left: 50px; margin-bottom: 15px; padding: 10px">
                                                                <table border="0" width="100%" style="border-collapse: collapse">
                                                                    <tr>
                                                                        <td width="450px">
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="info" style="background: #e7f3ff;">
                                                                        <td>
                                                                            Product
                                                                        </td>
                                                                        <td align="right">
                                                                            Check to select
                                                                        </td>
                                                                    </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="contenttext">
                                                                    <span class="info">
                                                                        <asp:HiddenField ID="productID" Value="<%#container.dataitem.hexid %>"  runat="server" />
                                                                        <%#Container.DataItem.name%>:&nbsp;</span><%#container.dataitem.description %>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:CheckBox ID="ProductCheck" Checked="<%#container.dataitem.checked %>" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <SeparatorTemplate>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <hr />
                                                                </td>
                                                            </tr>
                                                        </SeparatorTemplate>
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
                                                                    <tr class="info" style="background: #e7f3ff;">
                                                                        <td>
                                                                            Edition
                                                                        </td>
                                                                        <td align="right">
                                                                            Check to select
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div style="display: inline;">
                                                                 <span class="<%#container.dataitem.cssclass %>"> 
                                                                     <asp:HiddenField ID="editionID" Value="<%#container.dataitem.hexid %>"  runat="server" />
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
                            <tr>
                                <td>
                                    <asp:Label ID="ErrorMsg2" CssClass="error" runat="server" />
                                </td>
                                <td align="right">
                                    <cc1:VW2Btn ID="BtncontentEditor" CssClass="vwbright" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Step 2 - select ad category" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                        </table>
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
