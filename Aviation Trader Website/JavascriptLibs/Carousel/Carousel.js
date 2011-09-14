  //
  // global variables - defined in header, remain in scope across ajax page updates
  //  
   var currentMenuSelection = null;
   var AdID = -1;  
   var pics = new Array(); 
   var slideTimer = null;
   var belt = null;
   var carouselwidth = 0;
   var beltwidth =0; 
   
  function initialisePRM() {
 //
 // called on top level page load (no postback) to set up page load ajax event handler for updatepanel
 //
      var prm = Sys.WebForms.PageRequestManager.getInstance();
      prm.add_pageLoaded(loadBelt);  
   }
    
  function getImages(parAdID) {
 //
 // calls the server web method to get a new image list
 // call the server for new images only if the current ad number has changed
 //
     if (AdID != parAdID) {
        AdID = parAdID;            //update ad id in global
        PageMethods.GetImages(AdID,onSucceeded,onFailed);
     }
  }
  
  
 function onSucceeded(result,userContext,methodName) {
    pics = result;          // result array to global pics array
    preloadImages();        //get the images into cache
    loadBelt();
   }
    
 function onFailed(error,userContext,methodName) {
 }
    
 function preloadImages() {
 //
 // preloads the images in the pic array
 //
 
   for ( var i in pics) {
     var image = new Image(); 
     image.src = pics[i].THBURL;
     image = new Image(); 
     image.src = pics[i].LoresURL;
   }
 }   
    
   function changeAd(obj,parAdID) {
//
// client side handler for menu click. Changes the class name and gets new pics
//
    var div = obj.parentNode;
    var adlist = div.parentNode;
    
    var listitems = adlist.getElementsByTagName("div");
    for (var i in listitems) 
        listitems[i].className = "detailmenu";       //set all to up
    div.className = "detailmenuselected";            //set me to down
    getImages(parAdID);         // get new images from server if necessary
  }
 
   
  function loadBelt() {
  //
  // Loads images from the pic list onto the belt
  // but only if this is the tab with the carousel on it
  //
     HTMLcarousel = document.getElementById("picgallery");
     if (HTMLcarousel != null) {
        
        carouselwidth=parseInt(HTMLcarousel.style.width);       //save the width for later
        belt = HTMLcarousel.getElementsByTagName("div")[0];
        beltwidth=0;
          
        for ( var i in pics) {
           var imageHTML="<div class='panel'><img width='" + pics[i].THBWidth + "' src='" + pics[i].THBURL + "' onclick=\"showLores('" + pics[i].LoresURL + "')\" /></div>"
           belt.innerHTML +=imageHTML;
           beltwidth += pics[i].THBWidth + 10;
        }
        //
        // set the belt width
        //
        belt.style.width = beltwidth +  "px";
        showLores(pics[0].LoresURL);    // show first image in load area
     }
  }
 
 function rotateBelt(dirn) {
 //
 // rotates the belt left or right
 //
      var delta=150;        //#px to move
      var dx=15;           //# px at a time 
      if (dirn =='L') {
         delta= -delta;     //reverse direction
         dx = -dx;
       }
   
      newLeft = parseInt(belt.style.left) + delta;      //this is new posn of belt left end
 //
 // perform limit checks and if close to end of belt, give a half nudge then return to belt end
 //     
      if (dirn =='L') {
             var newRight = newLeft + beltwidth;
             if (newRight < carouselwidth ) {
                newLeft -= delta/2    //half the distance
                 slideTimer = window.setInterval("bounceToStop('" + dx + "','" + newLeft + "')",1);
             }
             else {    
                 slideTimer = window.setInterval("moveTo('" + dx + "','" + newLeft + "')",1);  
            }
      }
      else {
            if (newLeft > 0 ) {
                newLeft -=delta/2       //half the distance
                 slideTimer = window.setInterval("bounceToStop('" + dx + "','" + newLeft + "')",1);
           }
           else {    
                 slideTimer = window.setInterval("moveTo('" + dx + "','" + newLeft + "')",1);  
           }
     }
   
   }
  
 function moveTo(dx,finalLeft) {
//
// moves the belt dx at a time until finalLeft postion is reached. Sign of dx determines direction of move
//
    var curLeft = parseInt(belt.style.left);
    curLeft += Number(dx);      //get the new left
    if (dx > 0) {               //move right
      if (curLeft > finalLeft) 
        window.clearInterval(slideTimer);
      else 
        belt.style.left=curLeft + "px";  
    }
    else {                     //move left
      if (curLeft < finalLeft) 
        window.clearInterval(slideTimer);
      else 
        belt.style.left=curLeft + "px";
    }
 }
 
  function bounceToStop(dx,interimLeft) {
//
// bounces the belt to interimLeft and then then returns to right or left stop position
//
    var curLeft = parseInt(belt.style.left);
    curLeft += Number(dx);
    if (dx > 0) {        //move right
      if (curLeft > interimLeft) {
        window.clearInterval(slideTimer);
        dx = -dx;
        slideTimer = window.setInterval("moveTo('" + dx + "','0')",1);  
      }  
      else 
        belt.style.left=curLeft + "px";  
    }  
    else {                  //move left
      if (curLeft < interimLeft) {
         window.clearInterval(slideTimer);
         dx = -dx;
         var finalLeft = carouselwidth-beltwidth;
         if (finalLeft > 0) finalLeft = 0;      //carousel wider than belt
         slideTimer = window.setInterval("moveTo('" + dx + "','" + finalLeft + "')",1); 
       } 
      else 
        belt.style.left=curLeft + "px";
    }
 }
    
  
   function showLores(img) {
   //
   // display the lores version of the pic in the loadarea
   //
     var picviewer = document.getElementById("Picviewer");
     var imageHTML='<img src="'+ img + '" style="position:absolute; height:' + picviewer.style.height + ';" />' //Construct HTML for enlarged image
     picviewer.innerHTML=imageHTML
     featureImage=picviewer.getElementsByTagName("img")[0] //Reference enlarged image itself
    //
    // clip the pic without distorting it - does not work yet
    //
    var picWidth = parseInt(picviewer.style.width)-6 ;
    featureImage.style.clip = "rect(0px " + picWidth  + "px " + picviewer.style.height + " 0px)";
  } 