using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public static class ProcessElementFactory
    {
        public static ProcessElement CreateElementFromType(string type)
        {
            switch (type)
            {
                case "bpmn:StartEvent":
                    return new StartEvent();
                case "bpmn:EndEvent":
                    return new EndEvent();
                case "bpmn:Task":
                    return new Abstraction.Processes.Tasks.Task();
                case "bpmn:UserTask":
                    return new UserTask();
                case "bpmn:ScriptTask":
                    return new ScriptTask();
                case "bpmn:ServiceTask":
                    return new ServiceTask();
                case "bpmn:BusinessRuleTask":
                    return new BusinessRuleTask();
                case "bpmn:CallActivity":
                    return new CallActivity();
                case "bpmn:ParallelGateway":
                    return new ParallelGateway();
                case "bpmn:ExclusiveGateway":
                    return new ExclusiveGateway();
                case "bpmn:IntermediateThrowEvent":
                    return new Event();
                case "bpmn:BoundaryEvent":
                    return new BoundaryEvent();
                case "bpmn:TimerBoundaryEvent":
                    return new TimerBoundaryEvent();
                default:
                    throw new InvalidBpmnElementTypeException($"{type} is not a valid element type");
            }
        }
    }
}
