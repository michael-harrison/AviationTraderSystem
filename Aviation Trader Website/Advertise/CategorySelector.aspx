<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CategorySelector.aspx.vb" Inherits="CategorySelector" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
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
                <img src="../Graphics/AdThermo2-5.png" alt="step 1" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table border="0" width="100%">
                            <tr>
                                <td colspan="2" class="info">
                                    Step 2: Select the category and sub category that you want your ad to run in.
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" border="0" style="border: solid 1px #c0c0c0; border-collapse: collapse">
                                        <tr>
                                            <td class="contenttext right_text" width="200px">
                                                Category:
                                            </td>
                                            <td align="right" width="225px">
                                                <asp:DropDownList CssClass="contentfield" ID="CatDD" DataTextField="Name" DataValueField="HexID" AutoPostBack="true" runat="server" />
                                            </td>
                                            <td rowspan="2" class="contenttext" style="padding: 10px; padding-left: 50px;">
                                                Select the main advertising category, and the sub category for your ad.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="contenttext right_text">
                                                Sub Category:
                                            </td>
                                            <td align="right">
                                                <asp:DropDownList CssClass="contentfield" ID="ClsDD" DataTextField="Name" DataValueField="HexID" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <cc1:VW2Btn ID="BtnProductSelector" CssClass="vwbleft" ScriptManagerName="ScriptManager1" IsPostBackMode="true" Text="Step 1 - Select Products" runat="server" />
                                </td>
                                <td align="right">
                                    <cc1:VW2Btn ID="BtnContentEditor" CssClass="vwbright" ScriptManagerName="ScriptManager1" IsPostBackMode="true" Text="Step 3 - Construct Your Ad" runat="server" />
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
