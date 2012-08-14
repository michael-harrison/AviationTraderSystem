<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Editor4.aspx.vb" Inherits="Editor4" %>

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
            <div id="leftpanel">
                <uc2:LeftMenu ID="leftmenu" runat="server" />
            </div>
            <div id="contentpanel1">
                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="border: solid 1px #c0c0c0; padding-bottom: 10px;">
                            <table border="0" width="100%" style="border-collapse: collapse">
                                <tr>
                                    <td width="250px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="info">
                                        <uc1:TopMenu ID="tabbar" CSSClass="tabbar" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <uc8:ButtonBar ID="ButtonBar" ScriptManagerName="ScriptManager1" runat="server" />
                                    </td>
                                </tr>
                                
                                <tr><td colspan="2">Engine Parameters</td></tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Engine Port :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="EnginePortBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="EnginePortError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Job Timeout (ms) :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="JobTimeoutBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="JobTimeoutError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Engine Name :
                                    </td>
                                    <td align='left'>
                                        <asp:Label CssClass="contenttext" ID="engineNameLabel" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Engine IP Address :
                                    </td>
                                    <td align='left'>
                                        <asp:Label CssClass="contenttext" ID="EngineAddressLabel" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Production File Locations
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Source Image Folder (Original) :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="SourceImageOriginalFolderBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="SourceImageOriginalFolderError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="contenttext right_text">
                                        Source Image Folder (Working) :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="SourceImageWorkingFolderBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="SourceImageWorkingFolderError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Display Ad Folder :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="DisplayAdFolderBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="DisplayAdFolderError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="contenttext right_text">
                                        ClassAd Folder :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="ClassadfolderBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="ClassadFolderError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Prodn PDF Folder :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="ProdnPDFFolderBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="ProdnPDFFolderError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                       Classified Parameters
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Classfied Template Name :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="ClassadTemplateBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="ClassadTemplateError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                                               
                                <tr>
                                    <td class="contenttext right_text">
                                        Classified Line Height in mm :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="ClassadLineHeightBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="ClassadLineHeighterror" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Classified Pic Height in mm :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="ClassadPicHeightBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="ClassadPicHeightError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Display Parameters
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="contenttext right_text">
                                        Number of Display Columns :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="DisplayColumnCountBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="DisplayColumnCountError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="contenttext right_text">
                                        Display Column Height in mm :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="DisplayColumnHeightBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="DisplayColumnHeightError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Display Column Width in mm :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="DisplayColumnWidthBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="DisplayColumnWidthError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Display Gutter Width in mm :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="DisplayGutterWidthBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="DisplayGutterWidthError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td colspan="2">
                                        Rate Tables
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Latest Listing Loading :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width1" ID="LatestListingLoadingBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="LatestListingLoadingError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Latest Listing Kill Date :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width3" ID="LatestListingKillTimeBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="LatestListingKillTimeError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Rate table spreadsheet :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="RatesBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="RatesError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Display sheet :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="DisplayBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="DisplayError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <cc1:VW2Btn ID="BtnImportDisplay" CssClass="vwb" ScriptManagerName="ScriptManager1" IsPostBackMode="true" Text="Import New Display Rates" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contenttext right_text">
                                        Classified sheet :
                                    </td>
                                    <td align='left'>
                                        <asp:TextBox CssClass="contentfield width6" ID="ClassifiedBox" runat="server" />
                                        <asp:Label CssClass="contenttext error" ID="ClassifiedError" Visible="false" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <cc1:VW2Btn ID="btnImportClassified" CssClass="vwb" ScriptManagerName="ScriptManager1" IsPostBackMode="true" Text="Import New Classified Rates" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
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
