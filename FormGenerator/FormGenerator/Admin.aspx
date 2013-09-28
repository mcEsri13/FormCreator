<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="FormGenerator.Admin" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.0/themes/base/jquery-ui.css" />
    <link href="css/Admin.css" rel="stylesheet" type="text/css" />


    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.0/jquery-ui.js"></script>
    <script src="http://webapps-cdn.esri.com/tools/GoogleEventsTrackingWidget/include.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/nicEdit.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#sortable").sortable({
                stop: function () {

                    delimControlIDs = '';

                    $('input[id*="hidPageControlID"]').each(function () {
                        delimControlIDs += $(this).val() + '|';
                    });

                    $.ajax({
                        type: "POST",
                        url: "Admin.aspx/AJAX_SaveControlOrder",
                        data: "{'delimPageControl_IDs':'" + delimControlIDs + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            $('.formViewCSS').attr("src", $('.formViewCSS').attr("src"));
                        },
                        error: function (msg) {
                            alert(msg);
                        }
                    });
                }
            });

            $("#sortableOptions").sortable({
                stop: function () {

                    delimControlOptionIDs = '';

                    $('input[id*="hidControlOptionID"]').each(function () {
                        delimControlOptionIDs += $(this).val() + '|';
                    });

                    $.ajax({
                        type: "POST",
                        url: "Admin.aspx/SetControlOptionOrder",
                        data: "{'delimPageControl_IDs':'" + delimControlOptionIDs + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            $('.formViewCSS').attr("src", $('.formViewCSS').attr("src"));
                        },
                        error: function (msg) {
                            alert(msg);
                        }
                    });
                }
            });

            $("#sortable").disableSelection();

            bkLib.onDomLoaded(function () { nicEditors.allTextAreas() });


        });

        function OkToAdd() {

            isOK = true;
//            var controlListID = $("#ddlControlList").find(":selected").val();

//            if (controlListID == '-1' || $("#txtFieldName").val() == '' || $("#txtWatermark").val() == '') {
//                alert('select control, set field name and watermark.');
//                isOK = false;
//            }

            return isOK;
        }

        function IsValidFormName() {

            var string = document.getElementById("txtPageName").value;
            var str = new String(string);
            arr = str.split("-");

            if (arr.length == 3)
                return true;
            else {
                alert("Required form name format:\n\n[Campaign Name]-[Date]-[AVWORLD Username]");
                return false;
            }
        }

        function ConfirmDelete() {

            if (confirm('Are you sure?'))
                return true;
            else
                return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hidItemID" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="LeftSide">
        <asp:Panel ID="pnlLogin" runat="server" >
            <table>
                <tr>
                    <td><span>Username</span></td>
                    <td><asp:TextBox ID="txtUserName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><span>Password</span></td>
                    <td><asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnLogIn" runat="server" Text="Log In" style="float:right;" 
                            onclick="btnLogIn_Click" /></td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlAdmin" runat="server" Visible="false">
            <div id="header">
                <h1>Form Generator</h1>
            </div>
            <asp:Button ID="btnCreatNew" runat="server" Text="Create New Form" 
                onclick="btnCreatNew_Click" />
            <asp:Button ID="btnEditExisting" runat="server" Text="Edit Existing Form" 
                onclick="btnEditExisting_Click" />
            <asp:Button ID="btnLogout" runat="server" Text="Log Out" 
                onclick="btnLogout_Click"  />
            <br />
            <br />
            <asp:Panel ID="pnlPageInfoDisplay" runat="server" Visible="false">

            <table class="tblFormInfo">
                <thead>
                    <tr>
                        <th colspan="2">Form Info</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Sitecore ID</td>
                        <td><asp:Label ID="lblSCID" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Name</td>
                        <td><asp:Label ID="lblName" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Path</td>
                        <td><asp:Label ID="lblPath" runat="server"></asp:Label></td>
                    </tr>
                </tbody>
            </table>

            </asp:Panel>
            <br />
            <br />
            <asp:Panel ID="pnlCreatePage" runat="server" Visible="false">
                <div class="LeftSide">
                    <asp:TextBox ID="txtItemID" runat="server" placeholder="Sitecore Item ID" ToolTip="Sitecore Item ID"></asp:TextBox><br />
                    <asp:TextBox ID="txtPageName" runat="server" placeholder="Form Name" ToolTip="example: [Campaign Name]-[Date]-[AVWORLD Username]" ClientIDMode="Static"></asp:TextBox><br />
                    <asp:Button ID="btnAddPage" runat="server" Text="Add Form" OnClientClick="return IsValidFormName();" 
                        onclick="btnAddPage_Click" />
                </div>
                <div class="RightSide">
                    <asp:Image ID="imgTemplatePreview" runat="server"  />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlChoosePage" runat="server" Visible = "false">
                Form Name&nbsp;<asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="true" onselectedindexchanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Button ID="btnRemovePage" runat="server" Text="Remove" 
                    onclick="btnRemovePage_Click" Visible="false" OnClientClick="return ConfirmDelete();" />
                <asp:Button ID="btnSaveFormInfo" runat="server" Text="Save"  Visible="false" 
                    onclick="btnSaveFormInfo_Click" />
                <br />
                <asp:Panel ID="pnlPageInfo" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td>Style</td>
                            <td><asp:DropDownList ID="ddlStyles" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Campaign</td>
                            <td><asp:TextBox ID="txtCampaign" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Source</td>
                            <td><asp:TextBox ID="txtSource" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Page</td>
                            <td><asp:TextBox ID="txtPage" runat="server"></asp:TextBox></td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
            </asp:Panel>
            <asp:Panel ID="pnlSetPageFields" runat="server" Visible="false">
                <asp:Repeater ID="rptrPageControls" runat="server" 
                    onitemcommand="rptrPageControls_ItemCommand">
                <HeaderTemplate>
                    <ul id="sortable">
                </HeaderTemplate>
                <ItemTemplate>
                    <li class="ui-state-default">
                        <span class="ui-icon ui-icon-arrowthick-2-n-s"></span>
                        <%#DataBinder.Eval(Container.DataItem, "Watermark")%>
                        <div class="gridButtons">
                                <asp:CheckBox ID="chkRequired" runat="server" AutoPostBack="true" OnCheckedChanged="chkRequired_CheckChanged"  
                                                    PCID='<%#DataBinder.Eval(Container.DataItem,"PageControl_ID")%>'  
                                                    Visible='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"NotSubmit"))%>'
                                                    Text="Required"
                                                    Checked='<%#DataBinder.Eval(Container.DataItem,"IsRequired")%>'   />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" Enabled='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsEditable"))%>' CommandName="edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"PageControl_ID")%>' isEditable='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsEditable"))%>' />
                            <asp:Button ID="btnDelete" runat="server" Text="Remove" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"PageControl_ID")%>' />
                        </div>
                        <asp:HiddenField ID="hidPageControlID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"PageControl_ID")%>' />
                        <asp:HiddenField ID="hidOrderID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"Placeholder_Order_ID")%>' />
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
                </asp:Repeater>
                <br />
                <asp:Label Visible="false" ID="lblNoData" runat="server" Text="No fields assigned. Please add fields by selecting and adding them below."></asp:Label>
                <br />
                <asp:Panel ID="pnlControlList" runat="server" Visible="false">
                    <asp:DropDownList ID="ddlControlList" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="ddlControlList_SelectedIndexChanged1">
                    </asp:DropDownList>
                    <asp:Button ID="btnAdd" runat="server" Text="Add Field" OnClientClick="return OkToAdd()"  onclick="btnAdd_Click" />
                    <br />
                    <br />
                    <asp:HiddenField ID="hfControlListID_ForEdit" runat="server" />
                    <asp:HiddenField ID="hfPageControlID_ForEdit" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAddSubmitButton" runat="server" Visible="false">
                    <br />
                    Button Text&nbsp;<asp:TextBox ID="txtButtonText" runat="server"></asp:TextBox>
                    <asp:Button ID="btnUpdateButtonText" runat="server" Text="Update" 
                        onclick="btnUpdateButtonText_Click" />
                    <br />
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlAddEditTextBox" runat="server" Visible="false">
                </asp:Panel>
                <asp:Panel ID="pnlAddEditContentBlock" runat="server" Visible="false">
                    <asp:HiddenField ID="hfPageControlSetting_ID" runat="server" />
                    <asp:TextBox ID="txtRTE" TextMode="MultiLine" runat="server" CssClass="RTE"></asp:TextBox>
                    <asp:Button ID="btnUpdateRTE" runat="server" Text="Update" 
                        onclick="btnUpdateRTE_Click" />
                    <asp:Button ID="btnRTECancel" runat="server" Text="Done" 
                        onclick="btnRTECancel_Click" />
                </asp:Panel>
                <asp:Panel ID="pnlAddEditDropdown" runat="server" Visible="false">
                    <asp:TextBox ID="txtDefaultOption" placeholder="Default" runat="server"></asp:TextBox><br />
                    <asp:TextBox ID="txtText" placeholder="Text" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtValue" placeholder="Value" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSaveOption" runat="server" Text="Add" 
                        onclick="btnSaveOption_Click" />
                    <asp:Button ID="btnCancelOptions" runat="server" Text="Done" onclick="btnCancelOptions_Click" />
                    <asp:HiddenField ID="hfControlOptionID_ForEdit" runat="server" />

                    <asp:HiddenField ID="hfPageControlID_DD_ForEdit" runat="server" /><br />
                    <asp:Repeater ID="rptrOptions" runat="server" 
                        onitemcommand="rptrOptions_ItemCommand" >
                    <HeaderTemplate>
                        <table id="sortableOptions" class="tableStyle">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#DataBinder.Eval(Container.DataItem, "Value")%>,&nbsp;<%#DataBinder.Eval(Container.DataItem, "Text")%></td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="Remove" CssClass="delete" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ControlOption_ID")%>' />
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="edit" CommandName="edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ControlOption_ID")%>' />
                                <asp:HiddenField ID="hidOptionValue" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"Value")%>' />
                                <asp:HiddenField ID="hidControlOption_ID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ControlOption_ID")%>' />
                                <asp:HiddenField ID="hidOptionOrderID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"Option_Order_ID")%>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="alternating">
                            <td><%#DataBinder.Eval(Container.DataItem, "Value")%>,&nbsp;<%#DataBinder.Eval(Container.DataItem, "Text")%></td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="Remove" CssClass="delete" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ControlOption_ID")%>' />
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="edit" CommandName="edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ControlOption_ID")%>' />
                                <asp:HiddenField ID="hidOptionValue" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"Value")%>' />
                                <asp:HiddenField ID="hidControlOption_ID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ControlOption_ID")%>' />
                                <asp:HiddenField ID="hidOptionOrderID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"Option_Order_ID")%>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                        <asp:Label ID="lblEmptyMessageDropdown" runat="server" Text="No options attached to this control." Visible="false"></asp:Label>
                    </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
                <asp:Panel ID="pnlEditActions" runat="server" Visible="false">
                    <asp:DropDownList ID="ddlActions" runat="server" 
                        onselectedindexchanged="ddlActions_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Button ID="btnAddAction" runat="server" Text="Add" onclick="btnAddAction_Click" />
                    
                    <asp:HiddenField ID="hfPageControlID_Action_ForEdit" runat="server" /><br />
                    <asp:Repeater ID="rptrActions" runat="server" onitemcommand="rptrActions_ItemCommand" >
                    <HeaderTemplate>
                        <table id="sortableActions" class="tableStyle">
                    </HeaderTemplate>
                    <ItemTemplate>
		                    <tr>
                    	        <td><%#DataBinder.Eval(Container.DataItem, "Name")%></td>
			                    <td>
				                    <asp:Button ID="btnDelete" runat="server" Text="Remove" CssClass="delete" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ControlAction_ID")%>' />

                                    <asp:HiddenField ID="hidControlAction_ID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ControlAction_ID")%>' />
                                    <asp:HiddenField ID="hidPageControl_ID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"PageControl_ID")%>' />
                    	        </td>
		                    </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
		                    <tr class="alternating">
                    	        <td><%#DataBinder.Eval(Container.DataItem, "Name")%></td>
			                    <td>
				                    <asp:Button ID="btnDelete" runat="server" Text="Remove" CssClass="delete" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ControlAction_ID")%>' />

                                    <asp:HiddenField ID="hidControlAction_ID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ControlAction_ID")%>' />
                                    <asp:HiddenField ID="hidPageControl_ID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"PageControl_ID")%>' />
                    	        </td>
		                    </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                        <asp:Label ID="lblEmptyMessageDropdown" runat="server" Text="No actions selected." Visible="false"></asp:Label>
                    </FooterTemplate>
                    </asp:Repeater>
                    <br />
                    <asp:Panel ID="pnlReturnURL" runat="server" Visible="true">
                        Return URL&nbsp;<asp:TextBox ID="tbxReturnURL" runat="server" Height="30px" Width="500px"></asp:TextBox>
<%--                        <asp:Button ID="btnUpdate" runat="server" Text="Update" 
                            onclick="btnUpdate_Click" />--%>
                    </asp:Panel>
                    <asp:Panel ID="pnlAprimoInfo" runat="server" Visible="true">
                        <p>Aprimo Info</p>
                            <asp:TextBox ID="txtAprimoSubject" runat="server" placeholder="Subject"></asp:TextBox><br />
                            <asp:TextBox ID="txtAprimoID" runat="server" placeholder="ID"></asp:TextBox>
<%--                            <asp:Button ID="btnUpdateAprimoInfo" runat="server" Text="Save" 
                            onclick="btnUpdateAprimoInfo_Click" />--%>
                        <br />
                        <br />
                    </asp:Panel>
                    <br />
                    <asp:Button ID="btnCancelAction" runat="server" Text="Done" onclick="btnCancelAction_Click" />
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>         
    </div>
    <asp:Panel ID="pnlRightSide" runat="server" CssClass="RightSide" Visible="false">
        <asp:PlaceHolder ID="phRightSide" runat="server"></asp:PlaceHolder>
    </asp:Panel>
    </form>
</body>
</html>
