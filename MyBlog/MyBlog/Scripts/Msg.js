function LoadMsg(msg) {
    $("#popupwindow").dialog({
        autoOpen: false,
        modal: true,
        show: "fade",
        title: "Information",
        buttons: {
            OK: function () { $(this).dialog("close"); }
        }
    });
    $('#popupwindow').html(msg);
    $("#popupwindow").dialog("open");
}
function LoadSecondaryMsg(msg) {
    $("#popupwindow2").dialog({
        autoOpen: false,
        modal: true,
        show: "fade",
        title: "Information",
        buttons: {
            OK: function () { $(this).dialog("close"); }
        }
    });
    $('#popupwindow2').html(msg);
    $("#popupwindow2").dialog("open");
}