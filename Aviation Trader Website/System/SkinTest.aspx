<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SkinTest.aspx.vb" Inherits="SkinTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>

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
            <uc3:Headerbar ID="headerbar" ShowSearchBar="true" runat="server" />
        </div>
        <div id="wrapper">
            <div id="leftpanel">
                LEFT PANEL<br />
                Select Length:<br />
                <asp:TextBox ID="leftLength" Text="5" runat="server" /><br />
                <asp:Label ID="left" runat="server" />
            </div>
            <div id="rightpanel">
                RIGHT PANEL<br />
                Select Length:<br />
                <asp:TextBox ID="rightLength" Text="5" runat="server" /><br />
                <asp:Label ID="right" runat="server" />
            </div>
            <div id="contentpanel2">
                CONTENT PANEL<br />
                Select Length:<br />
                <asp:TextBox ID="contentLength" Text="5" runat="server" /><br />
                <asp:Label ID="content2" runat="server" />
            </div>
        </div>
        <div id="footer">
            <uc4:Footerbar ID="footerbar" runat="server" />
        </div>
    </div>
    <table width="400px">
        <tr>
            <td>
                Select Skin
            </td>
            <td>
                <asp:DropDownList ID="skinDD" DataValueField="Description" DataTextField="Description" runat="server" />
            </td>
            <td>
                <cc1:VW2Btn ID="BtnRefresh" CssClass="vwb" IsPostBackMode="true" Text="Refresh" runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
