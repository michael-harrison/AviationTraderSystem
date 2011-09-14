<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PriceBlock.ascx.vb" Inherits="ATControls_PriceBlock" %>
<div style="border: solid 1px #c0c0c0; margin-bottom: 5px; margin-right: 5px">
    <table width="100%" border="0">
        <tr>
            <td colspan="2" style="padding-bottom: 5px; border-bottom: solid 1px #c0c0c0; background: #eaeaea;">
                <asp:Label ID="productlabel" runat="server" />
            </td>
        </tr>
        <asp:Panel ID="classadPanel" runat="server">
            <!--
                        <tr>
                            <td style="width: 120px">
                                Computed Lines: &nbsp;
                            </td>
                            <td align="right">
                                <asp:Label ID="ComputedHeightlabel" CssClass="contentfield width2" runat="server" />
                            </td>
                        </tr>-->
            <tr>
                <td style="width: 150px" class="contenttext right">
                    Lines: &nbsp;
                </td>
                <td align="right" style="padding-right: 10px">
                    <asp:DropDownList ID="ClassadLinesDD" CssClass="contentfield width2" DataTextField="Description" DataValueField="Value" AutoPostBack="true" runat="server" />
                    <asp:Label ID="ClassadLinesLabel" CssClass="contentfield width2" runat="server" />
                </td>
            </tr>
        </asp:Panel>
        <asp:Panel ID="displayPanel" runat="server">
            <tr>
                <td style="width: 150px" class="contenttext right">
                    Computed size: &nbsp;
                </td>
                <td align="right" style="padding-right: 10px">
                    <asp:Label ID="ComputedSizelabel" CssClass="contentfield width2" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 150px" class="contenttext right">
                    Size: &nbsp;
                </td>
                <td align="right" style="padding-right: 10px">
                    <asp:DropDownList ID="DisplayHeightDD" CssClass="contentfield width2" DataTextField="Description" DataValueField="Value" AutoPostBack="true" runat="server" />
                    <asp:Label ID="DisplayHeightLabel" CssClass="contentfield width1" runat="server" />
                    &nbsp;x&nbsp;
                    <asp:DropDownList ID="DisplayWidthDD" CssClass="contentfield width2" DataTextField="Description" DataValueField="Value" AutoPostBack="true" runat="server" />
                    <asp:Label ID="DisplayWidthLabel" CssClass="contentfield width1" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 150px" class="contenttext right">
                    Colour: &nbsp;
                </td>
                <td align="right" style="padding-right: 10px">
                    <asp:DropDownList ID="DisplayColorDD" CssClass="contentfield width3" DataTextField="Description" DataValueField="Value" AutoPostBack="true" runat="server" />
                    <asp:Label ID="DisplayColorLabel" CssClass="contentfield width3" runat="server" />
                </td>
            </tr>
        </asp:Panel>
        
        <tr>
            <td style="width: 150px" class="contenttext right">
                Price: &nbsp;
            </td>
            <td align="right" style="padding-right: 10px">
                <asp:Label CssClass="contentfield" ID="priceLabel" runat="server" />
            </td>
        </tr>
        <tr id="priceadjustedit" runat="server">
            <td style="width: 150px" class="contenttext right">
                Price Adjustment: &nbsp;
            </td>
            <td align="right" style="padding-right: 10px">
                <asp:TextBox ID="PriceAdjustBox" CssClass="contentfield width1" AutoPostBack="true" runat="server" />
                <br />
                <asp:Label CssClass="error" ID="PriceAdjustError" Visible="false" runat="server" />
            </td>
        </tr>
        <tr id="priceadjustread" runat="server">
            <td style="width: 150px" class="contenttext right">
                Price Adjustment: &nbsp;
            </td>
            <td align="right" style="padding-right: 10px">
                 <asp:Label ID="PriceAdjustLabel" CssClass="contentfield width1" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td style="width: 150px" class="contenttext right">
                Price for this instance: &nbsp;
            </td>
            <td align="right" style="padding-right: 10px">
                <asp:Label ID="subtotalLabel" CssClass="contentfield" runat="server" />
            </td>
        </tr>
    </table>
</div>
