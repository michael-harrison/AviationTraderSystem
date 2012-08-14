<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TextEditor.aspx.vb" Inherits="Production_TextEditor" %>

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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
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
                                <td style="text-align: center">
                                    <asp:Label ID="FolderLabel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; padding: 3px">
                                    <asp:Image ID="Pic" Style="width: 152px;" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="AliasLabel" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="contentpanel1">
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
                                                <td style="width: 490px" class="contenttext">
                                                    Key Words:<br />
                                                    <asp:TextBox ID="KeyWords" CssClass="contentfield width6" Font-Names="Arial" Font-Size="14px" runat="server" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext">
                                                    Item Price:<br />
                                                    <asp:TextBox ID="ItemPrice" CssClass="contentfield width4" Font-Names="Arial" Font-Size="14px" runat="server" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext">
                                                    Summary:<br />
                                                    <asp:TextBox ID="SummaryBox" TextMode="MultiLine" class="textinput" Height="200" Width="450" runat="server" />
                                                </td>
                                                <td class="contenttext">
                                                    This summary of the ad will appear in ad lists.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext">
                                                    Full Ad Text:<br />
                                                    <asp:TextBox ID="AdText" TextMode="MultiLine" class="textinput" Height="280" Width="450" runat="server" />
                                                </td>
                                                <td class="contenttext">
                                                    Update the text of the ad in the panel on the left. Put either one or two \ characters to separate the ad text into keywords, summary and Text parts. The keywords and summary field will only be updated if they are blank.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext">
                                                    Original Ad text:<br />
                                                    <asp:TextBox ID="OriginalTextBox" ReadOnly="true" TextMode="MultiLine" class="textinput" Height="280" Width="450" runat="server" />
                                                </td>
                                                <td class="contenttext">
                                                    This is the original text of the ad as entered by the advertiser when the ad was booked. This remains on file as a permanent record in ‘read only’ form should future reference be required. On initial submission this text is duplicated and placed in the ‘Full Ad Text’ box in ‘open access’ form to facilitate selection and editing
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext">
                                                    Youtube Video Tag:<br />
                                                    <asp:TextBox ID="YoutubeVideoTagBox" CssClass="contentfield width4" Font-Names="Arial" Font-Size="14px" runat="server" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        </div>
    </form>
</body>
</html>
