
async function myScript()
{    
    if($('#stop').hasClass('disabled'))
    {
        return false;
    }else {
        $(document).ready(function () {
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
                    sleep(300)
                    Start();
                }
            });
        })
    }
    
}

function Test(milliseconds) {

}

function sleep(milliseconds) {
    const date = Date.now();
    let currentDate = null;
    do {
        currentDate = Date.now();
    } while (currentDate - date < milliseconds);
}
async function Start()
{
    const btn = document.getElementById('stop');
    let checker = false;
    btn.onclick = () => checker = true;
    if (checker === true) {
        return false;
    }
    else {
        myScript();
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