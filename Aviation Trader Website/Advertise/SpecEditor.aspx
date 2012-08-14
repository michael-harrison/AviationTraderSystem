<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SpecEditor.aspx.vb" Inherits="Advertise_SpecEditor" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <uc9:BarberPole ID="barberpole" Msg="Please wait - building previews" Left="450px" Top="270px" runat="server" />
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1"   runat="server">
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
                <img src="../Graphics/AdThermo3-4.png" alt="step 2" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td colspan="2">
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" IsPostBackMode="true" runat="server" />
                                    </td>
                                </tr>
       
                                <tr>
                                    <td colspan="2" style="padding-left: 10px">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td style="width: 520px">
                                                    <asp:Repeater ID="grouplist" runat="server">
                                                        <HeaderTemplate>
                                                            <table style="width: 510px; border: solid 1px #c0c0c0;" border="0px">
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="Specgroup">
                                                                    <%#container.DataItem.name %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Repeater ID="speclist" runat="server">
                                                                        <ItemTemplate>
                                                                            <uc7:SpecBuilder ID="spec" Spec="<%#container.dataitem %>" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </td>
                                                <td class="contenttext" style="vertical-align: top">
                                                    Specifications are optional, but if you include them, it will make it easier for buyers to search and find your ad. Enter specifications for your ad, by selecting each option on the list.
                                                    <br />
                                                    <br />
                                                    If you want a spec to appear in your ad, select the Inlcude checkbox. If this box is not checked, the spec will not be included.
                                                    <br />
                                                    <br />
                                                    Click the <span style="color: red; font-weight: bold">SAVE CHANGES</span> button to save your specifications. </td>
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
                                        <cc1:VW2Btn ID="BtnCategorySelector" CssClass="vwbleft" ScriptManagerName="ScriptManager1" IsPostBackMode="false" Text="Step 2 - Select Category" runat="server" />
                                    </td>
                                    <td align="right">
                                        <cc1:VW2Btn ID="btnSubmit" CssClass="vwb" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Submit ad" OnClientClick=" showPole(500);return true;" runat="server" />
                                    </td>
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
