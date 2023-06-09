﻿$(document).ready(function () {

    $('#addStaff').on('click', function () {
        fillSelect(1);

        $('#find-page').click(function () {

            var pageNumber = $('#page-number').val();

            if (pageNumber < 1) {
                alert('Page number must be at least 1.');
                return;
            }

            fillSelect(pageNumber);
        });
    });
}
);

function fillSelect(pageNumber) {
    $.ajax({
        url: '/admin/users/json' + '?page=' + pageNumber,
        type: "GET",
        dataType: "json",
        success: function (data) {
            $('#user-select').empty();

            $.each(data, (_, model) => {
                console.log(model)
                $('#user-select').append($('<option>', {
                    value: model.id,
                    text: model.email + " " + model.name
                }));
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus + ': ' + errorThrown);
        }
    });
}