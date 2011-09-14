<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Headerbar.ascx.vb" Inherits="ATControls_Headerbar" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<table width="100%" style="margin: 0px; padding: 0px; border: none; border-collapse: collapse">
    <tr>
        <td style="padding: 0px; width: 310px; vertical-align: top">
            <img id="Img1" src="~/Graphics/AVT_web.png" alt="Aviation Trader" height="100" runat="server" />
        </td>
        <td style="width: 480px;">
            <uc17:AdRotator ID="AdRotator2" category="MastHead" Height="89" runat="server" />
        </td>
        <td align="right" class="signin" style="padding-right: 10px">
            <asp:HyperLink ID="loginLink"  enableviewstate="false" runat="server" /><br />
            <asp:Label ID="LoginWelcome" enableviewstate="false" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="3" style="padding: 0px; margin: 0px;">
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="searchBar" Visible="false" runat="server">
                        <table width="100%" cellpadding="5px" style="border-collapse:collapse">
                            <tr>
                                <td style="width: 68px">
                                    Search in:
                                </td>
                                <td style="width: 184px">
                                    <asp:DropDownList ID="CategoryDD" CssClass="contentfield" DataValueField="value" DataTextField="Name" AutoPostBack="true" runat="server" />
                                </td>
                                <td style="width: 24px">
                                    For:
                                </td>
                                <td style="width: 265px">
                                    <asp:TextBox ID="SearchBox" CssClass="contentfield width4" runat="server" />
                                    <cc2:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="SearchBox" ServicePath="~/System/Webservices.asmx" ServiceMethod="GetCompletionList" MinimumPrefixLength="2" CompletionInterval="500" EnableCaching="false" CompletionSetCount="4" CompletionListCssClass="autoextender" CompletionListItemCssClass="autoextenderlist" CompletionListHighlightedItemCssClass="autoextenderhighlight">
                                    </cc2:AutoCompleteExtender>
                                </td>
                                <td>
                                    <cc1:VW2Btn ID="btnSearch" CssClass="vwb" Text="Find ads" IsPostBackMode="true" ScriptManagerName="ScriptManager1" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="nullSearchBar" Visible="true" Style="height: 34px;" runat="server" />
        </td>
    </tr>
</table>
<uc1:TopMenu ID="topmenu" CSSClass="topmenu" runat="server" />
