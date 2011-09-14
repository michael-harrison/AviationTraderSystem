<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AjaxTest.aspx.vb" Inherits="BrowseAndSearch_AjaxTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    time is
    <%=Now.ToString%>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" enablepartialrendering="true"   runat="server">
        <Services>
           <asp:ServiceReference Path="~/System/Webservices.asmx" />
        </Services>
    </asp:ScriptManager> 
             <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional"  runat="server">
            <ContentTemplate>
                <div style="position: absolute; top: 20px; left: 20px; width: 500px; height: 200px; background: #f0f0f0">
                    <table width="100%" cellpadding="5px" style="border-top: solid 1px #c0c0c0">
                        <tr>
                            <td style="width: 80px">
                                Search in:
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="DropDownList1" EnableViewState="true" CssClass="contentfield" DataValueField="value" DataTextField="Name" AutoPostBack="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="Btn1" Text="hit me" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="time1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                time is
                                <%=Now.ToString%>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn1" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <div style="position: absolute; top: 20px; left: 550px; width: 500px; height: 200px; background: #d0d0d0">
                    <table width="100%" cellpadding="5px" style="border-top: solid 1px #c0c0c0">
                        <tr>
                            <td style="width: 80px">
                                Search in:
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="DropdownList2" CssClass="contentfield" DataValueField="value" DataTextField="Name" AutoPostBack="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn2" Text="hit me" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="time2" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                time is
                                <%=Now.ToString%>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div style="position: absolute; top: 300px; left: 20px; width: 500px; height: 100px; background: #d0d0d0">
                    <asp:Label ID="time3" runat="server" />
                    <tr>
                        <td>
                            time is
                            <%=Now.ToString%>
                        </td>
                    </tr>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
