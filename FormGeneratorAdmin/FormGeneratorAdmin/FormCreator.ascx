<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormCreator.ascx.cs" Inherits="FormGeneratorAdmin.FormCreator" %>
<script type="text/javascript" src="http://api.demandbase.com/autocomplete/widget.js"></script>
<script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.js"></script>
<script type="text/javascript" src="http://code.jquery.com/ui/1.10.0/jquery-ui.js"></script>
<script type="text/javascript" src="FormCreator.js"></script>

<link href="http://code.jquery.com/ui/1.10.0/themes/base/jquery-ui.css" type="text/css" rel="stylesheet" />
<link href="FormCreator.css" type="text/css" rel="stylesheet" />

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
    <span>Form Name</span><input type="text" id="txtFormName" /><br />
    <span>Sitecore ID</span><input type="text" id="txtSitecoreID" /><br />    
    <span>Tracking Campaign</span><input type="text" id="txtTrackingCampaign" /><br />
    <span>Tracking Form</span><input type="text" id="txtTrackingForm" /><br />    
    <span>Tracking Source</span><input type="text" id="txtTrackingSource" /><br />  
    <input type="button" id="btnContinue" value="Continue" />
    <input type="button" class="closeButton" value="Close" />
</div>
<div id="divPreviewFormInfo">
    <asp:DropDownList ID="ddlFormList" runat="server" ClientIDMode="Static">
    </asp:DropDownList><br />
    <span>Form Name</span><div id="divFormName"></div><br />
    <span>Sitecore ID</span><div id="divSCID"></div><br />
    <span>Date Modified</span><div id="divDateCreated"></div><br />

    <span>Tracking Campaign</span><div id="divTrackingCampaign"></div><br />
    <span>Tracking Form</span><div id="divTrackingForm"></div><br />
    <span>Tracking Source</span><div id="divTrackingSource"></div><br />

    <span>Item Path</span><div id="divItemPath"></div><br />
    <input type="button" id="btnEditFormInfo" value="Edit" />
    <input type="button" class="closeButton" value="Close" />
</div>
<div id="divEditForm">
    <div id="divEditing">
        <div id="leftpane">
            <asp:Panel ID="pnlFields" runat="server" ClientIDMode="Static">
            </asp:Panel>
        </div>
        <div id="rightpane">
            <div id="divFormPreview">
                <asp:DropDownList ID="ddlLayout" runat="server" ClientIDMode="Static">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlStyle" runat="server" ClientIDMode="Static">
                </asp:DropDownList>
            </div>
            <!--testing drabble jquery-->
            <div id="testWrapper">
                <div id="droppable">
                </div>
                <div id="testRight">
                </div>
            </div>
            <!--end testing drabble jquery-->
        </div>
        
        <!--testing drabble jquery-->
        <input type="button" id="btnSaveFormChanges" value="Save" />
    </div>
</div>