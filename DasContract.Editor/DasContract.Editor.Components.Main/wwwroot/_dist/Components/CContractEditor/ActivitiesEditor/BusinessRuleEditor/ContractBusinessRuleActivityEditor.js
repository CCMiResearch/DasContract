exports.__esModule = true;
//const Modeler = require("dmn-js");
var Modeler_1 = require("dmn-js/lib/Modeler");
require("dmn-js/dist/assets/diagram-js.css");
require("dmn-js/dist/assets/dmn-js-decision-table.css");
require("dmn-js/dist/assets/dmn-js-decision-table-controls.css");
require("dmn-js/dist/assets/dmn-js-drd.css");
require("dmn-js/dist/assets/dmn-js-literal-expression.css");
require("dmn-js/dist/assets/dmn-js-shared.css");
require("dmn-js/dist/assets/dmn-font/css/dmn.css");
var Editor = /** @class */ (function () {
    function Editor(editor, mediator) {
        this.editor = editor;
        this.mediator = mediator;
    }
    return Editor;
}());
var editors = {};
window.DasContractComponents.ContractEditor.ActivitiesEditor.BusinessRuleEditor = {
    InitBPMN: function (id, editorXML, mediatorReference) {
        //console.log("Viewer:");
        //console.log(Modeler);
        //Clear the target
        var targetElement = document.getElementById(id);
        targetElement.innerHTML = "";
        //Create the editor
        var editor = new Modeler_1["default"]({
            container: "#" + id,
            keyboard: {
                bindTo: document
            }
        });
        //Save the editor
        editors[id] = new Editor(editor, mediatorReference);
        //Set the diagram
        this.SetDiagramXML(id, editorXML);
        //Hook up the change callback
        //editor.on("views.changed", function ()
        //{
        //    mediatorReference.invokeMethodAsync("DiagramChangeCallback");
        //});
    },
    SetDiagramXML: function (id, editorXML) {
        var savedEditor = editors[id];
        //Init the editor
        savedEditor.editor.importXML(editorXML, function (err) {
            if (err)
                throw new Error("Error importing DMN XML");
            //var canvas = editor.get("canvas");
            //canvas.zoom("fit-viewport");
        });
    },
    GetDiagramXML: function (id) {
        var res = null;
        editors[id].editor.saveXML(function (err, xml) {
            if (err)
                throw new Error("Error getting diagram xml");
            res = xml;
        });
        if (res == null)
            throw new Error("Returning string is null");
        return res;
    },
    RequestEditorsRedraw: function () {
        for (var id in editors) {
            if (document.getElementById(id))
                this.RequestEditorRedraw(id);
        }
    },
    RequestEditorRedraw: function (id) {
        editors[id].mediator.invokeMethodAsync("RequestEditorRedrawCallback");
    }
};
