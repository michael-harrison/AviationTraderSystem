<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UploadBar.ascx.vb" Inherits="ATControls_UploadBar" %>
<table border="0" width="100%">
    <tr>
        <td align="left">
            <asp:FileUpload CssClass="contentfield width6" ID="FileUpload1" runat="server" />
        </td>
        <td align="right" style="width: 135px">
            <cc1:VW2Btn CSSClass="vwb" ispostbackmode="true" Text="UPLOAD FILE" ID="BtUpload" OnClientClick="showPole(500);" runat="server" />
        </td>
        <td align="right" style="width: 155px">
            <cc1:VW2Btn CssClass="vwb" IsPostBackMode="true" Text="DELETE SELECTED" ID="BtDelete" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2" class="msg error">
            <asp:Label ID="msgbox" CssClass="msg" runat="server" />
        </td>
    </tr>
</table>
