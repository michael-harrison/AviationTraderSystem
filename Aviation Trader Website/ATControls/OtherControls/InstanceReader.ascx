<%@ Control Language="VB" AutoEventWireup="false" CodeFile="InstanceReader.ascx.vb" Inherits="InstanceReader" %>
<asp:Panel id="Basic" cssclass= "basicad" runat="server">
    <table width="100%" border="0">
        <tr>
            <td rowspan="2" style="vertical-align: top; width: 160px">
                    <asp:Image ID="BasicPic" Style="cursor:pointer; width: 150px;" runat="server" />
             </td>
            <td style="vertical-align: top; width: 302px;">
                <asp:Label ID="BasicKeyWords" Style="font-weight: bold; color: black" runat="server" />&nbsp;&nbsp;
                <asp:Label ID="BasicItemPrice" Style="font-weight: bold; color: black" runat="server" />
            </td>
            <td>
                    <asp:Image ID="BasicLink" Style="cursor: pointer" ImageUrl="~/Graphics/details.gif" runat="server" />
               </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style=" height: 70px; overflow: hidden">
                    <asp:Label ID="BasicText" Style="font-weight: normal; font-size: 12px; color: #404040" runat="server" />
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Premium" EnableViewState="false" cssclass="premiumad"  runat="server">
    <table width="100%" border="0">
        <tr>
            <td rowspan="2" style="vertical-align: top; width: 160px">
                    <asp:Image ID="PremiumPic" Style="cursor:pointer; width: 150px;" runat="server" />
             </td>
            <td style="vertical-align: top; width: 290px">
                <asp:Label ID="PremiumKeyWords" Style="font-weight: bold; color: #c70727" runat="server" />&nbsp;&nbsp;
                <asp:Label ID="PremiumItemPrice" Style="font-weight: bold; color: #c70727" runat="server" />
            </td>
            <td>
                     <asp:Image ID="PremiumLink" Style="cursor: pointer" ImageUrl="~/Graphics/details.gif" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="height: 70px; overflow: hidden">
                    <asp:Label ID="PremiumText" Style="font-weight: normal; font-size: 12px; color: #404040" runat="server" />
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="PDF" EnableViewState="false" cssclass="pdfad" runat="server">
    <table width="100%" border="0">
        <tr>
            <td style="vertical-align: top; width: 160px">
                    <asp:Image ID="PDFPic" Style="cursor: pointer; float: left; width: 150px;" runat="server" />
            </td>
            <td style="vertical-align: top; width: 290px">
                <asp:Label ID="PDFKeyWords" Style="font-weight: bold; color: #c70727" runat="server" />&nbsp;&nbsp;
                <asp:Label ID="PDFItemPrice" Style="font-weight: bold; color: #c70727" runat="server" /><br />
            </td>
            <td style="vertical-align: top;">
                    <asp:Image ID="PDFLink" Style="cursor: pointer" ImageUrl="~/Graphics/pdf.gif" runat="server" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="PDFText" EnableViewState="false" cssclass="pdftext"  runat="server">
    <table width="100%" border="0">
        <tr>
            <td rowspan="2" style="vertical-align: top; width: 160px">
                    <asp:Image ID="PDFTextPic" Style="cursor:pointer; float: left; width: 150px;" runat="server" />
  
            </td>
            <td style="vertical-align: top; width: 290px;">
                <asp:Label ID="PDFTextKeyWords" Style="font-weight: bold; color: #c70727" runat="server" />&nbsp;&nbsp;
                <asp:Label ID="PDFTextItemPrice" Style="font-weight: bold; color: #c70727" runat="server" />
            </td>
            <td>
                     <asp:Image ID="PDFTextLink" Style="cursor: pointer" ImageUrl="~/Graphics/pdf.gif" runat="server" />
   
            </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="height: 70px; overflow: hidden">
                        <asp:Label ID="PDFTextText" Style="font-weight: normal; font-size: 12px; color: #404040" runat="server" />
                    </div>
                </td>
            </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Featured" EnableViewState="false" CssClass="featuredad"  runat="server">
    <div class="imgbox">
             <asp:Image ID="FeaturedPic" style="cursor:pointer" runat="server" />
    </div>
    <asp:Label ID="FeaturedKeywords" Style="line-height: 12px; text-align: center; font-weight: bold; font-size: 12px; color: black" runat="server" /><br />
    <asp:Label ID="FeaturedText" Style="line-height: 14px; font-weight: normal; font-size: 11px; color: #404040" runat="server" />
</asp:Panel>
<asp:Panel ID="Classad" EnableViewState="false" runat="server">
    <asp:Image ID="ClassadPreview" Width="200px" style="cursor:pointer;border:solid 1px #c0c0c0; padding:10px;" runat="server" />
</asp:Panel>
<asp:Panel ID="DisplayAd" EnableViewState="false" runat="server">
         <asp:Image ID="DisplayPreview" style="cursor:pointer" runat="server" />
</asp:Panel>
