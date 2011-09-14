<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ForgotPW.aspx.vb" Inherits="ForgotPW" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>

    <script type="text/javascript">
    function FireDefaultButton(event,btnName) {
        if(event.keyCode ==13) {
            event.cancelBubble = true;
            event.returnValue=false;
            __doPostBack(btnName,btnName);
        }
    }
    
    
    </script>

</head>
<body style="background: white none">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1"   runat="server">
        <Services>
           <asp:ServiceReference Path="~/System/Webservices.asmx" />
        </Services>
    </asp:ScriptManager>
    <table width="100%" border="0" style="border-collapse: collapse">
        <tr>
            <td style="padding: 0px; width: 280px; height: 89px; vertical-align: top">
                <img id="Img1" src="~/Graphics/AVT_web_tagline.png" alt="Aviation Trader" height="100" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                <table width="430px" cellpadding="10px" style="margin-top:50px;margin-left:80px;border: solid 1px #c0c0c0; border-collapse: collapse;" border="0">
                   
                   <tr>
                       <td colspan="2" class="contenttext">
                           Please enter your email address that you used to register with Aviation Trader and click the button. An email will be sent to your address with your password.
                       </td>
                   </tr>
                   <tr>
                        <td class="contenttext right" style="width: 160px">
                            <span style="color: red">*&nbsp;</span>Email address :
                        </td>
                        <td align='left'>
                            <asp:TextBox CssClass="contentfield" ID="EmailBox" runat="server" /><br />
                            <asp:Label CssClass="contenttext error" ID="emailerror" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <cc1:VW2Btn ID="BtnSendEmail" CssClass="vwb" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Email Password" runat="server" />
                            <cc1:VW2Btn ID="BtnClose" CssClass="vwb" IsPostBackMode="false"  NavigateURL="" OnClientClick="window.close();return false;" Text="Close" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="contenttext left">
                            <span style="color: red">*&nbsp;</span>Denotes required field
                        </td>
                    </tr>
                </table>
                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            
        </tr>
    </table>
    </form>
</body>
</html>
