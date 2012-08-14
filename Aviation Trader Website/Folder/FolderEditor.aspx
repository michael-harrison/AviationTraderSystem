<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FolderEditor.aspx.vb" Inherits="FolderEditor" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
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
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td>
                                        <uc22:Navbar ID="NavBar" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td class="contenttext right_text" style="width: 250px">
                                                    Folder Name:
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield" ID="NameBox" runat="server" />
                                                    <br />
                                                    <asp:Label CssClass="contenttext error" ID="NameError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Sort Key:
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox CssClass="contentfield" ID="SortKeyBox" runat="server" /><br />
                                                    <asp:Label CssClass="contenttext error" ID="SortKeyError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Description:
                                                </td>
                                                <td align='left'>
                                                    <asp:TextBox ID="DescriptionBox" TextMode="MultiLine" CssClass="contentfield width4" Height="120" runat="server" />
                                                    <asp:Label CssClass="contenttext error" ID="DescriptionError" Visible="false" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Number of ads in this Folder:
                                                </td>
                                                <td align='left'>
                                                    <asp:Label CssClass="contentfield" ID="AdCountLabel" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Include Folder in Prodn Wizard:
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ProdnFolderCheck" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Folder is Spooled:
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="SpooledCheck" AutoPostBack="true" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <hr />
                                                </td>
                                            </tr>
                                            
                                            <asp:Panel ID="spoolerinfo" runat="server">
                                                <tr>
                                                    <td class="contenttext right_text">
                                                        Spooler is Active:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="SpoolerActiveCheck" runat="server" />
                                                    </td>
                                                </tr>
                                                
                                                
                                                
                                                <tr>
                                                    <td class="contenttext right_text">
                                                        Spooler Command:
                                                    </td>
                                                    <td align='left'>
                                                        <asp:DropDownList ID="SpoolerCommandDD" CssClass="contentfield" DataTextField="description" DataValueField="value" EnableViewState="true" runat="server" />
                                                    </td>
                                                </tr>  
                                                
                                                <tr>
                                                    <td class="contenttext right_text">
                                                        Done Folder:
                                                    </td>
                                                    <td align='left'>
                                                        <asp:DropDownList ID="DoneFolderDD" CssClass="contentfield" DataTextField="Name" DataValueField="hexid" EnableViewState="true" runat="server" />
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td class="contenttext right_text">
                                                        Error Folder:
                                                    </td>
                                                    <td align='left'>
                                                        <asp:DropDownList ID="ErrorFolderDD" CssClass="contentfield" DataTextField="Name" DataValueField="hexid" EnableViewState="true" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <hr />
                                                    </td>
                                                </tr>
                                                
                                            </asp:Panel>
                                           
                                            <tr>
                                                <td class="contenttext right_text">
                                                    Move all ads to another Folder
                                                </td>
                                                <td align='left'>
                                                    <asp:DropDownList ID="FolderDD" CssClass="contentfield" DataTextField="Name" DataValueField="hexid" EnableViewState="true" runat="server" />
                                                    &nbsp;Caution - do you really want to do this?
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
