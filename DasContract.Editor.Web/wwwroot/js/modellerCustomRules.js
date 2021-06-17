import inherits from 'inherits';

import RuleProvider from 'diagram-js/lib/features/rules/RuleProvider';


export default {
    __init__: ['customRules'],
    customRules: ['type', CustomRules]
};

/**
 * A custom rule provider that decides what elements can be
 * dropped where based on a `vendor:allowDrop` BPMN extension.
 *
 * See {@link BpmnRules} for the default implementation
 * of BPMN 2.0 modeling rules provided by bpmn-js.
 *
 * @param {EventBus} eventBus
 */
function CustomRules(eventBus, elementRegistry) {
    RuleProvider.call(this, eventBus);
    this._elementRegistry = elementRegistry;
}

inherits(CustomRules, RuleProvider);

CustomRules.$inject = ["eventBus", "elementRegistry"];


CustomRules.prototype.init = function () {
    // there exist a number of modeling actions
    // that are identified by a unique ID. We
    // can hook into each one of them and make sure
    // they are only allowed if we say so
    this.addRule('elements.move', 2000, function (context) {
        let ruleFlag = true;
        let target = context.target;
        context.shapes.forEach(function (shape) {
            let shapeBo = shape.businessObject;
            if (target != null
                && (shapeBo.$type == 'bpmn:IntermediateThrowEvent' || shapeBo.$type == 'bpmn:BoundaryEvent')
                && (target.type == 'bpmn:Process' || target.type == 'bpmn:Participant')) {
                console.log(shapeBo);
                console.log(target);
                ruleFlag = false;
            }
        });

        if (!ruleFlag)
            return false;
    });

    this.addRule('shape.create', 2000, function (context) {
        
        var shape = context.shape,
            target = context.target;
        

        // we check for a custom vendor:allowDrop attribute
        // to be present on the BPMN 2.0 xml of the target
        // node
        //
        // we could practically check for other things too,
        // such as incoming / outgoing connections, element
        // types, ...
        var shapeBo = shape.businessObject,
            targetBo = target.businessObject;

        if ((shapeBo.$type == 'bpmn:IntermediateThrowEvent' || shapeBo.$type == 'bpmn:BoundaryEvent')
            && (target.type == 'bpmn:Process' || target.type == 'bpmn:Participant')) {
            console.log(shapeBo);
            console.log(target);
            return false;
        }

        // not returning anything means other rule
        // providers can still do their work
        //
        // this allows us to reuse the existing BPMN rules
    });
};