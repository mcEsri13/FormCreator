
$(document).ready(function () {

    $("#btnLogin").click(function () {

        username = $("#txtUsername").val();
        password = $("#txtPassword").val();

        $.ajax({
            type: "POST",
            url: "ajax.aspx/LogIn",
            data: "{'username':'" + username + "', 'password':'" + password + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                var j = $.parseJSON(data.d);

                if (j[0].Admin_ID != null)
                    colapseAllExcept("divCreateOrEdit");
            },
            error: function (msg) {
                alert(msg);
            }
        });


    });

    $(".closeButton").click(function () {
        colapseAllExcept("divCreateOrEdit");
    });

    $("#btnCreateForm").click(function () {
        colapseAllExcept("divCreateForm");
    });

    $("#btnEditForm").click(function () {
        colapseAllExcept("divPreviewFormInfo");
    });

    $("#btnEditFormInfo").click(function () {
        colapseAllExcept("divCreateForm");
    });

    $(".doValidate").click(function () {
        alert("checked!");
    });

    $("#btnContinue").click(function () {

        formName = $("#txtFormName").val();
        sitecoreID = $("#txtSitecoreID").val();

        $.ajax({
            type: "POST",
            url: "ajax.aspx/AddForm",
            data: "{'formName':'" + formName + "', 'sitecoreID':'" + sitecoreID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                var j = $.parseJSON(data.d);

                if (j[0].Name != null) {

                    colapseAllExcept(null);
                    $("#divPreviewFormInfo").show();
                    $("#divEditForm").show();

                    $('#ddlFormList')
                         .append($("<option></option>")
                         .attr("value", j[0].Form_ID)
                         .text(j[0].Name));

                    $('#ddlFormList').val(j[0].Form_ID)

                    $("#divFormName").text(j[0].Name);
                    $("#divSCID").text(j[0].ItemID);
                    $("#divDateCreated").text(j[0].ModificationDate);

                    $("#divTrackingCampaign").text(j[0].Tracking_Campaign);
                    $("#divTrackingForm").text(j[0].Tracking_Form);
                    $("#divTrackingSource").text(j[0].Tracking_Source);
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });

        //add new form to dropdown list (selected)
        //show data in labels


    });

    $("#ddlFormList").change(function () {
        if ($(this).attr("selectedIndex") != "0")
            alert("select > 0");
    });

    $("#btnLogOut").click(function () {
        colapseAllExcept("divlogIn");
    });

    $(function () {
        $("#pnlFields span").draggable({
            appendTo: "body",
            helper: "clone"
        });
        $("#testRight").droppable({
            drop: function (event, ui) {

                if (ui.draggable.attr("ctype") == "TextBox") {
                    $(this).append("<div class='set'><span>" + ui.draggable.text() + "</span><br /><input type='text' /><input type='button' class='remove' value='X' id='btn" + ui.draggable.attr('clID') + "'><input type='checkbox' id='chk" + ui.draggable.attr('clID') + "' class='doValidate' >validate</div>");

                }
                else if (ui.draggable.attr("ctype") == "DropDownList") {
                    $(this).append("<div class='set'><span>" + ui.draggable.text() + "</span><br /><select><option value='-1'>select</option><input type='button' id='btn" + ui.draggable.attr('clID') + "' class='remove' value='X'></select><input type='checkbox' id='chk" + ui.draggable.attr('clID') + "' class='doValidate' >validate</div>");

                }
                else if (ui.draggable.attr("ctype") == "asdf") {
                    $(this).append("<div class='set'><span>" + ui.draggable.text() + "</span><br /><select><option value='-1'>select</option></select><input type='button' class='remove' value='X'><input type='checkbox' class='doValidate' >validate</div>");
                }

                BindCRUDEvents($(this), ui.draggable.attr("clID"));

            }
        });
    });

});

function BindCRUDEvents(element, clID) {

    //Remove
    element.find("#btn" + clID).click(function () {

        //ajax call to add to database, then returned formControl_id, add attribute to input field
        fcID = $(this).closest("span").attr("fcID");

        $.ajax({
            type: "POST",
            url: "ajax.aspx/RemoveField",
            data: "{'formControlID':'" + fcID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                result = data;
            },
            error: function (msg) {
                alert(msg);
            }
        });

    });

    //Validate
    element.find("#chk" + clID).click(function () {

        //ajax call to add to database, then returned formControl_id, add attribute to input field

    });
}

function AddFieldToForm(formID, clID, phID) { 

}

function colapseAllExcept(elementID) {

    $("#divlogIn").hide();
    $("#divCreateOrEdit").hide();
    $("#divCreateForm").hide();
    $("#divPreviewFormInfo").hide();
    $("#divEditForm").hide();

    if (elementID != null || elementID != "")
        $("#" + elementID).show();
}

function ajaxCall(url, values) {

    var result = "";

    $.ajax({
        type: "POST",
        url: url,
        data: values,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            result = data;
        },
        error: function (msg) {
            alert(msg);
        }
    });

    return result;

}