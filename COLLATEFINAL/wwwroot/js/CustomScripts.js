function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}

$(function () {

    const $deleteBtn = $('#btnDeleteSelected');
    const deleteUrl = $deleteBtn.data('delete-url');

    if (!deleteUrl) {
        console.warn('Delete URL not found on #btnDeleteSelected');
        return;
    }

    $('#selectAll').on('change', function () {
        $('.row-checkbox').prop('checked', this.checked).trigger('change');
    });

    $(document).on('change', '.row-checkbox', function () {
        $deleteBtn.prop(
            'disabled',
            $('.row-checkbox:checked').length === 0
        );
    });

    $deleteBtn.on('click', function () {

        const selectedIds = $('.row-checkbox:checked')
            .map(function () {
                return $(this).val();
            })
            .get();

        if (!selectedIds.length) return;
        if (!confirm(`Delete ${selectedIds.length} selected item(s)?`)) return;

        const token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: deleteUrl, 
            type: 'POST',
            data: {
                ids: selectedIds,
                __RequestVerificationToken: token
            },
            traditional: true,
            success: function () {
                location.reload();
            },
            error: function (xhr) {
                console.error(xhr.responseText);
                alert('Failed to delete selected items.');
            }
        });
    });

});