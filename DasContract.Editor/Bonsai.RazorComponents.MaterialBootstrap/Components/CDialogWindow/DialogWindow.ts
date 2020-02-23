

(window as any).MaterialBootstrapRazorComponents.DialogWindow = {

    Show: function (id: string)
    {
        $(`#${id}`).modal("show");
    },

    Hide: function (id: string)
    {
        $(`#${id}`).modal("hide");
    }
}

