
function myScript()
{
    for (i = 0; i < 1; i++){
        $(document).ready(function () {
            $.ajax({
                type: "GET",
                url: "Start",
                dataType: "html",
                success: function (result) {
                    // document.open();
                    // document.write(result);
                    // document.close();
                    $("#gr").html(result);
                },
                error: function (err) {
                    $("#gr").val("Error while uploading data: \n\n" + err);
                }
            });
        })
    }
   
}

function neMyScript()
{
    for (i = 0; i < 100; i++){
        $(document).ready(function () {
            $.ajax({
                type: "GET",
                url: "Restart",
                dataType: "html",
                success: function (result) {
                    // document.open();
                    // document.write(result);
                    // document.close();
                    $("#gr").html(result);
                },
                error: function (err) {
                    $("#gr").val("Error while uploading data: \n\n" + err);
                }
            });
        })
    }

}