CKEDITOR.plugins.add('wechatnews', {
    icons: 'wechatnews',
    init: function (editor) {
        if (false) {
            CKEDITOR.dialog.add('wechatnewsDialog', this.path + 'dialogs/wechatnews.js');
            editor.addCommand('wechatnews', new CKEDITOR.dialogCommand('wechatnewsDialog'));

        } else {
            var cmd = {
                exec: function (editor) {
                    //alert(10)
                    globalEditor = editor;
                    getImgResource(1);
                }
            };
            editor.addCommand('wechatnews', cmd);
        }

        editor.ui.addButton('wechatnews', {
            label: '微信素材',
            command: 'wechatnews'
        });
    }
});
var result;
var globalEditor;



//function getResource() {
//    var token = getToken();
//    $.ajax({
//        url: "/CmsContainer/GetResource?token=" + token,
//        type: "post",
//        success: function (data) {
//            if (data["result"] == 0) {
//                $("#imgcontent").empty();

//                var content = "<ul>";
//                $.each(data["data"], function (i, item) {
//                    content += "<li><img style='width: 100px;height: 100px;' src='" + item.FilePath + "' alt='" + item.Version + "'/>" +
//                        "<input type='radio' name='resourceImg' value='" + item.Version + "'/>Select</li>";
//                });
//                content += "<ul>";

//                $("#imgcontent").append(content);
//                showImgWindow();
//                result = data["data"];

//                //$("#btnSelect").on("click", data["data"][0], onChoosed);

//            }
//        }
//    });
//}

function showImgWindow() {
    console.log($('#ImageModal'))
    $('#ImageModal').modal('show')

    $("#bg").css({
        display: "block", height: $(document).height()
    });

    //var $box = $('#imgBox');
    //$box.css({
    //    //设置弹出层距离左边的位置
    //    left: ($("body").width() - $box.width()) / 2 - 20 + "px",
    //    //设置弹出层距离上面的位置
    //    top: ($(window).height() - $box.height()) / 2 + $(window).scrollTop() + "px",
    //    display: "block"
    //});

    //$("input[name='resourceImg']").click(function () {
    //    $("#imgVersionP").show();
    //    $("#imgVersionSel").empty();
    //    var maxVersion = parseInt($("input[name='resourceImg']:checked").val());
    //    var options = "";
    //    for (var i = 1; i < maxVersion + 1; i++) {
    //        options += "<option>" + i + "</option>";
    //    }
    //    $("#imgVersionSel").append(options);
    //});
}

function closeImgWindow() {
    $("#ImageModal").modal('hide')
    //$(".overlay,#imgBox").css("display", "none");
    //$("#imgcontent").empty();
}

$(function () {
    var imgButtonSel = function () {
        var radios = $("input[name='resourceImg']");

        $("#imgcontent").find("img").each(function (i) {
            if ($(this).hasClass("pic-border"))
            {
                onImgChoosed(result[i]);
            }
        })

        $('#ImageModal').modal('hide')


        //for (var i = 0; i < radios.length; i++) {
        //    if (radios[i].checked == true) {
        //        // alert("i:" + result[i].FilePath);
        //        //alert("i:" + i);
        //        onImgChoosed(result[i]);
        //    }
        //}
    }


    var onImgChoosed = function (value) {
        var version = $("#imgVersionSel").val();
        var html = "<img src='" + value.FilePath + "' data-content-id='" + value.ID + "' data-version-no='" + version + "' />";
        var url = "/CmsContainer/GetWeixinImgUrl";

        //如果该素材不存在微信的图片链接，调用接口并生成
        $.get(url, { contentId: value.ID, version: version, url: encodeURIComponent(value.FilePath), w: Math.random() },
            function (data) {
                if (data["errCode"] == 0) {
                    //var html = "<img src='" + value.FilePath + "' data-content-id='" + value.ID + "' data-version-no='" + version + "' />";
                    globalEditor.insertHtml(html);
                } else {
                    alert("选择失败");
                }

            }, "json");
        //globalEditor.insertHtml(html);


        //var url = "/CmsContainer/GetWeixinImgUrl";

        //如果该素材不存在微信的图片链接，调用接口并生成
        //$.get(url, { contentId: value.ID, version: version, w: Math.random() },
        //    function (data) {
        //        if (data["errCode"] == 0) {
        //            var html = "<img src='" + data["url"] + "' data-content-id='" + value.ID + "' data-version-no='" + version + "' data-weixin-url='" + value.weixinUrl + "'  />";
        //            globalEditor.insertHtml(html);
        //        } else {
        //            alert("system error");
        //        }

        //    }, "json");
        //closeImgWindow();
    }


    //$(".close").click(function () {
    //   // $(".overlay,#imgBox").css("display", "none");
    //    $("#imgcontent").empty();
    //});

    $("#btnImgSelect").on("click", imgButtonSel);
});