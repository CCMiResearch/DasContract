

(window as any).MaterialBootstrapRazorComponents.Alert = {

    Close: function (id: string)
    {
        $(`#${id}`).alert("close");
    }
}

