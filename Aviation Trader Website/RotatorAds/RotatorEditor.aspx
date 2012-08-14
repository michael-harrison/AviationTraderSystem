<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RotatorEditor.aspx.vb" Inherits="RotatorEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <uc9:BarberPole ID="barberpole" Msg="Please wait - building previews" Left="450px" Top="270px" runat="server" />
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
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellpadding="5">
                                            <tr>
                                                <td class="contenttext right_text" style="width: 180px">
                                                    Name:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="NameBox" CssClass="contentfield width6" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="NameError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Artwork URL:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="ImageURLbox" CssClass="contentfield width6" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="ImageURLError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Click Thru URL:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="ClickURLBox" CssClass="contentfield width6" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="ClickURLError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Category:
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="contentfield width3" ID="CategoryDD" DataTextField="Description" DataValueField="Value" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Type:
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="contentfield width3" ID="TypeDD" DataTextField="Description" DataValueField="Value" runat="server" />
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Width:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="WidthBox" CssClass="contentfield width1" runat="server" />&nbsp;[Top banner: 480px x 89px, vertical ads: use width of 180px] 
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="widthError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Height:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="HeightBox" CssClass="contentfield width1" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="heightError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Space Above:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="MarginTopBox" CssClass="contentfield width1" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="MarginTopError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Space Below:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="MarginBottomBox" CssClass="contentfield width1" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="MarginBottomError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Space Left:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="MarginLeftBox" CssClass="contentfield width1" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="MarginLeftError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Space Right:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="MarginRightBox" CssClass="contentfield width1" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="MarginRightError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    BG Color:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="BGColorBox" CssClass="contentfield width1" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="bgcolorError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Total Inserts:
                                                </td>
                                                <td>
                                                    <asp:Label ID="usageCount" CssClass="contentfield" runat="server" />
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Total Click-throughs:
                                                </td>
                                                <td>
                                                    <asp:Label ID="clickCount" CssClass="contentfield" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Conversion Rate:
                                                </td>
                                                <td>
                                                    <asp:Label ID="conversionRate" CssClass="contentfield" runat="server" />
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
