<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TechnoteEditor.aspx.vb" Inherits="TechnoteEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" cellpadding="5" cellspacing="5">
                                <tr>
                                    <td>
                                        <uc22:Navbar ID="NavBar" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="scriptmanager1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td style="text-align:left;width:55px">
                                                    <asp:Label ID="titleLabel" runat="server" />
                                                </td>
                                                <td style="text-align:left;width:360px">
                                                    <asp:TextBox CssClass="contentfield width5" ID="Namebox" runat="server" />
                                                <br />
                                                    <asp:Label ID="NameError" class="error" runat="server" />
                                                </td>
                                                <td style="text-align: left; width: 50px">
                                                    Status:
                                                </td>
                                                <td style="text-align: left; width: 80px">
                                                    <asp:DropDownList CssClass="contentfield width2" ID="StatusDropDown" DataTextField="Name" DataValueField="Value" runat="server" />
                                                </td>
                                                <td style="text-align: left; width: 70px">
                                                    Resolution:
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="contentfield width2" ID="ResolutiondropDown" DataTextField="Name" DataValueField="Value" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr noshade="noshade" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td align="left" class="info">
                                                    Description
                                                </td>
                                                <td align="left" class="info">
                                                    Fix
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contentfield">
                                                    Reported by :
                                                    <asp:DropDownList CssClass="contentfield width1" ID="ReportedByDropDown" DataTextField="Name" DataValueField="Value" runat="server" />
                                                    On
                                                    <asp:Label ID="ReportedBox" runat="server" Text="" />
                                                </td>
                                                <td class="contentfield">
                                                    Fixed by
                                                    <asp:DropDownList CssClass="contentfield width1" ID="FixedByDropDown" DataTextField="Name" DataValueField="Value" runat="server" />
                                                    On
                                                    <asp:Label ID="FixedBox" runat="server" Text="" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox CssClass="contentfield" Width="360" ID="ProblemDescriptionBox" TextMode="MultiLine" Rows="15" runat="server" />
                                                    <asp:Label ID="ProblemDescriptionError" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox CssClass="contentfield" Width="360" ID="ProblemFixBox" TextMode="MultiLine" Rows="15" runat="server" />
                                                    <asp:Label ID="ProblemFixError" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
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
