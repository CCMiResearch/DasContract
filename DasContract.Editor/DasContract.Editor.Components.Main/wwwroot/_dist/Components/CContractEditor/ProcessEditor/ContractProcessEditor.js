exports.__esModule = true;
var Modeler_1 = require("bpmn-js/lib/Modeler");
require("bpmn-js/dist/assets/diagram-js.css");
require("bpmn-js/dist/assets/bpmn-font/css/bpmn.css");
//import ""
//declare let Modeler: any;
var editors = {};
window.DasContractComponents.ContractEditor.ContractProcessEditor = {
    InitBPMN: function (id, editorXML, mediatorReference) {
        //console.log("Modeler:");
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
        editors[id] = editor;
        //Set the diagram
        this.SetDiagramXML(id, editorXML);
        //Hook up the change callback
        editor.on('commandStack.changed', function () {
            mediatorReference.invokeMethodAsync("DiagramChangeCallback");
        });
    },
    SetDiagramXML: function (id, editorXML) {
        var editor = editors[id];
        //Init the editor
        if (editorXML != "" && editorXML != null) {
            editor.importXML(editorXML, function (err) {
                if (err)
                    throw new Error("Error importing BPMN XML");
                var canvas = editor.get("canvas");
                canvas.zoom("fit-viewport");
            });
        }
        else {
            editor.createDiagram();
        }
    },
    GetDiagramXML: function (id) {
        var res = null;
        editors[id].saveXML(function (err, xml) {
            if (err)
                throw new Error("Error getting diagram xml");
            res = xml;
        });
        if (res == null)
            throw new Error("Returning string is null");
        return res;
    }
};
