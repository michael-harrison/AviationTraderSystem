<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HomeINH.aspx.vb" Inherits="HomeINH" %>

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
            <div id="leftpanel" style="padding: 0px;" runat="server">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="rightpanel" style="padding: 0px;">
            </div>
            <div id="contentpanel2">
                <table border="0" width="100%" style="border-collapse: collapse">
                    <cc1:InsertFileText ID="InsertFileText" Filename="statictext/HomeINH.txt" runat="server" />
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
                    
                    
                    <tr>
                        <td>
                            <table style="width: 100%; border: solid 1px #c0c0c0; margin-top: 15px">
                                <tr>
                                    <td style="width: 60px">
                                    </td>
                                    <td>
                                        <span class="storyintro">From the development labs...#1
                                            <br />
                                        </span><span class="storybody">Advertisers can now include video of their product. This is a proof-of-concept demo to show how user-generated video content can be included in ads. See BA for details on how to insert video.</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%; border: solid 1px #c0c0c0; margin-top: 15px">
                                <tr>
                                    <td style="width: 60px">
                                        <asp:HyperLink ID="slideshowlink"  runat="server">
                             <img style="margin: 5px; border: none 0px" src="Graphics/slideshow.jpg" width="40px" alt="slideshow" />
                                        </asp:HyperLink>
                                    </td>
                                    <td>
                                        <span class="storyintro">From the development labs...#2
                                            <br />
                                        </span><span class="storybody">A multimedia slideshow. This is a demo to show how images can be collated to a slide show, and an audio track added. Make sure your sound is ON! The sound track could be a voiceover describing the aircraft for sale. This demo cycles thru all the classads in the current edition which have color pics. In the final version you will be able to click on any of the pics as it comes up to view the ad details, then return to the show. Alternative data sources could be the featured ads list, or all of the images for a particular ad. IE each advertiser can get his own slideshow with a professional voiceover describing the product.</span>
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
