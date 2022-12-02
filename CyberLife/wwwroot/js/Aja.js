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

let timerId = null;
let speedAjax = 70;

function start()
{
    $("#speed").html(speedAjax);
    if(timerId == null){
        timerId = setInterval(ajaxUpdate, speedAjax);
    }
}

function plusSpeed()
{
    stop();
    speedAjax -= 10;
    $("#speed").html(speedAjax);
}

function minusSpeed()
{
    stop();
    speedAjax += 10;
    $("#speed").html(speedAjax);
}

function stop()
{
    clearInterval(timerId);
    timerId = null;
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