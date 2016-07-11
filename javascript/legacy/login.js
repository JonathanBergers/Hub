// login textbox
var login = "ctl2_2";
var name   = "ctl2_1";

addLoadListener(init);

function init() {
    attachEventListener(document, "keypress", confirm, false);
    // set focus tb1
    if (document.getElementById(name))
    {
        var e = document.getElementById(name);
        e.focus();
    }
    return true;
}


function confirm(e){
var f = document.activeElement;
var keycode;

if (f.id == login)
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
}

