<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FakeSitecorePage.aspx.cs" Inherits="FormGenerator.Admin2.FakeSitecorePage" %>
<%@ Register TagPrefix="fg" TagName="Admin" Src="FormGeneratorAdmin.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#btnAddEditForm").click(function () {
                $("#dialog").dialog();
            });

            $("#btnAddForm").click(function () {
                $("#divAddForm").show();
                $("#divEditForm").hide();
            });

            $("#btnEditForm").click(function () {
                $("#divEditForm").show();
                $("#divAddForm").hide();
            });

            $("#btnCreateForm").click(function () {
                $("#dialog").dialog("close");
                $("#divAdmin").dialog({ width: 1200, height: 800 });
            });

        });

    </script>
    <style type="text/css">
        
        .toolbox
        {
            border-right: 1px solid #cecece;
            width: 300px;
            height: 725px;
            position:relative;
            float: left;
        }
        
        .divPreview
        {
            float: left;
        }        
        
        #divControlList
        {
            
        }
        
        .properties
        {
            position:absolute; bottom:0;
        }        
        
        #ifPageView
        {
            border: 1px solid #000000;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div style="text-align: center" >
            <input id="btnAddEditForm" type="button" value="Add / Edit Form"  />
        </div>

        <fg:Admin runat="server" ID="admin"></fg:Admin>
    </form>
</body>
</html>
