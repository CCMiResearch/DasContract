import Modeler from 'bpmn-js/lib/Modeler';

export function hookEvents() {
    console.log("Hooking events");
    var eventBus = window.modeler.get('eventBus');

    // you may hook into any of the following events
    var events = [
        'element.changed',
        'element.click',
        //'element.dblclick',
        //'element.mousedown',
        //'element.mouseup',
        'shape.added',
        'shape.removed',
        //'shape.changed',
        'element.updateId',
        'connection.added'
    ];

    events.forEach(function (event) {

        eventBus.on(event, function (e) {
            // e.element = the model element
            // e.gfx = the graphical element
            if (modellerLib.eventHandlerInstanceRef != null) {
                let eventObj = copyEventInformation(e);
                modellerLib.eventHandlerInstanceRef.invokeMethodAsync("HandleCamundaEvent", eventObj);
            }
            console.log(event, 'on', e, ' element id: ', e.element.id);
        });
    });
}

function copyEventInformation(e) {
    let eventObj = { type: e.type };
    if (e.newId != null)
        eventObj.newId = e.newId;
    if (e.element != null) {
        eventObj.element = {
            id: e.element.id,
            type: e.element.type
        }
        let businessObject = e.element.businessObject;
        if (businessObject != null) {
            eventObj.element.name = businessObject.name;
            if (businessObject.loopCharacteristics != null) {
                eventObj.element.isSequential = businessObject.loopCharacteristics.isSequential;
                eventObj.element.loopType = businessObject.loopCharacteristics.$type;
            }
        }
    }
    return eventObj;
}
// create a modeler
export function createModeler() {
    window.modeler = new Modeler({
        container: document.getElementById('canvas'),
        keyboard: {
            bindTo: document
        }
    });
    hookEvents();
    window.modeler.createDiagram();
}



export function setEventHandlerInstance(dotNetObjectRef) {
    modellerLib.eventHandlerInstanceRef = dotNetObjectRef;
}


export async function getDiagramXML() {
    var xml = (await window.modeler.saveXML({ format: true })).xml;
    debugger
    return xml;
}