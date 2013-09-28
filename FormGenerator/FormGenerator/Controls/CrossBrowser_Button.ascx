<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrossBrowser_Button.ascx.cs" Inherits="FormGenerator.Controls.CrossBrowser_Button" %>
<asp:Button ID="txtCrossBrowserSubmit" runat="server" OnClick="ButtonClicked" data-getw_action="Submit Form" data-getw_category="esri.forms"  />
<span id="lblErrorMessage" class="fgErrorMessage"><%= ErrorMessage %></span>
<asp:Label ID="lblSuccessMessage" runat="server" CssClass="fgSuccessMessage" Visible="false" ClientIDMode="Static"></asp:Label>