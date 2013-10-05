
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
        $("#hidFormID").val("");
    });

    $("#btnEditForm").click(function () {

        colapseAllExcept("divPreviewFormInfo");
    });

    $("#btnEditFormInfo").click(function () {

        $("#txtFormName").val($("#divFormName").text());
        $("#txtSitecoreID").val($("#divSCID").text());
        $("#txtDateCreated").val($("#divDateCreated").text());
        $("#txtTrackingCampaign").val($("#divTrackingCampaign").text());
        $("#txtTrackingForm").val($("#divTrackingForm").text());
        $("#txtTrackingSource").val($("#divTrackingSource").text());
        $("#hidFormID").val($("#ddlFormList").val());

        colapseAllExcept("divCreateForm");

    });

    $(".doValidate").click(function () {
        alert("checked!");
    });



    $("#btnAddFCGItem").click(function () {
        text = $("#txtFCGItemText").val();
        value = $("#txtFCGItemValue").val();
        fcid = $("#hidSetFC_ID").val();
        //fcgid = $(this).attr("fcgid");
        fcgid = '0';

        if (text != "" || value != "") {

            $.ajax({
                type: "POST",
                url: "ajax.aspx/AddFormControlGroupItem",
                data: "{'formControl_ID':'" + fcid + "', 'controlList_ID':'32','text':'" + text + "','value':'" + value + "','formControlGroup_ID':'" + fcgid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var j = $.parseJSON(data.d);
                    status = j[0].Status;
                    fcgID = j[0].FormControlGroup_ID;

                    if (status == "Added" || status == "Activated")
                        $("#tblFCGItems").append('<tr><td>' + text + '</td><td>' + value + '</td><td><input class="removeFCGItem" type="button" fcgID="' + fcgID + '" value="X" /></td></tr>');
                },
                error: function (msg) {

                    alert("Item save fail!");
                    return;
                }
            });

            FCGItemsRemoveInit();
        }
    });




    $("#btnAddAction").click(function () {
        text = $("#ddlActions option:selected").text();

        if ($("#ddlActions").val() != "-1") {

            if ($("#ddlActions").val() == "3") { //ecas stuff

                if ($("#ecasReturnURL").val() != "") {

                    $.ajax({
                        type: "POST",
                        url: "ajax.aspx/UpdateReturnURLByForm_ID",
                        data: "{'formID':'" + $("#ddlFormList").val() + "', 'returnURL':'" + $("#ecasReturnURL").val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {

                            $("#ecasReturnURL").val("");
                        },
                        error: function (msg) {

                            alert("URL save fail!");
                            return;
                        }
                    });
                }
                else {
                    alert("ECAS requires return Url.");
                    return;
                }
            }
            else {

            }

            fcid = $("#hidSetAction").val();

            $.ajax({
                type: "POST",
                url: "ajax.aspx/SetElementAction",
                data: "{'formControlID':'" + fcid + "', 'controlActionTypeID':'" + $("#ddlActions").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var j = $.parseJSON(data.d);
                    status = j[0].Status;
                    atid = j[0].ControlAction_ID;

                    if (status == "Added" || status == "Activated")
                        $("#tblActions").append('<tr><td>' + $("#ddlActions option:selected").text() + "</td><td><input class='removeAction' atid='" + atid + "' type='button' fcgID='' value='X' /></td></tr>");

                },
                error: function (msg) {

                    alert("action save fail!");
                    return;
                }
            });
        }

        ActionRemoveInit();
    });

    $("#ddlActions").change(function () {

        if ($(this).val() == "3") {
            $("#divReturnURL").show();
        }
        else {
            $("#divReturnURL").hide();
        }
    });


    $("#btnContinue").click(function () {

        formName = $("#txtFormName").val();
        sitecoreID = $("#txtSitecoreID").val();
        campaign = $("#txtTrackingCampaign").val();
        source = $("#txtTrackingSource").val();
        tform = $("#txtTrackingForm").val();

        $.ajax({
            type: "POST",
            url: "ajax.aspx/AddForm",
            data: "{'formName':'" + formName + "', 'sitecoreID':'" + sitecoreID + "', 'trackingCampaign':'" + campaign + "', 'trackingSource':'" + source + "', 'trackingForm':'" + tform + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                var j = $.parseJSON(data.d);

                if (j[0].Name != null) {

                    colapseAllExcept(null);
                    $("#divPreviewFormInfo").show();
                    $("#divEditForm").show();


                    if ($("#hidFormID").val() != "") {

                        $('#ddlFormList').val($("#hidFormID").val()) 

                    }
                    else {
                        $('#ddlFormList')
                         .append($("<option></option>")
                         .attr("value", j[0].Form_ID)
                         .text(j[0].Name));

                        $('#ddlFormList').val(j[0].Form_ID)                    
                    }


                    $("#divFormName").text(j[0].Name);
                    $("#divSCID").text(j[0].ItemID);
                    $("#divDateCreated").text(j[0].ModificationDate);

                    $("#divTrackingCampaign").text(j[0].Tracking_Campaign);
                    $("#divTrackingForm").text(j[0].Tracking_Form);
                    $("#divTrackingSource").text(j[0].Tracking_Source);

                    $("#txtFormName").val("");
                    $("#txtSitecoreID").val("");
                    $("#txtDateCreated").val("");
                    $("#txtTrackingCampaign").val("");
                    $("#txtTrackingForm").val("");
                    $("#txtTrackingSource").val("");
                    $("#hidFormID").val("");

                }
            },
            error: function (msg) {
                alert(msg);
            }
        });

        //add new form to dropdown list (selected)
        //show data in labels


    });

    DialogInit("mSubmit", "btnEdit");
    DialogInit("mFormControlGroup", "btnCBGroup");
    DialogInit("mRadioGroup", "btnRGroup");
    DialogInit("mCustomDropdown", "btnCustomDD");



    $("#ddlFormList").change(function () {
        if ($(this).attr("selectedIndex") != "0") {

            formID = $(this).val();

            $.ajax({
                type: "POST",
                url: "ajax.aspx/GetAllFormDataByFormID",
                data: "{'formID':'" + formID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var j = $.parseJSON(data.d);

                    if (j.formData[0].Name != null) {

                        colapseAllExcept(null);
                        $("#divPreviewFormInfo").show();
                        $("#divEditForm").show();

                        $('#ddlFormList').val(j.formData[0].Form_ID)

                        $("#divFormName").text(j.formData[0].Name);
                        $("#divSCID").text(j.formData[0].ItemID);
                        $("#divDateCreated").text(j.formData[0].ModificationDate);

                        $("#divTrackingCampaign").text(j.formData[0].Tracking_Campaign);
                        $("#divTrackingForm").text(j.formData[0].Tracking_Form);
                        $("#divTrackingSource").text(j.formData[0].Tracking_Source);

                        //render form
                        $("#hiddenModalForm").empty();

                        $("#hiddenModalForm").append(createHeader(j.formData[0].Header));

                        $("#hiddenModalForm").append("<div id='pnlContainer' class='clearfix'></div>");

                        $("#pnlContainer").append(createPlaceholders(j.formContainers));

                        $.each(j.formElements, function (key, element) {
                            $("#" + element.ContainerName).append(createSet(element));
                        });

                        //actions
                        $("#tblActions").empty();
                        $("#tblActions").append(createActions(j.elementActions));
                        $("#ecasReturnURL").val(j.formActions[0].ECASReturnURL);
                        ActionRemoveInit();


                        //formControlGroup
                        $("#tblFCGItems").empty();
                        $("#tblFCGItems").append(createFCGItems(j.formChildElements));
                        FCGItemsRemoveInit();


                        DragDropInit();

                        $(".remove").click(function () {

                            current = $(this);
                            fcid = current.closest("div").attr("fcid");

                            $.ajax({
                                type: "POST",
                                url: "ajax.aspx/RemoveElement",
                                data: "{'formControlID':'" + fcid + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    current.parent().remove();
                                },
                                error: function (msg) {
                                    alert('oops!');
                                }
                            });
                        });

                        $(".doValidate").click(function () {

                            current = $(this);
                            fcid = current.closest("div").attr("fcid");

                            if (current.is(':checked'))
                                checked = "true";
                            else
                                checked = "false";

                            $.ajax({
                                type: "POST",
                                url: "ajax.aspx/SetElementValidation",
                                data: "{'formControlID':'" + fcid + "','isChecked':'" + checked + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {

                                },
                                error: function (msg) {
                                    alert('oops!');
                                }
                            });
                        });

                        $(".btnEdit").click(function () {

                            clid = $(this).parent().attr("clid")
                            fcid = $(this).parent().attr("fcid")

                            if (clid == '13') {
                                $("#mSubmit").dialog("open");
                                $("#hidSetAction").val(fcid);
                            }
                            if (clid == '32') {
                                $("#mFormControlGroup").dialog("open");
                                $("#hidSetFC_ID").val(fcid);
                            }
                            $(".ui-widget-overlay").css("background", "#000");
                        });

                    }
                },
                error: function (msg) {
                    alert("failed.");
                }
            });

        }
    });

    $("#btnLogOut").click(function () {
        colapseAllExcept("divlogIn");
    });

});

function DragDropInit() {

    $(function () {
        $("#pnlFields span").draggable({
            appendTo: "body",
            helper: "clone"
        });
        $("#phColumn1, #phColumn2, #phBottom").droppable({
            drop: function (event, ui) {

                current = $(this);
                clID = ui.draggable.attr("clID");
                phID = current.attr("id");
                formID = $("#ddlFormList").val();
                fcid = "";

                if (ui.draggable[0].localName == 'span') {

                    $.ajax({
                        type: "POST",
                        url: "ajax.aspx/AddElementToContainer",
                        data: "{'controlList_ID':'" + clID + "','formID':'" + formID + "','placeholderName':'" + phID + "','formControl_ID':'','text':''}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            fcid = data;
                            current.attr("fcid", fcid);
                        },
                        error: function (msg) {
                            alert('oops!');
                        }
                    });

                    if (ui.draggable.attr("ctype") == "TextBox") {
                        $(this).append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span><input type='text' /><input type='button' class='remove' value='X'  ><input type='checkbox' id='chk" + ui.draggable.attr('clID') + "' class='doValidate' >validate</div>");

                        BindCRUDEvents($(this), ui.draggable.attr("clID"), null);
                    }
                    else if (ui.draggable.attr("ctype") == "DropDownList") {
                        $(this).append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span><select><option value='-1'>select</option></select><input type='button'  class='remove' value='X'><input type='checkbox' id='chk" + ui.draggable.attr('clID') + "' class='doValidate' >validate</div>");

                        BindCRUDEvents($(this), ui.draggable.attr("clID"), null);
                    }
                    else if (ui.draggable.attr("ctype") == "Group") {
                        $(this).append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span>Group of Checkboxes<input type='button'  class='remove' value='X'><input type='checkbox' id='chk" + ui.draggable.attr('clID') + "' class='doValidate' >validate<input type='button' class='btnCBGroup' value='Edit' id='btn_edit_" + ui.draggable.attr('clID') + "'></div>");

                        BindCRUDEvents($(this), ui.draggable.attr("clID"), "mCheckboxGroup");
                    }
                    else if (ui.draggable.attr("ctype") == "Submit") {
                        $(this).append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span><input type='button' value='Submit' /><input type='button' class='remove' value='X' ><input type='button' class='btnEdit' value='Edit' id='btn_edit_" + ui.draggable.attr('clID') + "'>");

                        BindCRUDEvents($(this), ui.draggable.attr("clID"), "mSubmit");
                    }


                } //end if

            }
        });

        $("#phColumn1, #phColumn2, #phBottom").sortable({
            stop: function (event, ui) {

                fcidCollection = "";

                $("div[class=set]").each(function () {
                    fcidCollection += $(this).attr("fcid") + ",";
                });

                fcidCollection = fcidCollection.substring(0, fcidCollection.length - 1);

                $.ajax({
                    type: "POST",
                    url: "ajax.aspx/SetFormElementOrder",
                    data: "{'delimitedIDs':'" + fcidCollection + "','delimiter':','}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                    },
                    error: function (msg) {
                        alert('sort fail.');
                    }
                });

            }
        });
    });

}

function BindCRUDEvents(element, clID, triggerModal) {

    //Remove
    //element.find("#btn_remove_" + clID).click(function () {
    element.find(".remove").click(function () {

        //ajax call to add to database, then returned formControl_id, add attribute to input field
        fcID = $(this).closest("div").attr("fcid");

        //        $.ajax({
        //            type: "POST",
        //            url: "ajax.aspx/RemoveElement",
        //            data: "{'formControlID':'" + fcID + "'}",
        //            contentType: "application/json; charset=utf-8",
        //            dataType: "json",
        //            success: function (data) {
        //                $(this).parent().remove();
        //            },
        //            error: function (msg) {
        //                alert(msg);
        //            }
        //        });

        $(this).parent().remove();
    });

    if (triggerModal != null) {
        element.find("#btn_edit_" + clID).click(function () {

            fcID = $(this).closest("span").attr("fcID");

            $("#" + triggerModal).dialog("open");
            $(".ui-widget-overlay").css("background", "#000");
        });
    }

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

function DialogInit(name, triggerClass) {

    //----------- Dialog Box Stuff
    var d = $("#" + name).dialog({
        autoOpen: false,
        height: 430,
        width: 790,
        modal: true
    });
    d.parent().find('a').find('span').attr('class', 'ui-icon ui-icon-minus');
    d.parent().draggable({
        containment: 'form',
        opacity: 0.70
    });
    $('.ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-draggable.ui-resizable').css("zIndex", 200);

    $('form').append(d.parent());

    $(".closeDialog").click(function () {
        $("#" + name).dialog('close');
    });
    //----------- End Dialog

}


function createSet(element)
{
    return "<div id='" + element.AprimoFieldName + "' class='set' fcid='" + element.FormControl_ID + "' clid='" + element.ControlList_ID + "'><span>" + element.LabelName + "</span>" + createElement(element) + "</div>";
}

function createElement(element) {

    type = element.ElementType;
    validate = "";

    if (element.Validate == "True")
        validate = "checked='true'";

    if (type == "TextBox")
        return "<input  type='text' /><input type='button' class='remove' value='X'><input type='checkbox' class='doValidate' " + validate + " >validate</div>";
    else if(type == "DropDownList")
        return  "<select ><option value='-1'>" + element.AprimoFieldName + "</option></select><input type='button' class='remove' value='X'  ><input type='checkbox' class='doValidate' " + validate + " >validate</div>";
    else if (type == "Submit")
        return "<input  type='button' value='Submit' /><input type='button' class='remove' value='X'  ><input type='button' class='btnEdit' value='Edit' ></div>";
    else if (type == "Group")
        return "<input  type='checkbox' />checkbox group<input type='button' class='remove' value='X'  ><input type='button' class='btnEdit' value='Edit' ></div>";

}

function createHeader(headerName) {

    return "<div id='headerDiv' class='header'><h2>" + headerName + "</h2><a id='bannerClose' class='closeDialog' >X</a></div>";
}

function createPlaceholders(placeholders) {

    //foreach placeholder, create divs
    var html = "";

    for (i = 0; i < placeholders.length; i++)
        html += "<div id='" + placeholders[i].ContainerID + "'></div>";

    return html;
}

function createActions(actions) {

    var html = "";

    for (i = 0; i < actions.length; i++)
        html += "<tr><td>" + actions[i].ActionName + "</td><td><input class='removeAction' type='button' atid='" + actions[i].ControlAction_ID + "' fcgid='" + actions[i].FormControl_ID + "' value='X'></td></tr>";

    return html;
}

function createFCGItems(items) {

    var html = "";

    for (i = 0; i < items.length; i++)
        html += "<tr><td>" + items[i].Text + "</td><td>" + items[i].Value + "</td><td><input class='removeFCGItem' type='button'  fcgid='" + items[i].FormControlGroup_ID + "' value='X'></td></tr>";

    return html;
}

function ActionRemoveInit() {

    $(".removeAction").click(function () {

        current = $(this);

        fcgid = current.attr("fcgid");

        $.ajax({
            type: "POST",
            url: "ajax.aspx/RemoveElementAction",
            data: "{'controlAction_ID':'" + fcgid + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                current.parent().closest('tr').remove();
            },
            error: function (msg) {
                alert('Couldnt remove action!');
            }
        });

    });
}

function FCGItemsRemoveInit() {

    $(".removeFCGItem").click(function () {

        current = $(this);
        fcgid = current.attr("fcgid");

        $.ajax({
            type: "POST",
            url: "ajax.aspx/RemoveFormControlGroupItem",
            data: "{'formControlGroup_ID':'" + fcgid + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                current.parent().closest('tr').remove();
            },
            error: function (msg) {
                alert('Couldnt remove item!');
            }
        });

    });
}