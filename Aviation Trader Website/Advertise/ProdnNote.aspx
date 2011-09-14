<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProdnNote.aspx.vb" Inherits="Advertise_ProdnNote" %>

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
                                </tr>       <tr>
                                    <td colspan="2">
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" IsPostBackMode="true" runat="server" />
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td colspan="2" style="padding-left: 10px">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="left" style="width: 520px">
                                                    <asp:TextBox ID="ProdnRequest" TextMode="MultiLine" class="textinput" Height="180" Width="450" runat="server" />
                                                </td>
                                                <td class="contenttext">
                                                    Please enter any special production instuctions for Aviation Trader production editors here.
                                                  </td>                                            </tr>
                                            <td>
                                                <td style="height: 10px">
                                                </td>
                                            </td>
                                            <tr>
                                                <td align="left" style="width: 500px">
                                                    <asp:TextBox ID="ProdnResponse" TextMode="MultiLine"  ReadOnly="true" class="textinput" Height="180" Width="450" runat="server" />
                                                </td>
                                                <td class="contenttext">
                                   You can view our response to any request you make by viewing this panel through the ‘Manage My Ads’ function on your account management page. We will endeavour to post responses within 24 hours of ad submission.
                                                </td>
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
