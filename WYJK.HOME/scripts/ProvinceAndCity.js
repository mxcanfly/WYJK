
$(function () {

    $("#province").change(function () {

        var code = $(this).val();

        $.ajax({
            url: '/Calculator/CitysByProvince/' + code,
            dataType: 'Json',
            success: function (data) {
                $("#city").empty();
                $.each(data, function (i, item) {
                    $("#city").append("<option value='" + item.Value + "'>" + item.Text + "<option>");
                });
            }
        });

    });

});
