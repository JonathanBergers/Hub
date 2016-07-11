addLoadListener(init);

var pw  = "ctl1_2"
var pwr = "ctl1_4";

function init() {
    attachEventListener(document, "keyup", checkpw, false);
    attachEventListener(document, "keypress", confirm, false);
    format();
    checkpw();
    // set focus pw
    if (document.getElementById(pw))
    {
        var e = document.getElementById(pw);
        e.focus();
    }
    return true;
};

function format(){
    if(document.getElementById(pwr) && document.getElementById(pw)){
       var e = document.getElementById(pwr);
       var a = document.getElementById(pw);
       e.removeAttribute("readonly",0);
       e.className = "input";
       a.value = "";
    }          
    return true;
};

function checkpw() {
    if(document.getElementById(pw) && document.getElementById(pwr)){
        var a = document.getElementById(pw);
        var b = document.getElementById(pwr);
       
        var l = getElementsByClass("button");
        for(var i = 0; i < l.length; i++) {
               l[i].disabled = l[i].name == "cancel" ? false : true;
               if (l[i].name != "cancel")
               {
                    l[i].href = "javascript:void(0);"
               }
        } 
        
                   
        if (a.value == b.value && a.value.length != 0) {
            for(var i = 0; i < l.length; i++) {
                   l[i].disabled = false;
                   l[i].href ="javascript:__doPostBack('" + l[i].id + "','');";
            } 
        }
    }
};

function confirm(e){
var f = document.activeElement;
var keycode;

if (f.id == pwr)
    {

    if (window.event) keycode = window.event.keyCode;
    else if (e) keycode = e.which;
    else return true;
    
    if(keycode == 13)
    {
        __doPostBack('ctl4_16','');
        return false;
    }
    else
        return true;
    }
    return true;
};