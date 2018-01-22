var app = {
    // Application Constructor
    initialize: function () {
        document.addEventListener('deviceready', this.onDeviceReady.bind(this), false);
    },

    // deviceready Event Handler
    //
    // Bind any cordova events here. Common events are:
    // 'pause', 'resume', etc.
    onDeviceReady: function () {
        window.open = cordova.InAppBrowser.open;
        if (navigator.splashscreen && navigator.splashscreen.hide) {   // Cordova API detected
            navigator.splashscreen.hide();
        }
    }
};

app.initialize();

$(document).on("mobileinit", function () {
});


//function onDeviceReady() {
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
//}
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


$(".page").on("swipeleft", function (event) {
    var target = $(this);
    $.mobile.changePage("#Account");
});
$(".page").on("swiperight", function (event) {
    var target = $(this);
    $.mobile.changePage("#Home");
});
$.mobile.loadServerPage = function (url, container, data) {

    $.ajax({
        url: url,
        type: "get",
        dataType: "html",
        data: data,
        beforeSend: function (XMLHttpRequest) {
            $.mobile.loading('show', { text: '正在加载' })
        },
        success: function (html) {
            $(container).empty();
            $(container).html(html);
            $(container).trigger('create');
            $(container).find('ul').listview('refresh');
            $(container).find("div").header('refresh');
            $.mobile.loading('hide');

        },
        complete: function (XMLHttpRequest, textStatus) {
            $.mobile.loading('hide');
        },
        error: function (data) {
            $.mobile.loading('hide');
            console.log(data);
        }
    });
};


function openUrl() {
    var url = "http://192.168.2.104:11568";
    $.ajax({
        url: url,
        type: "get",
        dataType: "html",
        data: { t: Math.random() },
        beforeSend: function (XMLHttpRequest) {
            $.mobile.loading('show', { text: '正在加载' })
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
            console.log(data);
        }
    });
}