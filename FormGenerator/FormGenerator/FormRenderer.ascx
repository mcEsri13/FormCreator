<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormRenderer.ascx.cs" Inherits="FormGenerator.FormRenderer" %>

<div id="fgForm">
    <iframe  runat="server" class="formRendererCSS" id="ifPageView" marginheight="0" frameborder="0" scrolling="no"></iframe>
</div>

<script type="text/javascript">
    $(".formRendererCSS").load(function () {
        var newFrameHeight = parseInt($(this).contents().find("body").height()) + 50;
        $(this).attr("height", newFrameHeight + "px");
    });
</script>