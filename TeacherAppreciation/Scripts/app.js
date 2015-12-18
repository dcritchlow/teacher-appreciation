(function (teacher) {

    // The global jQuery object is passed as a parameter
    teacher(window.jQuery, window, document);

}(function ($, window, document) {

    $(function () {

        var form = $("#form");
        var loadFileButton = $("#loadFileButton");
        var inputFile = $('#inputFile');
        var alert = $("#alert");

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
    });

    function loadFile(file)
    {
        var data = { inputFile: file }
        
    }

}));