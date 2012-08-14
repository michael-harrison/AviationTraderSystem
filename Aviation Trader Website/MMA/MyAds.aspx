<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MyAds.aspx.vb" Inherits="Advertise_MyAds" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>

    <script language="javascript" type="text/javascript">
 
    
      function deleteConfirm() {
          return confirm('This ad cannot be recovered once it is deleted. Are you sure you want to delete the ad?');
    }
    
    </script>

</head>
<body onload="resizePanels()">
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
                    <table border="0" width="100%">
                        <tr>
                            <td class="info">
                                <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Repeater ID="adList" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" style="border-collapse: collapse">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 75px">
                                                <asp:HyperLink CssClass="contenttext" Font-Underline="false" ID="adlink" NavigateUrl="<%#container.dataitem.NavTarget %>" runat="server">
                                    <%#Container.DataItem.adnumber%></asp:HyperLink>
                                            </td>
                                            <td style="width: 475px">
                                                <uc20:AdReader ID="AdReader" Ad="<%#container.dataitem %>" runat="server" />
                                            </td>
                                            <td>
                                                <table width="100%" style="background: #eaeaea; border: solid 1px #c0c0c0; border-collapse: collapse">
                                                    <tr>
                                                        <td align="center" style="background: white; padding: 5px">
                                                            <cc1:VW2Btn ID="btnRead" Visible="false" CssClass="vwb" Text="View Content" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="background: white; padding: 5px">
                                                            <cc1:VW2Btn ID="btnPreview" Visible="false" CssClass="vwb" Text="View Ad" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="background: white; padding: 3px">
                                                            <cc1:VW2Btn ID="btnCopyAd" Visible="false" CssClass="vwb" Text="Duplicate Ad" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="background: white; padding: 3px">
                                                            <cc1:VW2Btn ID="btnRepeatAd" Visible="false" CssClass="vwb" Text="Repeat Ad" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="background: white; padding: 3px">
                                                            <cc1:VW2Btn ID="btnEditAd" Visible="false" CssClass="vwb" Text="Change Ad" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="background: white; padding: 3px">
                                                            <cc1:VW2Btn ID="btnSubmitAd" Visible="false" CssClass="vwb" Text="Submit Ad" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="background: white; padding: 3px">
                                                            <cc1:VW2Btn ID="btnDeleteAd" Visible="false" CssClass="vwb" Text="Delete Ad" IsPostBackMode="true" OnClientClick="return deleteConfirm()" OnClick="deleteAd" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="background: white; padding: 3px">
                                                            <cc1:VW2Btn ID="btnContactProdn" Visible="false" CssClass="vwb" Text="Contact Prodn" runat="server" />
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td align="center" style="background: white; padding: 3px">
                                                            <asp:Label ID="ErrorMsg" name="ErrorMsg" runat="server" CssClass="msg" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        <tr>
                                            <td colspan="3">
                                                <hr />
                                            </td>
                                        </tr>
                                    </SeparatorTemplate>
                                    <FooterTemplate>
                                        <tr>
                                            <td colspan="3" style="background: #eaeaea;">
                                                <%#itemCount%>
                                                Ads found
                                            </td>
                                        </tr>
                                        </table></FooterTemplate>
                                </asp:Repeater>
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
