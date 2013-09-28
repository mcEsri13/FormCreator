<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrossBrowser_Dropdown.ascx.cs" Inherits="FormGenerator.Controls.CrossBrowser_Dropdown" %>
<fieldset id="leadForm" class="fieldsetNoBorder">
    <label class="input" for="<%= Id %>">
        <span class="labelSpan"><%= Placeholder %></span>
        <input type="text" name="<%= Id %>" id="<%= Id %>" maxlength="100" title="<%= ToolTip %>" class="<%= CssClass %>" validate="<%= Validate %>" controlname="<%= ControlName %>" />
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
    </label>
</fieldset>