<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserEditor1.aspx.vb" Inherits="UserEditor1" %>

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
                                                <td class="contenttext right" style="width: 270px">
                                                    Email Address :
                                                </td>
                                                <td align='left' style="width:270px">
                                                    <asp:label CssClass="contentfield width4" ID="EmailBox" runat="server" />
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="emailcheck" runat="server" />
                                                    </td>
                                                <td align="left" style="width: 150px">
                                                    Check the box to allow readers to see the field in your ads
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td class="contenttext right">
                                                    First Name :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width4" ID="FNameBox" runat="server" />
                                         </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="FNameCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Last Name :
                                                </td>
                                                <td align='left'>
                                                    <asp:label CssClass="contentfield width4" ID="LNameBox" runat="server" />
                                                       </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="LNameCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Company :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width4" ID="CompanyBox" runat="server" /><br />
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="CompanyCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    ACN/ABN :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width3" ID="ACNBox" runat="server" /><br />
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="ACNCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Website :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width4" ID="WebsiteBox" runat="server" /><br />
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="WebsiteCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Phone :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width3" ID="PhoneBox" runat="server" /><br />
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="PhoneCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    After Hours Phone :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width3" ID="AHPhoneBox" runat="server" />
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="AHPhoneCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Mobile :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width3" ID="MobileBox" runat="server" />
                                                 </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="MobileCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Fax :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width3" ID="FaxBox" runat="server" />
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="FaxCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Address 1 :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width4" ID="Addr1Box" runat="server" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Address 2 :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width4" ID="Addr2Box" runat="server" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Suburb :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width4" ID="SuburbBox" runat="server" />
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="AddrCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    PostCode :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width2" ID="PostcodeBox" runat="server" />
                                                 </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    State :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield" ID="Statebox" runat="server" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Country :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width4" ID="CountryBox"  runat="server" />
                                                </td>
                                                <td>
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
