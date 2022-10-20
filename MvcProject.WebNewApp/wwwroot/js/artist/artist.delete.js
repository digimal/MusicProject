$(document).ready(function () {

    var $delete = $('#btn-delete');

    $delete.on('click', function (e) {
        var id = $delete.attr('data-artist-id');
        var $gridElement = $('#artist' + id);
        $.post('/Artist/Delete/?id=' + id)
            .done(function () {
                $('#modalArtist').modal('hide');
                $("#grid-artists").masonry('remove', $gridElement);
                $("#grid-artists").masonry();
            });
    });
});