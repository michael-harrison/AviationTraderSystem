<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NewsEditor.aspx.vb" Inherits="NewsEditor" %>

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
                <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                    <table border="0" width="100%" cellpadding="5" cellspacing="5">
                        <tr>
                            <td>
                                <uc8:ButtonBar ID="ButtonBar" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellpadding="5">
                                    <tr>
                                        <td align="right" style="width: 80px">
                                            Status:
                                        </td>
                                        <td style="text-align:">
                                            <asp:DropDownList CssClass="contentfield width3" ID="StatusDropDown" DataTextField="Name" DataValueField="Value" runat="server" />
                                        </td>
                                        <td style="width:135px"/>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr noshade="noshade" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            Head:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox CssClass="contentfield" Width="560px" ID="Namebox" runat="server" />
                                            <br />
                                            <asp:Label ID="NameError" CssClass="contenttext error" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Intro:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox CssClass="contentfield" Width="560" ID="IntroBox" TextMode="MultiLine" Rows="10" runat="server" />
                                            <br />
                                            <asp:Label ID="IntroError" CssClass="contenttext error" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Body:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox CssClass="contentfield" Width="560" ID="BodyBox" TextMode="MultiLine" Rows="15" runat="server" />
                                            <br />
                                            <asp:Label ID="BodyError" CssClass="contenttext error" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Pic:
                                        </td>
                                        <td>
                                            <asp:FileUpload CssClass="contentfield width5" ID="FileUpload1" runat="server" />
                                        </td>
                                        <td align="right">
                                            <cc1:VW2Btn CssClass="vwb" IsPostBackMode="true" Text="UPLOAD IMAGE" ID="BtUpload" OnClientClick="showPole(500);" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="2" class="msg error">
                                            <asp:Label ID="msgbox" CssClass="msg" runat="server" />
                                        </td>
                                    </tr>
                                    <asp:Panel ID="picPanel" runat="server">
                                        <tr>
                                           <td></td>
                                                <td>
                                                    <asp:Image ID="pic" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="deleteCheck" Text="Delete" runat="server" />
                                                </td>
                                           
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                Pic Caption:
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox CssClass="contentfield" Width="560" ID="PicCaptionBox" TextMode="MultiLine" Rows="3" runat="server" />
                                                <br />
                                                <asp:Label ID="PicCaptionError" CssClass="contenttext error" runat="server" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="footer">
            <uc4:Footerbar ID="footerbar" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
