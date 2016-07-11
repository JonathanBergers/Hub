function showmenu(btn, pnl){
    
    if (document.getElementById(pnl))
    {
        var b = document.getElementById(btn);
        var m = document.getElementById(pnl);
        if (m.style.display == "none")
        {
            m.style.display = "block";
            if (document.getElementById(btn))
            {
                b.innerHTML = "[Hide me]";
            }
            var result = writeSessionCookie(pnl,"show"); 
          
        }
        else
        {
           m.style.display = "none";
           if (document.getElementById(btn))
           {
               b.innerHTML = "[Show me]";
           }
           // set cookie
           var result = writeSessionCookie(pnl,"hide");         
        }   
    }
}

function showmenustartup(btn, pnl){
    if (document.getElementById(pnl))
    {
        var m = document.getElementById(pnl);
        var b = document.getElementById(btn);
        var c = getCookieValue(pnl);
        // on load cookie is not set or show
        if (c == false || c == "show")
        {
            m.style.display = "block";
            if (document.getElementById(btn))
            {
                b.innerHTML = "[Hide me]";
            }
        }
        else
        {
           m.style.display = "none";
           if (document.getElementById(btn))
           {
               b.innerHTML = "[Show me]";
           }
        }   
    }

}



function showsubmenu(pnl){
    
    if (document.getElementById(pnl))
    {
        var m = document.getElementById(pnl);
        
        // get the child items
        var i = 1;
        var seq = 0
        while (!isNaN(Right(m.id, i)))
        {
            seq = Right(m.id, i);
            i++;
        }
        
        var i = 1;
        while (document.getElementById(m.id + "_ctlmenu_" + parseInt(parseInt(seq) + i)))
        {
            sm = document.getElementById(m.id + "_ctlmenu_" + parseInt(parseInt(seq) + i));        
            if (sm.style.display == "none")
            {
                sm.style.display = "block";
                var result = writeSessionCookie(pnl,"show")
                m.className = "menuitemopen"
            }
            else
            {
               sm.style.display = "none";
               // set cookie
               var result = writeSessionCookie(pnl,"hide"); 
               m.className = "menuitem"     
            }              
            i++;
        }        
    }
}


function showsubmenustartup() { 
	var itms = getElementsByClass("menuitem");
	var i;
	for(i=0; i< itms.length; i++) 
	{
		showsubmenuonload(itms[i]);
    }
} 



function showsubmenuonload(pnl){
    
    if (document.getElementById(pnl.id))
    {
        var m = document.getElementById(pnl.id);
        // get the child items
        var i = 1;
        var seq = 0
        while (!isNaN(Right(m.id, i)))
        {
            seq = Right(m.id, i);
            i++;
        }  
        // no childeren
        if(!document.getElementById(m.id + "_ctlmenu_" + parseInt(parseInt(seq) + 1)))
        {
            m.className = "menuitemempty"
        }
        
        var i = 1;
        while (document.getElementById(m.id + "_ctlmenu_" + parseInt(parseInt(seq) + i)))
        {       
            var sm = document.getElementById(m.id + "_ctlmenu_" + parseInt(parseInt(seq) + i));    

            var c = getCookieValue(pnl.id);
         
            // on load cookie is not set or show
            if (c == false || c == "hide")
            {
                sm.style.display = "none";
            }
            else
            {
                sm.style.display = "block";
                m.className = "menuitemopen"
            }              
            i++;
       }        
    }
}
