export function initializeSelect2(componentId) {
    $(document).ready(function () {
        $(`.${componentId}`).select2({
            theme: "bootstrap-5",
            closeOnSelect: false
        });
    });
}