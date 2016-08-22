addLoadListener(init);

// --------------------------------------------//
// javascript to secure unselected columns     //
// --------------------------------------------// 


// we add the loadlistener which resides in
// generic.js to the script. This function captures the
// OnLoad event and adds the init() to it.




// this init is firec on load. It fires secureColumn
// on its turn. This init() is kept if we want add more
// logic to the screen, for instance ond KeyUp events.
function init() {
    //attachEventListener(document, "keyup", test, false);	
	secureColumn();	
    return true;
};


// this function secures the unselected columns
function secureColumn(){	
	var c, i;
	// all the columns
	var itms = getElementsByClass("col");
	// array with the months
	var m = [["5","Sept"], ["6","Oct"], ["7","Nov"], ["8","Dec"], ["9","Jan"], ["10","Feb"], ["11","Mar"], ["12","Apr"], ["13","May"], ["14","Jun"], ["15","Jul"], ["16","Aug"]];
	
	// get the selected month
	if(document.getElementById("ctl1_8"))
	{
		var e = document.getElementById("ctl1_8");
		var p = e.options[e.selectedIndex].value;
	};
	
	
	// loop through all columns
	for(i=0; i< itms.length; i++) 
	{
		// to be sure we are picking columns only
		if(itms[i].id.match(/ctl_hv5_/) == "ctl_hv5_")
		{
			// use split() to parse the element.id. 3 is the column, 4 the row
			var s = itms[i].id.split("_")[3];
			
			// set all parentNodes which are the actual <td>'s to the values
			// of the inputboxes, EXCEPT the column that matches the selected period
			if(m[s - 5][1] != p.substring(0, 3)){			

					var item_value = itms[i].value
				     var parsed_value = parseFloat(itms[i].value.replace(',', '.'));
			        if (!isNaN(parsed_value)) {
			            item_value = parsed_value.toLocaleString(client_locale)
			           
			        }
				itms[i].parentNode.innerHTML = item_value;
			};			
		};
	 };
};


// e o f