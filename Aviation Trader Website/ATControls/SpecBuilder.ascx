<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SpecBuilder.ascx.vb" Inherits="ATControls_SpecBuilder" %>
<table width="100%" border="0">
    <tr>
        <td style="width: 130px">
           <asp:Label ID="nameLabel" runat="server" />
            :
        </td>
        <td style="width: 290px">
            <asp:TextBox ID="texttype" CssClass="contentfield width4" runat="server" />
            <asp:TextBox ID="textareatype" CssClass="contentfield" TextMode="MultiLine" Width="180" Height="130" runat="server" />
            <asp:RadioButtonList ID="radioverticaltype" CssClass="contentfield" RepeatDirection="Vertical" runat="server" />
            <asp:RadioButtonList ID="radiohorizontaltype" CssClass="contentfield" RepeatDirection="Horizontal"  runat="server" />
            <asp:CheckBoxList ID="checkboxtype" CssClass="contentfield" runat="server" />
        </td>
        <td align="center">
            Include<br />
            <asp:CheckBox ID="activecheck" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <hr />
        </td>
    </tr>
</table>
