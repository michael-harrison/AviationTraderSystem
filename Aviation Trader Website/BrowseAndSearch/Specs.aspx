<%@ Page Language="VB" EnableEventValidation="false" AutoEventWireup="false" CodeFile="Specs.aspx.vb" Inherits="Specs" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels();">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager2"   runat="server">
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
               <div style="margin-left: 4px">
                <uc17:AdRotator ID="AdRotator" category="Left" Height="540" runat="server" />
           </div> </div>
            <div id="rightpanel">
                <uc17:AdRotator ID="AdRotator1" category="Right" Height="540" runat="server" />
            </div>
            <div id="contentpanel2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                      
                        <table border="0" width="100%">
                          
                            <tr>
                                <td align="right">
                                    <cc1:VW2Btn ID="return2list" CssClass="vwb" Text="return to list" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="Pic" Style="float: left; margin-right: 10px" runat="server" />
                                    <asp:Label ID="KeyWords" Style="font-size: 18px; font-weight: bold; color: #505050" runat="server" /><br />
                                    <asp:Label ID="ItemPrice" Style="font-size: 14px; font-weight: bold; color: #505050" runat="server" /><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="adconstructor" style="border: solid 1px #c0c0c0; padding-top: 5px; min-height: 400px; height: auto!important; height: 400px;">
                                        <uc1:TopMenu ID="tabbar" EnableViewState="true" CSSClass="tabbar" runat="server" />
                                        <div style="margin: 10px">
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td style="width: 520px">
                                                        <div style="width: 570px; border: solid 0px #c0c0c0;">
                                                            <asp:Repeater ID="grouplist" runat="server">
                                                                <HeaderTemplate>
                                                                    <table width="550px" border="0px">
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
                                                                                <HeaderTemplate>
                                                                                    <table width="550px" border="0px">
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td style="width: 200px; font-size: 12px;">
                                                                                            <%#container.DataItem.name %>
                                                                                            :
                                                                                        </td>
                                                                                        <td style="font-weight: bold; font-size: 12px;">
                                                                                            <%#container.DataItem.value %>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                    <tr style="background: #eaeaea">
                                                                                        <td style="width: 200px; font-size: 12px;">
                                                                                            <%#container.DataItem.name %>
                                                                                            :
                                                                                        </td>
                                                                                        <td style="font-weight: bold; font-size: 12px;">
                                                                                            <%#container.DataItem.value %>
                                                                                        </td>
                                                                                    </tr>
                                                                                </AlternatingItemTemplate>
                                                                                <FooterTemplate>
                                                                                    </table>
                                                                                </FooterTemplate>
                                                                            </asp:Repeater>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    </table>
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
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
