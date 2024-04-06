
$(".addtocart").click(function () {
    var productID = $(this).attr('data-id');
    if ($.cookie('CartProducts') == null) {
        $.cookie('CartProducts', productID, { path: '/' });
    }
    else {
        var cooky = "" + $.cookie('CartProducts');
        alert("current coockie" + cooky);
        cooky = cooky + "," + productID
        $.removeCookie("CartProducts", { path: '/' });
        $.cookie('CartProducts', cooky, { path: '/' });
    }
    //alert("Product Add to cart");
    // productID.join('-')
});

