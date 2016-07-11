window.onload = function () {
    console.log('window on load')
    initMaterialize();
    fixForm();
    fix_session();
};


var initMaterialize = function() {
    // initialize all thats needed for materialize
    $(".dropdown-button").dropdown();
    $('select').material_select();
    $("input").change();

    // for the menu
    $('.collapsible')
        .collapsible({
            accordion: false
            // A setting that changes the collapsible behavior to expandable instead of the default accordion style
        });
};

var convertDate = function(currVal) {

    var dateVal = date(currVal)
    var dateConv = dateVal.getFullYear() + '/' + dateVal.getMonth() + '/' + dateVal.getDay();
    console.log("converted date to: ", dateConv);
    return dateConv;


};

var submitForm = function() {

    console.log("clicked button");
    console.log(this)


    var formParsley = $("#frm").parsley();


    var dateFields = $('[dateField]');

    console.log($('#frm'))


    if (dateFields != null && dateFields != undefined && dateFields.length != 0) {

        console.log(dateFields);


        console.log(dateFields.val())

        //dateFields.val(convertDate(dateFields.val()));


    }

    if (formParsley.isValid()) {
        __doPostBack(this.id, "");
    }


}

function go_back() {
    window.history.back();
}


function fix_session() {


    // Initialize collapse button
    // number of reconnects
    var count = 0;

    // maximum reconnects setting
    var max = 240;

    // errors
    var errors = [];

    // reconnect
    function reconnect() {
        count++;
        if (count < max) {
            var img = new Image(1, 1);
            img.src = "reconnect.aspx?" + Math.random();
        }
    }


    setInterval(reconnect, 60000);

}

function fixForm() {

    var buttons = $('[formsubmit]');
    var form = $('#form')

    var cancel_buttons = $('a[name="cancel"]');
    console.log(cancel_buttons)



    buttons.attr("href", "javascript:void()")
    cancel_buttons.attr("href", "javascript:go_back()")
    buttons.click(submitForm);

}

function convert_number_locale(element, locale) {

    var id = element.id;
    console.log(id);
//    var value = parseFloat(element.html().split('<script>')[0].replace(',', '.'));

    $('document').ready(function() {
        var value = parseFloat(element.html().split('<script>')[0].replace(',', '.'));

        
        if (!isNaN(value)) {
            var new_value = value.toLocaleString(locale)
            console.log('convert: ', new_value)
            element.text(new_value);
        }

    })
//    if (!isNaN(value)) {
//        var new_value = value.toLocaleString(locale)
//        console.log('convert: ', new_value)
//        element.text(new_value);
//    }
    
}



// Initialize collapsible (uncomment the line below if you use the dropdown variation)
//$('.collapsible').collapsible();