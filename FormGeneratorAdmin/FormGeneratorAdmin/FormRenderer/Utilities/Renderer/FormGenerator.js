$(document).ready(function () {

    //ready..
    //initAutocomplete();


    $('#btnSubmit').click(function () {
        if (IsValid()) {
            //Json objects
            var formData = "{";
            var demandData = "{";
            var actionData = "{";
            var formActionData = "{";
            var groupData = "{";

            //Iteration through form objects (javascript called)
            $("div[class=set]").each(function () {
                var value;
                var labelName;

                if ($(this).children().is("input")) {
                    labelName = $(this).children('input').attr('apr');
                    value = $(this).children('input').val();
                } else if ($(this).children().is("select")) {
                    labelName = $(this).children('select').attr('apr');
                    value = $(this).children("select").val();
                }

                //append form data..
                formData += '"' + labelName + '" : "' + value + '",';
            });

            //
            formData = formData.substring(0, formData.length - 2);
            formData += '\"}';

            //call demand base function and load data..
            demandData = GetDemandBaseData(demandData);
            demandData = demandData.substring(0, demandData.length - 2);
            demandData += '\"}';

            actionData = GetActions(actionData);
            actionData = actionData.substring(0, actionData.length - 2);
            actionData += '\"}';

            formActionData = GetFormActions(formActionData);
            formActionData = formActionData.substring(0, formActionData.length - 2);
            formActionData += '\"}';

            groupData = GetGroups(groupData);
            groupData = groupData.substring(0, groupData.length - 2);
            groupData += '\"}';

            SendJson(formData, demandData, actionData, formActionData, groupData);
        }
    });
});

//Group Function
function GetGroups(groupData) {
    var groupValue = "";
    var groupName;

    $(".group").children("span").each(function (currentIndex) {
        var currentTextValue = $(this).text();
        groupName = $(this).attr("group");

        //validation for checked checkboxes
        if ($(this).children('input[type=checkbox]').is(':checked')) {
            groupValue += currentTextValue + ",";
        }
    });

    groupData += '"' + groupName + '" : "' + groupValue + '"';
    return groupData;
};

//Actions Function
function GetActions(actionData) {
    $("#actions").children('input[type=hidden]').each(function () {
        var isEcas = $(this).attr('redirect');
        //validates for actions (non ecas)
        if (typeof isEcas == 'undefined' || isEcas == false) {
            actionData += '"actionName" : "' + this.value + '",';
        } 
    });
    return actionData;
};


//Form Actions Function
function GetFormActions(formActionData) {
    $("#actions").children('input[type=hidden]').each(function () {
        var isEcas = $(this).attr('redirect');
        if ( typeof isEcas !== 'undefined') {
            formActionData += '"' + this.value + '"' + ' : ' +'"' + isEcas + '",';
        }

    });
    
    return formActionData;
};

//Demand Base Function
function GetDemandBaseData(demandData) {
    $("#db-form-hidden").children('input').each(function () {
        demandData += '"db_' + this.id + '" : "' + this.value + '",';
    });
    return demandData;
};

function SendJson(formData, demandData, actionData, formActionData, groupData) {
    var json = "{Form:" + formData + "," + "Actions:" + actionData + "," + "FormActions:" + formActionData + "," + "Groups:" + groupData + "," + "DB:" + demandData + "}";

    //Call handler...
    $.ajax({
        type: "POST",
        url: "/FormGenerator/Data/FormGeneratorHandler.ashx",
        data: { results: json },
        dataType: "json",
        success: function (data) {
            self.parent.location = "https://webaccounts.esri.com/CAS/index.cfm/CAS/index.cfm?resturnurl=" + $("#ecasRedirect").attr('redirect');
        },
        error: function (msg) {
        }
    });
};

function IsValid() {

    try {

        isValid = true;

        $('input[validate="True"]').each(function () {

            if ($(this).attr("controlname") == "date") {

                if ($(this).val() == "") {
                    showErrorBorder($(this), true);
                    isValid = false;
                }
                else
                    showErrorBorder($(this), false);
            }
            else if ($(this).attr("controlname") == "generic text"
                    || $(this).attr("controlname") == "first name"
                    || $(this).attr("controlname") == "last name"
                    || $(this).attr("controlname") == "city"
                    || $(this).attr("controlname") == "zip code"
                    || $(this).attr("controlname") == "company"
                    || $(this).attr("controlname") == "suite"
                    || $(this).attr("controlname") == "street address"
                    ) {

                if ($(this).val() == "") {
                    showErrorBorder($(this), true);
                    isValid = false;
                }
                else
                    showErrorBorder($(this), false);
            }
            else if ($(this).attr("controlname") == "email") {

                if (!IsEmail($(this).val())) {
                    showErrorBorder($(this), true);
                    isValid = false;
                }
                else
                    showErrorBorder($(this), false);
            }
            else if ($(this).attr("controlname") == "phone") {

                if (!IsPhoneNumber($(this).val())) {
                    showErrorBorder($(this), true);
                    isValid = false;
                }
                else
                    showErrorBorder($(this), false);
            }
            else if ($(this).attr("controlname") == "number") {

                if (!IsInt($(this).val()))
                    showErrorBorder($(this), true);
                else
                    showErrorBorder($(this), false);
            }
            else if ($(this).attr("controlname") == "url") {

                if (!validateURL($(this).val())) {
                    showErrorBorder($(this), true);
                    isValid = false;
                }
                else
                    showErrorBorder($(this), false);
            }

        });

        $('textarea[validate="True"]').each(function () {

            if ($(this).attr("controlname") == "comments") {

                if ($(this).val() == "") {
                    showErrorBorder($(this), true);
                    isValid = false;
                }
                else
                    showErrorBorder($(this), false);
            }
        });

        $('select[validate="True"]').each(function () {

            if ($(this).attr("selectedIndex") == "0") {
                showErrorBorder($(this), true);
                isValid = false;
            }
            else
                showErrorBorder($(this), false);

        });

        if (isValid == false) {
            $('.errorMessage').show();
        }
        else {
            $('.errorMessage').hide();
        }

        return isValid;

    }
    catch (e) {

        $('.errorMessage').show();
        return false;
    }
}

function validateURL(textval) {
    var urlregex = new RegExp("^(http:\/\/www.|https:\/\/www.|ftp:\/\/www.|www.){1}([0-9A-Za-z]+\.)");
    return urlregex.test(textval);
}

function IsEmail(email) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function IsPhoneNumber(phone) {
    var regex = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    return regex.test(phone);
}

function IsInt(number) {
    var regex = /^\d+$/;
    return regex.test(number);
}

function IsDate(input) {
    var validformat = /^\d{2}\/\d{2}\/\d{4}$/
    var returnval = false

    if (!validformat.test(input))
        returnval = false
    else {
        var monthfield = input.split('/')[0]
        var dayfield = input.split('/')[1]
        var yearfield = input.split('/')[2]
        var dayobj = new Date(yearfield, monthfield - 1, dayfield)

        if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield))
            returnval = false
        else
            returnval = true
    }
    alert(returnval);
    return returnval
}

function showErrorBorder(element, toggle) {

    if (toggle == true)
        element.css({ 'border-width': 'medium', 'border-color': '#FF8C00' });
    else
        element.css({ 'border-width': '', 'border-color': '#A4B97F' });
}


function autoResize(id) {
    var newheight;
    var newwidth;

    if (document.getElementById) {
        newheight = document.getElementById(id).contentWindow.document.body.scrollHeight;
        newwidth = document.getElementById(id).contentWindow.document.body.scrollWidth;
    }

    document.getElementById(id).height = (newheight) + "px";
    document.getElementById(id).width = (newwidth) + "px";
}

function SetUpTexBoxBehavior(elementID) {

    var phValue = $('#' + elementID).attr('placeholder'); ;

    $("#" + elementID).val(phValue).addClass('watermark');

    $("#" + elementID).blur(function () {
        if ($(this).val() == "") {
            $(this).val(phValue);
            $(this).addClass('watermark');
        }
        else
            $("#" + elementID).css({ 'border-width': '', 'border-color': '#A4B97F' });
    });

    $("#" + elementID).focus(function () {
        if ($(this).val() == phValue)
            $(this).val('').removeClass('watermark');
    });
}