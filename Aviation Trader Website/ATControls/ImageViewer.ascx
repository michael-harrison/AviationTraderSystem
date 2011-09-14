<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ImageViewer.ascx.vb" Inherits="ATControls_ImageViewer" %>

<link href="JavascriptLibs/DynamicDrive/StepCarousel2.css" type="text/css" rel="Stylesheet" />


<!-- HR Image load area -->

<div style="position: absolute; background-color: white; border: solid 0px; left: 85px; top: 105px; width: 640px; height: 250px;">
    <div id="loadarea" style="position: absolute; background-color: white; left: 0px; top: 0px; width: 640px; height: 450px; margin: 0px;">
    </div>
    <div class="subhead" style="position: absolute; top: 430px; left: 5px;">
        <asp:Label CssClass="text" ID="picaddr" runat="server" />
        Price:
        <asp:Label ID="picprice" runat="server" />
    </div>
</div>


<!-- Step Carousel -->
<div id="Picgallery" class="stepcarousel" style="position: absolute; left: 85px; top: 385px; width: 640px">
    <div class="belt">
        <asp:Repeater ID="Carousel" runat="server">
            <ItemTemplate>
                <div class="panel">
                    <img class="panelpic" onclick="fader.loadimage('<%#Container.DataItem.LowResURL %>');" style="border: 1px #808080" src="<%#Container.DataItem.ThumbnailURL%>" alt="<%#Container.DataItem.LowResURL%>" />
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
