using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.BpmnEvents;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public static class ProcessElementFactory
    {
        public static ProcessElement CreateElementFromType(string type)
        {
            switch (type)
            {
                case BpmnConstants.BPMN_ELEMENT_START_EVENT:
                    return new StartEvent();
                case BpmnConstants.BPMN_ELEMENT_END_EVENT:
                    return new EndEvent();
                case BpmnConstants.BPMN_ELEMENT_TASK:
                    return new Abstraction.Processes.Tasks.Task();
                case BpmnConstants.BPMN_ELEMENT_USER_TASK:
                    return new UserTask();
                case BpmnConstants.BPMN_ELEMENT_SCRIPT_TASK:
                    return new ScriptTask();
                case BpmnConstants.BPMN_ELEMENT_SERVICE_TASK:
                    return new ServiceTask();
                case BpmnConstants.BPMN_ELEMENT_BUSINESS_RULE_TASK:
                    return new BusinessRuleTask();
                case BpmnConstants.BPMN_ELEMENT_CALL_ACTIVITY:
                    return new CallActivity();
                case BpmnConstants.BPMN_ELEMENT_PARALLEL_GATEWAY:
                    return new ParallelGateway();
                case BpmnConstants.BPMN_ELEMENT_EXCLUSIVE_GATEWAY:
                    return new ExclusiveGateway();
                case BpmnConstants.BPMN_ELEMENT_INTERMEDIATE_THROW_EVENT:
                    return new Event();
                case BpmnConstants.BPMN_ELEMENT_BOUNDARY_EVENT:
                    return new BoundaryEvent();
                case BpmnConstants.BPMN_ELEMENT_TIMER_BOUNDARY_EVENT:
                    return new TimerBoundaryEvent();
                default:
                    throw new InvalidBpmnElementTypeException($"{type} is not a valid element type");
            }
        }
    }
}
