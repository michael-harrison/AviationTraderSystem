<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BrowseAllClassifications.aspx.vb" Inherits="BrowseAllClassifications" %>

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
            <table width="100%" border="0" style="border-collapse: collapse">
                <tr>
                    <td>
                        <uc3:Headerbar ID="headerbar" ShowSearchBar="true" ScriptManagerName="scriptmanager1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="wrapper">
            <div id="leftpanel" style="padding: 0px;" runat="server">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="rightpanel">
                <uc17:AdRotator ID="AdRotator" category="Right" Height="540" runat="server" />
            </div>
            <div id="contentpanel2">
                <table id="table" border="0" width="100%" runat="server">
                    <tr>
                        <td style="vertical-align: top">
                        </td>
                        <td style="vertical-align: top">
                        </td>
                        <td style="vertical-align: top">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="footer">
            <uc4:Footerbar ID="footerbar" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
