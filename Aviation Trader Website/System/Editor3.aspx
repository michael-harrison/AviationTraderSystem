<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Editor3.aspx.vb" Inherits="Editor3" %>

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
                                <uc8:ButtonBar ID="ButtonBar" runat="server" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="contenttext right_text">
                                Twitter User Name :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width4" ID="TwitUserNameBox" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="TwitUserNameError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="contenttext right_text">
                                Twitter Consumer Key :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width6" ID="TwitConsumerKeyBox" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="TwitConsumerKeyError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Twitter Consumer Key Secret:
                            </td>
                            <td colspan="2" align='left'>
                                <asp:TextBox CssClass="contentfield width6" ID="TwitConsumerKeySecretBox" runat="server" /><br />
                                <asp:Label CssClass="contenttext error" ID="TwitConsumerKeySecretError" Visible="false" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Twitter OAuth Token :
                            </td>
                            <td colspan="2" align='left'>
                                <asp:Label CssClass="contenttext" ID="TwitOAuthTokenBox" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contenttext right_text">
                                Twitter OAuth Token Secret:
                            </td>
                            <td colspan="2" align='left'>
                                <asp:Label CssClass="contenttext" ID="TwitOAuthTokenSecretBox" runat="server" />
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
