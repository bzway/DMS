
$(document).on("pageinit", "", function (event) {
    loadCart();
});

function loadCart() {
    $(".product-number").each(function (index, element) {
        console.log($(element).data("product"));
        var url = "/ShoppingCart/Product";
        var data = { pid: $(element).data("product") };
        $.ajax({
            url: url,
            type: "post",
            dataType: "json",
            data: data,
            success: function (result) {
                if (result.code == 0) {
                    $(element).html(result.data);
                }
            }
        });
    });
    var url = "/ShoppingCart/Product";

    $.ajax({
        url: url,
        type: "post",
        dataType: "json",
        success: function (result) {
            if (result.code == 0) {
                if (result.data <= 0) {
                    $("#totalItem").hide();
                } else {
                    $("#totalItem").show();
                }
                $("#totalItem").text(result.data);
            }
        }
    });


}


function addCart(pid, number) {

    var url = "/ShoppingCart/AddItem";
    var data = { Id: pid, number: number };
    $.ajax({
        url: url,
        type: "post",
        dataType: "json",
        data: data,
        beforeSend: function (XMLHttpRequest) {
            $.mobile.loading('show', { text: '正在加载' })
        },
        success: function (result) {
            $.mobile.loading('hide');
            if (result.code != 0) {
                alert(result.message);
            } else {
                if (result.data.totalItem <= 0) {
                    $("#totalItem").hide();
                } else {
                    $("#totalItem").show();
                }
                $("#totalItem").text(result.data.totalItem);
                $("#productItem" + pid).text(result.data.productItem);
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
            $.mobile.loading('hide');
        },
        error: function (data) {
            $.mobile.loading('hide');
            console.log(data);
        }
    });
    return false;
}

function removeCart(pid, number) {

    var url = "/ShoppingCart/RemoveItem";
    var data = { Id: pid, number: number };
    $.ajax({
        url: url,
        type: "post",
        dataType: "json",
        data: data,
        beforeSend: function (XMLHttpRequest) {
            $.mobile.loading('show', { text: '正在加载' })
        },
        success: function (result) {
            $.mobile.loading('hide');
            if (result.code != 0) {
                alert(result.message);
            } else {
                if (result.data.totalItem <= 0) {
                    $("#totalItem").hide();
                } else {
                    $("#totalItem").show();
                }
                $("#totalItem").text(result.data.totalItem);
                $("#productItem" + pid).text(result.data.productItem);
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
            $.mobile.loading('hide');
        },
        error: function (data) {
            $.mobile.loading('hide');
            console.log(data);
        }
    });
    return false;
} 