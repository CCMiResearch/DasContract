using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public static class BpmnConstants
    {

        public const string BPMN_ELEMENT_START_EVENT = "bpmn:StartEvent";
        public const string BPMN_ELEMENT_END_EVENT = "bpmn:EndEvent";
        public const string BPMN_ELEMENT_TASK = "bpmn:Task";
        public const string BPMN_ELEMENT_USER_TASK = "bpmn:UserTask";
        public const string BPMN_ELEMENT_SCRIPT_TASK = "bpmn:ScriptTask";
        public const string BPMN_ELEMENT_SERVICE_TASK = "bpmn:ServiceTask";
        public const string BPMN_ELEMENT_BUSINESS_RULE_TASK = "bpmn:BusinessRuleTask";
        public const string BPMN_ELEMENT_CALL_ACTIVITY = "bpmn:CallActivity";
        public const string BPMN_ELEMENT_PARALLEL_GATEWAY = "bpmn:ParallelGateway";
        public const string BPMN_ELEMENT_EXCLUSIVE_GATEWAY = "bpmn:ExclusiveGateway";
        public const string BPMN_ELEMENT_INTERMEDIATE_THROW_EVENT = "bpmn:IntermediateThrowEvent";
        public const string BPMN_ELEMENT_BOUNDARY_EVENT = "bpmn:BoundaryEvent";
        public const string BPMN_ELEMENT_TIMER_BOUNDARY_EVENT = "bpmn:TimerBoundaryEvent";

        public const string BPMN_ELEMENT_PROCESS = "bpmn:Process";
        public const string BPMN_ELEMENT_PARTICIPANT = "bpmn:Participant";

        public const string BPMN_EVENT_CLICK = "element.click";
        public const string BPMN_EVENT_ELEMENT_CHANGED = "element.changed";
        public const string BPMN_EVENT_SHAPE_ADDED = "shape.added";
        public const string BPMN_EVENT_SHAPE_REMOVED = "shape.removed";
        public const string BPMN_EVENT_UPDATE_ID = "element.updateId";
        public const string BPMN_EVENT_CONNECTION_REMOVED = "connection.removed";
        public const string BPMN_EVENT_CONNECTION_ADDED = "connection.added";
        public const string BPMN_EVENT_ROOT_ADDED = "root.added";
        public const string BPMN_EVENT_ROOT_REMOVED = "root.removed";
    }
}
