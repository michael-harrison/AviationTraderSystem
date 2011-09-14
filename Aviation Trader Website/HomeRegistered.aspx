<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HomeRegistered.aspx.vb" Inherits="HomeRegistered" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>
    <meta name="description" content="Aviation advertising marketplace - buy sell airplanes helicopters property jobs" />
    <meta name="keywords" content="aviation aircraft airplanes helicopters sell buy sale" />

    <script type="text/javascript" src="JavascriptLibs/Carousel/carousel2.js"></script>

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
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                    <table border="0" width="100%">
                        
                        <tr>
                            <td style="padding: 50px">
                                <span class="storyhead">Welcome to Aviation Trader's online acccount management portal.<br />
                                </span><span class="storybody">Choosing to manage your relationship with us on line will help us store and manage your profile, activity and advertising material more effectively and hopefully create a seamless and secure real time environment for you that’s accessible 24/7. <br /><br />
                                This facility has been developed for your convenience but will also assist us to manage costs and therefore keep advertising rates competitive. It is however not the only way you can deal with us. We are committed to maintaining personal contact with our readers, advertisers and subscribers and will always accept material or information by phone, fax or email.<br /><br />
                                 If you need any assistance with the ad lodgment process please feel free to call us on 1800 025 776, 8:30AM – 5:30PM Monday to Friday. This number will divert to a call back service, available 24/7, if you ring outside these hours.<br /><br />
                                  As is the case with all client focused teams we welcome your feedback. Open communication lines are generally the most effective and maintaining effective communication for us is an absolute priority. Please feel free to offer any advice, recommendation, acknowledgement or criticism at any time. We will willingly accept it and do our best to use it to improve the way we conduct our business.</span>
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
        </div>
        <div id="footer">
            <uc4:Footerbar ID="footerbar" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
