 <%@ Page Language="VB" AutoEventWireup="false" CodeFile="Review.aspx.vb" Inherits="Review" %>

<!DocType html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aviation Trader - Your total aviation marketplace</title>
</head>
<body onload="resizePanels()">
    <uc9:BarberPole ID="barberpole" Msg="Please wait while your ad is processed..." Left="450px" Top="270px" runat="server" />
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
                <img src="../Graphics/AdThermo4-5.png" alt="step 4" />
            </div>
            <div id="contentpanel1">
                <table border="0" width="100%" style="border-collapse: collapse">
                    <tr>
                        <td colspan="2" class="info">
                            Step 4: File Your Ad.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="contenttext">
                            Are you ready to submit your ad? If so, click the ‘Submit Ad’ button below. Once your ad has been submitted it will move into our production system. We will prepare the ad for print, provide a firm quotation and post a final version to your ad management space. If you have selected a display ad or requested a proof for a classified ad at step 3 we will post it to your ads awaiting approval tab. 
                             If not you will find it by clicking your approved ads tab.<br />
                            <br /> If you are not ready to submit your ad you can return to any previous submission step, cancel your submission or save it for completion later. If you choose to save it you will find it under the saved ads tab in your ad management space. We will do our best to upload your material within 24 hours of receipt of your submission.<br /><br />
                              Use the buttons below to activate your choice.<br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <table width="700px" style="border: solid 1px #c0c0c0;">
                                <tr>
                                    <td style="padding: 10px; border: solid 1px #c0c0c0">
                                        Click this button to return to step 3 where you can make changes to your ad content.
                                    </td>
                                    <td style="padding: 10px; border: solid 1px #c0c0c0" align="center">
                                        <cc1:VW2Btn ID="BtnContentEditor" CssClass="vwbleft" ScriptManagerName="ScriptManager1" Text="Step 3 - Construct Your  Ad" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px; border: solid 1px #c0c0c0">
                                        Click this button to cancel your ad. Your ad will not be submitted to Production and will not be saved.
                                    </td>
                                    <td style="padding: 10px; border: solid 1px #c0c0c0" align="center">
                                        <cc1:VW2Btn ID="BtnCancel" CssClass="vwb" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Cancel ad" OnClientClick=" showPole(500);return true;" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px; border: solid 1px #c0c0c0">
                                        Click this button if you have not yet finished the ad. Your ad will not be submitted to Production but instead will be saved in your own area. Use the Manage My Ads feature to find and recall any of your saved ads.                                   </td>
                                    <td style="padding: 10px; border: solid 1px #c0c0c0" align="center">
                                        <cc1:VW2Btn ID="btnSave" CssClass="vwb" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Save ad" OnClientClick=" showPole(500);return true;" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px; border: solid 1px #c0c0c0">
                                        Click this button to submit your ad to Aviation Trader production services. After submission you will not normally be able to change your ad, except by special request.
                                    </td>
                                    <td style="padding: 10px; border: solid 1px #c0c0c0" align="center">
                                        <cc1:VW2Btn ID="btnSubmit" CssClass="vwb" IsPostBackMode="true" ScriptManagerName="ScriptManager1" Text="Submit ad" OnClientClick=" showPole(500);return true;" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
