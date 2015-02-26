function browseToFileManagerServer() {
    //var popupWindow = $('#popupwindowBrowse', window.parent.document);
    //popupWindow.dialog({
    //    autoOpen: false,
    //    modal: true,
    //    show: "fade",
    //    title: "Information",
    //    height: 800,
    //    width: 600
    //});

    //$.ajax({
    //    type: 'POST',
    //    url: '/FileBrowser/Index',
    //    //dataType: 'html',
    //    cache: false,
    //    async: true,
    //    success: function (result) {
    //        popupWindow.html(result);
    //        popupWindow.dialog("open");
    //    }
    //});
    window.open('/FileBrowser/Index', '_blank');

    
    return true;
};