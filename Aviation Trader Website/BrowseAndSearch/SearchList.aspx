<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchList.aspx.vb" Inherits="SearchList" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>

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
               <div style="margin-left: 4px">
                <uc17:AdRotator ID="AdRotator" category="Left" Height="1800" runat="server" />
            </div></div>
            <div id="rightpanel" style="padding: 0px;">
                <uc17:AdRotator ID="AdRotator1" category="Right" Height="1800" runat="server" />
            </div>
            <div id="contentpanel2">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table border="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="pageinfo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr style="color: #c0c0c0" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Repeater ID="adList" runat="server">
                                        <HeaderTemplate>
                                            <table border="0" width="100%" cellpadding="0px" style="border-collapse: collapse">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <uc13:InstanceReader ID="instanceReader" AdInstance="<%#container.dataitem %>" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <SeparatorTemplate>
                                            <tr>
                                                <td>
                                                    <hr style="color: #c0c0c0" />
                                                </td>
                                            </tr>
                                        </SeparatorTemplate>
                                        <FooterTemplate>
                                            <tr>
                                                <td>
                                                    <hr style="color: #c0c0c0" />
                                                </td>
                                            </tr>
                                            </table></FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <uc24:PageBar2 ID="PageBar2" runat="server" />
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
