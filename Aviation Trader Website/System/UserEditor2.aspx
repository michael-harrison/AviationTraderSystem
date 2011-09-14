<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserEditor2.aspx.vb" Inherits="UserEditor2" %>

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
                                    <td class="info">
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
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
                                                <td class="contenttext right" style="width: 160px">
                                                    Email address :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="EmailBox" runat="server" /><br />
                                                    <asp:Label CssClass="contenttext error" ID="emailError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    </span>Password :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="PWBox" runat="server"></asp:TextBox><br />
                                                    <asp:Label CssClass="contenttext error" ID="PWError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                        
                                            <tr>
                                                <td class="contenttext right" style="width: 250px">
                                                    Account Alias :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="AcctAliasBox" runat="server" /><br />
                                                    <asp:Label CssClass="contenttext error" ID="AcctAliasError" Visible="false" runat="server" />
                                                </td>
                                              
                                            </tr>
                                            <tr>
                                                <td class="contenttext right" style="width: 250px">
                                                    Discount % :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width1" ID="DiscountBox" runat="server" /><br />
                                                    <asp:Label CssClass="contenttext error" ID="DiscountError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Login level :
                                                </td>
                                                <td align='left'>
                                                    <asp:DropDownList CssClass="contentfield width4" ID="LoginDD" DataTextField="Description" DataValueField="Value" runat="server" />
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Edition Visibility :
                                                </td>
                                                <td align='left'>
                                                    <asp:DropDownList CssClass="contentfield width3" ID="EditionVisibilityDD" DataTextField="Description" DataValueField="Value" runat="server" />
                                                </td>
                                            </tr>
                                            
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    User is GST Exempt :
                                                </td>
                                                <td align='left'>
                                                <asp:CheckBox ID="GSTCheck" runat="server" />&nbsp;Check this box if user is GST exempt
                                                </td></tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Skin :
                                                </td>
                                                <td align='left'>
                                                    <asp:DropDownList CssClass="contentfield width3" ID="SkinDD" DataTextField="Description" DataValueField="Description" runat="server" />
                                                    
                                                    <cc1:VW2Btn ID="BtnSkinTest" visible="false" CssClass="vwb" Text="Go To Skin Test" runat="server" />
                                                     </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Move all ads to another user
                                                </td>
                                                <td align='left'>
                                                    <asp:DropDownList ID="UsrDD" CssClass="contentfield width3" DataTextField="FullName" DataValueField="hexid" EnableViewState="true" runat="server" />
                                                    &nbsp;Caution - do you really want to do this?
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
