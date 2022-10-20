var tagnames = new Bloodhound({
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    remote: {
        url: '/ArtistTag/Tags?query=%QUERY',
        wildcard: '%QUERY',
    }
});
tagnames.initialize();

function InitTags() {

    $('#tagsInput').tagsinput({
        allowDuplicates: false,
        itemValue: "Id",
        itemText: "Name",
        typeaheadjs: {
            name: 'tagnames',
            displayKey: 'Name',
            source: tagnames.ttAdapter()
        }
    });
};

function TagsAddInitialValues(model) {
    var tags = JSON.parse(model);
    for (var i = 0; i < tags.length; i++) {
        $('#tagsInput').tagsinput('add', tags[i])
    }
}


function SubmitTags(artistId) {
    $.ajax({
        url: '/ArtistTag/EditTagsForArtist',
        type: 'POST',
        data: {
            id: artistId,
            tags: $('#tagsInput').val()
        },
        success: function (result) {
            $("#tags_edit_view").replaceWith($(result));
        },
        error: function () {
            alert("Error!!!");
        }
    });
}


function InitView(model) {
    InitTags();
    TagsAddInitialValues(model);
}