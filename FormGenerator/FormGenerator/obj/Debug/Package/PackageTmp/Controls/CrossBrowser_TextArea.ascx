<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrossBrowser_TextArea.ascx.cs" Inherits="FormGenerator.Controls.CrossBrowser_TextArea" %>
<%--<fieldset id="leadForm" class="fieldsetNoBorder">
    <label class="input" for="<%= Id %>">
        <span class="labelSpan" id="TALabel" ><%= Placeholder %></span>
        <textarea name="<%= Id %>" id="<%= Id %>" title="<%= ToolTip %>" rows="" cols="" class="<%= CssClass %>" validate="<%= Validate %>" controlname="<%= ControlName %>" ></textarea>
    </label>
</fieldset>--%>

<fieldset id="leadForm" class="fieldsetNoBorder">
    <label class="input" for="<%= tbxCrossBrowser.ClientID %>">
        <span class="labelSpan"><%= Watermark %></span>
        <asp:TextBox ID="tbxCrossBrowser" runat="server" TextMode="MultiLine" ></asp:TextBox>
    </label>
</fieldset>