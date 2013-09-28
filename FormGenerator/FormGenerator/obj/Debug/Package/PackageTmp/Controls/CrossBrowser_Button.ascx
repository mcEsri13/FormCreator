<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrossBrowser_Button.ascx.cs" Inherits="FormGenerator.Controls.CrossBrowser_Button" %>
<asp:Button ID="txtCrossBrowserSubmit" runat="server" OnClick="ButtonClicked"  />
<span id="lblErrorMessage" class="fgErrorMessage"><%= ErrorMessage %></span>
<%--<span id="lblSuccessMessage" class="fgSuccessMessage"><%= SuccessMessage%></span>--%>
<%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="fgErrorMessage" Visible="false" ClientIDMode="Static"></asp:Label>--%>
<asp:Label ID="lblSuccessMessage" runat="server" CssClass="fgSuccessMessage" Visible="false" ClientIDMode="Static"></asp:Label>