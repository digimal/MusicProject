$(document).ready(function () {

    $(document).on('click', '.glyphicon-heart.active', onHeartClick);

    function onHeartClick(e) {
        var id = $(this).data('artist-id');
        $.post('/Artist/ChangeLikeState/', {'id': id + ""})
            .done(function (item) {
                GetLikeBox(id).replaceWith($(item));
            });
    }

    function GetLikeBox(artistId) {
        return $("#artist-" + artistId + "-likes");
    }

});