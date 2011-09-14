<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SubsManager.aspx.vb" Inherits="SubsManager" %>

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
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                    <table border="0" width="100%">
                        <tr>
                            <td style="padding: 50px">
                                <span class="storyhead">Welcome to Aviation Trader Subscription Manager<br />
                                </span>This portal allows you to manage your subscription to Aviation Trader and if you wish, create a new account or accounts. Follow the instructions provided below for the option that best suits your need and we’ll respond accordingly.
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <table style="margin-left: 130px; margin-bottom: 20px; width: 500px; border: solid 1px #c0c0c0">
                                    <tr>
                                        <td>
                                            <span class="storyhead">Activate Subscription...<br />
                                            </span>Ready to subscribe? Excellent! All you need to do is click the ‘subscribe’ button. This will alert our subscription team who will activate your subscription and contact you to arrange payment.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="padding: 6px;">
                                            <cc1:VW2Btn ID="btnActivate" CssClass="vwb" Text="Subscribe" IsPostBackMode="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="msgActivate" class="contenttext error" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="margin-left: 130px; margin-bottom: 20px; width: 500px; border: solid 1px #c0c0c0">
                                    <tr>
                                        <td>
                                            <span class="storyhead">Renew Subscription...<br />
                                            </span>Like to renew your subscription? That’s great! We look forward to providing you with ongoing service. Simply click the ‘renew subscription’ button and our subscription team will renew your subscription and contact you to arrange payment.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="padding: 6px;">
                                            <cc1:VW2Btn ID="btnRenew" CssClass="vwb" Text="Renew Subscription" IsPostBackMode="true" runat="server" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <asp:Label ID="msgRenew" class="contenttext error" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <table style="margin-left: 130px; margin-bottom: 20px; width: 500px; border: solid 1px #c0c0c0">
                                    <tr>
                                        <td>
                                            <span class="storyhead">Create additional subscripton...<br />
                                            </span>Like to create an additional subscription for an associate, friend or family member? Great gift idea! Simply click the ‘create subscription’ button, and our subscription team will contact you to make the necessary arrangements.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="padding: 6px;">
                                            <cc1:VW2Btn ID="btnCreate" CssClass="vwb" Text="Create Subscription" IsPostBackMode="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="msgCreate" class="contenttext error" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="margin-left: 130px; width: 500px; border: solid 1px #c0c0c0">
                                    <tr>
                                        <td>
                                            <span class="storyhead">Cancel Subscription...<br />
                                            </span>Are you sure? If so, click the ‘cancel subscription’ button and our subscription team will remove you from our circulation database. Being a customer focused team, we may call you to make sure your decision was not the result of poor service or something we’ve done that disappointed you.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="padding: 6px;">
                                            <cc1:VW2Btn ID="btnCancel" CssClass="vwb" Text="Cancel Subscription" IsPostBackMode="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="msgCancel" class="contenttext error" runat="server" />
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
    </div>
    </form>
</body>
</html>
