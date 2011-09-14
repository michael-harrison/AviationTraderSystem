<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NewsRotator.ascx.vb" Inherits="ATControls_NewsRotator" %>
<asp:Repeater EnableViewState="false" ID="RotatorList" runat="server">
    <HeaderTemplate>
        <table class="news">
            <tr>
                <td class="rotatortitle">
                    Client Media Releases
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 30px">
                    <cc1:VW2Btn ID="BtnAllNews" CssClass="vwb" ScriptManagerName="ScriptManager1" Text="All Releases" runat="server" />
                </td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td class="rotatorhead">
                <asp:HyperLink ID="HyperLink1" NavigateUrl="<%#Container.DataItem.NavTarget%>" runat="server">
                    <asp:Label ID="StoryHead" Text="<%#Container.DataItem.Name%>" runat="server" />
                </asp:HyperLink>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
