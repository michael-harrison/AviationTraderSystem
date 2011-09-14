<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LeftMenu.ascx.vb" Inherits="ATControls_LeftMenu" %>

<table border="0" class="menulist">
    <asp:Repeater ID="menulist" runat="server">
        <ItemTemplate>
            <tr>
                <td id="listitem" runat="server" class="<%#container.dataitem.cssclass %>" style="font-weight:bold">
                    <a href="<%#container.dataitem.navtarget %>">
                        <asp:Label ID="menuitem" Text="<%#container.dataitem.text %>" runat="server" />
                    </a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
