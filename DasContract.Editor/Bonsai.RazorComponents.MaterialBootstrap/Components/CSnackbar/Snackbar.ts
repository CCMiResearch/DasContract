

(window as any).MaterialBootstrapRazorComponents.Snackbar = {

    Show: function (id: string)
    {
        ($(`#${id}`) as any).snackbar("show");
    },

    Hide: function (id: string)
    {
        ($(`#${id}`) as any).snackbar("hide");
    }
}

