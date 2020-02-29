
import Modeler from "bpmn-js/lib/Modeler";
import "bpmn-js/dist/assets/diagram-js.css";
import "bpmn-js/dist/assets/bpmn-font/css/bpmn.css";

//import ""
//declare let Modeler: any;

let editors: any = {};

(window as any).DasContractComponents.ContractEditor.ContractProcessEditor = {



    InitBPMN: function (id: string, editorXML: string, mediatorReference: any)
    {
        //console.log("Modeler:");
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
        editor.on('commandStack.changed', function ()
        {
            mediatorReference.invokeMethodAsync("DiagramChangeCallback");
        });
    },

    SetDiagramXML: function (id: string, editorXML: string)
    {
        let editor = editors[id];

        //Init the editor
        if (editorXML != "" && editorXML != null)
        {
            editor.importXML(editorXML, function (err: any)
            {
                if (err)
                    throw new Error("Error importing BPMN XML");

                var canvas = editor.get("canvas");
                canvas.zoom("fit-viewport");
            });
        }
        else
        {
            editor.createDiagram();
        }
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
}


