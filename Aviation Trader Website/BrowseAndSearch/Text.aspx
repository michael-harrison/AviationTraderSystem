<%@ Page Language="VB" EnableEventValidation="false" AutoEventWireup="false" CodeFile="Text.aspx.vb" Inherits="Text" %>

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
<body onload="resizePanels();">
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
              <div style="margin-left: 4px">
                <uc17:AdRotator ID="AdRotator" category="Left" Height="540" runat="server" />
            </div>  </div>
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
                                        <div id="PDFPanel" style="margin: 10px" runat="server">
                                        A PDF version of this ad is available.
                                            <asp:HyperLink ID="PDFLink" Target="_blank" runat="server">
                                            Click here to view the PDF.
                                            </asp:HyperLink>
                                        </div>
                                        
                                        
                                        <div style="margin: 10px">
                                            <asp:Label ID="Text" Style="font-weight: normal; color: #404040" runat="server" />
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
