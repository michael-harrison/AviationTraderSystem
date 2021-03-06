﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SubmitConfirmation.aspx.vb" Inherits="SubmitConfirmation" %>

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
                <img src="../Graphics/AdThermo5-5.png" alt="step 6" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table border="0" width="100%">
                            <tr>
                                <td width="250px">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="storybody">
                                    Thank you for submitting your ad which has now entered our production system. Your ad reference is
                                    <%=ad.Adnumber %>.<br />
                                    <br />
                                    Our production team will proof your material, optimise the text and any images and confirm the cost. You can view the status of your ad and make or request changes by clicking the ‘Manage My Ads’ button on your account management page.
                                    <br><br />
                                    </br> Click HOME on the navigation bar to return to your account management page or simply log out if you have finished your submission session.
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="errorMsg" class="contenttext error" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <!-- publication repeater -->
                                    <asp:Repeater ID="PublicationList" runat="server">
                                        <HeaderTemplate>
                                            <table style="margin-bottom: 10px; border: solid 1px #c0c0c0; border-collapse: collapse" border="0">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td colspan="2" style="font-weight: bold; height: 30px; margin-top: 5px;">
                                                    <%#container.dataitem.name %>
                                                </td>
                                            </tr>
                                            <!-- edition repeater -->
                                            <asp:Repeater ID="EditionList" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td colspan="1" style="height: 20px; margin-top: 20px; padding-left: 40px;">
                                                            <%#container.dataitem.name %>
                                                        </td>
                                                        <td colspan="1" style="text-align: right; height: 20px; margin-top: 20px; padding-left: 40px;">
                                                            Onsale date :
                                                            <%# container.DataItem.onsaledate %>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="height: 5px;">
                                                        </td>
                                                    </tr>
                                                    <!-- product repeater -->
                                                    <asp:Repeater ID="ProductList" runat="server">
                                                        <HeaderTemplate>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <div style="border: solid 1px #c0c0c0; margin-left: 100px; margin-bottom: 5px; padding: 10px">
                                                                        <table border="0" width="100%" style="border-collapse: collapse">
                                                                            <tr>
                                                                                <td width="450px">
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                            <!--  <tr>
                                                                <td colspan="3" class="error">
                                                                    Reserved for feedback
                                                                </td>
                                                            </tr>-->
                                                                            <tr class="info" style="background: #e7f3ff;">
                                                                                <td colspan="2">
                                                                                    Product
                                                                                </td>
                                                                            </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td colspan="2" class="contenttext">
                                                                    <span>
                                                                        <%#Container.DataItem.name%>:&nbsp;</span>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table></div>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            </div> </td> </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table></FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:VW2Btn ID="BtnNewAd" CssClass="vwbleft" ScriptManagerName="ScriptManager1" Text="Place another ad" runat="server" />
                                </td>
                                   <td align="right">
                                    <cc1:VW2Btn ID="btnEmail" CssClass="vwb" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Send Email Confirmation" runat="server" />
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
    </div>
    </form>
</body>
</html>
