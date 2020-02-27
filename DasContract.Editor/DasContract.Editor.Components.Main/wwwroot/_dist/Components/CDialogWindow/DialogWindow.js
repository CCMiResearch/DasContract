window.MaterialBootstrapRazorComponents.DialogWindow = {
    Show: function (id) {
        $("#" + id).modal("show");
    },
    Hide: function (id) {
        $("#" + id).modal("hide");
    }
};
