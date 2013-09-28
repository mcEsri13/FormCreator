<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrossBrowser_TextBox.ascx.cs" Inherits="FormGenerator.Controls.CrossBrowser_TextBox" %>
<fieldset id="leadForm" class="fieldsetNoBorder">
    <label class="input" for="<%= tbxCrossBrowser.ClientID %>">
        <span class="labelSpan"><%= Watermark %></span>
        <asp:TextBox ID="tbxCrossBrowser" runat="server"  maxlength="100" ></asp:TextBox>
    </label>
</fieldset>