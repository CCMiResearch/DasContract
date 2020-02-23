window.MaterialBootstrapRazorComponents.Snackbar = {
    Show: function (id) {
        $("#" + id).snackbar("show");
    },
    Hide: function (id) {
        $("#" + id).snackbar("hide");
    }
};
