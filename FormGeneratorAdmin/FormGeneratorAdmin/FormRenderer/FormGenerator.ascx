<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormGenerator.ascx.cs" Inherits="Esri.Web.FormGenerator.FormGenerator" %>

<%--Referencing Stylesheet Class--%>
<link rel="stylesheet" type="text/css" href="http://www.esri.com/styles/includes/fat-footer.css" />
<link rel="stylesheet" type="text/css" href="http://www.esri.com/styles/includes/main.css" />
<link rel="stylesheet" type="text/css" href="http://www.esri.com/components/responsive/css/products_responsive.css"/>
<link rel="stylesheet" type="text/css" href="http://www.esri.com/common/location-analytics/styles.css"/>
<link rel="stylesheet" href="http://www.esri.com/styles/esri-colors.css" />
<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
<link rel="stylesheet" href="http://webapps-cdn.esri.com/CDN/forms/form-generator/css/form-renderer.css" />
<style>
        .floatfix
        {
            float:right;
        }
        .floatfix:hover
        {
            float:right;
        }
</style>

<%--Referencing Javascript Files--%>
<%--    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.0/jquery-ui.js"></script>
    <script type="text/javascript" src="/FormRenderer/Utilities/Renderer/FormGenerator.js"></script>
    <script src="http://webapps-cdn.esri.com/CDN/forms/location-analytics/js/LA_Form_General.js" type="text/javascript"></script>--%>

<%--Main Form Section--%>
<div id="hiddenModalForm" class="ui-dialog-content ui-widget-content" style="display: block; width: auto; min-height: 0px; max-height: none; height: 428px;">
    <%--Header Section--%>
    <div id="headerDiv" class="header">
        <h2><asp:Literal ID="litHeader" runat="server"></asp:Literal></h2>
    </div><!--end headerDiv-->
    
    <%--Error Message Section--%>
    <div class="errorMessage">
        <span id="errorMessageText">Please complete the highlighted item(s) to continue.</span>
    </div>
    
    <%--Dynamic Content Section--%>
    <asp:Panel ID="pnlContainer" runat="server" ClientIDMode="Static">
        <%--Dynamic content loaded here..--%>
    </asp:Panel>
    
    <%--Confirmation Section--%>
    <div id="confirmation">
        <iframe id="ifConfirmation" class="clsIframeConfirmation" src=""></iframe>
        <a id="confirmationClose" class="closeDialog">X</a>
    </div>
</div>