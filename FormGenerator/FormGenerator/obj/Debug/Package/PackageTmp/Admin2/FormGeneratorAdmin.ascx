<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormGeneratorAdmin.ascx.cs" Inherits="FormGenerator.Admin2.FormGeneratorAdmin" %>


<div id="dialog" title="Form Generator" style="display:none">

  <p>What would you like to Do?</p>

  <input id="btnAddForm" type="button" value="Add Form" />
  &nbsp;
  <input id="btnEditForm" type="button" value="Edit Form" />

  <div id="divAddForm" style="display:none">
  <p>Add Form</p>
      <input id="txtFormName" type="text" placeholder="Form Name" /><br />
      <input id="txtFormID" type="text" placeholder="Item ID" /><br />
      <asp:DropDownList ID="ddlTemplates" runat="server">
      </asp:DropDownList>
      <br />
      <br />
      <input id="btnCreateForm" type="button" value="Create Form" />
  </div>

  <div id="divEditForm" style="display:none">
    <p>Edit Form</p>
  </div>


</div>

<div id="divAdmin" title="Form Generator" style="display:none">
    <div id="divToolBox" class="toolbox">
        <div id="divControlList" class="controlList">
            <p>Fields</p>
            <input id="Text1" type="text" placeholder="Date" /><br />
            <input id="Text2" type="text" placeholder="First Name" /><br />
            <input id="Text3" type="text" placeholder="Last Name" /><br />
            <select id="Select1">
                <option>States</option>
            </select><br />
            <textarea id="TextArea1" cols="20" rows="2" placeholder="Comments"></textarea><br />
        </div>
        <div id="divProperties" class="properties">
            <p>Properties</p>
            <input id="txtCssClass" type="text" placeholder="Css Class" /><br />
            <input id="chkValidate" type="checkbox"  />&nbsp;Validate<br />
            <input id="txtWaterMark" type="text" placeholder="Watermark"  /><br />
            <input id="btnOptions" type="button" value="Options"  />            
        </div>
    </div>
    <div id="divPreview" class="preview">
        <iframe id="ifPageView" runat="server" ></iframe>
    </div>
</div>