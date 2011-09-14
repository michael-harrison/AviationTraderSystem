<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AllStories.aspx.vb" Inherits="AllStories" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1"   runat="server">
        <Services>
           <asp:ServiceReference Path="~/System/Webservices.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="container">
        <div id="header">
            <uc3:Headerbar ID="headerbar" runat="server" />
        </div>
        <div id="wrapper">
            <div id="leftpanel">
                <uc17:AdRotator ID="AdRotator1" category="Left" Height="1080" runat="server" />
            </div>
            <div id="contentpanel1">
                <div style="border: solid 1px #c0c0c0; padding-top: 5px; padding-bottom: 5px;">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="info">
                                <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Repeater EnableViewState="false" ID="NewsList" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="5" border="0">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label ID="StoryHead" CssClass="storyhead" Text="<%#Container.DataItem.Name%>" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="picpanel" CssClass="storypicpanel" Visible="<%#Container.DataItem.hasImage%>" runat="server">
                                                    <asp:Image ID="StoryPic" CssClass="storypic" ImageUrl="<%#Container.DataItem.imageURL%>" runat="server" /><br />
                                                    <asp:Label ID="StoryPicCaption" CssClass="storypiccaption" Text="<%#Container.DataItem.piccaption%>" runat="server" />
                                                </asp:Panel>
                                                <asp:Label ID="StoryIntro" CssClass="storyintro" Text="<%#Container.DataItem.HTMLIntro%>" runat="server" /><br />
                                                <br />
                                                <asp:Label ID="StoryBody" CssClass="storybody" Text="<%#Container.DataItem.HTMLBody%>" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>
                                    </SeparatorTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="footer">
            <uc4:Footerbar ID="footerbar" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
