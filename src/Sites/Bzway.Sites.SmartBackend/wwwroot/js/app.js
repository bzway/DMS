var app = {
    // Application Constructor
    initialize: function () {
        document.addEventListener('deviceready', this.onDeviceReady.bind(this), false);
    },
    onDeviceReady: function () {
        window.open = cordova.InAppBrowser.open;
        if (navigator.splashscreen && navigator.splashscreen.hide) {   // Cordova API detected
            navigator.splashscreen.hide();
        }
        //if (navigator.splashscreen && navigator.splashscreen.hide) {   // Cordova API detected
        //    navigator.splashscreen.hide();
        //}
        //if (!window.device) {
        //    window.device = { platform: 'Browser' };
        //}
        //if (parseFloat(window.device.version) >= 7.0) {
        //    $("div.ui-header").css("padding-top", "20px");
        //}
    }
};

app.initialize();

$(document).on("pageinit", "", function (event) {
    var target = $(event.target);
    target.find('.banner').slick({
        dots: true,
        infinite: true,
        autoplay: true,
        speed: 500,
        fade: true,
        cssEase: 'linear'
    });

    target.on("swiperight", function (event) {
        if (window.location.pathname == '/category' || window.location.pathname == '/article' || window.location.pathname == '/account') {
            return false;
        }
        window.history.back();
    });

    target.on("swipeleft", function (event) {
    });
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
