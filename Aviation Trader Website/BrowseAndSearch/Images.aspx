<%@ Page Language="VB" EnableEventValidation="false" AutoEventWireup="false" CodeFile="Images.aspx.vb" Inherits="Images" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>
    <script type="text/javascript" src="../JavascriptLibs/Carousel/carousel.js"></script>

</head>
<body onload="resizePanels();getImages(<%= Ad.ID %>)">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
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
            </div>
            </div>
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
                                        <!-- HR Image load area -->
                                        <div id="Picviewer" style="background-color: white; width: 569px; height: 350px; margin: 0px; padding-left: 5px">
                                        </div>
                                        <!-- Step Carousel -->
                                        <div id="picgallery" class="stepcarousel" style="height: 110px; width: 574px">
                                            <div id="belt" class="belt" style="left: 0px;">
                                            </div>
                                            <img style="position: absolute; top: 35px; left: 0px" src="../Graphics/arrowLeft.png" onclick="rotateBelt('R')" />
                                            <img style="position: absolute; top: 35px; right: 0px" src="../Graphics/arrowRight.png" onclick="rotateBelt('L')" />
                                        </div>
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
