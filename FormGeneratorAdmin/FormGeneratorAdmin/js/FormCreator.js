
$(document).ready(function () {
    //Textboxes
    var txtFormName         = $("#txtFormName");
    var txtSitecoreID       = $("#txtSitecoreID");
    var txtDateCreated      = $("#txtDateCreated");
    var txtTrackingCampaign = $("#txtTrackingCampaign");
    var txtTrackingForm     = $("#txtTrackingForm");
    var txtTrackingSource   = $("#txtTrackingSource");
    var txtHeader           = $("#txtHeader");
    var txtAprimoID         = $("#txtAprimoID");
    var txtAprimoSubject    = $("#txtAprimoSubject");
    var txtUsername         = $("#txtUsername");
    var txtPassword         = $("#txtPassword");

    //Labels
    var divFormName         = $("#divFormName");
    var divSCID             = $("#divSCID");
    var divDateCreated      = $("#divDateCreated");
    var divTrackingCampaign = $("#divTrackingCampaign");
    var divTrackingForm     = $("#divTrackingForm");
    var divTrackingSource   = $("#divTrackingSource");
    var divAprimoID         = $("#divAprimoID");
    var divAprimoSubject    = $("#divAprimoSubject");
    var divHeader           = $("#divHeader");
    var divTemplate         = $("#divTemplate");
    var divStyle            = $("#divStyle");

    //Modal
    var hiddenModalForm     = $("#hiddenModalForm");

    //divs
    var divlogIn            = $("#divlogIn");
    var divCreateOrEdit     = $("#divCreateOrEdit");
    var divCreateForm       = $("#divCreateForm");
    var divPreviewFormInfo  = $("#divPreviewFormInfo");
    var divEditForm         = $("#divEditForm");

    //hidden



    $('.collapse').click(function () {
        if ($('#divPreviewFormInfo').css('display') == 'none') {
            $('#divPreviewFormInfo').show();
            $('#divHiddenPreview').hide();
        }
        else {
            $('#divPreviewFormInfo').hide();
            $('#divHiddenPreview').show();
        }
    });

    $("#btnLogin").click(function () {

        var username = txtUsername.val();
        var password = txtPassword.val();
        
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

    $(".closeCustomField").click(function () {

        var fcid = $("#hidCustomFC_ID").val();
        var customLabel = $("#txtLabelName").val();
        var customControlType = $("#ddlControlTypes").val();
        var aprimoColumn = $("#txtCustomAprimoColumn").val();
        var isSpecial = $("#cbIsSpecial").is(":checked");

        $.ajax({
            type: "POST",
            url: "ajax.aspx/SaveCustomFieldInfo",
            data: "{'formControl_ID':'" + fcid + "','customLabel':'" + customLabel + "','customControlType':'" + customControlType + "','aprimoColumn':'" + aprimoColumn + "','isSpecial':'" + String(isSpecial) + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                $("#mCustomField").dialog('close');

                $("#hidCustomFC_ID").val("");
                $("#txtLabelName").val("");
                $("#ddlControlTypes").val("-1");
                $("#txtCustomAprimoColumn").val("");

                $("#tblCustomDropdownOptions").empty();

                $("#divCustomDLL").hide();

                $("#ddlFormList").trigger("change");
            },
            error: function (msg) {

                alert("Couldnt save custom field info!");
                return;
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

        txtFormName.val(divFormName.text());
        txtSitecoreID.val(divSCID.text());
        txtDateCreated.val(divDateCreated.text());
        txtTrackingCampaign.val(divTrackingCampaign.text());
        txtTrackingForm.val(divTrackingForm.text());
        txtTrackingSource.val(divTrackingSource.text());
        txtHeader.val(divHeader.text());

        $txtAprimoID.val(divAprimoID.text());
        txtAprimoSubject.val(divAprimoSubject.text());

        $("#ddlLayout option").filter(function () {
            return $(this).text() == divTemplate.text();
        }).prop('selected', true);

        $("#ddlStyle option").filter(function () {
            return $(this).text() == divStyle.text();
        }).prop('selected', true);

        $("#hidFormID").val($("#ddlFormList").val());

        colapseAllExcept("divCreateForm");

    });

    $(".doValidate").click(function () {
        alert("checked!");
    });

    $("#btnAddFCGItem").click(function () {
        var text = $("#txtFCGItemText").val();
        var value = $("#txtFCGItemValue").val();
        var fcid = $("#hidSetFC_ID").val();
        var fcgid = '0';
        
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

    $("#btnCustomAdd").click(function () {
        var text = htmlEncode($("#txtCustomText").val());
        var value = htmlEncode($("#txtCustomValue").val());
        var fcid = $("#hidCustomFC_ID").val();

        if (text != "" || value != "") {

            $.ajax({
                type: "POST",
                url: "ajax.aspx/AddElementOption",
                data: "{'formControl_ID':'" + fcid + "','text':'" + text + "','value':'" + value + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    
                    var j = $.parseJSON(data.d);
                    status = j[0].Status;
                    coID = j[0].ControlOption_ID;

                    if (status == "Added" || status == "Activated")
                        $("#tblCustomDropdownOptions").append('<tr><td>' + text + '</td><td>' + value + '</td><td><input class="removeElementOption" type="button" coID="' + coID + '" value="X" /></td></tr>');

                    ElementOptionsRemoveInit();

                    $("#txtCustomText").val('');
                    $("#txtCustomValue").val('');
                    
                },
                error: function (msg) {

                    alert("Option save fail!");
                    return;
                }
            });

        }
    });

    $("#btnAddAction").click(function () {
        var text = $("#ddlActions option:selected").text();

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

    $("#ddlControlTypes").change(function () {
        var selected = $(this).val();
        if (selected == "3" || selected == "1012" || selected == "1015") {
            $("#divCustomDLL").show();
        }
        else {
            $("#divCustomDLL").hide();
        }
    });

    $("#btnContinue").click(function () {

        var formName = htmlEncode(txtFormName.val());
        var sitecoreID = htmlEncode(txtSitecoreID.val());
        var campaign = htmlEncode(txtTrackingCampaign.val());
        var source = htmlEncode(txtTrackingSource.val());
        var tform = htmlEncode(txtTrackingForm.val());

        var aprimoID = htmlEncode($txtAprimoID.val());
        var aprimoSubject = htmlEncode(txtAprimoSubject.val());

        var header = htmlEncode(txtHeader.val());
        var templateID = $("#ddlLayout").val();
        var styleID = $("#ddlStyle").val();

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


                    divFormName.text(j[0].Name);
                    divSCID.text(j[0].ItemID);
                    divDateCreated.text(j[0].ModificationDate);

                    divTrackingCampaign.text(j[0].Tracking_Campaign);
                    divTrackingForm.text(j[0].Tracking_Form);
                    divTrackingSource.text(j[0].Tracking_Source);
                    
                    divAprimoID.text(j[0].Aprimo_ID);
                    divAprimoSubject.text(j[0].Aprimo_Subject);

                    divHeader.text(j[0].Header);
                    divTemplate.text(j[0].TemplateName);
                    divStyle.text(j[0].StyleName);


                    txtFormName.val("");
                    txtSitecoreID.val("");
                    txtDateCreated.val("");
                    txtTrackingCampaign.val("");
                    txtTrackingForm.val("");
                    txtTrackingSource.val("");
                    $txtAprimoID.val("");
                    txtAprimoSubject.val("");
                    $("#hidFormID").val("");
                    txtHeader.val("");
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
    DialogInit("mCustomField", "btnCustomField");

    $('.ui-dialog-titlebar').hide();


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

                        divFormName.text(htmlDecode(j.formData[0].Name));
                        divSCID.text(htmlDecode(j.formData[0].ItemID));
                        divDateCreated.text(htmlDecode(j.formData[0].ModificationDate));

                        divTrackingCampaign.text(htmlDecode(j.formData[0].Tracking_Campaign));
                        divTrackingForm.text(htmlDecode(j.formData[0].Tracking_Form));
                        divTrackingSource.text(htmlDecode(j.formData[0].Tracking_Source));

                        divAprimoID.text(htmlDecode(j.formData[0].Aprimo_ID));
                        divAprimoSubject.text(htmlDecode(j.formData[0].Aprimo_Subject));

                        divHeader.text(htmlDecode(j.formData[0].Header));
                        divTemplate.text(j.formData[0].TemplateName);
                        divStyle.text(j.formData[0].StyleName);

                        //render form
                        hiddenModalForm.empty();

                        hiddenModalForm.append(createHeader(j.formData[0].Header));

                        hiddenModalForm.append("<div id='pnlContainer' class='clearfix'></div>");

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

                        //HACK: Find a better place for this...
                        $("#btnEditActionParams").click(function () {

                            var caid = $(this).attr('data-caid')

                            $.ajax({
                                type: "POST",
                                url: "ajax.aspx/GetControlActionParametersByControlAction_ID",
                                data: "{'ControlAction_ID':'" + caid + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {

                                    var j = $.parseJSON(data.d);


                                    for (i = 0; i < j.ActionParams.length; i++) {

                                        var name = j.ActionParams[i].Name;
                                        var val = j.ActionParams[i].Value;

                                        switch (name)
                                        {
                                            case 'To':
                                                $('#txtTo').val(val);
                                                break;
                                            case 'From':
                                                $('#txtFrom').val(val);
                                                break;
                                            case 'Subject':
                                                $('#txtSubject').val(val);
                                                break;
                                            case 'CC':
                                                $('#txtCC').val(val);
                                                break;
                                        }

                                    }

                                },
                                error: function (msg) {
                                    alert('could not load data!');
                                }
                            });

                            $("#divEmailDetails").show();
                        });

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

                        $("input[hk=Customtrigger]").click(function () {

                            fcid = $(this).attr("fcid");
                            $.ajax({
                                type: "POST",
                                url: "ajax.aspx/GetCustomFieldInfo",
                                data: "{'formControl_ID':'" + fcid + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {

                                    var j = $.parseJSON(data.d);

                                    var typeID = j.formElements[0].CustomControlType;
                                    var label = j.formElements[0].CustomLabel;
                                    var aprimoColumn = j.formElements[0].AprimoColumn;
                                    var isSpecial = j.formElements[0].IsSpecial;

                                    $('#ddlControlTypes').val(typeID);
                                    $('#txtLabelName').val(label);
                                    $('#txtCustomAprimoColumn').val(aprimoColumn);

                                    if(isSpecial == true)
                                        $('#cbIsSpecial').attr('checked', true);
                                    else
                                        $('#cbIsSpecial').attr('checked', false);

                                    $("#tblCustomDropdownOptions").empty();
                                    $("#tblCustomDropdownOptions").append(createElementOptions(j.formControlOptions));

                                    //$('#mCustomField').css('height', 'auto');

                                    ElementOptionsRemoveInit();

                                    if (typeID == '3' || typeID == '1012' || typeID == '1015') {
                                        $("#divCustomDLL").show();
                                    }
                                    else
                                        $("#divCustomDLL").hide();

                                },
                                error: function (msg) {
                                    alert('Custom Field load fail!');
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
                                    alert('failed to remove element!');
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
                                    alert('failed to set element validation!');
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
                            if (clid == '20') {
                                $("#mCustomField").dialog("open");
                                $("#hidCustomFC_ID").val(fcid);
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
                            customLabel = htmlEncode($('#txtCustomGroupLabel').val());
                            customAprimoColumn = htmlEncode($('#txtCustomGroupAprimoColumnName').val());

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

                        $('.tab-order').css('width', '20px');  //TODO: find out why CSS isn't working
                        $('.tab-order').blur(function () {
                            var fcid = $(this).parents().eq(4).attr('fcid');
                            var tabOrder = isInt($(this).val());
                            
                            $.ajax({
                                type: "POST",
                                url: "ajax.aspx/SetTabOrder",
                                data: "{'formControlID':'" + fcid + "','tabOrder':'" + tabOrder + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (data) {

                                },
                                error: function (msg) {
                                    alert('failed to save tabs');
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

    $("#btnSaveCustomEmail").click(function () {

        var to = $('#txtTo').val();
        var from = $('#txtFrom').val();
        var subject = $('#txtSubject').val();
        var cc = $('#txtCC').val();
        //var caType = $("#ddlActions").val();
        var caType = '6';
        var fcid = $("#hidSetAction").val();
        var delim = 'To|' + to + '^From|' + from + '^Subject|' + subject + '^CC|' + cc ;


        $.ajax({
            type: "POST",
            url: "ajax.aspx/SaveMultipleActionParamsByID",
            data: "{FormControl_ID:'" + fcid + "',ControlActionType_ID:'" + caType + "',DelimData:'" + delim + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

            },
            error: function (msg) {
                alert('failed to save email details');
            }
        });

        $("#divEmailDetails").hide();
        $('#txtTo').val('');
        $('#txtFrom').val('');
        $('#txtSubject').val('');
        $('#txtCC').val('');
    });
       
});

function DragDropInit() {

    $(function () {
        $(".draggable").draggable({
            appendTo: "body",
            helper: "clone",
            //connectToSortable: '.sortable',       //Breaks everyting
            revert: 'invalid'
        });
        
        $(".form-draggable").draggable({
            appendTo: "body",
            helper: "clone",
            revert: 'invalid'
        });
        $(".sortable").droppable({
            drop: function (event, ui) {

                draggedObject = ui.draggable;
                current = $(this);
                clID = draggedObject.attr("clID");

                var fcid;
                if(draggedObject.attr("fcid") == null)
                    fcid = "";
                else
                    fcid = draggedObject.attr("fcid");

                phID = current.attr("id");
                pageName = current.parent().attr("id");
                formID = $("#ddlFormList").val();
                label = draggedObject.html();

                //alert("clID = " + clID + ", phID = " + phID + ", pageName = " + pageName + ", formID = " + formID + ", label = " + label + ", formControl_ID = " + fcid);


                //if (!DoesFieldExist(label)) {

                    if (ui.draggable[0].localName == 'span') {

                        $.ajax({
                            type: "POST",
                            url: "ajax.aspx/AddControlToPagePlaceHolder",
                            data: "{'controlList_ID':'" + clID + "','formID':'" + formID + "','placeholderName':'" + phID + "','formControl_ID':'" + fcid + "','text':'','pageName':'" + pageName + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                $("#ddlFormList").trigger("change");
                            },
                            error: function (msg) {
                                alert('add control fail!');
                            }
                        });

                    //}



                } //end if

            }
        });

        $(".sortable").sortable({
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
        //height: 600,
        width: 850,
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
    return "<div id='" + element.AprimoFieldName + "' class='set' fcid='" + element.FormControl_ID + "' clid='" + element.ControlList_ID + "'><span clid='" + element.ControlList_ID + "' fcid='" + element.FormControl_ID + "'>" + element.LabelName + "</span>" + createElement(element) + "</div>";  //class='draggable'
}

function createElement(element) {

    var tabOrder = "";
    type = element.ElementType;
    clID = element.ControlList_ID;
    controlName = element.ControlName;

    if (element.tabOrder != 'undefined')
        tabOrder = element.TabOrder;

    validate = "";
    var ret = "";

    if (element.Validate == "True")
        validate = "checked='true'";
    
    if (clID == 20) //Custom Field
        ret = "<input  type='text' /><input type='button' class='btnEdit' value='Edit' fcid='" + element.FormControl_ID + "' hk='Customtrigger' />";
    else if (type == "TextBox" && clID != 20) {
        ret = "<input  type='text' />";
    }
    else if (type == "Multi-line" && clID != 20)
        ret = "<textarea rows='6' cols='25'></textarea>";
    else if (type == "DropDownList" && clID != 20)
        ret = "<select ><option value='-1'>" + element.AprimoFieldName + "</option></select>";
    else if (type == "Submit" && clID != 20)
        ret = "<input  type='button' value='Submit' /><input type='button' class='btnEdit' value='Edit' >";
    else if (type == "Group" && clID != 20)
        ret = element.ControlName + "<input type='button' class='btnEdit' value='Edit' fcid='" + element.FormControl_ID + "' hk='CBtrigger' />";
    else if (type == "CheckboxList" && clID != 20)
        ret = element.ControlName + "CheckboxList";

    ret += "<table>" +
	            "<tr>" +
		            "<td><input type='button' class='remove' value='X'></td>" +
		            "<td><input type='checkbox' class='doValidate' " + validate + " >validate</td>" +
		            "<td>tab order<input type='text' class='tab-order' value='" + tabOrder + "' /></td>" +
	            "</tr>" +
            "</table>";

    return ret;
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
    return "<div id='" + element.ContainerID + "' class='sortable'></div>";
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

    for (i = 0; i < actions.length; i++) {
        html += "<tr><td>" + actions[i].ActionName + "</td><td><input class='removeAction' type='button' atid='" + actions[i].ControlAction_ID + "' fcgid='" + actions[i].FormControl_ID + "' value='X'>"

        if (actions[i].ActionName == "Send Data via Email") {
            html += "<input type='button' value='Edit' id='btnEditActionParams' data-caid='" + actions[i].ControlAction_ID + "'>";
        }
    }

    html += "</td></tr>";

    return html;
}

function createFCGItems(items) {

    var html = "";

    for (i = 0; i < items.length; i++)
        html += "<tr><td>" + items[i].Text + "</td><td>" + items[i].Value + "</td><td><input class='removeFCGItem' type='button'  fcgid='" + items[i].FormControlGroup_ID + "' value='X'></td></tr>";

    return html;
}

function createElementOptions(items) {

    var html = "";

    for (i = 0; i < items.length; i++)
        html += "<tr><td>" + items[i].Text + "</td><td>" + items[i].Value + "</td><td><input class='removeElementOption' type='button'  coID='" + items[i].ControlOption_ID + "' value='X'></td></tr>";

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

function ElementOptionsRemoveInit() {

    $(".removeElementOption").click(function () {

        current = $(this);
        coID = current.attr("coID");

        $.ajax({
            type: "POST",
            url: "ajax.aspx/RemoveElementOption",
            data: "{'controlOption_ID':'" + coID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                current.parent().closest('tr').remove();
            },
            error: function (msg) {
                alert('Couldnt remove option!');
            }
        });

    });
}

function htmlEncode(str)
{
    var str = String(str).replace("'", "&#039;");

    return str;
}

function htmlDecode(str) {
    var str = String(str).replace("&#039;", "'");

    return str;
}

function isInt(val)
{
    var intRegex = /^\d+$/;
    if (intRegex.test(val))
        return val;
    else
        return 0;
}