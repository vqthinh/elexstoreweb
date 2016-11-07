$(document).ready(function() {
    $('#btn_delete_all').off('click').on('click', function() {
        $.ajax({
            url: 'Cart/DeleteAllCart',
            dataType: 'json',
            type: 'POST',
            success:function(rs) {
                if (rs.status === true) {
                    window.location.href = "/Cart";
                }
            }
        });
    });

    $('.btn_delete').off('click').on('click', function () {
        $.ajax({
            url: 'Cart/Delete',
            dataType: 'json',
            data:{id:$(this).data('id')},
            type: 'POST',
            success: function (rs) {
                if (rs.status === true) {
                    window.location.href = "/Cart";
                }
            }
        });
    });
});