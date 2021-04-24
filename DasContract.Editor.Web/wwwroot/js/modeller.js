import Modeler from 'bpmn-js/lib/Modeler';
import PaletteProvider from 'bpmn-js/lib/features/palette/PaletteProvider';
import {
    START_EVENT,
    INTERMEDIATE_EVENT,
    END_EVENT,
    BOUNDARY_EVENT,
    TASK,
    GATEWAY,
    PARTICIPANT 
} from 'bpmn-js/lib/features/replace/ReplaceOptions';
import ReplaceMenuProvider from 'bpmn-js/lib/features/popup-menu/ReplaceMenuProvider';


export function hookEvents() {
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
        'connection.added',
        'connection.removed'
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

export function updateElementName(elementId, elementName) {
    const elementRegistry = window.modeler.get('elementRegistry'),
        modeling = window.modeler.get('modeling');

    const element = elementRegistry.get(elementId);

    modeling.updateProperties(element, { name: elementName});
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

    removeUnusedReplaceMenuItems();
}


//Removes unused palette entries
var _getPaletteEntries = PaletteProvider.prototype.getPaletteEntries;
PaletteProvider.prototype.getPaletteEntries = function (element) {
    var entries = _getPaletteEntries.call(this, element);
    delete entries['create.data-store'];
    delete entries['create.subprocess-expanded'];
    delete entries['create.group'];
    delete entries['create.data-object'];
    return entries;
}

//Removes unused loop entry
var _getLoopEntries = ReplaceMenuProvider.prototype._getLoopEntries;
ReplaceMenuProvider.prototype._getLoopEntries = function (element) {  
    let loopEntries = _getLoopEntries.call(this, element);
    for (var i = loopEntries.length - 1; i >= 0; --i) {
        if (loopEntries[i].id == "toggle-loop") {
            loopEntries.splice(i, 1);
        }
    }
    return loopEntries;
}

function removeUnusedReplaceMenuItems() {
    removeUnusedReplaceMenuItem(TASK, UsedReplaceTaskItems);
    removeUnusedReplaceMenuItem(START_EVENT, UsedReplaceStartEventItems);
    removeUnusedReplaceMenuItem(END_EVENT, UsedReplaceEndEventItems);
    removeUnusedReplaceMenuItem(BOUNDARY_EVENT, UsedReplaceBoundaryEventItems);
    removeUnusedReplaceMenuItem(INTERMEDIATE_EVENT, UsedReplaceIntermediateEventItems);
    removeUnusedReplaceMenuItem(GATEWAY, UsedReplaceGatewayItems);
    removeUnusedReplaceMenuItem(PARTICIPANT, []);
}

function removeUnusedReplaceMenuItem(options, optionsToKeep) {
    for (var i = options.length - 1; i >= 0; --i) {
        if (!optionsToKeep.includes(options[i].label)) {
            options.splice(i, 1);
        }
    }
}

var UsedReplaceTaskItems = [
    'Task',
    'User Task',
    'Business Rule Task',
    'Service Task',
    'Script Task',
    'Call Activity',
]

var UsedReplaceStartEventItems = [
    'Start Event',
    'End Event'
]

var UsedReplaceIntermediateEventItems = [
    'Start Event',
    'End Event',
]

var UsedReplaceEndEventItems = [
    'Start Event',
    'End Event',
]

var UsedReplaceBoundaryEventItems = [
    'Timer Boundary Event'
]

var UsedReplaceGatewayItems = [
    'Exclusive Gateway',
    'Parallel Gateway'
]


export function setEventHandlerInstance(dotNetObjectRef) {
    modellerLib.eventHandlerInstanceRef = dotNetObjectRef;
}

export async function getDiagramXML() {
    var xml = (await window.modeler.saveXML({ format: true })).xml;
    debugger
    return xml;
}