<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserEditor3.aspx.vb" Inherits="UserEditor3" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
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
                                                <td class="contenttext right_text" style="width: 160px">
                                                    Email address :
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield width4" ID="EmailBox" runat="server" /><br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>Password :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield" ID="PWBox" runat="server"></asp:TextBox><br />
                                                    <asp:Label CssClass="contenttext error" ID="PWError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>Registration Type :
                                                </td>
                                                <td align='left'>
                                                    <asp:DropDownList CssClass="contentfield width4" ID="UserTypeDD" DataTextField="Description" DataValueField="Value" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>First Name :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="FNameBox" runat="server"></asp:TextBox><br />
                                                    <asp:Label CssClass="contenttext error" ID="FNameError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>Last Name :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="LNameBox" runat="server"></asp:TextBox><br />
                                                    <asp:Label CssClass="contenttext error" ID="LNameError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Company :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="CompanyBox" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="contenttext error" ID="CompanyError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    ACN/ABN :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width3" ID="ACNBox" runat="server"></asp:TextBox>
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="ACNError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Website :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="WebsiteBox" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="contenttext error" ID="WebsiteError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>Phone :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width3" ID="PhoneBox" runat="server"></asp:TextBox>
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="PhoneError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    AH Phone :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width3" ID="AHPhoneBox" runat="server"></asp:TextBox>
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="AHPhoneError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Mobile :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width3" ID="MobileBox" runat="server"></asp:TextBox>
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="MobileError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Fax :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width3" ID="FaxBox" runat="server"></asp:TextBox>
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="FaxError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>Address 1 :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="Addr1Box" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="contenttext error" ID="Addr1Error" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Address 2 :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="Addr2Box" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="contenttext error" ID="Addr2Error" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>Suburb :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width4" ID="SuburbBox" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="contenttext error" ID="SuburbError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>State :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield" ID="StateBox" runat="server"></asp:TextBox><br />
                                                    <asp:Label CssClass="contenttext error" ID="StateError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>Postcode :
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield width1" ID="PostcodeBox" runat="server"></asp:TextBox><br />
                                                    <asp:Label CssClass="contenttext error" ID="PostcodeError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    <span style="color: red">*&nbsp;</span>Country :
                                                </td>
                                                <td align='left'>
                                                    <asp:DropDownList CssClass="contentfield width4" ID="CountryDD" DataTextField="Description" DataValueField="Name" runat="server" />
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td colspan="2" class="contenttext">
                                                    <span style="color: red">*&nbsp;</span>Denotes required field
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
    </div>
    </form>
</body>
</html>
