$(document).ready(function () {
    $("#rim").hover(function () {
        $(this).find("ul:first").show();
    }, function () {
        $(this).find("ul:first").hide();
    })
})