<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ImageUploader.aspx.vb" Inherits="Advertise_ImageUploader" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <uc9:BarberPole ID="barberpole" Msg="Please wait - uploading image" Left="450px" Top="270px" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/System/Webservices.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container_top">
        <div id="header">
            <uc3:Headerbar ID="headerbar" runat="server" />
        </div>
    </div>
    <div id="container">
        <div id="wrapper">
            <div id="leftpanel" style="background: #eaeaea">
                <img src="../Graphics/AdThermo3-5.png" alt="step 2" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td colspan="2" class="info">
                                        Step 3: Construct Your Ad.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" IsPostBackMode="true" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <uc10:UploadBar ID="UploadBar" Text="Upload Image" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left: 10px">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td style="vertical-align: top;width:520px">
                                                    <div style="width: 490px; border: solid 1px #c0c0c0;">
                                                        <asp:Repeater ID="ImageList" runat="server">
                                                            <HeaderTemplate>
                                                                <table width="100%" border="0" style="border-collapse: collapse">
                                                                    <tr>
                                                                        <td align="center" style="width: 65px; background: #eaeaea">
                                                                            Set<br />
                                                                            Main<br />
                                                                            Image
                                                                        </td>
                                                                        <td style="background: #eaeaea">
                                                                        </td>
                                                                        <td align="center" style="width: 60px; background: #eaeaea">
                                                                            Delete<br />
                                                                            Image
                                                                        </td>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td align="center" style="background: #eaeaea">
                                                                        <cc1:GroupRadioButton ID="ismainimageradio" GroupName="groupradio" Checked="<%#container.dataitem.IsMainImage %>" runat="server" />
                                                                    </td>
                                                                    <td style="border: 1px solid #c0c0c0; padding: 10px">
                                                                        <asp:Image ID="image" ImageUrl="<%#container.dataitem.loresURL %>" Width="340" runat="server" />
                                                                    </td>
                                                                    <td align="center" style="background: #eaeaea">
                                                                        <asp:CheckBox ID="deletecheck" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr>
                                                                    <td colspan="3" style="height: 10px; background: #eaeaea">
                                                                    </td>
                                                                </tr>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </td>
                                                <td class="contenttext" style="vertical-align: top">
                                                    Click the ‘Browse’ button to locate an image on your computer and upload it. You can upload up to five images. After you finish uploading, you need to select one image as the master image. This will be the primary image for your online listing and will also be the image that is used for a photo classie in our print edition and the feature image if you have chosen a photo display ad in our print edition.’<br /><br />
                                                    To delete one or more images after uploading, check the Delete image box, and then click Delete Selected.
                                                    <br />
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <cc1:VW2Btn ID="BtnCategorySelector" CssClass="vwbleft" ScriptManagerName="ScriptManager1" IsPostBackMode="true" Text="Step 2 - Select Category" runat="server" />
                                    </td>
                                    <td align="right">
                                        <cc1:VW2Btn ID="BtnReview" CssClass="vwbright" ScriptManagerName="ScriptManager1" IsPostBackMode="true" Text="Step 4 - Continue" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
