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

function getSizeAndScaleInt(isUp){
    let gr;
    gr = document.getElementById("gr");
    let gridTemplateColumns = gr.style.gridTemplateColumns;
    let sizeAndScale = gridTemplateColumns.match(/\d+/g);
    let sizeAndScaleInt = sizeAndScale.map(Number);
    if(sizeAndScaleInt[1] <= 14 && isUp === 0){
        sizeAndScaleInt[1] = -1;
        return sizeAndScaleInt;
    }
    if(sizeAndScaleInt[1] >= 90 && isUp === 1){
        sizeAndScaleInt[0] = -1;
        return sizeAndScaleInt;
    }
    return sizeAndScaleInt;
}

function moveStatistics(){
    let h = document.getElementById('gr').offsetHeight;
    let ss = document.styleSheets[0];
    let rules = ss.cssRules || ss.rules;
    let h1Rule = null;
    for (let i = 0; i < rules.length; i++) {
        let rule = rules[i];
        if (/(^|,) *\#statistics *(,|$)/i.test(rule.selectorText)) {
            h1Rule = rule;
            break;
        }
    }
    h1Rule.style.marginLeft= h;
}

function loadingBot(){
    stop();
    let bots = document.getElementsByClassName("bot");
    for (let i = 0; i < bots.length; i++){
        bots[i].onmouseover = function (){
            this.style.borderWidth = "2";
        };
        bots[i].onmouseleave = function (){
            this.style.borderWidth = "";
        };
        bots[i].onclick = function (){
            this.style.borderWidth = "";
            let bots = document.getElementsByClassName("bot");
            for (let i = 0; i < bots.length; i++){
                bots[i].onmouseover = null;
                bots[i].onmouseleave = null;
                bots[i].onclick = null;
            }
        };
    }
    
}


function saveBot(){
    stop();
    let bots = document.getElementsByClassName("bot");
    for (let i = 0; i < bots.length; i++){
        bots[i].onmouseover = function (){
            this.style.borderWidth = "2";
        };
        bots[i].onmouseleave = function (){
            this.style.borderWidth = "";
        };
        bots[i].onclick = function (){
            selectSaveBot(this.attributes.botId.value);
            this.style.borderWidth = "";
            let bots = document.getElementsByClassName("bot");
            for (let i = 0; i < bots.length; i++){
                bots[i].onmouseover = null;
                bots[i].onmouseleave = null;
                bots[i].onclick = null;
            }
        };
    }
}

function selectSaveBot(botId){
    stop();
    let xy = botId.split(",");
    let nameBot = prompt("Ввидите имя бота");
    $.ajax({
        type: "POST",
        url: "SaveBot",
        data: {
            x: xy[0],
            y: xy[1],
            name: nameBot
        },
        async: false,
        success: function (result) {
            let isSave = JSON.parse(result);
            if(isSave["save"]){
                alert("Бот сохранен");
            }else {
                alert("Бот не сохранен");
            }
        },
        error: function (err) {
            alert("Бот не сохранен");
        },
        complete: function (){
            return true;
        }
    });
}

function mouseOverBot(){
    let gr = document.getElementById();
}


document.onkeyup = function (event){
    if (event.code === 'KeyQ') {
        let sizeAndScaleInt = getSizeAndScaleInt(1);
        if(sizeAndScaleInt[0] <= -1){
            return false;
        }
        document.getElementById("gr").style = ("grid-template-columns: repeat(" + sizeAndScaleInt[0] + ", " 
            + (sizeAndScaleInt[1] + 2) + "px); " +
            "grid-template-rows: repeat(" + sizeAndScaleInt[0] + ", " + (sizeAndScaleInt[1] + 2) + "px)");
        moveStatistics();
    }
    if (event.code === 'KeyW') {
        let sizeAndScaleInt = getSizeAndScaleInt(0);
        if(sizeAndScaleInt[1] <= -1){
            return false;
        }
        document.getElementById("gr").style = ("grid-template-columns: repeat(" + sizeAndScaleInt[0] + ", "
            + (sizeAndScaleInt[1] - 2) + "px); " +
            "grid-template-rows: repeat(" + sizeAndScaleInt[0] + ", " + (sizeAndScaleInt[1] - 2) + "px)");
        moveStatistics();
    }
};

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