$(document).ready(function(){

    function changeImage() {
        $('#imgchart').prop('src', '/chart/draw/' + btoa($('#pather').val()));
    }

    $("#clicker").click(function () {
        changeImage();

    });

    $('#pather').on('keypress', function (e) {
        if (e.which == 13) {
            changeImage();

        }
    });

})