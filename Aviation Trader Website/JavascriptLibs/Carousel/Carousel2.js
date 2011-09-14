 
   var slideTimer = null;
   var belt = null;
   var carouselwidth = 0;
   var beltwidth =0; 
   
 
 
 
   
  function readBelt() {
  //
  // Loads images from the pic list onto the belt
  // but only if this is the tab with the carousel on it
  //
     HTMLcarousel = document.getElementById("picgallery");
     if (HTMLcarousel != null) {        
        carouselwidth=parseInt(HTMLcarousel.style.width);       //save the width for later
        belt = HTMLcarousel.getElementsByTagName("div")[0];
        beltwidth = parseInt(belt.style.width);
     }
  }
 
 function rotateBelt(dirn) {
 //
 // rotates the belt left or right
 //
      var delta=190;        //#px to move
      var dx=19;           //# px at a time 
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