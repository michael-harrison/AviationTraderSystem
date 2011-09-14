<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TextEditor.aspx.vb" Inherits="Advertise_TextEditor" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>
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
            <div id="leftpanel" style="background: #eaeaea">
                <img src="../Graphics/AdThermo3-5.png" alt="step 2" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td colspan="2" class="info">
                                        Step 3: Construct Your Ad.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" IsPostBackMode="true" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 10px">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td style="width: 490px" class="contenttext">
                                                    Ad text:<br />
                                                    <asp:TextBox ID="AdText" TextMode="MultiLine" class="textinput" Height="280" Width="450" runat="server" />
                                                </td>
                                                <td class="contenttext">
                                       Enter the text for your ad in the panel on the left and upload an image or images if you have chosen a photo classie or photo display ad.<br /><br />
                                        You can change your text or any image you upload at any time before submission and also cancel your submission or save and retrieve it for completion through your ‘Manage My Ads’ function. You will find these options at step 4.        </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <cc1:VW2Btn ID="BtnCategorySelector" CssClass="vwbleft" ScriptManagerName="ScriptManager1" IsPostBackMode="true" Text="Step 2 - Select Category" runat="server" />
                                    </td>
                                    <td align="right">
                                        <cc1:VW2Btn ID="BtnReview" CssClass="vwbright" ScriptManagerName="ScriptManager1" IsPostBackMode="true" Text="Step 4 - Continue" runat="server" />
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
