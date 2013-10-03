
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

    $("#btnAddCheckbox").click(function () {
        text = $("#txtCBText").val();
        value = $("#txtCBValue").val();

        if (text != "" || value != "") {
            $("#tblCheckboxes").append('<tr><td>' + text + '</td><td>' + value + '</td><td><input class="removeCB" type="button" fcgID="" value="X" /></td></tr>');

            $(".removeCB").click(function () {
                $(this).parent().closest('tr').remove();
            });
        }
    });

    $("#btnAddAction").click(function () {
        text = $("#ddlActions option:selected").text();

        if ($("#ddlActions").val() != "-1") {

            if ($("#ddlActions").val() == "3") {

                if ($("#ecasReturnURL").val() != "") {
                    $("#tblActions").append('<tr><td>' + text + '</td><td><input class="removeAction" type="button" fcgID="" value="X" /></td></tr>');
                    $("#ecasReturnURL").val("");
                }
                else {
                    alert("ECAS requires return Url.");
                }
            }
            else {
                $("#tblActions").append('<tr><td>' + text + '</td><td><input class="removeAction" type="button" fcgID="" value="X" /></td></tr>');
            }
        }

        $(".removeAction").click(function () {
            $(this).parent().closest('tr').remove();
        });
    });

    $("#ddlActions").change(function () {

        if ($(this).val() == "3") {
            $("#divReturnURL").show();
        }
        else {
            $("#divReturnURL").hide();
        }
    });

    $("#btnAddAction").click(function () {
        text = $("#txtCBText").val();
        value = $("#txtCBValue").val();

        if (text != "" || value != "") {
            $("#tblCheckboxes").append('<tr><td>' + text + '</td><td>' + value + '</td><td><input class="removeCB" type="button" fcgID="" value="X" /></td></tr>');

            $(".removeCB").click(function () {
                $(this).parent().closest('tr').remove();
            });
        }
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

    DialogInit("mSubmit", "btnEdit");
    DialogInit("mCheckboxGroup", "btnCBGroup");
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

                            if(clid == '13')
                            {
                                $("#mSubmit").dialog("open");
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

                $.ajax({
                    type: "POST",
                    url: "ajax.aspx/AddElementToContainer",
                    data: "{'controlList_ID':'" + clID + "','formID':'" + formID + "','placeholderName':'" + phID + "','formControl_ID':'','text':''}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        current.attr("fcid", data);
                    },
                    error: function (msg) {
                        alert('oops!');
                    }
                });

                if (clID != null) {

                    if (ui.draggable.attr("ctype") == "TextBox") {
                        $(this).append("<div class='set'><span>" + ui.draggable.text() + "</span><input type='text' /><input type='button' class='remove' value='X'  ><input type='checkbox' id='chk" + ui.draggable.attr('clID') + "' class='doValidate' >validate</div>");

                        BindCRUDEvents($(this), ui.draggable.attr("clID"), null);
                    }
                    else if (ui.draggable.attr("ctype") == "DropDownList") {
                        $(this).append("<div class='set'><span>" + ui.draggable.text() + "</span><select><option value='-1'>select</option></select><input type='button'  class='remove' value='X'><input type='checkbox' id='chk" + ui.draggable.attr('clID') + "' class='doValidate' >validate</div>");

                        BindCRUDEvents($(this), ui.draggable.attr("clID"), null);
                    }
                    else if (ui.draggable.attr("ctype") == "Group") {
                        $(this).append("<div class='set'><span>" + ui.draggable.text() + "</span>Group of Checkboxes<input type='button'  class='remove' value='X'><input type='checkbox' id='chk" + ui.draggable.attr('clID') + "' class='doValidate' >validate<input type='button' class='btnCBGroup' value='Edit' id='btn_edit_" + ui.draggable.attr('clID') + "'></div>");

                        BindCRUDEvents($(this), ui.draggable.attr("clID"), "mCheckboxGroup");
                    }
                    else if (ui.draggable.attr("ctype") == "Submit") {
                        $(this).append("<div class='set'><span>" + ui.draggable.text() + "</span><input type='button' value='Submit' /><input type='button' class='remove' value='X' ><input type='button' class='btnEdit' value='Edit' id='btn_edit_" + ui.draggable.attr('clID') + "'>");

                        BindCRUDEvents($(this), ui.draggable.attr("clID"), "mSubmit");
                    }

                } //end if
            }
        });

        $("#phColumn1, #phColumn2, #phBottom").sortable();
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
