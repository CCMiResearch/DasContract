//const Modeler = require("dmn-js");
import Modeler from "dmn-js/lib/Modeler";
import "dmn-js/dist/assets/diagram-js.css";
import "dmn-js/dist/assets/dmn-js-decision-table.css";
import "dmn-js/dist/assets/dmn-js-decision-table-controls.css";
import "dmn-js/dist/assets/dmn-js-drd.css";
import "dmn-js/dist/assets/dmn-js-literal-expression.css";
import "dmn-js/dist/assets/dmn-js-shared.css";
import "dmn-js/dist/assets/dmn-font/css/dmn.css";

let editors: any = {};

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
        editors[id] = editor;

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
        let editor = editors[id];

        //Init the editor
        editor.importXML(editorXML, function (err: any)
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
        editors[id].saveXML(function (err: any, xml: any)
        {
            if (err)
                throw new Error("Error getting diagram xml");

            res = xml;
        });

        if (res == null)
            throw new Error("Returning string is null");
        return res;
    }
};