//const Modeler = require("dmn-js");
import Modeler from "dmn-js/lib/Modeler";
import "dmn-js/dist/assets/diagram-js.css";
import "dmn-js/dist/assets/dmn-js-decision-table.css";
import "dmn-js/dist/assets/dmn-js-decision-table-controls.css";
import "dmn-js/dist/assets/dmn-js-drd.css";
import "dmn-js/dist/assets/dmn-js-literal-expression.css";
import "dmn-js/dist/assets/dmn-js-shared.css";
import "dmn-js/dist/assets/dmn-font/css/dmn.css";

class Editor
{
    public editor: any;
    public mediator: any;

    constructor(editor: any, mediator: any)
    {
        this.editor = editor;
        this.mediator = mediator;
    }
}

let editors: {[key:string]: Editor} = {};

(window as any).DasContractComponents.ContractEditor.ActivitiesEditor.BusinessRuleEditor = {
    InitBPMN: function (id: string, editorXML: string, mediatorReference: any)
    {
        //console.log("Viewer:");
        //console.log(Modeler);

        //Clear the target
        let targetElement = document.getElementById(id);
        targetElement.innerHTML = "";

        //Create the editor
        let editor = new Modeler({
            container: `#${id}`,
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

    SetDiagramXML: function (id: string, editorXML: string)
    {
        let savedEditor = editors[id];

        //Init the editor
        savedEditor.editor.importXML(editorXML, function (err: any)
        {
            if (err)
                throw new Error("Error importing DMN XML");

            //var canvas = editor.get("canvas");
            //canvas.zoom("fit-viewport");
        });
        
    },

    GetDiagramXML: function (id: string): string
    {
        var res = null;
        editors[id].editor.saveXML(function (err: any, xml: any)
        {
            if (err)
                throw new Error("Error getting diagram xml");

            res = xml;
        });

        if (res == null)
            throw new Error("Returning string is null");
        return res;
    },

    RequestEditorsRedraw: function ()
    {
        for (var id in editors)
        {
            if (document.getElementById(id))
                this.RequestEditorRedraw(id);
        }
    },

    RequestEditorRedraw: function (id: string)
    {
        editors[id].mediator.invokeMethodAsync("RequestEditorRedrawCallback");
    }
};