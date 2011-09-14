<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HomeGuest.aspx.vb" Inherits="HomeGuest" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>
    <meta name="description" content="Aviation advertising marketplace - buy sell aircraft airplanes helicopters property jobs" />
    <meta name="keywords" content="aviation aircraft airplanes helicopters sell buy sale" />

    <script type="text/javascript" src="JavascriptLibs/Carousel/carousel2.js"></script>

    <script type="text/javascript">

  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-9016102-2']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();

    </script>

</head>
<body onload="resizePanels();readBelt()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/System/Webservices.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container">
        <div id="header">
            <uc3:Headerbar ID="headerbar" ShowSearchBar="true" runat="server" />
        </div>
        <div id="wrapper">
            <div id="leftpanel" style="padding: 0px;" runat="server">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
                <div style="margin-left: 4px">
                    <uc17:AdRotator ID="AdRotator1" category="HomeLeft" Height="540" runat="server" />
                </div>
            </div>
            <div id="rightpanel" style="padding: 0px;">
                <uc23:NewsRotator ID="NewsRotator" runat="server" />
                <uc17:AdRotator ID="AdRotator" category="HomeRight" Height="540" runat="server" />
            </div>
            <div id="contentpanel2">
                <table border="0" width="100%" style="border-collapse: collapse">
                    <tr>
                        <td>
                            <uc25:FlashOrImage ID="FrontPicImage" Width="570" Height="400" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="font-size: 13px; padding: 5px; text-align: center">
                            <span style="color: white; font-size: 0px">Aircraft for sale Aviation advertising marketplace buy sell aircraft airplanes helicopters property jobs</span>
                            <asp:Label ID="FrontPicCaption" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="border: solid 1px #c0c0c0; background: #eaeaea; padding-left: 3px; padding-bottom: 4px">
                            <asp:Repeater ID="featuredAdsList" runat="server">
                                <HeaderTemplate>
                                    <div>
                                        <span style="font-size: 14px; font-weight: bold; color: #c70727">&nbsp;&nbsp;Headline ads...</span>
                                        <!-- Step Carousel -->
                                        <div id="picgallery" class="stepcarousel2" style="height: 147px; width: 574px">
                                            <div id="belt" class="belt" style="left: 0px; width: <%# beltWidth %>px">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class='panel'>
                                        <uc13:InstanceReader ID="InstanceReader" AdInstance="<%#container.dataitem %>" runat="server" />
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                    <img style="position: absolute; top: 45px; left: 0px" src="Graphics/arrowLeft.png" onclick="rotateBelt('R')" />
                                    <img style="position: absolute; top: 45px; right: 0px" src="Graphics/arrowRight.png" onclick="rotateBelt('L')" />
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%; border: solid 1px #c0c0c0; margin-top: 15px">
                                <tr>
                                    <td style="width: 60px">
                                        <asp:HyperLink ID="twitlink" Target="_blank" runat="server">
                             <img style="margin: 5px; border: none 0px" src="Graphics/t_logo-c.png" alt="twitter" />
                                        </asp:HyperLink>
                                    </td>
                                    <td>
                                        <span class="storyintro">Follow our ads on Twitter.
                                            <br />
                                        </span><span class="storybody">Selected ads are published to Twitter. Click the Twitter icon to see the feed.</span>
                                    </td>
                                </tr>
                            </table>
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
