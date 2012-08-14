<%@ Page Language="VB" EnableEventValidation="false" AutoEventWireup="false" CodeFile="Seller.aspx.vb" Inherits="Seller" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels();">
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
               <div style="margin-left: 4px">
                <uc17:AdRotator ID="AdRotator" category="Left" Height="540" runat="server" />
            </div></div>
            <div id="rightpanel">
                <uc17:AdRotator ID="AdRotator1" category="Right" Height="540" runat="server" />
            </div>
            <div id="contentpanel2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                   
                        <table border="0" width="100%">
                           
                            <tr>
                                <td align="right">
                                    <cc1:VW2Btn ID="return2list" CssClass="vwb" Text="return to list" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="Pic" Style="float: left; margin-right: 10px" runat="server" />
                                    <asp:Label ID="KeyWords" Style="font-size: 18px; font-weight: bold; color: #505050" runat="server" /><br />
                                    <asp:Label ID="ItemPrice" Style="font-size: 14px; font-weight: bold; color: #505050" runat="server" /><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="adconstructor" style="border: solid 1px #c0c0c0; padding-top: 5px; min-height: 400px; height: auto!important; height: 400px;">
                                        <uc1:TopMenu ID="tabbar" EnableViewState="true" CSSClass="tabbar" runat="server" />
                                        <div style="margin: 10px">
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td style="width: 520px">
                                                        <div style="width: 570px; border: solid 0px #c0c0c0;">
                                                            <table width="550px" border="0px">
                                                                <tr id="NameRow" runat="server">
                                                                    <td style="width: 200px; text-align: right; font-size: 12px; padding-right: 20px">
                                                                        Name :
                                                                    </td>
                                                                    <td style="font-weight: bold; font-size: 12px;">
                                                                        <asp:Label ID="Name" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="EmailRow" runat="server">
                                                                    <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                        Email Address :
                                                                    </td>
                                                                    <td style="font-weight: bold; font-size: 12px;">
                                                                        <asp:Label ID="Email" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="companyrow" runat="server">
                                                                    <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                        Company :
                                                                    </td>
                                                                    <td style="font-weight: bold; font-size: 12px;">
                                                                        <asp:Label ID="Company" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="websiterow" runat="server">
                                                                    <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                        Website :
                                                                    </td>
                                                                    <td style="font-weight: bold; font-size: 12px;">
                                                                        <asp:HyperLink ID="Website" Target="_blank" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="PhoneRow" runat="server">
                                                                    <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                        Phone :
                                                                    </td>
                                                                    <td style="font-weight: bold; font-size: 12px;">
                                                                        <asp:Label ID="Phone" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="AHPhoneRow" runat="server">
                                                                    <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                        After Hours Phone :
                                                                    </td>
                                                                    <td style="font-weight: bold; font-size: 12px;">
                                                                        <asp:Label ID="AHPhone" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="MobileRow" runat="server">
                                                                    <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                        Mobile :
                                                                    </td>
                                                                    <td style="font-weight: bold; font-size: 12px;">
                                                                        <asp:Label ID="Mobile" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="FaxRow" runat="server">
                                                                    <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                        Fax :
                                                                    </td>
                                                                    <td style="font-weight: bold; font-size: 12px;">
                                                                        <asp:Label ID="Fax" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="AddrRow" runat="server">
                                                                    <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                        Address :
                                                                    </td>
                                                                    <td style="font-weight: bold; font-size: 12px;">
                                                                        <asp:Label ID="Addr" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>                                       </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
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
