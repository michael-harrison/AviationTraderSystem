<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SpecDefinitionEditor.aspx.vb" Inherits="SpecDefinitionEditor" %>

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
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td>
                                        <uc22:Navbar ID="NavBar" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td class="contenttext right" style="width:250px">
                                                    SpecDefinition Name:
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield" ID="NameBox" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="NameError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Sort Key:
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield" ID="SortKeyBox" runat="server" /><br />
                                                    <asp:Label CssClass="contenttext error" ID="SortKeyError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Display Type :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="displaytypeDD" CssClass="contentfield" DataTextField="Name" DataValueField="value" runat="server" />
                                                </td>
                                                </tr><tr>
                                                    <td class="contenttext right">
                                                    Default values :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="valuelistbox" CssClass="contentfield" TextMode="MultiLine" Height="200" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="valuelistError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Send to another spec group :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="specgroupDD" CssClass="contentfield" DataValueField="hexID" DataTextField="Name" runat="server" />
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
