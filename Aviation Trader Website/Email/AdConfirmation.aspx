<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AdConfirmation.aspx.vb"
    Inherits="AdConfirmation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>Ad Confirmation</title>
</head>
<body style="font-size: 13px; font-family: Trebuchet MS; color: #707070;">
    <table border="0" width="600px">
        <tr>
            <td width="250px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="storybody">
                Hello
                <%=Ad.Usr.FullName%><br />
                <br />
                Thank you for submitting your ad which has now entered our production system. Your
                ad reference is <span style="color: #c70727;">
                    <%=ad.Adnumber %>.<br />
                </span>
                <br />
                Our production team will proof your material, optimise the text and any images and
                confirm the cost. You can view the status of your ad and make or request changes
                by clicking the ‘Manage My Ads’ button on your account management page.
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="font-weight: bold; height: 60px; margin-top: 5px;">
                Classification:
                <%=Ad.CategoryName %>
                -
                <%=Ad.ClassificationName%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <!-- publication repeater -->
                <asp:Repeater ID="PublicationList" runat="server">
                    <HeaderTemplate>
                        <table style="margin-bottom: 10px; border: solid 1px #c0c0c0; border-collapse: collapse"
                            border="0">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td colspan="2" style="font-weight: bold; height: 30px; margin-top: 5px;">
                                <%#container.dataitem.name %>
                            </td>
                        </tr>
                        <!-- edition repeater -->
                        <asp:Repeater ID="EditionList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td colspan="1" style="height: 20px; margin-top: 20px; padding-left: 40px;">
                                        <%#container.dataitem.name %>
                                    </td>
                                    <td colspan="1" style="text-align: right; height: 20px; margin-top: 20px; padding-right: 5px;
                                        padding-left: 40px;">
                                        Onsale date :
                                        <%#Container.DataItem.onsaledate.toshortdatestring%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 5px;">
                                    </td>
                                </tr>
                                <!-- product repeater -->
                                <asp:Repeater ID="ProductList" runat="server">
                                    <HeaderTemplate>
                                        <tr>
                                            <td colspan="2">
                                                <div style="border: solid 1px #c0c0c0; margin-left: 100px; margin-bottom: 5px; margin-right: 5px;
                                                    padding: 10px;background:#eaeaea">
                                                    <table border="0" width="100%" style="border-collapse: collapse">
                                                        <tr>
                                                            <td width="450px">
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td colspan="2" class="contenttext">
                                                <span>
                                                    <%#Container.DataItem.name%></span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table></div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </asp:Repeater>
                        </div> </td> </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table></FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr>
            <td align="left">
                <br />
                Regards,<br />
                <br />
                The team at <b>Aviation Trader</b>
            </td>
        </tr>
    </table>
</body>
</html>
