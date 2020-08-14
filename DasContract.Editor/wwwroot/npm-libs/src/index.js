import Modeler from 'bpmn-js/lib/Modeler';

// create a modeler
export function CreateModeler() {
    var editor = new Modeler({
        container: document.getElementById('canvas')
    });

    editor.createDiagram();
}