
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
        $("#txtHeader").val($("#divHeader").text());

        $("#txtAprimoID").val($("#divAprimoID").text());
        $("#txtAprimoSubject").val($("#divAprimoSubject").text());

        $("#ddlLayout option").filter(function () {
            return $(this).text() == $("#divTemplate").text();
        }).prop('selected', true);

        $("#ddlStyle option").filter(function () {
            return $(this).text() == $("#divStyle").text();
        }).prop('selected', true);

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
                data: "{'formControl_ID':'" + fcid + "','text':'" + text + "','value':'" + value + "','formControlGroup_ID':'" + fcgid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var j = $.parseJSON(data.d);
                    status = j[0].Status;
                    fcgID = j[0].FormControlGroup_ID;

                    if (status == "Added" || status == "Activated")
                        $("#tblFCGItems").append('<tr><td>' + text + '</td><td>' + value + '</td><td><input class="removeFCGItem" type="button" fcgID="' + fcgID + '" value="X" /></td></tr>');

                    FCGItemsRemoveInit();
                },
                error: function (msg) {

                    alert("Item save fail!");
                    return;
                }
            });

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

                    ActionRemoveInit();
                },
                error: function (msg) {

                    alert("action save fail!");
                    return;
                }
            });

            ActionRemoveInit();
        }
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

        aprimoID = $("#txtAprimoID").val();
        aprimoSubject = $("#txtAprimoSubject").val();

        header = $("#txtHeader").val();
        templateID = $("#ddlLayout").val();
        styleID = $("#ddlStyle").val();

        $.ajax({
            type: "POST",
            url: "ajax.aspx/AddForm",
            data: "{'formName':'" + formName + "', 'sitecoreID':'" + sitecoreID + "', 'trackingCampaign':'" + campaign + "', 'trackingSource':'" + source + "', 'trackingForm':'" + tform + "', 'header':'" + header + "', 'templateID':'" + templateID + "', 'styleID':'" + styleID + "', 'aprimoID':'" + aprimoID + "', 'aprimoSubject':'" + aprimoSubject + "'}",
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
                    
                    $("#divAprimoID").text(j[0].Aprimo_ID);
                    $("#divAprimoSubject").text(j[0].Aprimo_Subject);

                    $("#divHeader").text(j[0].Header);
                    $("#divTemplate").text(j[0].TemplateName);
                    $("#divStyle").text(j[0].StyleName);


                    $("#txtFormName").val("");
                    $("#txtSitecoreID").val("");
                    $("#txtDateCreated").val("");
                    $("#txtTrackingCampaign").val("");
                    $("#txtTrackingForm").val("");
                    $("#txtTrackingSource").val("");
                    $("#txtAprimoID").val("");
                    $("#txtAprimoSubject").val("");
                    $("#hidFormID").val("");
                    $("#txtHeader").val("");
                    $("#ddlLayout").val("-1");
                    $("#ddlStyle").val("-1");

                    $("#ddlFormList").trigger("change");
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
                url: "ajax.aspx/GetAllFormDataByFormID_Pagination",
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

                        $("#divAprimoID").text(j.formData[0].Aprimo_ID);
                        $("#divAprimoSubject").text(j.formData[0].Aprimo_Subject);

                        $("#divHeader").text(j.formData[0].Header);
                        $("#divTemplate").text(j.formData[0].TemplateName);
                        $("#divStyle").text(j.formData[0].StyleName);

                        //render form
                        $("#hiddenModalForm").empty();

                        $("#hiddenModalForm").append(createHeader(j.formData[0].Header));

                        $("#hiddenModalForm").append("<div id='pnlContainer' class='clearfix'></div>");

                        $("#pnlContainer").append(createPages(j.formPages));

                        $.each(j.formContainers, function (key, element) {
                            $("#" + element.PageName).append(createPlaceholder(element));
                        });

                        $.each(j.formElements, function (key, element) {
                            $("#" + element.PageName).find("#" + element.ContainerName).append(createSet(element));
                        });

                        //actions
                        $("#tblActions").empty();
                        $("#tblActions").append(createActions(j.elementActions));
                        $("#ecasReturnURL").val(j.formActions[0].ECASReturnURL);
                        ActionRemoveInit();

                        //Group
                        $("input[hk=CBtrigger]").click(function () {

                            fcid = $(this).parent().attr("fcid");

                            $.ajax({
                                type: "POST",
                                url: "ajax.aspx/GetAllFormControlGroupItemsByFormControl_ID",
                                data: "{'formControl_ID':'" + fcid + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {

                                    var j = $.parseJSON(data.d);

                                    $("#tblFCGItems").empty();
                                    $("#tblFCGItems").append(createFCGItems(j));
                                    FCGItemsRemoveInit();
                                },
                                error: function (msg) {
                                    alert('load fail!');
                                }
                            });

                            $.ajax({
                                type: "POST",
                                url: "ajax.aspx/GetCustomGroupInfoByFormControl_ID",
                                data: "{'formControl_ID':'" + fcid + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    
                                    var j = $.parseJSON(data.d);

                                    $('#txtCustomGroupLabel').val(j[0].CustomLabel);
                                    $('#txtCustomGroupAprimoColumnName').val(j[0].AprimoColumn);
                                },
                                error: function (msg) {
                                    alert('Group Info load fail!');
                                }
                            });
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
                            fcid = $(this).parent().attr("fcid")

                            if (clid == '13') {
                                $("#mSubmit").dialog("open");
                                $("#hidSetAction").val(fcid);
                            }
                            if (clid == '32' || clid == '1034' || clid == '1036') {
                                $("#mFormControlGroup").dialog("open");
                                $("#hidSetFC_ID").val(fcid);
                            }
                            $(".ui-widget-overlay").css("background", "#000");
                        });

                        $("#btnSaveCustomInfo").click(function () {

                            current = $(this);
                            formControlID = current.parent().attr("fcgid");
                            customLabel = $('#txtCustomGroupLabel').val();
                            customAprimoColumn = $('#txtCustomGroupAprimoColumnName').val();

                            $.ajax({
                                type: "POST",
                                url: "ajax.aspx/SaveCustomGroupInfo",
                                data: "{'formControl_ID':'" + fcid + "','customLabel':'" + customLabel + "','aprimoColumn':'" + customAprimoColumn + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {
                                    $("#mFormControlGroup").dialog('close');
                                    $('#txtCustomGroupLabel').val('');
                                    $('#txtCustomGroupAprimoColumnName').val('');
                                    $('#txtFCGItemText').val('');
                                    $('#txtFCGItemValue').val('');
                                },
                                error: function (msg) {
                                    alert('Couldnt save info!');
                                }
                            });

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
                pageName = current.parent().attr("id");
                formID = $("#ddlFormList").val();
                label = ui.draggable.html();
                fcid = "";

                alert("clID = " + clID + ", phID = " + phID + ", pageName = " + pageName + ", formID = " + formID + ", label = " + label );

                if (!DoesFieldExist(label)) {

                    if (ui.draggable[0].localName == 'span') {

                        $.ajax({
                            type: "POST",
                            url: "ajax.aspx/AddControlToPagePlaceHolder",
                            data: "{'controlList_ID':'" + clID + "','formID':'" + formID + "','placeholderName':'" + phID + "','formControl_ID':'','text':'','pageName':'" + pageName + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                alert("success. fcid = " + data.d);
                                fcid = data.d;
                                current.attr("fcid", fcid);

                                if (ui.draggable.attr("ctype") == "TextBox") {
                                    current.append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span><input type='text' /><input type='button' class='remove' value='X'  ><input type='checkbox' id='chk" + clID + "' class='doValidate' checked >validate</div>");

                                    BindCRUDEvents(current, ui.draggable.attr("clID"), null);
                                }
                                if (ui.draggable.attr("ctype") == "Multi-line") {
                                    current.append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span><textarea rows='6' cols='25'></textarea><input type='button' class='remove' value='X'  ><input type='checkbox' id='chk" + clID + "' class='doValidate' checked >validate</div>");

                                    BindCRUDEvents(current, ui.draggable.attr("clID"), null);
                                }
                                else if (ui.draggable.attr("ctype") == "DropDownList") {
                                    current.append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span><select><option value='-1'>select</option></select><input type='button'  class='remove' value='X'><input type='checkbox' id='chk" + clID + "' class='doValidate' checked >validate</div>");

                                    BindCRUDEvents(current, ui.draggable.attr("clID"), null);
                                }
                                else if (ui.draggable.attr("ctype") == "Group") {
                                    current.append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span>" + label + "<input type='button'  class='remove' value='X'><input type='checkbox' id='chk" + clID + "' class='doValidate' checked >validate<input type='button' class='btnCBGroup' value='Edit' id='btn_edit_" + ui.draggable.attr('clID') + "'></div>");

                                    BindCRUDEvents(current, ui.draggable.attr("clID"), "mFormControlGroup");
                                }
                                else if (ui.draggable.attr("ctype") == "CheckboxList") {
                                    current.append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span><div>" + label + "</div><input type='button'  class='remove' value='X'><input type='checkbox' id='chk" + clID + "' class='doValidate' checked >validate</div>");

                                    BindCRUDEvents(current, ui.draggable.attr("clID"), null);
                                }
                                else if (ui.draggable.attr("ctype") == "Submit") {
                                    current.append("<div class='set' fcid='" + fcid + "'><span>" + ui.draggable.text() + "</span><input type='button' value='Submit' /><input type='button' class='remove' value='X' ><input type='button' class='btnEdit' value='Edit' id='btn_edit_" + clID + "'>");

                                    BindCRUDEvents(current, ui.draggable.attr("clID"), "mSubmit");
                                }
                            },
                            error: function (msg) {
                                alert('oops!');
                            }
                        });

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

    element.find(".remove").click(function () {

        current = $(this);
        //ajax call to add to database, then returned formControl_id, add attribute to input field
        fcID = $(this).parent().attr("fcid");

        $.ajax({
            type: "POST",
            url: "ajax.aspx/RemoveElement",
            data: "{'formControlID':'" + fcID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                current.parent().remove();
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    if (triggerModal != null) {
        element.find("#btn_edit_" + clID).click(function () {
            fcID = $(this).parent().attr("fcid");

            if (triggerModal == 'mSubmit')
                $("#hidSetAction").val(fcID);
            else if (triggerModal == 'mFormControlGroup')
                $("#hidSetFC_ID").val(fcID);
            
            $("#" + triggerModal).dialog("open");
            $(".ui-widget-overlay").css("background", "#000");
        });
    }

    //Validate
    element.find("#chk" + clID).click(function () {

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
                alert('validation save fail!');
            }
        });

    });
}

function AddFieldToForm(formID, clID, phID) {

}

function DoesFieldExist(labelName) {

    ret = false;
    $("div[class=set]").each(function () {
        setLabel = $(this).find("span").html();

        if (setLabel == labelName) {
            ret = true;
            return false;
        }
    });

    return ret;

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
    else if (type == "Multi-line")
        return "<textarea rows='6' cols='25'></textarea><input type='button' class='remove' value='X'><input type='checkbox' class='doValidate' " + validate + " >validate</div>";
    else if (type == "DropDownList")
        return "<select ><option value='-1'>" + element.AprimoFieldName + "</option></select><input type='button' class='remove' value='X'  ><input type='checkbox' class='doValidate' " + validate + " >validate</div>";
    else if (type == "Submit") 
        return "<input  type='button' value='Submit' /><input type='button' class='remove' value='X'  ><input type='button' class='btnEdit' value='Edit' ></div>";
    else if (type == "Group") 
        return element.ControlName + "<input type='button' class='remove' value='X'  ><input type='checkbox' class='doValidate' " + validate + " >validate<input type='button' class='btnEdit' value='Edit' fcid='" + element.FormControl_ID + "' hk='CBtrigger' >";
    else if (type == "CheckboxList") 
        return element.ControlName + "&nbsp;<input type='button' class='remove' value='X'  ><input type='checkbox' class='doValidate' " + validate + " >validate";
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

function createPlaceholder(element) {
    return "<div id='" + element.ContainerID + "'></div>";
}

function createPages(pages) {

    //foreach placeholder, create divs
    var html = "";

    for (i = 0; i < pages.length; i++)
        html += "<div id='" + pages[i].Name + "'></div>";

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

        atid = current.attr("atid");

        $.ajax({
            type: "POST",
            url: "ajax.aspx/RemoveElementAction",
            data: "{'controlAction_ID':'" + atid + "'}",
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

