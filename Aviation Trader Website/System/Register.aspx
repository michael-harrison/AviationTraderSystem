<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Register.aspx.vb" Inherits="Register" %>

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
                <table border="0">
                    <tr>
                        <td style="width: 520px" class="info">
                            Please supply the following registration information
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="490px" cellpadding="10px" style="border: solid 1px #c0c0c0; border-collapse: collapse;" border="0">
                                <tr>
                                    <td class="contenttext right" style="width: 160px">
                                        <span style="color: red">*&nbsp;</span>Email address :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield" ID="EmailBox" runat="server" /><br />
                                        <asp:Label CssClass="contenttext error" ID="emailError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        <span style="color: red">*&nbsp;</span>Password :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield" ID="PWBox" TextMode="Password" runat="server"></asp:TextBox><br />
                                        <asp:Label CssClass="contenttext error" ID="PWError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
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
                                    <td class="contenttext right">
                                        <span style="color: red">*&nbsp;</span>First Name :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width4" ID="FNameBox" runat="server"></asp:TextBox><br />
                                        <asp:Label CssClass="contenttext error" ID="FNameError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        <span style="color: red">*&nbsp;</span>Last Name :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width4" ID="LNameBox" runat="server"></asp:TextBox><br />
                                        <asp:Label CssClass="contenttext error" ID="LNameError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        Company :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width4" ID="CompanyBox" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="contenttext error" ID="CompanyError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        ACN/ABN :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width3" ID="ACNBox" runat="server"></asp:TextBox>
                                        <br />
                                        <asp:Label CssClass="contenttext error" ID="ACNError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        Website :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width4" ID="WebsiteBox" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="contenttext error" ID="WebsiteError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        <span style="color: red">*&nbsp;</span>Phone :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width3" ID="PhoneBox" runat="server"></asp:TextBox>
                                        <br />
                                        <asp:Label CssClass="contenttext error" ID="PhoneError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        <span style="color: red">*&nbsp;</span>Address 1 :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width4" ID="Addr1Box" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="contenttext error" ID="Addr1Error" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        Address 2 :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width4" ID="Addr2Box" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="contenttext error" ID="Addr2Error" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        <span style="color: red">*&nbsp;</span>Suburb :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width4" ID="SuburbBox" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="contenttext error" ID="SuburbError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        <span style="color: red">*&nbsp;</span>State :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield" ID="StateBox" runat="server"></asp:TextBox><br />
                                        <asp:Label CssClass="contenttext error" ID="StateError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        <span style="color: red">*&nbsp;</span>Postcode :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="PostcodeBox" runat="server"></asp:TextBox><br />
                                        <asp:Label CssClass="contenttext error" ID="PostcodeError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right">
                                        <span style="color: red">*&nbsp;</span>Country :
                                    </td>
                                    <td align='left'>
                                        <asp:DropDownList CssClass="contentfield width4" ID="CountryDD" DataTextField="Description" DataValueField="Name" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <cc1:VW2Btn ID="BTNRegister" CssClass="vwb" ScriptManagerName="ScriptManager1" IsPostBackMode="true" Text="Update details and login" runat="server" />
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
                            Registration with Aviation Trader is easy.<br />
                            <br />
                            Just fill in the registration form. Your email address will be your Aviation Trader user name.
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
