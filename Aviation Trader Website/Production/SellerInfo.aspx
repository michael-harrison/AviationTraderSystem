<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SellerInfo.aspx.vb" Inherits="Production_SellerInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <uc9:BarberPole ID="barberpole" Msg="Please wait - building previews" Left="450px" Top="270px" runat="server" />
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="leftpanel">
                        <uc2:LeftMenu ID="leftmenu" runat="server" />
                        <table style="margin-top: 10px; background: white; border: solid 1px #c0c0c0; border-collapse: collapse; width: 160px; margin-left: 10px">
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="CatLabel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="ClsLabel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="statusLabel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="FolderLabel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; padding: 3px">
                                    <asp:Image ID="Pic" Style="width: 152px;" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="AliasLabel" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="contentpanel1">
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td>
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td style="width: 520px">
                                                    <div style="width: 570px; border: solid 0px #c0c0c0;">
                                                        <table width="550px" border="0px">
                                                            <tr>
                                                                <td style="width: 200px; text-align: right; font-size: 12px; padding-right: 20px">
                                                                    Alias :
                                                                </td>
                                                                <td style="font-weight: bold; font-size: 12px;">
                                                                    <asp:Label ID="Aliasx" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                    Re-Assign ad to another user :
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="AliasDD" CssClass="contentfield" DataTextField="AcctAlias" DataValueField="Hexid" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 200px; text-align: right; font-size: 12px; padding-right: 20px">
                                                                    Name :
                                                                </td>
                                                                <td style="font-weight: bold; font-size: 12px;">
                                                                    <asp:Label ID="Name" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                    Email Address :
                                                                </td>
                                                                <td style="font-weight: bold; font-size: 12px;">
                                                                    <asp:Label ID="Email" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                    Company :
                                                                </td>
                                                                <td style="font-weight: bold; font-size: 12px;">
                                                                    <asp:Label ID="Company" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                    Website :
                                                                </td>
                                                                <td style="font-weight: bold; font-size: 12px;">
                                                                    <asp:HyperLink ID="Website" Target="_blank" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                    Phone :
                                                                </td>
                                                                <td style="font-weight: bold; font-size: 12px;">
                                                                    <asp:Label ID="Phone" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                    After Hours Phone :
                                                                </td>
                                                                <td style="font-weight: bold; font-size: 12px;">
                                                                    <asp:Label ID="AHPhone" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                    Mobile :
                                                                </td>
                                                                <td style="font-weight: bold; font-size: 12px;">
                                                                    <asp:Label ID="Mobile" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right; font-size: 12px; padding-right: 20px">
                                                                    Fax :
                                                                </td>
                                                                <td style="font-weight: bold; font-size: 12px;">
                                                                    <asp:Label ID="Fax" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
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
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
