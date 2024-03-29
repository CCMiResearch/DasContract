﻿import Modeler from 'bpmn-js/lib/Modeler';
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
import ContextPadProvider from 'bpmn-js/lib/features/context-pad/ContextPadProvider';

import customRules from "./modellerCustomRules";
import { downloadSvg, downloadSvgAsPng } from "./fileSaver";

const ELEMENT_EVENTS = [
    'element.changed',
    'element.click',
    'shape.added',
    'shape.create',
    'shape.removed',
    'element.updateId',
    'connection.added',
    'connection.removed',
    'root.added',
    'root.removed'
];


export function hookEvents() {
    var eventBus = window.modeler.get('eventBus');

    ELEMENT_EVENTS.forEach(function (event) {
        eventBus.on(event, eventHandler)
    });
}

export function unhookEvents() {
    var eventBus = window.modeler.get('eventBus');

    ELEMENT_EVENTS.forEach(function (event) {
        eventBus.off(event, eventHandler)
    });
}

async function eventHandler(e) {
    if (modellerLib.eventHandlerInstanceRef != null) {
        let eventObj = copyEventInformation(e);
        await modellerLib.eventHandlerInstanceRef.invokeMethodAsync("HandleBpmnElementEvent", eventObj);
    }
}

export async function saveAsSvg(diagramName) {
    const svg = (await window.modeler.saveSVG()).svg;
    downloadSvg(`process-diagram-${diagramName}.svg`, svg);
}

export async function saveAsPng(diagramName) {
    const viewport = $('.viewport')[0].getBoundingClientRect();
    const svg = (await window.modeler.saveSVG()).svg;
    downloadSvgAsPng(`process-diagram-${diagramName}.png`, svg, viewport.width, viewport.height );
}

export function updateElementName(elementId, elementName) {
    const elementRegistry = window.modeler.get('elementRegistry'),
        modeling = window.modeler.get('modeling');

    const element = elementRegistry.get(elementId);

    modeling.updateProperties(element, { name: elementName});
}

export function updateElementId(oldElementId, newElementId) {
    const elementRegistry = window.modeler.get('elementRegistry'),
        modeling = window.modeler.get('modeling');

    const element = elementRegistry.get(oldElementId);

    modeling.updateProperties(element, { id: newElementId });
}

function copyEventInformation(e) {
    let eventObj = { type: e.type };
    if (e.newId != null)
        eventObj.newId = e.newId;
    if (e.element != null) {
        eventObj.element = {
            id: e.element.id,
            type: e.element.type,
            processId: getElementProcessRef(e.element)
        }
        copyIncomingAndOutgoing(e, eventObj.element);
        copySourceAndTarget(e, eventObj.element);
        let businessObject = e.element.businessObject;
        if (businessObject != null) {
            eventObj.element.name = businessObject.name;
            //Copy loop information about a task element
            if (businessObject.loopCharacteristics != null) {
                eventObj.element.isSequential = businessObject.loopCharacteristics.isSequential;
                eventObj.element.loopType = businessObject.loopCharacteristics.$type;
            }
            //Copy attached to information about a boundary event
            if (businessObject.attachedToRef != null) {
                eventObj.element.attachedTo = businessObject.attachedToRef.id;
            }
            //Copy additional informations about an event element
            if (businessObject.eventDefinitions != null) {
                if (businessObject.eventDefinitions[0] != null) {
                    if (e.element.type == "bpmn:BoundaryEvent" && businessObject.eventDefinitions[0].$type == "bpmn:TimerEventDefinition")
                        eventObj.element.type = "bpmn:TimerBoundaryEvent";
                }
            }
        }
    }
    console.log(eventObj);
    return eventObj;
}

function copySourceAndTarget(event, newElement) {
    if (event.element.source != null) {
        newElement.source = event.element.source.id;
    }
    if (event.element.source != null) {
        newElement.target = event.element.target.id;
    }

}

function copyIncomingAndOutgoing(event, newElement) {
    if (event.element.incoming != null) {
        newElement.incoming = [];
        event.element.incoming.forEach((inc) => {
            newElement.incoming.push(inc.id);
        })
    }
    if (event.element.outgoing != null) {
        newElement.outgoing = [];
        event.element.outgoing.forEach((outg) => {
            newElement.outgoing.push(outg.id);
        })
    }
}

//Newly added shapes do not yet have parent process id estabilished in their businessObject.
//The shape does however point to a participant that contains this reference
function getElementProcessRef(element) {
    let businessObject = element.businessObject;
    if (businessObject != null) {
        let businessObjectParent = businessObject.$parent;
        if (businessObject.processRef != null)
            return businessObject.processRef.id;
        if (businessObjectParent != null) {
            if (businessObjectParent.type == "bpmn:Process")
                return businessObjectParent.id;
        }
        
    }
    let elementParent = element.parent;
    if (elementParent != null) {
        if (elementParent.type == "bpmn:Process")
            return elementParent.id;
        if (elementParent.type == "bpmn:Participant") {
            return elementParent.businessObject.processRef.id;
        }
    }

    return "";
}

// create a modeler
export async function createModeler(modelerXml, canvasId) {
    window.modeler = new Modeler({
        container: document.getElementById(canvasId),
        keyboard: {
            bindTo: document
        },
        additionalModules: [customRules]
    });

    unhookEvents();

    if (modelerXml !== '') {
        await window.modeler.importXML(modelerXml);
        hookEvents(); //Hook events after the import is done
    }
    else {
        hookEvents(); //Fresh diagram, the events must be hooked right away to register the default start event
        window.modeler.createDiagram(modelerXml); 
    }

    window.modelerElement = document.getElementById(canvasId);
    removeUnusedReplaceMenuItems();
}

export function restoreModelerElement(canvasId) {
    let canvasElement = document.getElementById(canvasId);
    if (canvasElement != null) {                
        canvasElement.parentNode.replaceChild(window.modelerElement, canvasElement);
    }
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

//Removes unused context pad entries for pool object
var _getContextPadEntries = ContextPadProvider.prototype.getContextPadEntries;
ContextPadProvider.prototype.getContextPadEntries = function (element) {
    var entries = _getContextPadEntries.call(this, element);
    console.log(entries);
    delete entries['lane-divide-three'];
    delete entries['lane-divide-two'];
    delete entries['lane-insert-above'];
    delete entries['lane-insert-below'];
    delete entries['append.intermediate-event'];
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
    var xml = (await window.modeler.saveXML()).xml;
    return xml;
}