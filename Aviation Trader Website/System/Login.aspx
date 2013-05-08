<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1"   runat="server">
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
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table border="0">
                            <tr>
                                <td style="width: 520px" class="info">
                                    Please sign in
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="login_form" width="490px" style="border: solid 1px #c0c0c0; border-collapse: collapse;" border="0">
                                        <tr>
                                            <td class="contenttext" style="width: 160px;text-align:right;padding:10px">
                                                <span style="color: red">*&nbsp;</span>Email address :
                                            </td>
                                            <td style="text-align:left">
                                                <asp:TextBox CssClass="contentfield" ID="EmailBox" runat="server" /><br />
                                                <asp:Label CssClass="contenttext error" ID="emailerror" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="contenttext" style="width: 160px;text-align:right;padding:10px">
                                                <span style="color: red">*&nbsp;</span>Password :
                                            </td>
                                            <td align='left'>
                                                <asp:TextBox CssClass="contentfield" ID="PWBox" TextMode="Password" runat="server"></asp:TextBox><br />
                                                <asp:Label CssClass="contenttext error" ID="PWError" Visible="false" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td>
                                                <span id="forgotpw" style="cursor: pointer; color: #c70727;" runat="server">Forgot password?</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <cc1:VW2Btn ID="BtnLogin" CssClass="vwb" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="SIGN IN" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="contenttext">
                                                <span style="color: red">*&nbsp;</span>Denotes required field
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="contenttext">
                                    To lodge an ad or subscribe to our publication on line you must register. If you have already registered, please enter your email address and password, and click ‘Sign In’ to continue.
                                    <br />
                                    <br />
                                    If you have not yet registered, click the Register link on the left to register as an Aviation Trader user
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <uc4:Footerbar ID="footerbar" runat="server" />
    </div>
    </form>
</body>
</html>
