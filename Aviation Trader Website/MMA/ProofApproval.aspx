<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProofApproval.aspx.vb" Inherits="ProofApproval" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body>
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
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
                <table style="margin-top: 10px; background: white; border: solid 1px #c0c0c0; border-collapse: collapse; width: 160px; margin-left: 10px">
                    <tr>
                        <td style="text-align: center">
                            <asp:Label ID="CatLabel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <asp:Label ID="ClsLabel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <asp:Label ID="statusLabel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; padding: 3px">
                            <asp:Image ID="Pic" Style="width: 152px;" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td>
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="left" style="width: 520px">
                                                    <asp:TextBox ID="ProdnRequest" TextMode="MultiLine" class="textinput" Height="180" Width="450" runat="server" />
                                                </td>
                                                <td class="contenttext">
                                                    Please enter any special production instuctions for Aviation Trader production editors here.
                                                </td>
                                            </tr>
                                            <td>
                                                <td style="height: 10px">
                                                </td>
                                            </td>
                                            <tr>
                                                <td align="left" style="width: 500px">
                                                    <asp:TextBox ID="ProdnResponse" TextMode="MultiLine" class="textinput"  ReadOnly="true" Height="180" Width="450" runat="server" />
                                                </td>
                                                <td class="contenttext">
                                                    This section is reserved for Aviation Trader staff to provide feedback to you on the production of your ad.
                                                </td>
                                            </tr>
                                        </table>
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
