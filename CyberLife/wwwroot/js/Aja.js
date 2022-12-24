function ajaxUpdate() {
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

function start() {
    $("#speed").html(speedAjax);
    if(timerId == null){
        timerId = setInterval(ajaxUpdate, speedAjax);
    }
}

function plusSpeed() {
    stop();
    if (speedAjax > 0)
        speedAjax -= 10;
    $("#speed").html(speedAjax);
}

function minusSpeed() {
    stop();
    speedAjax += 10;
    $("#speed").html(speedAjax);
}

function stop() {
    clearInterval(timerId);
    timerId = null;
}

function saveBot() {
    stop();
    let bots = document.getElementsByClassName("bot");
    for (let i = 0; i < bots.length; i++){
        bots[i].onmouseover = function (){
            this.style.borderWidth = "3px";
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

function selectSaveBot(botId) {
    stop();
    let xy = botId.split(",");
    let nameBot = prompt("Введите имя бота");
    $.ajax({
        type: "POST",
        url: "/Home/SaveBot",
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

function getAllBot() {
    stop();
    $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "/Home/GetAllBot",
            dataType: "html",
            success: function (result) {
                $("#botListModalBody").html(result);
            },
            error: function (err) {
                $("#gr").val("Error while uploading data: \n\n" + err);
            }
        });
    })
}

function deleteBot(perceptronId) {
    stop();
    
    $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "/Home/DeleteBot",
            dataType: "html",
            data: {
                perceptronId: perceptronId
            },
            success: function (result) {
                $("#botListModalBody").html(result);
            },
            error: function (err) {
                $("#gr").val("Error while uploading data: \n\n" + err);
            }
        });
    })
}

function changeBot(perceptronId) {
    stop();
    let nameBot = prompt("Введите имя бота");
    $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "/Home/ChangeBot",
            dataType: "html",
            data: {
                perceptronId: perceptronId,
                name: nameBot
            },
            success: function (result) {
                $("#botListModalBody").html(result);
            },
            error: function (err) {
                $("#gr").val("Error while uploading data: \n\n" + err);
            }
        });
    })
}

let loadingBots = [];

function selectBotLoading(botId, color) {
    stop();
    loadingBots.push(botId);
    let bots = document.getElementsByClassName("bot");
    for (let i = 0; i < bots.length; i++) {
        bots[i].onmouseover = function () {
            this.style.borderWidth = "3px";
        };
        bots[i].onmouseleave = function () {
            this.style.borderWidth = "";
        };
        bots[i].onclick = function () {
            loadingBots.push(this.attributes.botId.value);
            this.style.backgroundColor = color;
        }
    }
}

function loadingBot() {
    stop();
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "/Home/LoadingBot",
            data: {
                bots : loadingBots
            },
            dataType: "html",
            success: function (result) {
                $("#allBot").html(result);
                loadingBots = [];
            },
            error: function (err) {
                $("#gr").val("Error while uploading data: \n\n" + err);
            }
        });
    })
}

function getAllMaps() {
    stop();
    $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "/Home/GetAllMaps",
            dataType: "html",
            success: function (result) {
                $("#mapListModalBody").html(result);
            },
            error: function (err) {
                $("#gr").val("Error while uploading data: \n\n" + err);
            }
        });
    })
}

function saveMap() {
    stop();
    let mapName = prompt("Введите название карты");
    $.ajax({
        type: "POST",
        url: "/Home/SaveMap",
        data: {
            name: mapName
        },
        async: false,
        success: function (result) {
            let isSave = JSON.parse(result);
            if(isSave["save"]){
                alert("Карта сохранена");
            }else {
                alert("Карта не сохранена");
            }
        },
        error: function (err) {
            alert("Карта не сохранена");
        },
        complete: function (){
            return true;
        }
    });
}

function selectMapForLoading() {
    stop();
    let maps = document.getElementsByClassName("mapLoading");
    for (let i = 0; i < maps.length; i++) {
        maps[i].onmouseover = function () {
            this.style.border = "1px solid black";
        };
        maps[i].onmouseleave = function () {
            this.style.border = "";
        };
        maps[i].onclick = function () {
            loadMap(this.id);
            this.style.border = "1px solid black";
            let maps = document.getElementsByClassName("mapLoading");
            for (let i = 0; i < maps.length; i++) {
                maps[i].onmouseover = null;
                maps[i].onmouseleave = null;
                maps[i].onclick = null;
            }
        };
    }
}

function loadMap(id) {
    stop();
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "/Home/LoadMap",
            data: {
                mapId: id
            },
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

document.onkeyup = function (event){
    if (event.code === 'KeyQ') {
        let sizeAndScaleInt = getSizeAndScaleInt(1);
        if(sizeAndScaleInt[0] <= -1){
            return false;
        }
        document.getElementById("gr").style = ("grid-template-columns: repeat(" + sizeAndScaleInt[0] + ", " 
            + (sizeAndScaleInt[1] + 2) + "px); " +
            "grid-template-rows: repeat(" + sizeAndScaleInt[2] + ", " + (sizeAndScaleInt[1] + 2) + "px)");
        moveStatistics();
    }
    if (event.code === 'KeyW') {
        let sizeAndScaleInt = getSizeAndScaleInt(0);
        if(sizeAndScaleInt[1] <= -1){
            return false;
        }
        document.getElementById("gr").style = ("grid-template-columns: repeat(" + sizeAndScaleInt[0] + ", "
            + (sizeAndScaleInt[1] - 2) + "px); " +
            "grid-template-rows: repeat(" + sizeAndScaleInt[2] + ", " + (sizeAndScaleInt[1] - 2) + "px)");
        moveStatistics();
    }
}


function moveStatistics(){
    let w = document.getElementById('gr').offsetWidth;
    let h = document.getElementById('gr').offsetHeight;
    let statistics = document.getElementById('statistics');
    statistics.style.height = h + "px";
    let ss = document.styleSheets[2];
    let rules = ss.cssRules || ss.rules;
    let h1Rule = null;
    for (let i = 0; i < rules.length; i++) {
        let rule = rules[i];
        if (/(^|,) *\#statistics *(,|$)/i.test(rule.selectorText)) {
            h1Rule = rule;
            break;
        }
    }
    h1Rule.style.marginLeft = w + "px";
    h1Rule.style.height = h + "px";
}

function getSizeAndScaleInt(isUp){
    let gr;
    gr = document.getElementById("gr");
    let gridTemplateColumns = gr.style.gridTemplateColumns;
    let gridTemplateRows = gr.style.gridTemplateRows;
    let sizeAndScale = gridTemplateColumns.match(/\d+/g);
    sizeAndScale.push(gridTemplateRows.match(/\d+/g)[0]);
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

function restart()
{
    let btn = document.getElementById("restartBtn");
    if (isStart) {
        stop();
        toggle();
    }
    btn.classList.add("fa-spin");
    btn.addEventListener( "animationend",  function() {
        btn.classList.remove("fa-spin");
    });
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