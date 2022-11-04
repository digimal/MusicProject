$(document).ready(function () {

    var $formCreate = $('#formCreate');


    $(document).on('click', '#btn-discard', discardCreation);
    $(document).on('change', '#btn-upload-pic', uploadPicture);

    $(document).on('submit', '#formCreate', submitFormCreate);
    $(document).on('submit', '#formEdit', submitFormEdit);

    function submitFormCreate(e) {
        var prevent = e.preventDefault();
        var formData = new FormData(this);
        
        if ($(this).valid()) {
            $.ajax({
                type: $(this).attr('method'),
                url: $(this).attr('action'),
                data: formData,
                contentType: false,
                processData: false,
                success: function (item)
                {
                    $('#modalArtist').modal('hide');
                    var $elem = $(item);
                    $("#grid-artists").append($elem).masonry('appended', $elem);
                }
            });
        }
        return prevent;
    }

    function submitFormEdit(e) {
        var res = e.preventDefault();
        $.validator.unobtrusive.parse($(this));

        if ($(this).valid()) {
            $.ajax({
                type: $(this).attr('method'),
                url: $(this).attr('action'),
                data: new FormData(this),
                contentType: false,
                processData: false,
                success: function (item) {
                    $('#modalArtist').modal('hide');
                    var id = $('#hiddenId').val();
                    $('#dialogCreateEdit').remove();
                    var $elem = $(item);
                    $('#artist_container' + id).replaceWith($elem);
                }
            });
        }
        return res;
    }

    function uploadPicture(e) {
        var tgt = e.target;
        var files = tgt.files;
        var fileData = new FormData();
        fileData.append(files[0].name, files[0]);
        $.ajax({
            type: 'POST',
            url: '/Artist/UploadPicture',
            data: fileData,
            contentType: false,
            processData: false,
            success: function (result) { $('#PictureIdUpload').val(result.picId); },
            error: function () {
                alert('Picture has not been loaded.');
            }
        });
    }

    function discardCreation() {
        var picId = $('#PictureIdUpload').val();
        if (picId !== "") {
            $.ajax({
                type: 'POST',
                url: '/Artist/DeletePicture',
                data: { id: picId + "" },
                error: function () {
                    alert("Obsolete picture has not been deleted.");
                }
            });
        }

    }
});