$(function () {



    var isValidDate = function (dateStr, format) {


    


        if (format == null) { format = "MDY"; }
        format = format.toUpperCase();
        if (format.length != 3) { format = "MDY"; }
        if ((format.indexOf("M") == -1) || (format.indexOf("D") == -1) || (format.indexOf("Y") == -1)) { format = "MDY"; }
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
        if ((reg1.test(dateStr) == false) && (reg2.test(dateStr) == false)) { return false; }
        var parts = dateStr.split(RegExp.$1); // Split into 3 parts based on what the divider was
        // Check to see if the 3 parts end up making a valid date
        if (format.substring(0, 1) == "M") { var mm = parts[0]; } else if (format.substring(1, 2) == "M") { var mm = parts[1]; } else { var mm = parts[2]; }
        if (format.substring(0, 1) == "D") { var dd = parts[0]; } else if (format.substring(1, 2) == "D") { var dd = parts[1]; } else { var dd = parts[2]; }
        if (format.substring(0, 1) == "Y") { var yy = parts[0]; } else if (format.substring(1, 2) == "Y") { var yy = parts[1]; } else { var yy = parts[2]; }
        if (parseFloat(yy) <= 50) { yy = (parseFloat(yy) + 2000).toString(); }
        if (parseFloat(yy) <= 99) { yy = (parseFloat(yy) + 1900).toString(); }
        var dt = new Date(parseFloat(yy), parseFloat(mm) - 1, parseFloat(dd), 0, 0, 0, 0);
        if (parseFloat(dd) != dt.getDate()) { return false; }
        if (parseFloat(mm) - 1 != dt.getMonth()) { return false; }



        return true;
    }

    window.Parsley.addValidator('datevalidation', {
        validateString: function (value, requirement) {

            return isValidDate(value, requirement);
            //return true;


        },
        requirementType: 'string',
        messages: {
            en: 'Invalid date format : %s'
            
        }
    });

   

    var checkButtons = function (form) {
        // get buttons
        var buttons = $('[formsubmit]');



        if (!form.isValid()) {

            console.log("form is invalid");
            if (buttons != null) {

                buttons.addClass("disabled");
                buttons.prop('disabled', true);
                buttons.css('pointer-evens', 'none');

            }


        } else {
            if (buttons != null) {


                buttons.removeClass("disabled");
                buttons.prop('disabled', false);

            }
        }

    };


    var form = $('form');
    var formParsley = form.parsley({
        excluded: '.no-parsley'
    });

    formParsley.on('form:validated', function() {
        checkButtons(this);

    });
    formParsley.on('form:init', function () {
     

    });


    formParsley.on('field:error', function(){
        checkButtons(this);
      console.log(this)
      console.log(this.$element.id)
      this.$element.addClass("invalid");

      var errormessages = this.getErrorsMessages();
      var errormessage = "Invalid"
      if(errormessages.length>0){
        errormessage = errormessages[0];

      }
      console.log(this.getErrorsMessages())

      console.log('validation error for', this.$element);
     


      var label = $('[for="' + this.$element.context.id + '"]');

      if(label != undefined){
        
          console.log('found the label', label)
          label.css("width", "100%")
          label.attr('data-error', errormessage)
          label.addClass("active")

          

      }

       


    });


formParsley.on('field:success', function(){
      var label = $('[for="' + this.$element.context.id + '"]');

      if(label != undefined){
        
          console.log('found the label', label)
          label.css("width", "100%")
		    label.addClass("active")

          

      }

       


    });




    //on success

    formParsley.on('form:init', function () {
        this.validate();
        console.log("init form");
        checkButtons(this);
    });

    formParsley.on('field:validated', function() {

        checkButtons(formParsley);

    });


    formParsley.on('form:submit', function () {
    console.log("ON SUBMIT")
    console.log("ON SUBMIT" , this.$element)

    return false; // Don't submit form for this demo
  });





   checkButtons(formParsley);
  formParsley.validate();
  formParsley.trigger('click')


});
