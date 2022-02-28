import Modeler from 'bpmn-js/lib/Modeler';

// create a modeler
export function createModeler() {
    window.modeler = new Modeler({
        container: document.getElementById('canvas')
    });

    window.modeler.createDiagram();
}

export async function getDiagramXML() {
    var xml = (await window.modeler.saveXML({ format: true })).xml;
    debugger
    return xml;
}