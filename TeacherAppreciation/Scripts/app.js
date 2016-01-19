(function (teacher) {

    // The global jQuery object is passed as a parameter
    teacher(window.jQuery, window, document);

}(function ($, window, document) {
    var path = window.location.pathname;

    $(function () {
        console.log(path);
        var form = $("#form");
        var loadFileButton = $("#loadFileButton");
        var inputFile = $('#inputFile');
        var alert = $("#alert");
        var exportToExcel = $("#exportToExcel");

        loadFileButton.on({
            "click": function(e) {
                e.preventDefault();
                var file = inputFile[0].files[0];
                if (file === undefined) {
                    alert.removeClass("hidden");
                } else {
                    console.log(file);
                    form.submit();
                }
                
            }
        });

        inputFile.on({
            "click": function() {
                alert.addClass("hidden");
            }
        });

        exportToExcel.on({
            "click": function () {
                var url = "ExportToExcel";
                $('<iframe id="iframe" src= "' + url + '"></iframe>').appendTo('body').hide();
            }
        });
    });
    
}));