import Modeler from 'bpmn-js/lib/Modeler';

export function hookEvents() {
    console.log("Hooking events");
    var eventBus = window.modeler.get('eventBus');

    // you may hook into any of the following events
    var events = [
        'element.hover',
        'element.out',
        'element.click',
        'element.dblclick',
        'element.mousedown',
        'element.mouseup',
        'create.ended',
        'commandStack.elements.create.postExecute',
        'shape.added'
    ];

    events.forEach(function (event) {

        eventBus.on(event, function (e) {
            // e.element = the model element
            // e.gfx = the graphical element

            console.log(event, 'on', e);
        });
    });
}
// create a modeler
export function createModeler() {

    console.log("hello 2");
    window.modeler = new Modeler({
        container: document.getElementById('canvas')
    });

    window.modeler.createDiagram();
    hookEvents();
}


export async function getDiagramXML() {
    var xml = (await window.modeler.saveXML({ format: true })).xml;
    debugger
    return xml;
}