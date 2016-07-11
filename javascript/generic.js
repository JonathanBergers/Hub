
// global colors
var ctotal = "#D5F3FE";
var cinput = "#F1F7FF";
var cheader = "#A3DAF2";
var cwhite = "#ffffff";
var corder = "#99CCCC";
var cerror = "#FF0000";
var cgreen = "#006600";


// set all disabled items to readonly
function disableme() {
	var itms = getDisabledItems();
	for(i=0; i< itms.length; i++) 
	{
		itms[i].readOnly = true;
    }
}

// get the disabled items
function getDisabledItems(){
	var el = new Array();
	var tags = document.getElementsByTagName('*');
	var tcl = " disabled ";
	var i; var j;
	
	for(i=0,j=0; i<tags.length; i++) { 
		var test = " " + Right(tags[i].className, 8) + " ";
		if (test.indexOf(tcl) != -1) 
				
			el[j++] = tags[i];
	} 
	return el;
}

// get the mandatory items
function getMandatoryItems(){
	var el = new Array();
	var tags = document.getElementsByTagName('*');
	var tcl = " mandatory ";
	var i; var j;
	
	for(i=0,j=0; i<tags.length; i++) { 
		var test = " " + Right(tags[i].className, 9) + " ";
		if (test.indexOf(tcl) != -1) 
				
			el[j++] = tags[i];
	} 
	return el;
}

// get date items
function getDateItems(){
	var el = new Array();
	var tags = document.getElementsByTagName('*');
	var tcl = " date ";
	var i; var j;
	
	for(i=0,j=0; i<tags.length; i++) { 
		var test = " " + tags[i].className.match(/date/) + " ";
		if (test.indexOf(tcl) != -1)	
			el[j++] = tags[i];
	} 
	return el;
}

// set buttons to gray when a process is running after click
function grayoutbutton() { 
    attachEventListener(document, "click", buttns, false);
    return true;
}

function buttns(){
var f = document.activeElement;
if (f.className == "button")
    {
		setTimeout("makeInactive('" + f.id + "')", 500);
    }
}

function makeInactive(e){
var f = document.getElementById(e);
if (f.disabled == false)
    {
        f.disabled = true;
        f.href = "javascript:void(0);"; 
        // waitscreen
        var w = document.getElementById("wait");
		var b = document.getElementById("sandbox");
		b.style.background = "url(images/wheel.gif) no-repeat center";
		w.height = "100%";
		w.width = "100%";
		w.style.display = "block";		
		b.style.display = "block";
    }
}

function checkMandatory() {
    var itms = getMandatoryItems();
    var haserrors = 0;
    for(i=0; i< itms.length; i++) 
    {
	    if(itms[i].value.length == 0)
	    {
	        haserrors ++;
	        // store the errors in the error array
	        errors.push(itms[i].id);
	    }
    }
    // set buttons
    var l = getElementsByClass("button");
    for(var i = 0; i < l.length; i++)
    {
        // exclude cancel button
        if (l[i].name != "cancel")
        {
           if (haserrors > 0 || l[i].disabled)
           {
                l[i].disabled = true;
                l[i].href = "javascript:void(0);";
           }
           else
           {
               l[i].disabled = false;
               l[i].href="javascript:__doPostBack('" + l[i].id + "','')";
           }
        }      
    }

    return true;
}

function checkElement(type) {

    var o = document.activeElement;
    var r = false;
    
    // quit when no checking is required
    if(type.length == 0 && o.className != "mandatory")
    {
        return;
    }
    // ------- string
    // char[n]
    // char 
    // varchar[n]
    // ------- numbers
    // integer
    // numeric
    // notzero
    // ------- other
    // email
    // date
    // ------- mandatory
    // mandatory
    
    // check char with fixed characters
    if (type.substring(0,4) == "char" && type.length > 4)
    {
        var c = type.substring(4,8);
        o.value = o.value.substring(0,c);
        if (o.value.length != c)
        {
            r = false;
        }
        else
        {
            r = true;
        }
    }
 
    // check other datatypes       
    else
    {
        switch (type) {
        case "char":
            // nothing
            r = true;
              break;
          
        case "integer":
           if(isInteger(o.value)){
            r = true;
           }
           break; 
           
        case "numeric":
           if(isNumeric(o.value)){
            r = true;
           }
           break; 
           
         case "email":
           if(isValidEmail(o.value)){
            r = true;
           }
           break; 
           
         case "date":
           if (o.value.indexOf(":") == -1) // no time
           {
                // empty date is allowed
                if(isValidDate(o.value, "DMY") || o.value.length == 0){
                r = true;
                }
           }
           else
           {
                var t = o.value.replace("  ", " ").split(" ");
                if(isValidDate(t[0], "DMY") && isValidTime(t[1])){
                r = true;
                }
           }
           break; 
           
         case "notzero":
           if(isNotZero(o.value)){
            r = true;
           }
           break; 
               
        default:
            r = true;
          break;
        }
    }
   
   // check length 
    if (type.substring(0,7) == "varchar" && type.length > 7)
    {
        var c = type.substring(7,12);
        if (o.value.length >= c)
        {
            o.value = o.value.substring(0,c);
        }
    }
    
    // check mandatory    
    if(Right(o.className, 9) == "mandatory" && o.value.length == 0 && r == true)
    {
        r = false;
    }
        
    // set color   
    if (r)
    {  
       o.style.color = "";   
       
       // delete from error array
        var test = false;
        var len = errors.length;
        for (i=0; i<len; i++)
        {
            if (errors[i] == o.id)
            {
                test = true;
                break;
            }
        }
        
        if (test)
        {
           errors.splice(i,1);
        }
          
        var haserrors = errors.length;
        if (haserrors == 0)
        {
           var l = getElementsByClass("button");
           for(var i = 0; i < l.length; i++)
            {
               // check
               l[i].disabled = false;
               l[i].href = "javascript:__doPostBack('" + l[i].id + "','');";
            }
        }
    }
    else
    {
       o.style.color = cerror;
      
        // add to error array
        var test = false;
        var len = errors.length;
        for (i=0; i<len; i++)
        {
            if (errors[i] == o.id)
            {
                test = true;
                break;
            }
        }
        
        if (!test)
        {
           errors.push(o.id);
        }
              
       var l = getElementsByClass("button");
       for(var i = 0; i < l.length; i++)
        {
            var haserrors = errors.length;
            if (haserrors > 0)
            {
               l[i].disabled = l[i].name == "cancel" ? false : true;
               if (l[i].name != "cancel")
               {
                    l[i].href = "javascript:void(0);"
               }
               
            }
            else
            {
                // check
                l[i].disabled = false;
                l[i].href ="javascript:__doPostBack('" + l[i].id + "','');";
            }
        }      
    }  
  return;
}
  

// toolbox functions       
function addLoadListener(fn) {
    if (typeof window.addEventListener != 'undefined') {
        window.addEventListener('load', fn, false);
    } else if (typeof document.addEventListener != 'undefined') {
        document.addEventListener('load', fn, false);
    } else if (typeof window.attachEvent != 'undefined') {
        window.attachEvent('onload', fn);
    } else {
        var oldfn = window.onload;
        if (typeof window.onload != 'function') {
            window.onload = fn;
        } else {
            window.onload = function () {
                oldfn();
                fn();
            };
        }
    }
}

function attachEventListener(target, eventType, functionRef, capture) {
    if (typeof target.addEventListener != "undefined") {
        target.addEventListener(eventType, functionRef, capture);
    } else if (typeof target.attachEvent != "undefined") {
        target.attachEvent("on" + eventType, functionRef);
    } else {
        eventType = "on" + eventType;

        if (typeof target[eventType] == "function") {
            var oldListener = target[eventType];

            target[eventType] = function () {
                oldListener();

                return functionRef();
            }
        } else {
            target[eventType] = functionRef;
        }
    }

    return true;
}


function Right(str, n) {
    if (n <= 0) return "";
    else if (n > String(str).length) return str;
    else {
        var iLen = String(str).length;
        return String(str).substring(iLen, iLen - n);
    }

}

function isNumeric(value) {
  if (value != null && !value.toString().match(/^[-]?\d*\.?\d*$/)) return false;
  return true;
}

function isValidEmail(value) {
  if (value != null && !value.toString().match(/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i)) return false;
  return true;
}

function isNotZero(value) {
  if (value != null && value == 0) return false;
  return true;
}

function isNegInt(value) {
  if (value != null && !value.toString().match(/(^0|-\d\d*$)/)) return false;
  return true;
}

function isPosInt(value) {
  if (value != null && !value.toString().match(/(^\d\d*$)/)) return false;
  return true;
}

function isInteger(value) {
  if (value != null && !value.toString().match(/(^-?\d\d*$)/)) return false;
  return true;
}

function isValidYear(value) {
  if (value != null && !value.toString().match(/(19|20)\d\d/)) return false;
  return true;
}


function isValidDate(dateStr, format) {
   if (format == null) { format = "MDY"; }
   format = format.toUpperCase();
   if (format.length != 3) { format = "MDY"; }
   if ( (format.indexOf("M") == -1) || (format.indexOf("D") == -1) || (format.indexOf("Y") == -1) ) { format = "MDY"; }
   if (format.substring(0, 1) == "Y") { // If the year is first
      var reg1 = /^\d{2}(\-|\/|\.)\d{1,2}\1\d{1,2}$/
      var reg2 = /^\d{4}(\-|\/|\.)\d{1,2}\1\d{1,2}$/
   } else if (format.substring(1, 2) == "Y") { // If the year is second
      var reg1 = /^\d{1,2}(\-|\/|\.)\d{2}\1\d{1,2}$/
      var reg2 = /^\d{1,2}(\-|\/|\.)\d{4}\1\d{1,2}$/
   } else { // The year must be third
      var reg1 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{2}$/
      var reg2 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$/
   }
   // If it doesn't conform to the right format (with either a 2 digit year or 4 digit year), fail
   if ( (reg1.test(dateStr) == false) && (reg2.test(dateStr) == false) ) { return false; }
   var parts = dateStr.split(RegExp.$1); // Split into 3 parts based on what the divider was
   // Check to see if the 3 parts end up making a valid date
   if (format.substring(0, 1) == "M") { var mm = parts[0]; } else if (format.substring(1, 2) == "M") { var mm = parts[1]; } else { var mm = parts[2]; }
   if (format.substring(0, 1) == "D") { var dd = parts[0]; } else if (format.substring(1, 2) == "D") { var dd = parts[1]; } else { var dd = parts[2]; }
   if (format.substring(0, 1) == "Y") { var yy = parts[0]; } else if (format.substring(1, 2) == "Y") { var yy = parts[1]; } else { var yy = parts[2]; }
   if (parseFloat(yy) <= 50) { yy = (parseFloat(yy) + 2000).toString(); }
   if (parseFloat(yy) <= 99) { yy = (parseFloat(yy) + 1900).toString(); }
   var dt = new Date(parseFloat(yy), parseFloat(mm)-1, parseFloat(dd), 0, 0, 0, 0);
   if (parseFloat(dd) != dt.getDate()) { return false; }
   if (parseFloat(mm)-1 != dt.getMonth()) { return false; }
   return true;
}

function isValidTime(timeStr) {
    if (timeStr != null && !timeStr.toString().match(/(^[0-1][0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]$)/)) return false;
    return true;
}


function findPosLeft(obj) {
    var curleft = 0;
    while (obj) {
        curleft += obj.offsetLeft
        obj = obj.offsetParent
        }
    return curleft;
}


function findPosTop(obj) {
    var curtop = 0;
    while (obj) {
        curtop += obj.offsetTop
        obj = obj.offsetParent
        }
    return curtop;
}


function writeSessionCookie (cookieName, cookieValue) {
  if (testSessionCookie()) {
    document.cookie = escape(cookieName) + "=" + escape(cookieValue) + "; path=/";
    return true;
  }
  else return false;
}


function getCookieValue (cookieName) {
  var exp = new RegExp (escape(cookieName) + "=([^;]+)");
  if (exp.test (document.cookie + ";")) {
    exp.exec (document.cookie + ";");
    return unescape(RegExp.$1);
  }
  else return false;
}


function testSessionCookie () {
  document.cookie ="testSessionCookie=Enabled";
  if (getCookieValue ("testSessionCookie")=="Enabled")
    return true 
  else
    return false;
} 
 
function trim(str, chars) {
	return ltrim(rtrim(str, chars), chars);
}
 
function ltrim(str, chars) {
	chars = chars || "\\s";
	return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}
 
function rtrim(str, chars) {
	chars = chars || "\\s";
	return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}


function getElementsByClass(searchClass) { 
	var el = new Array();
	var filter;
	
	switch (searchClass) {
        case "button":
            filter = "a";
              break;
              
        case "menuitem":
            filter = "div";
              break;
              
         case "checkbox":
            filter = "input";
              break;
        
         default:
            filter = "*";
              break;
    }
	
    var tags = document.getElementsByTagName(filter);
	var tcl = " "+searchClass+" ";
	var i; var j;
	
	for(i=0,j=0; i<tags.length; i++) { 
		var test = " " + tags[i].className + " ";
		if (test.indexOf(tcl) != -1) 
				
			el[j++] = tags[i];
	} 
	return el;
}

// get the moddate
function getModDate(f){
    var d = new Date();
    var r;
    
    switch(f) {
    
    case "DMY": 
        r =  Right("00" + d.getDate(), 2)+ "/" +
        Right("00" + (d.getMonth() + 1), 2) + "/" + d.getFullYear() +
        " " + Right("00" + d.getHours(), 2) + ":" + Right("00" +
        d.getMinutes(), 2) + ":" + Right("00" + d.getSeconds(), 2);
        break;
        
    case "MDY":
        r =   Right("00" + (d.getMonth() + 1), 2) + "/" +
        Right("00" + d.getDate(), 2) + "/" + d.getFullYear() +
        " " + Right("00" + d.getHours(), 2) + ":" + Right("00" +
        d.getMinutes(), 2) + ":" + Right("00" + d.getSeconds(), 2);
        break;
        
    case "YDM":
        r =   d.getFullYear() + "/" +
        Right("00" + d.getDate(), 2) + "/" + Right("00" + (d.getMonth() + 1), 2) +
        " " + Right("00" + d.getHours(), 2) + ":" + Right("00" +
        d.getMinutes(), 2) + ":" + Right("00" + d.getSeconds(), 2);
        break;

    case "YMD":
        r =   d.getFullYear() + "/" +
        Right("00" + (d.getMonth() + 1), 2) + "/" + Right("00" + d.getDate(), 2) +
        " " + Right("00" + d.getHours(), 2) + ":" + Right("00" +
        d.getMinutes(), 2) + ":" + Right("00" + d.getSeconds(), 2);
        break;

    default:
        r = d;   
    }    
      
return r;
}

function popitup(url) {
	newwindow=window.open(url,'name','height=600,width=800');
	if (window.focus) {newwindow.focus()}
	return false;
}


function waitOpen(){
	// waitscreen
	var w = document.getElementById("wait");
	var b = document.getElementById("sandbox");
	b.style.background = "url(images/wheel.gif) no-repeat center";
	w.height = "100%";
	w.width = "100%";
	w.style.display = "block";		
	b.style.display = "block";		
	setTimeout("waitClose()" , 4000);
};

function waitClose(){
	// waitscreen
	var w = document.getElementById("wait");
	var b = document.getElementById("sandbox");
	b.style.background = "url(images/wheel.gif) no-repeat center";
	w.height = "100%";
	w.width = "100%";
	w.style.display = "none";		
	b.style.display = "none";		
};

function addDefinitionLink()
{
    var l = getElementsByClass("colhead");
	for(var i = 0; i < l.length; i++) 
	{	
		var t = l[i].firstChild.href.split("=");
		var p = t[1].split("&");
		var u = "index.aspx?page=rcrdefinitionshow&filter=nofilter|"+ p[0] + "|AND|" + l[i].id;
		l[i].firstChild.href = "javascript:void window.open('" + [u] + "','Definition','height=550pt, width=650pt, scrollbars=no');"
		l[i].firstChild.onclick = function() {waitOpen();};
	};
 };