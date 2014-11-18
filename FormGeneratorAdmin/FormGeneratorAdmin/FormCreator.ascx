<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormCreator.ascx.cs" Inherits="FormGeneratorAdmin.FormCreator" %>
<script type="text/javascript" src="http://api.demandbase.com/autocomplete/widget.js"></script>
<script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.js"></script>
<script type="text/javascript" src="http://code.jquery.com/ui/1.10.0/jquery-ui.js"></script>
<script type="text/javascript" src="js/FormCreator.js"></script>

<link href="http://code.jquery.com/ui/1.10.0/themes/base/jquery-ui.css" type="text/css" rel="stylesheet" />
<link href="css/FormCreator.css" type="text/css" rel="stylesheet" />

<div id="divlogIn">
    <div class="login">
        <span>Username</span><input type="text" id="txtUsername" /><br />
        <span>Password</span><input type="password" id="txtPassword" /><br />
        <input type="button" id="btnLogin" value="Submit" />
    </div>
</div>
<div id="divCreateOrEdit">
    <input type="button" id="btnCreateForm" value="Create Form" />
    <input type="button" id="btnEditForm" value="Edit Existing Form" />
    <input type="button" id="btnLogOut" value="Log Out" />
</div>
<div id="divCreateForm">
    <input type="hidden" id="hidFormID" />
    <span>Form ID</span><input type="text" id="txtFormID_edit" /><br />
    <span>Form Name</span><input type="text" id="txtFormName" /><br />
    <span>Sitecore ID</span><input type="text" id="txtSitecoreID" /><br />    
    <span>Tracking Campaign</span><input type="text" id="txtTrackingCampaign" /><br />
    <span>Tracking Form</span><input type="text" id="txtTrackingForm" /><br />    
    <span>Tracking Source</span><input type="text" id="txtTrackingSource" /><br />  
    <span>Aprimo ID</span><input type="text" id="txtAprimoID" /><br />  
    <span>Aprimo Subject</span><input type="text" id="txtAprimoSubject" /><br />  
    <span>Confirmation URL</span><input type="text" id="txtConfirmationURL" /><br />  

    <span>Header</span><input type="text" id="txtHeader" /><br />  
    <asp:DropDownList ID="ddlLayout" runat="server" ClientIDMode="Static">
    </asp:DropDownList><br />  
    <asp:DropDownList ID="ddlStyle" runat="server" ClientIDMode="Static">
    </asp:DropDownList><br />  
    <input type="button" id="btnContinue" value="Continue" />
    <input type="button" class="closeButton" value="Close" />
</div>
<div id="divPreviewFormInfo">
        <asp:DropDownList ID="ddlFormList" runat="server" ClientIDMode="Static">
        </asp:DropDownList><br />
        <span>Form ID</span><div id="divFormID"></div><br />
        <span>Form Name</span><div id="divFormName"></div><br />
        <span>Sitecore ID</span><div id="divSCID"></div><br />
        <span>Date Modified</span><div id="divDateCreated"></div><br />

        <span>Tracking Campaign</span><div id="divTrackingCampaign"></div><br />
        <span>Tracking Form</span><div id="divTrackingForm"></div><br />
        <span>Tracking Source</span><div id="divTrackingSource"></div><br />

        <span>Aprimo ID</span><div id="divAprimoID"></div><br />
        <span>Aprimo Subject</span><div id="divAprimoSubject"></div><br />
        <span>Confirmation URL</span><div id="divConfirmationURL"></div><br />

        <span>Header</span><div id="divHeader"></div><br />
        <span>Template</span><div id="divTemplate"></div><br />
        <span>Style</span><div id="divStyle"></div><br />

        <span>Item Path</span><div id="divItemPath"></div><br />
        <input type="button" id="btnEditFormInfo" value="Edit" />
        <input type="button" class="closeButton" value="Close" />
        <input type="button" class="collapse" value="Hide Form Settings" />
</div>
<div id="divHiddenPreview">
    <input type="button" class="collapse" value="Show Form Settings" />
</div>
<div id="divEditForm">
    <div id="divEditing">
        <div id="leftpane">
            <asp:Panel ID="pnlFields" runat="server" ClientIDMode="Static">
            </asp:Panel>
        </div>
        <div id="rightpane">
            <div id="divFormPreview">
                <%--<input type="button" id="btnSaveFormChanges" value="Save" />--%>
            </div>
            <!--testing drabble jquery-->

            <div id='hiddenModalForm' >
            </div>

            <!--end testing drabble jquery-->
        </div>
        
        <!--testing drabble jquery-->

    </div>
</div>

<!--edit modals-->
    <div id="mFormControlGroup">
        <input type="hidden" id="hidSetFC_ID" />
        <p>Checkbox Edit!</p>
        <div id="divAddFCG">
            Text<input type="text" id="txtFCGItemText" />
            Value<input type="text" id="txtFCGItemValue" />
            <input id="btnAddFCGItem" type="button" value="Add" />
        </div>
        <table id="tblFCGItems">
            <tr>
                <th>Text</th>
                <th>Value</th>
                <th>Remove</th>
            </tr>
        </table><br />
        <table id="customGroupInfo">
            <tr>
                <td>Text</td>
                <td><input type="text" id="txtCustomGroupLabel" /></td>
            </tr>
            <tr>
                <td>Aprimo Column Name</td>
                <td><input type="text" id="txtCustomGroupAprimoColumnName" /></td>
            </tr>
        </table>
        <input type="button" id="btnSaveCustomInfo"  value="Done" /><br />
    </div><!--end FCG -->
    <div id="mRadioGroup">
        <h1>Radio Edit!</h1>
        <br />
        <input type="button" class="closeDialog" value="Done" />
    </div><!--end Radio group-->
    <div id="mTermsAndConditions">
        <input type="hidden" id="hidTNCFC_ID" />
        <p>Terms and Conditions Edit!</p>
        URL&nbsp;<input type="text" id="txtTNCURL" />
        <br />
        <input type="button" id="btnSaveTNC" class="closeDialog" value="Done" />
    </div><!--end Terms and Conditions-->
    <div id="mSubmit">
        <input type="hidden" id="hidSetAction" />
        <p>Submit Edit!</p>
        Text&nbsp;<asp:TextBox ID="txtSubmitText" runat="server" ClientIDMode="Static"></asp:TextBox><br />
        <asp:DropDownList ID="ddlActions" runat="server" ClientIDMode="Static">
        </asp:DropDownList>
        <input id="btnAddAction" type="button" value="Add" /><br />
        <div id="divReturnURL">
            return URL
            <input type="text" id="ecasReturnURL" />
        </div>
        <table id="tblActions">
            <tr>
                <th>Action</th>
                <th>Remove</th>
            </tr>
        </table><br />
        <div id="divEmailDetails">
            <table id="tblEmailDetails" class="defaultTable">
                <tr>
                    <td>To</td>
                    <td><input id='txtTo' type='text' /></td>
                </tr>
                <tr>
                    <td>From</td>
                    <td><input id='txtFrom' type='text' /></td>
                </tr>
                <tr>
                    <td>Subject</td>
                    <td><input id='txtSubject' type='text' /></td>
                </tr>
                <tr>
                    <td>CC</td>
                    <td><input id='txtCC' type='text' /></td>
                </tr>
                <tr>
                    <td><input id='btnSaveCustomEmail' type='button' value='Save' /></td>
                    <td></td>
                </tr>
            </table>
        </div>
        <input type="button" class="closeDialog" value="Done" />
    </div><!--end checkbox group-->
    <div id="mCustomField">
        <input type="button" class="closeCustomField" value="Done" />
        <br />
        <br />
        <input type="hidden" id="hidCustomFC_ID" />

            Field<br />
            <asp:DropDownList ID="ddlControlTypes" runat="server" ClientIDMode="Static"></asp:DropDownList>
            &nbsp;&nbsp;<input type="checkbox" id="cbIsSpecial" />Special Field<br />
            <asp:DropDownList ID="ddlCustomControlFunctions" runat="server" ClientIDMode="Static"></asp:DropDownList>
            <br />
            <br />
        <div class="aprimoFields">
        <table>
            <tr>
                <td>
                    Label<br />
                    <asp:TextBox ID="txtLabelName" runat="server" ClientIDMode="Static"></asp:TextBox>
                </td>
                <td>
                    Aprimo Column<br />
                    <asp:TextBox ID="txtCustomAprimoColumn" runat="server" ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
        </table>
        </div>
        <br />
        <br />
        <div id="divCustomDLL">
            <table id="cf_TextValue">
                <tr>
                    <td>
                        Text<br />
                        <input type="text" id="txtCustomText" />
                    </td>
                    <td>
                        Value<br />
                        <input type="text" id="txtCustomValue" />
                    </td>
                    <td>
                        <br />
                        <input id="btnCustomAdd" type="button" value="Add" />
                    </td>
                </tr>
            </table>
            <br />            
            <table id="tblCustomDropdownOptions">
                <thead>
                    <tr>
                        <th>Text</th>
                        <th>Value</th>
                        <th>Remove</th>
                    </tr>
                </thead>
                <tbody class='sortableOption'>

                </tbody>
            </table>

        </div>
    </div><!--end edit custom field-->
<!--end edit modals-->