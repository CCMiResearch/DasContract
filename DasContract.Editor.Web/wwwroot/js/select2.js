export function initializeSelect2(componentId, selectedItems, options, multiple, minimumResultsForSearch, dotnetRef) {
    $(`#${componentId}`).select2({
        theme: "bootstrap-5",
        width: '100%',
        multiple: multiple,
        minimumResultsForSearch: minimumResultsForSearch
    });

    options.forEach((option) => {
        var addedOption = new Option(option.text, option.value, false, false);
        $('#' + componentId).append(addedOption);
    });

    $('#' + componentId).val(selectedItems);

    $('#' + componentId).on('select2:select', function (e) {
        dotnetRef.invokeMethodAsync('OnSelect', e.params.data.id);
    });

    $('#' + componentId).on('select2:unselect', function (e) {
        console.log(e);
        dotnetRef.invokeMethodAsync('OnUnselect', e.params.data.id);
    });

    $('#' + componentId).on('change', function (e) {
        dotnetRef.invokeMethodAsync('OnChange');
    });
}

export function addOption(componentId, value, text) {
    var addedOption = new Option(text, value, false, false);
    $('#' + componentId).append(addedOption);
}

export function removeOption(componentId, value) {
    $('#' + componentId).find('[value="' + value + '"]').remove();
}

export function refreshOptions(componentId, options, selected) {
    $('#' + componentId + " option").remove();

    options.forEach((option) => {
        var addedOption = new Option(option.text, option.value, false, false);
        $('#' + componentId).append(addedOption);
    });                     

    refreshSelectedItems(componentId, selected);
}

export function getSelectedIds(id) {
    return $.makeArray($('#' + id).find(':selected').map(function () { return this.value }));
}

export function refreshSelectedItems(componentId, selected) {
    $('#' + componentId).val(null)
    $('#' + componentId).val(selected)
    //$('#' + componentId).trigger('change');
}