<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PayAndSubmit.aspx.vb" Inherits="PayAndSubmit" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Avation Trader - Your total aviation marketplace</title>

    <script type="text/javascript">
    var selectedPaymentType='0';
       
    function paymentChange(dropdown) {
       var ErrorMsg = document.getElementById('ErrorMsg');
        ErrorMsg.innerHTML='\u00A0'; //&nbsp;
        setPaymentChange(dropdown);          
    }
    
    function setPaymentChange(dropdown) {
       resetError('FNameLabel');
       resetError('LNameLabel');
       resetError('Addr1Label');
       resetError('SuburbLabel');
       resetError('StateLabel');
       resetError('PostcodeLabel');
       resetError('StateLabel');
       resetError('CountryLabel');
       resetError('CCNumberLabel');
       resetError('CCVLabel');
       
       var CCPanel = document.getElementById('CCPanel');
       var ErrorMsg = document.getElementById('ErrorMsg');
       var submitButton = document.getElementById('btnSubmit');
       CCPanel.style.visibility="hidden";
       var selectedIndex = dropdown.selectedIndex;
       selectedPaymentType = dropdown.options[selectedIndex].value;
       var span = submitButton.getElementsByTagName("span")[0]
        
       span.innerText='SUBMIT AD';
       
       setPoleText('Please wait while your ad is sent');
        if (!(selectedPaymentType == '0' || selectedPaymentType == '5')) {
            CCPanel.style.visibility="visible";
            span.innerText='PAY AND SUBMIT ORDER';
            setPoleText('Please wait while payment is processed');
        }
        
    }
    
    function validateInput()  {
       var ErrorMsg = document.getElementById('ErrorMsg');
   
        switch(selectedPaymentType) {
          case '0': {            
             ErrorMsg.innerHTML='Please select a payment method.';
             return false;
             break;
             }
             
          case '5': {
             showPole(500);
             return true;
             break;
             }
            
        default: {
            var anyerrors = false;
            anyerrors = anyerrors | !val('FNameLabel','FNameBox');
            anyerrors = anyerrors | !val('LNameLabel','LNameBox');
            anyerrors = anyerrors | !val('Addr1Label','Addr1Box');
            anyerrors = anyerrors | !val('SuburbLabel','SuburbBox');
            anyerrors = anyerrors | !val('StateLabel','StateBox');
            anyerrors = anyerrors | !val('PostcodeLabel','PostcodeBox');
            anyerrors = anyerrors | !val('CountryLabel','CountryBox');
            anyerrors = anyerrors | !val('CCNumberLabel','CCNumberBox');
            anyerrors = anyerrors | !val('CCVLabel','CCVBox');
            if (anyerrors) {
                    ErrorMsg.innerHTML='There was a problem with your submission.  Please see highlighted errors below.';
                    return false;
                }
                else {
         ErrorMsg.innerHTML='\u00A0'; //&nbsp;
                showPole(500);
                  return true;
                }  
            }
       }
       
   }
   
   function reset(fieldlabelID) {
         var label = document.getElementById(fieldlabelID);
         label.className = "contenttext right";
  }
   
    
function val(fieldlabelID,fieldvalueID) {
        var label = document.getElementById(fieldlabelID);
        var field = document.getElementById(fieldvalueID);
        if (field.value==null || field.value=='') {
            label.className = "contenttext error right";
            return false;
           }
           else {
             label.className = "contenttext right";
             return true;
           }        
}


function resetError(fieldlabelID) {
        var label = document.getElementById(fieldlabelID);
        label.className = "contenttext right";
 }



function hideMe(obj) {
// hides the button so it cant be clicked again  
   obj.parentElement.parentElement.style.visibility="hidden"
}       
    
    </script>

</head>
<body onload="setPaymentChange(document.form1.paymentMethodDD);resizePanels()">
    <uc9:BarberPole ID="barberpole" Msg="Please wait while your image is uploaded..." Left="450px" Top="270px" runat="server" />
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
                <img src="../Graphics/AdThermo5.png" alt="step 5" />
            </div>
            <div id="contentpanel1">
                <table border="0" width="100%">
                    <tr>
                        <td width="250px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="info">
                            Step 5: Select payment options and submit ad.
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td style="text-align: right; font-weight: bold">
                            Total Price:
                        </td>
                        <td style="width: 80px; text-align: right; font-weight: bold">
                            $<%=ATLib.CommonRoutines.Integer2Dollars(TotalPrice)%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="ErrorMsg" name="ErrorMsg" runat="server" CssClass="msg" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0" style="border:solid 1px #c0c0c0"width="100%">
                                <tr>
                                    <td class="contenttext right" style="width: 200px;">
                                        PAYMENT METHOD : *
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList CssClass="contentfield width2" Width="160px" ID="paymentMethodDD" DataTextField="Description" DataValueField="Value" runat="server" />
                                        <img style="vertical-align: bottom" alt="payment method" src="../Graphics/CCOptions.gif" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table id="CCPanel" border="0" width="100%" style="visibility: hidden">
                                            <tr>
                                                <td id="CCNumberLabel" class="contenttext right" width="200px">
                                                    Credit Card Number : *
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="contentfield" ID="CCNumberBox" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="CCVLabel" class="contenttext right">
                                                    Security Code : *
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="contentfield width1" ID="CCVBox" runat="server"></asp:TextBox>
                                                    <span style="cursor: pointer; text-decoration: underline" onclick="popup('../Popups/CCV.htm','a')">What is a security code?</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="contenttext right">
                                                    Expiration Date : *
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList CssClass="contentfield width1" ID="CCExpiryMonthDD" DataTextField="Name" DataValueField="Value" runat="server" />
                                                    <asp:DropDownList CssClass="contentfield width1" ID="CCExpiryYearDD" DataTextField="Name" DataValueField="Value" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp
                                                </td>
                                            </tr>
                                            <tr>
                                                <td  class="contenttext right">
                                                    BILLING ADDRESS:
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td id="FNameLabel" class="contenttext right">
                                                    First Name : *
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="contentfield width4" ID="FNameBox" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="LNameLabel" class="contenttext right">
                                                    Last Name : *
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="contentfield width4" ID="LNameBox" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="Addr1Label" class="contenttext right">
                                                    Address 1 : *
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="contentfield width4" ID="Addr1Box" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="Addr2Label" class="contenttext right">
                                                    Address 2 : &nbsp
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="contentfield width4" ID="Addr2Box" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="SuburbLabel" class="contenttext right">
                                                    Suburb : *
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="contentfield width4" ID="SuburbBox" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="StateLabel" class="contenttext right">
                                                    State : *
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="contentfield width3" ID="StateBox" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="PostcodeLabel" class="contenttext right">
                                                    Postcode : *
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="contentfield width2" ID="PostcodeBox" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td id="CountryLabel" class="contenttext right">
                                                    Country : *
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="contentfield width3" ID="CountryBox" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="background: white; text-align: left">
                            * Denotes required fields
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="left">
                            <cc1:VW2Btn ID="btnReview" CSSClass="vwbleft"  IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Step 4 - review ad" runat="server" />
                        </td>
                        <td align="right">
                            <cc1:VW2Btn ID="btnSubmit" CssClass="vwb" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Confirm payment and submit ad" OnClientClick="if(validateInput()){hideMe(this);return true}else{return false};" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="footer">
            <uc4:Footerbar ID="footerbar" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
