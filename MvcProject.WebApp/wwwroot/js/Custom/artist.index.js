$(document).ready(function () {


    $(document).on('click', 'button#createArtist', onCreateModalLoad);

    $(document).on('click', 'button.edit-artist', onEditModalLoad);

    $(document).on('click', 'button.delete-artist', onDeleteModalLoad);

    $('.modal').on('hidden.bs.modal', function () {
        //$('.modal-dialog > form').trigger('reset');
        $('#dialogCreateEdit').remove();
    });

    function onCreateModalLoad(e) {
        $('#modalArtist').load('/Artist/Create', function (res) {
            $.validator.unobtrusive.parse($('#formCreate'));
            $('.datepicker').datepicker();
            $('.datepicker').attr('autocomplete', 'off');
        });

        return e.preventDefault();
    }

    function onEditModalLoad(e) {
        var id = $(this).data('artist-id');
        var url = '/Artist/Edit/' + id;
        $('#modalArtist').load(url, function (res) {
            $.validator.unobtrusive.parse($('#formEdit'));
            $('.datepicker').datepicker();
            $('.datepicker').attr('autocomplete', 'off');
        });


    }

    function onDeleteModalLoad(e) {
        var id = $(this).data('artist-id');

        $('#btn-delete').attr("data-artist-id", id);
        $('#delete-artist-name').text($("#artist" + id + "name").text());

        return e.preventDefault;
    }

});