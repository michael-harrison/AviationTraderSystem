<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BarberPole.ascx.vb" Inherits="ATControls_BarberPole" %>

<script language="javascript" type="text/javascript">
var poletimer = null;                      
function showPole(delay) {
   if(delay==0) 
        showPole2();
   else
    poletimer = setTimeout("showPole2()",500);
}        
                     
function showPole2() {
  var pole = document.getElementById('<%=poleClientID %>');
  pole.style.visibility="visible";
}

function setPoleText(text) {
 var msg = document.getElementById('<%=msgClientID %>');
  msg.innerHTML = text;
}

</script>

<div id="pole" style="z-index: 500; visibility: hidden; position: fixed; left: 450px; top: 315px; width: 240px; height: 55px; padding: 5px; border:  2px outset #eaeaea; background-color: #e0e0e0; color: #1046b6; font-family: Trebuchet MS; font-size: 12px; text-align: center" runat="server">
    <asp:Label ID="msgbox" runat="server" />
    <img alt="Processing" onload="if(poletimer != null)clearTimeout(poletimer);" style="position: absolute; top: 30px; left: 20px;" src= "../Graphics/loading.gif">
</div>
