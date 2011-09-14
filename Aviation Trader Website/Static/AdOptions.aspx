<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AdOptions.aspx.vb" Inherits="AdOptions" %>

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
            <uc3:Headerbar ID="headerbar" ShowSearchBar="true" runat="server" />
        </div>
        <div id="wrapper">
            <div id="leftpanel" style="padding: 0px;" runat="server">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                    <table border="0" width="100%">
                        <tr>
                            <td class="info">
                                <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <cc1:VW2Btn ID="btnRegister" CssClass="vwb" ScriptManagerName="ScriptManager1" Text="Register as new user" runat="server" />
                            </td>
                        </tr>
                        <cc1:InsertFileText ID="InsertFileText" Filename="statictext/adOptions.txt" runat="server" />
                        
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
