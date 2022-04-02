using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public static class BpmnConstants
    {
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
