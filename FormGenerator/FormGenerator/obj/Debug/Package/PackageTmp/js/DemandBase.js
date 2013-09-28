var container =  $('body');
var action =  'dspthankyou.cfm';
var id =  'webinarForm';
var referrer = location.pathname;
var getVars = getUrlVars();
var results = {
    person: {}
};
var geoItems = {};



if (typeof jQuery == "undefined") {
    document.write('<script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>');
}

document.write(
'<script type="text/javascript" src="http://api.demandbase.com/autocomplete/widget.js"></script>' +
"<script type='text/javascript' src='http://dev.esri.com/apps/products/esrimaps/ibmcognosdemo/unique_apps.js'></script>"
);

function getUrlVars() {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}

//render the hidden fields that come back from demandbase	
function renderHiddenDBFields(name, value) {
    var field_name = name;
    var field_html =
			'<input runat="server" type="hidden" id="' + field_name + '" name="' + field_name + '" value="' + value + '" />';
    return field_html;
}

function initAutocomplete() {
    Demandbase.CompanyAutocomplete.widget({
        company: "company",  // attach autocomplete to input field with id = company
        email: "email",             // attach autocomplete to input field with id = email
        key: "ceab1aa689651315fbeab8b1d1b4fbd679fdac75",     // add your Demandbase Forms key here
        callback: function (data) {
            handleDBResponse(data);
        } // the callback to call after receiving data.
    });
}

//API hits
function getResultsIP() {
    $.getJSON('http://api.demandbase.com/api/v2/ip.json?key=ceab1aa689651315fbeab8b1d1b4fbd679fdac75', {}, handleDBResponse);
}

function getResultsDomain(email) {
    $.getJSON('http://api.demandbase.com/api/v1/domain.json?key=ceab1aa689651315fbeab8b1d1b4fbd679fdac75&query=' + email, {}, handleDBResponse);
}

//function demoData() {
//    alert('final data: \n' + JSON.stringify(this.results.complete), null, '\n');
//    return false;
//}

function getInputs() {
    return $('#' + id + ' input[type!=hidden]');
}

/*-----------------------------
//TRACKING
-----------------------------*/
//render tracking fields
function renderHiddenTrackingFields() {
    var ht_html = '';
    //getvars
    for (var f in getVars) {
        ht_html += "<input runat='server' type='hidden' name='T_" + f + "' value='" + getVars[f] + "' />";
    }
    //referrer

    ht_html += "<input runat='server' type='hidden' name='T_ref' value='" + referrer + "' />";

    $('#trackingFields').html(ht_html);
}

function renderTrackingGET() {
    var tg_html = (action.indexOf('?') == -1) ? '?' : '&';
    for (var f in getVars) {
        tg_html += f + "=" + getVars[f] + '&';
    }
    tg_html += "ref=" + referrer;
    return tg_html;
}


function handleDBResponse(data) {
    if (data.pick) { results.company = data.pick; }
    else if (data.person) { results.email = data.person; }
    else { results.ip = data; }

    //add form fields to hidden vars
    $('#' + id + ' input[type!=hidden]').each(function () {
        results.person[id] = this.value;
    })

    results.complete = $.extend({}, results.person, results.company, results.email, results.ip);
    $("#hfJson").val(results.complete);

    var hidden_html = '';
    var delimData = '';
    for (var p in results.complete) {
        if (!(p in results.person)) {
            hidden_html += renderHiddenDBFields(p, results.complete[p]);
            delimData += 'db_' + p + '^' + results.complete[p] + '|'
        }
    }
    delimData = delimData.substring(0, delimData.length - 1);

    $('#db-form-hidden').html(hidden_html);
    $('#hfDBResults').val(delimData);

}
	


	