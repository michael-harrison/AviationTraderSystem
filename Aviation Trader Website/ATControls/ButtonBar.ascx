<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ButtonBar.ascx.vb" Inherits="ATControls_ButtonBar" %>
<table id="bb" border="0" width="100%" runat="server">

<tr>
    <td class="msg error">
        <asp:Label ID="msgbox" CssClass="msg" runat="server" />
    </td>

    <td align="left" style="width: 25%">
        <cc1:VW2Btn ID="a0" CSSClass="vwb" IsPostBackMode="true" Visible="false" runat="server" />
    </td>
    <td align="center" style="width: 25%">
        <cc1:VW2Btn ID="a1" CssClass="vwb" IsPostBackMode="true" Visible="false" runat="server" />
    </td>
    <td align="right" style="width: 25%">
        <cc1:VW2Btn ID="a2" CssClass="vwb" IsPostBackMode="true" Visible="false" runat="server" />
    </td>
   
</tr>
<tr>
    <td colspan="4">
        <hr />
    </td>
</tr>
</table> 