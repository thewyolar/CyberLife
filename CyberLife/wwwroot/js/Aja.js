function ajaxUpdate()
{
    return $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "Start",
            dataType: "html",
            async: false,
            success: function (result) {
                $("#gr").html(result);
            },
            error: function (err) {
                $("#gr").val("Error while uploading data: \n\n" + err);
            },
            complete: function (){
                return true;
            }
        });
    })
}

let timerId;

function start()
{
    timerId = setInterval(ajaxUpdate, 300);
}

function stop()
{
    clearInterval(timerId);
    // timerId = null;
}

function restart()
{
    stop();
    $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "Restart",
            dataType: "html",
            success: function (result) {
                $("#gr").html(result);
            },
            error: function (err) {
                $("#gr").val("Error while uploading data: \n\n" + err);
            }
        });
    })
}