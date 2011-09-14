<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Slideshow.aspx.vb" Inherits="Slideshow" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>

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
<body onload="resizePanels()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/System/Webservices.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container">
        <div id="header">
            <uc3:Headerbar ID="headerbar" ShowSearchBar="true" ScriptManagerName="scriptmanager1" runat="server" />
        </div>
        <div id="wrapper">
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="rightpanel" style="padding: 0px;">
             </div>
            <div id="contentpanel2">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table border="0" width="100%">
                            <tr>
                                <td>
                                    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0" width="630" height="440" id="slideshow" />
                                    <param name="movie" value="../flash/slideshow.swf?xml_source=../browseandsearch/SlideShowXMLContent.aspx" />
                                    <param name="quality" value="high" />
                                    <param name="bgcolor" value="#000000" />
                                    <param name="allowScriptAccess" value="sameDomain" />
                                    <embed src="../flash/slideshow.swf?xml_source=../browseandsearch/SlideShowXMLContent.aspx" quality="high" bgcolor="#000000" width="630" height="440" name="slideshow" allowscriptaccess="sameDomain" swliveconnect="true" type="application/x-shockwave-flash" pluginspace="http://www.macromedia.com/go/getflashplayer">
</embed>
                                    </object>
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
