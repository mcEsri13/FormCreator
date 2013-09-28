<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/Default.Master" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="FormGenerator.Demo" %>
<%@ Register Src="FormRenderer.ascx" TagName="FormRenderer" TagPrefix="esri" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <esri:FormRenderer ID="ifFormRenderer" ItemID="999999999" runat="server" />
</asp:Content>

