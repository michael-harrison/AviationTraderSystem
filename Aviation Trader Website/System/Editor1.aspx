<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Editor1.aspx.vb" Inherits="Editor1" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
                <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                    <table border="0" width="100%" style="border-collapse: collapse">
                        <tr>
                            <td width="250px">
                            </td>
                            <td>
                            </td>
                            <td width="135px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="info">
                                <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <uc8:ButtonBar ID="ButtonBar"  runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                System Build Info :
                            </td>
                            <td align='left'>
                                <asp:Label ID="buildinfo" CssClass="contentfield" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                System Name :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield" ID="NameBox" runat="server" />
                                <br />
                                <asp:Label CssClass="contenttext error" ID="NameError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Website Physical Path :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width6" ID="PhysicalApplicationPathBox" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="PhysicalApplicationPathError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Internal URL :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width6" ID="InternalURLBox" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="InternalURLError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                External URL :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width6" ID="ExternalURLBox" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="ExternalURLError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Thumbnail Image Height :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width2" ID="THBImageHeightBox" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="THBImageHeightError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Low Res Image Height :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width2" ID="LRImageHeightBox" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="LRImageHeightError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="contenttext right_text">
                                Front Pic Image Type:
                            </td>
                            <td>
                                <asp:DropDownList CssClass="contentfield width3" ID="FrontPicTypeDD" DataTextField="Description" DataValueField="Value" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Front Pic Image Source:
                            </td>
                            <td>
                                <asp:FileUpload CssClass="contentfield width5" ID="FileUpload1" runat="server" />
                            </td>
                            <td align="right">
                                <cc1:VW2Btn CssClass="vwb" IsPostBackMode="true" Text="UPLOAD IMAGE" ID="BtUpload1" OnClientClick="showPole(500);" runat="server" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                            </td>
                            <td colspan="2" class="msg error">
                                <asp:Label ID="msgbox1" CssClass="msg" runat="server" />
                            </td>
                        </tr>
                        
                        
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                            <uc25:FlashOrImage ID="Image1" Width="200" Height="140" runat="server" />
                           
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="contenttext right_text">
                                Front Pic Caption :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width4" ID="FrontPicCaptionBox" TextMode="MultiLine" Width="350" Height="100" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="FrontPicCaptionError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Back Pic Image Type:
                            </td>
                            <td>
                                <asp:DropDownList CssClass="contentfield width3" ID="BackPicTypeDD" DataTextField="Description" DataValueField="Value" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Back Pic Image Source:
                            </td>
                            <td>
                                <asp:FileUpload CssClass="contentfield width5" ID="FileUpload2" runat="server" />
                            </td>
                            <td align="right">
                                <cc1:VW2Btn CssClass="vwb" IsPostBackMode="true" Text="UPLOAD IMAGE" ID="BtUpload2" OnClientClick="showPole(500);" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2" class="msg error">
                                <asp:Label ID="msgbox2" CssClass="msg" runat="server" />
                            </td>
                        </tr>
                       
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <uc25:FlashOrImage ID="Image2" Width="200" Height="140" runat="server" />
                            </td>
                        </tr>
                       
                        <tr>
                            <td class="contenttext right_text">
                                Back Pic Caption :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width4" ID="BackPicCaptionBox" TextMode="MultiLine" Width="350" Height="100" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="BackPicCaptionError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Cover Pic Source:
                            </td>
                            <td>
                                <asp:FileUpload CssClass="contentfield width5" ID="FileUpload3" runat="server" />
                            </td>
                            <td align="right">
                                <cc1:VW2Btn CssClass="vwb" IsPostBackMode="true" Text="UPLOAD IMAGE" ID="BtUpload3" OnClientClick="showPole(500);" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2" class="msg error">
                                <asp:Label ID="msgbox3" CssClass="msg" runat="server" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <asp:Image ID="Image3" Width="200px" runat="server" />
                            </td>
                        </tr>
                      
                        
                        
                      
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
