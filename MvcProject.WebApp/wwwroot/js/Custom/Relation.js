
var container = {};

var artistId = 0;
var relationId = 0;

var table = 'relationTable';
var memberRow = 'memberRow';
var memberRowNew = 'memberRowNew';
var memberRowNewCandidates = 'memberRowNewCandidates';
var memberRowNewCandidatesMenu = 'memberRowNewCandidatesMenu';

var intervalStart = 'start';
var intervalEnd = 'end';

var addMemberConfirm = 'addMemberConfirm';
var addMemberCancel = 'addMemberCancel';

var editMember = 'editMember';
var editMemberConfirm = 'editMemberConfirm';
var editMemberCancel = 'editMemberCancel';

var deleteMember = 'deleteMember';
var deleteMemberConfirm = 'deleteMemberConfirm';
var deleteMemberCancel = 'deleteMemberCancel';

var Interval = "Interval";
var StartDate = "StartDate";
var EndDate = "EndDate";

function Refresh(id) {
    $(IndexPlusId(intervalStart, id)).datepicker("setDate", container[id][Interval][StartDate]);
    $(IndexPlusId(intervalEnd, id)).datepicker("setDate", container[id][Interval][EndDate]);
}

function onAdd() {
    if ($(Index(memberRowNew)).length !== 0) {
        return;
    }
    $(Index(table) + ' > tbody:last-child').append($('<tr id="' + memberRowNew + '"></tr>'));
    $(Index(memberRowNew)).append($(Cell('<div id="' + memberRowNewCandidatesMenu + '" class="tt-dropdown-menu"></div>')));
    $(Index(memberRowNewCandidatesMenu)).append($('<input id="' + memberRowNewCandidates + '"  type="text" class="typeahead" />'));
    $(Index(memberRowNewCandidatesMenu)).append($('<input id="newCandidateId" type="hidden" value="" />'));
    initCandidates();
    $(Index(memberRowNew)).append($(Cell('<input type="text" id="' + intervalStart + '" class="datepicker"  />')));
    $(Index(memberRowNew)).append($(Cell('<input type="text" id="' + intervalEnd + '" class="datepicker"  />')));
    $(Index(memberRowNew)).append($(Cell('<button type="button" id="' + addMemberConfirm + '" class="btn btn-primary" onclick="Add()">Confirm</button>')));
    $(Index(memberRowNew)).append($(Cell('<button type="button" id="' + addMemberCancel + '" class="btn btn-primary" onclick="cancelAdd()">Cancel</button>')));
    $(Index(intervalStart)).datepicker();
    $(Index(intervalEnd)).datepicker();
}

function initCandidates() {
    var candidates = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/Artist/RelationCandidates?artistId=' + artistId + '&relationId=' + relationId + '&query=%QUERY',
            wildcard: '%QUERY'
        }
    });
    candidates.initialize();

    $(Index(memberRowNewCandidates)).typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    },
        {
            name: 'candidates',
            display: 'Name',
            source: candidates
        });

    $(Index(memberRowNewCandidates)).bind('typeahead:select',
        function (ev, suggestion) {
            $('#newCandidateId').val(suggestion.Id);
        });

}

function onEdit(id) {
    $(IndexPlusId(intervalStart,id)).datepicker('enable');
    $(IndexPlusId(intervalEnd,id)).datepicker('enable');
    $(IndexPlusId(editMember, id)).replaceWith($('<button type="button" id="' + editMemberConfirm + id + '" class="btn btn-primary" onclick="Edit(\'' + id + '\')">Save</button>'));
    $(IndexPlusId(deleteMember,id)).replaceWith($('<button type="button" id="' + editMemberCancel + id + '" class="btn btn-primary" onclick="cancelEdit(\'' + id + '\')">Cancel</button>'));
}

function onDelete(id) {
    $(IndexPlusId(editMember,id)).replaceWith($('<button type="button" id="' + deleteMemberConfirm + id + '" class="btn btn-primary" onclick="Delete(\'' + id +'\')">Confirm removal</button>'));
    $(IndexPlusId(deleteMember,id)).replaceWith($('<button type="button" id="' + deleteMemberCancel + id + '" class="btn btn-primary" onclick="cancelDelete(\'' + id +'\')">Cancel</button>'));
}

function cancelAdd() {
    $(Index(memberRowNew)).remove();
}

function cancelEdit(id) {
    Refresh(id);
    $(IndexPlusId(intervalStart, id)).datepicker('disable');
    $(IndexPlusId(intervalEnd, id)).datepicker('disable');
    var buttonEdit = '<button type="button" id="' + editMember + id + '" class="btn btn-primary" onclick="onEdit(\'' + id + '\')">Edit</button>';
    $(IndexPlusId(editMemberConfirm, id)).replaceWith($(buttonEdit));
    $(IndexPlusId(editMemberCancel, id)).replaceWith($('<button type="button" id="' + deleteMember + id + '" class="btn btn-primary" onclick="onDelete(\'' + id + '\')">Delete</button>'));
}

function cancelDelete(id) {
    $(IndexPlusId(deleteMemberConfirm,id)).replaceWith($('<button type="button" id="' + editMember + id + '" class="btn btn-primary" onclick="onEdit(\'' + id +'\')">Edit</button>'));
    $(IndexPlusId(deleteMemberCancel,id)).replaceWith($('<button type="button" id="' + deleteMember + id + '" class="btn btn-primary" onclick="onDelete(\'' + id +'\')">Delete</button>'));
}

function Add() {
    var candidateId = $('#newCandidateId').val();
    var candidateName = $(Index(memberRowNewCandidates)).typeahead('val');
    if (candidateId !== "") {
        var newRelation = {
            ArtistId: artistId,
            RelationId: relationId,
            Interval: {
                StartDate: $(Index(intervalStart)).datepicker('getDate').toDateString(),
                EndDate: $(Index(intervalEnd)).datepicker('getDate').toDateString(),
            },
            Name: candidateName,
            Id: candidateId
        };
        $.post('/Artist/RelationMembersAdd', { model: JSON.stringify(newRelation) })
            .done(function (item) {
                $(Index(table) + ' > tbody:last-child').append($(item));
                $(Index(memberRowNew)).remove();
            });
    }
}

function Edit(id) {
    container[id][Interval][StartDate] = $(IndexPlusId(intervalStart, id)).datepicker('getDate').toDateString();
    container[id][Interval][EndDate] = $(IndexPlusId(intervalEnd, id)).datepicker('getDate').toDateString();
    var start = $(IndexPlusId(intervalStart, id)).datepicker('getDate');
    var end = $(IndexPlusId(intervalEnd, id)).datepicker('getDate');
    $.ajax({
        url: '/Artist/RelationMembersEdit',
        type: 'POST',
        data: {
            model: JSON.stringify(container[id])
        },
        
        success: function () {
            container[id][Interval][StartDate] = start;
            container[id][Interval][EndDate] = end;
            cancelEdit(id);
        },
        error: function () {
            container[id][Interval][StartDate] = start;
            container[id][Interval][EndDate] = end;
            cancelEdit(id);
            alert("Error!!!");
        }
    });
}

function Delete(id) {
    $.ajax({
        url: '/Artist/RelationMembersDelete',
        type: 'POST',
        data: {
            model: JSON.stringify(container[id])
        },
        success: function () {
            $(IndexPlusId(memberRow, id)).remove();
        },
        error: function () {
            cancelDelete(id);
            alert("Removal aborted!!!");
        }
    });
}

function IndexPlusId(elemName, id) {
    return '#' + elemName + id;
}

function Index(elemName) {
    return '#' + elemName;
}

function Cell(content) {
    return '<td>' + content + '</td>';
}


function CSharpDateToJSDate(date) {
    return new Date(Date.parse(date));
}