function validateForm() {
    errors = "";
    msg = "Please enter the following:\n";
    CheckShortContactInfo();

    if (errors) {
        //alert(msg + "");
        $('.showErrormsg').show();
        window.location = '#top';
        return false;
    }
    else {
        return true;
    }
}

function CheckShortContactInfo() {
    var badEmailAddress = "";
    // if the fields are empty or still use the default values
    if (($("#fname").val() == "") || ($("#fname").val() == "First Name")) {
        msg += "- First name\n";
    }
    if (($("#lname").val() == "") || ($("#lname").val() == "Last Name")) {
        $("#lname").val("");
        msg += "- Last name\n";
    }

    if (($("#email").val() == "") || ($("#email").val() == "E-mail Address")) {
        $("#email").val("");
        msg += "- E-mail address\n";
    }
    if (($("#email").val() != "") && (echeck($("#email").val()) == false)) {
        badEmailAddress = $("#email").val();
        $("#email").val("");
        msg += "- Valid e-mail address\n";
    }

    errors += required($("#fname"));
    errors += required($("#lname"));
    errors += required($("#company"));
    errors += required($("#city"));
    errors += required($("#zip"));
    errors += required($("#phone"));
    errors += required($("#email"));
    errors += required($("#F_reason"));
    //validation for country picker
    errors += required($("#countries-select"), "select");
    /*if($(":input[name='F_country']").attr('value') == 'US' && $(":input[name='F_country']").attr('type') !='hidden')
    {			
    $('#province').val('');
    updateForm($(":input[name='F_country'] option:selected").val());
    }
		
    if($(":input[name='country']").length && $(":input[name='country']").attr('type') !='hidden')
    {
    errors += required($("#country"));		
    if ($("#country").val() == "US" )
    {					
    $('#province').val('');
    errors += required($("#state"));
    }
    else{				
    $('#state').val('');
    errors += required($("#province"));
    }
    }
    else */
    errors += required($("#state"));


    if (badEmailAddress != "") {
        $("#email").val(badEmailAddress);
    }
}

function echeck(str) {
    var at = "@"
    var dot = "."
    var lat = str.indexOf(at)
    var lstr = str.length
    var ldot = str.indexOf(dot)
    if (str.indexOf(at) == -1) {
        return false;
    }
    if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) {
        return false;
    }
    if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) {
        return false;
    }
    if (str.indexOf(at, (lat + 1)) != -1) {
        return false;
    }
    if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot) {
        return false;
    }
    if (str.indexOf(dot, (lat + 2)) == -1) {
        return false;
    }
    if (str.indexOf(" ") != -1) {
        return false;
    }
    return true;
}

function required(vid, vtype, vContainer) {
    var highlightBG = "#E48028";
    var highlightColor = "#FFFFFF";
    var normalBG = "";
    var normalColor = "#000000";

    if (vtype == null)
        vtype = "text";
    if (vtype == "text") {
        if ($.trim(vid.val()) == "") {
            vid.css("border", "2px solid #E5812A");
            return "Error.";
        }
        else {
            vid.css("border", "1px solid #a4b97f");
            return "";
        }
    }
    if (vtype == "radiogroup") {
        var rgSelected = false;
        vid.each(function (i) {
            if ($(this).attr("checked") && !rgSelected) {
                rgSelected = true;
            }
        });
        if (rgSelected) {
            vContainer.css("background-color", normalBG); ;
            vContainer.css("color", normalColor);
            return '';
        }
        else {
            vContainer.css("background-color", highlightBG);
            vContainer.css("color", highlightColor);
            msg += "- Select a webinar\n";
            return 'Error.';
        }
    }
    if (vtype == "select") {
        if (vid.val() == "") {
            vid.css("border", "2px solid #E5812A");
            return "Error.";
        }
        else {
            vid.css("border", "1px solid #a4b97f");
            return "";
        }
    }
}

/* from http://www.bloggingdeveloper.com/post/Disable-Form-Submit-on-Enter-Key-Press.aspx */
function disableEnterKey(e) {
    var key;
    if (window.event)
        key = window.event.keyCode; //IE
    else
        key = e.which; //firefox
    return (key != 13);
}

/*

Copyright (c) 2009 Stefano J. Attardi, http://attardi.org/

Modified by Saritha and Teresa

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/
(function ($) {
    function toggleLabel() {
        var input = $(this);
        setTimeout(function () {
            var def = input.attr('title');
            if (!input.val() || (input.val() == def)) {
                input.prev('span.labelSpan').css('visibility', '');
                if (def) {
                    var dummy = $('<label></label>').text(def).css('visibility', 'hidden').appendTo('body');
                    //input.prev('span').css('margin-left', dummy.width() + 3 + 'px');
                    dummy.remove();
                }
            } else {
                input.prev('span.labelSpan').css('visibility', 'hidden');
            }
        }, 0);
    };

    function resetField() {
        var def = $(this).attr('title');
        if (!$(this).val() || ($(this).val() == def)) {
            $(this).val(def);
            $(this).prev('span.labelSpan').css('visibility', '');
        }
    };

    $('input, textarea').live('keyup keydown', toggleLabel);
    $('input, textarea').live('paste', toggleLabel);
    $('input, textarea, select').live('mouseover', toggleLabel);
    $('input, textarea, select').live('mouseout', toggleLabel);
    $('input, textarea, select').live('mousemove', toggleLabel);
    $('input, textarea, select').live('blur', toggleLabel);
    $('select').live('keyup keydown', toggleLabel);
    $('select').live('change', toggleLabel);

    $('input, textarea').live('focusin', function () {
        $(this).prev('span.labelSpan').css('color', '#c3c3c3');
        $(this).css('background-color', '#fff');
        $(this).css('border', '1px solid #007AC2');
    });
    $('input, textarea').live('focusout', function () {
        $(this).prev('span.labelSpan').css('color', '#8f8f8f');
        $(this).css('border', '1px solid #007AC2');
    });

    $(function () {
        $('input, textarea, select').each(function () { toggleLabel.call(this); });
    });

})(jQuery);
