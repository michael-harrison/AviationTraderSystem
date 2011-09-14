<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SaveConfirmation.aspx.vb" Inherits="SaveConfirmation" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>
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
             Your ad has been saved. The ad number is <%=ad.adnumber %>. Your ad can now be retrieved and completed by clicking the ‘Saved’ button in your ad management space. Click HOME on the navigation bar to return to your account management page or simply log out if you have finished your submission session                    </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="errorMsg" class="contenttext error" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:VW2Btn ID="BtnNewAd" CssClass="vwbleft" ScriptManagerName="ScriptManager1" Text="Place another ad" runat="server" />
                                </td>
                                <td align="right">
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
