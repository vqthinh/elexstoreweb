$(document).ready(function () {    

        $('.productPrice,ins,del').autoNumeric('init', {
            aSep: ',',
            aDec: '.',
            aPad: false,
            aSign:'$'
        });

 
    $('.add-to-cart').off('click').on('click', function () {
        var pid = $(this).attr("data-id");
        var soluong = $('#txtQuantity').val();
        if (soluong === 'undefined' || soluong == null) {
            soluong = 1;
        }
        var itemImg = $(this).closest('.product').find('#' + pid);
        $.ajax({
            url: "/Cart/checkQuantity",
            data: { productID: pid, quantity: soluong },
            success: function (response) {
               if (response) {
                   alert('Out of stock!');
               } else {
                   $.ajax({
                       url: "/Cart/AddItem",
                       data: { productID: pid, quantity: soluong },
                       success: function (response) {
                           $("#productCount").html(response.Sum);
                           $("#productPrice").html('$'+Number(response.Total).toLocaleString('en'));
                       }
                   });
                   //Scroll to top if cart icon is hidden on top
                   $('html, body').animate({
                       'scrollTop': $(".cart_anchor").position().top
                   });
                   //Select item image and pass to the function  
                   flyToElement($(itemImg), $('.cart_anchor'));
               }
            }
        });
  
       
    });
});