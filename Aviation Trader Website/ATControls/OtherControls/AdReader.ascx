<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AdReader.ascx.vb" Inherits="ATControls_AdReader" %>

    <table width="100%" border="0">
        <tr>
            <td style="vertical-align: top; width: 160px">
                <asp:Image ID="BasicPic" Style="width: 150px;" runat="server" />
            </td>
            <td style="vertical-align: top">
                <div style="background: white; height: 160px; overflow: hidden">
                    <asp:Label ID="BasicKeyWords" Style="font-weight: bold; color: black" runat="server" />
                    <div style="float: right">
                        <asp:Label ID="BasicItemPrice" Style="font-weight: bold; color: black" runat="server" />
                    </div>
                    <br />
                    <br />
                    <asp:Label ID="BasicText" Style="font-weight: normal; font-size: 12px; color: #404040" runat="server" />
                </div>
            </td>
        </tr>
    </table>
