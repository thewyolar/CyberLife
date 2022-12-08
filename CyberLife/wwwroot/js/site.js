// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    moveStatistics();
    $('.nav-link').each(function () {
        let location = window.location.href;
        let link = this.href;
        if(location == link) {
            $(this).addClass("active");
        }
    });
});

let isStart = false;

function toggle() {
    let btn = document.getElementById("playBtn");
    if(!isStart) {
        isStart = true;
        btn.classList.add("fa-pause");
        btn.classList.remove("fa-play");
        start();
    } else {
        isStart = false;
        btn.classList.add("fa-play");
        btn.classList.remove("fa-pause");
        stop();
    }
}