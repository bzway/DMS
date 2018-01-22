// Use the addEventListener() method to associate events with DOM elements.
document.addEventListener("app.Ready", onAppReady, false);
document.addEventListener('deviceready', onDeviceReady, false);
document.addEventListener("onload", onAppReady, false);
function onDeviceReady() {
    //if (navigator.splashscreen && navigator.splashscreen.hide) {   // Cordova API detected
    //    navigator.splashscreen.hide();
    //}
    //if (!window.device) {
    //    window.device = { platform: 'Browser' };
    //}
    //if (parseFloat(window.device.version) >= 7.0) {
    //    $("div.ui-header").css("padding-top", "20px");
    //}

    //handleExternalURLs();
}
//function handleExternalURLs() {
//    // Handle click events for all external URLs
//    if (device.platform.toUpperCase() === 'ANDROID') {
//        $(document).on('click', 'a[href^="http"]', function (e) {
//            var url = $(this).attr('href');
//            navigator.app.loadUrl(url, { openExternal: true });

//            e.preventDefault();
//        });
//    }
//    else if (device.platform.toUpperCase() === 'IOS') {
//        $(document).on('click', 'a[href^="http"]', function (e) {
//            var url = $(this).attr('href');
//            window.open(url, '_system');
//            e.preventDefault();
//        });
//    }
//    else {
//        // Leave standard behaviour
//    }
//}
//$(window).on("navigate", function (event, data) {
//    alert("navigate");
//});

//$(document).bind('pageinit', function () {
//    alert("ANDROID");
//    alert(window.device.version);
//});
$(document).on("pageinit", "#Home", function (event) {
    var target = $(event.target);
    target.find('.banner').slick({
        dots: true,
        infinite: true,
        speed: 500,
        fade: true,
        cssEase: 'linear'
    });
});

$(".page").on("pagebeforeshow", function (event) {
    //var target = $(event.target);
    //var id = target.attr("id");
    //var link = target.find("a[href='#" + id + "']");
    //link.addClass("ui-btn-active");
});

jQuery(".page").on("pageaftershow", function (event) {

    //var target = $(event.target);
    //var id = target.attr("id");
    //var link = target.find("a[href='#" + id + "']");
    //link.addClass("ui-btn-active");
});

$("p").on("swiperight", function () {
    alert("向右滑动!");
});

function openUrl(url) {
    $.mobile.changePage(url);
    return false;
    var url = "http://192.168.1.101:11568/app/login";
    $.ajax({
        url: url,
        type: "get",
        dataType: "html",
        data: { t: Math.random() },
        beforeSend: function (XMLHttpRequest) {
            $.mobile.loading('show', { text: 'test', theme: 'a' })
        },
        success: function (data) {
            $('#Home').empty();
            $('#Home').html(data);
            $("#Home").trigger('create');
            $('ul').listview('refresh');
            $("#Home").find("div").header('refresh');
        },
        complete: function (XMLHttpRequest, textStatus) {
            $.mobile.loading('hide');
        },
        error: function (data) {
            alert("密码发送失败，请重试:" + data);
            console.log(data);
        }
    });

}