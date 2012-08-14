<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProofList.aspx.vb" Inherits="Production_ProofList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
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
            <div id="leftpanel" style="padding: 0px;">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="rightpanel" style="background: #c0c0c0; padding: 0px;" runat="server">
                <table style="margin: 10px; background: white; border: solid 1px #c70727; border-collapse: collapse; width: 166px; height: 110px;">
                    <tr>
                        <td style="color: #c70727; text-align: left; padding-left: 4px">
                            Find ad...
                        </td>
                    </tr>
                    <tr>
                        <td class="contenttext" style="padding-left: 4px">
                            Ad Number:
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:TextBox ID="AdNumberbox" CssClass="contentfield" Style="margin-left: 4px; width: 148px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="FindError" class="contenttext error" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding: 6px;">
                            <cc1:VW2Btn ID="btnFindAd" CssClass="vwb" Text="Find" IsPostBackMode="true" runat="server" />
                        </td>
                    </tr>
                </table>
                <table style="margin: 10px; background: white; border: solid 1px #c70727; border-collapse: collapse; width: 166px; height: 110px;">
                    <tr>
                        <td style="color: #c70727; text-align: left; padding-left: 4px">
                            Search Preferences...
                        </td>
                    </tr>
                    <tr>
                        <td class="contenttext" style="padding-left: 4px">
                            Filter ads by alias:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="AliasBox" CssClass="contentfield" Style="margin-left: 4px; width: 148px" runat="server" />
                            <cc2:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="AliasBox" ServicePath="~/System/Webservices.asmx" ServiceMethod="GetAliasList" MinimumPrefixLength="2" CompletionInterval="500" EnableCaching="false" CompletionSetCount="4" CompletionListCssClass="autoextender" CompletionListItemCssClass="autoextenderlist" CompletionListHighlightedItemCssClass="autoextenderhighlight">
                            </cc2:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="contenttext" style="padding-left: 4px">
                            Filter ads by Category:
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 184px">
                            <asp:DropDownList ID="CategoryDD" CssClass="contentfield" Style="margin-left: 4px; width: 153px" DataValueField="value" DataTextField="Name" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="contenttext" style="padding-left: 4px">
                            Sort Ads by:
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:DropDownList ID="AdSortDD" CssClass="contentfield" Style="margin-left: 4px; width: 153px" DataValueField="Value" DataTextField="Description" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding: 6px;">
                            <cc1:VW2Btn ID="btnSearchPrefs" CssClass="vwb" Text="Update Prefs" IsPostBackMode="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
           
            <div id="contentpanel2">
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
                                    <table border="0" width="100%" style="border-collapse: collapse">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td rowspan="2" style="width: 75px">
                                            <asp:HyperLink CssClass="contenttext" Font-Underline="false" ID="adlink" NavigateUrl="<%#container.dataitem.NavTarget %>" runat="server">
                                    <%#Container.DataItem.adnumber%></asp:HyperLink><br />Clicks: <%#Container.DataItem.clickcount%>
                                            
                                        </td>
                                        <td>
                                            Category:
                                            <%#container.dataitem.classification.category.name %>
                                            -
                                            <%#container.dataitem.classification.name %>
                                            &nbsp;&nbsp;&nbsp;Advertiser:
                                            <%#Container.DataItem.usr.acctalias%>
                                            &nbsp;-&nbsp;<%#Container.DataItem.ProdnStatus%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink CssClass="contenttext" Font-Underline="false" ID="HyperLink1" NavigateUrl="<%#container.dataitem.NavTarget %>" runat="server">
                                                <uc20:AdReader ID="AdReader" Ad="<%#container.dataitem %>" runat="server" />
                                            </asp:HyperLink>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    <tr>
                                        <td colspan="2">
                                            <hr />
                                        </td>
                                    </tr>
                                </SeparatorTemplate>
                                <FooterTemplate>
                                    <tr>
                                        <td colspan="2" style="background: #eaeaea;">
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
    </form>
</body>
</html>
