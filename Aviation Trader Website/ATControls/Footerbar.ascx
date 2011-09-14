<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Footerbar.ascx.vb" Inherits="ATControls_Footerbar" %>

<script type="text/javascript">

   function FireDefaultButton(event,btnName) {
        if(event.keyCode ==13) {
            event.cancelBubble = true;
            event.returnValue=false;
            __doPostBack(btnName,btnName);
        }
    }
    
    function incrRotatorClickCount(adHexID) {
        WebServices.IncrRotatorClickCount(adHexID);
    }
    
       function incrAdClickCount(adHexID) {
       WebServices.IncrAdClickCount(adHexID);
    }
    

 function resizePanels() {
//
// called on body.load - makes all panels the same length
//
   var wrapper = document.getElementById("wrapper");
   var leftpanel = document.getElementById("leftpanel");
   var rightpanel = document.getElementById("rightpanel");
   var contentpanel1 = document.getElementById("contentpanel1");
   var contentpanel2 = document.getElementById("contentpanel2");
 
   var leftHeight =0;
   var rightHeight=0;
   var contentHeight=0;

   if (leftpanel) leftHeight= leftpanel.offsetHeight;
   if (rightpanel) rightHeight= rightpanel.offsetHeight;
   if (contentpanel1) contentHeight= contentpanel1.offsetHeight;
   if (contentpanel2) contentHeight= contentpanel2.offsetHeight;
   
    
   var maxHeight = Math.max(leftHeight,rightHeight,contentHeight);
   
   wrapper.style.height = maxHeight + "px";
   if (leftpanel) leftpanel.style.height = maxHeight + "px";
   if (rightpanel) rightpanel.style.height = maxHeight + "px";
   if (contentpanel1) contentpanel1.style.height = maxHeight + "px";
   if (contentpanel2) contentpanel2.style.height = maxHeight + "px";
   
  
 }
 
function popup(url,windowname) {
//
// pdf bug - if file contains .pdf, open, close, open.
//
    var x=url.toLowerCase().indexOf('.pdf');
    if(x != -1) {
 //       var xx = window.open('',windowname);
 //       xx.close();
    }
    
    window.open(url,windowname,'width=570,height=500,toolbar=no,status=no,scrollbars=yes,resizable=yes').focus();
    return false;           //avoid postback when called from button
}
</script>

<table border="0" style="width: 100%;" cellpadding="5">
    <!-- Note the 'a' parameter in the following calls means overwrite the same popup window. If you want to display
multiple popups simultaneously, choose different params -->
    <tr>
        <td align="left" width="450px" valign="top">
            Copyright 2010 Aviation Trader | <span id ="termsofuse" style="cursor: pointer;"  runat="server">Terms of Use</span> | <span id="privacypolicy" style="cursor: pointer;" runat="server">Privacy Policy</span>
        </td>
        
    </tr>
</table>

<!-- START Nielsen Online SiteCensus V6.0 -->
<!-- COPYRIGHT 2009 Nielsen Online -->

<script src="//secure-au.imrworldwide.com/v60.js"></script>

<script type="text/javascript">
 var pvar = { cid: "auditbc-au", content: "0", server: "secure-au" };
 var trac = nol_t(pvar);
 trac.record().post();
</script>

<noscript>
    <div>
        <img src="//secure-au.imrworldwide.com/cgi-bin/m?ci=auditbc-au&cg=0&cc=1&ts=noscript" width="1" height="1" alt="" /></div>
</noscript>
<!-- END Nielsen Online SiteCensus V6.0 -->
