$(document).ready(function () {

    $(document).on('click', '.glyphicon-heart.active', onHeartClick);

    function onHeartClick(e) {
        var id = $(this).data('artist-id');
        console.log(id);
        e.preventDefault();
        $.ajax({
            url: '/Like/ChangeLikeState/' + id,
            type: 'POST',
            success: function (result) {
                GetLikeBox(id).replaceWith($(result));
            },
            error: function () {
                alert("Error during getting related artists!");
            }
        });
    }

    function GetLikeBox(artistId) {
        return $("#artist-" + artistId + "-likes");
    }

});