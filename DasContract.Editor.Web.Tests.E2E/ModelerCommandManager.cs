using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Task = System.Threading.Tasks.Task;

namespace DasContract.Editor.Web.Tests.E2E
{
    public class ModelerCommandManager
    {
        private Contract _contract;
        private int _posCounter;

        private IDictionary<Type, string> typeMappings = new Dictionary<Type, string>()
        {
            { typeof(StartEvent), "bpmn:StartEvent" },
            { typeof(EndEvent), "bpmn:EndEvent" },
            { typeof(Abstraction.Processes.Tasks.Task), "bpmn:Task" },
            { typeof(UserTask), "bpmn:UserTask" },
            { typeof(ScriptTask), "bpmn:ScriptTask" },
            { typeof(ServiceTask), "bpmn:ServiceTask" },
            { typeof(BusinessRuleTask), "bpmn:BusinessRuleTask" },
            { typeof(CallActivity), "bpmn:CallActivity" },
            { typeof(ParallelGateway), "bpmn:ParallelGateway" },
            { typeof(ExclusiveGateway), "bpmn:ExclusiveGateway" },
            { typeof(Event), "bpmn:IntermediateThrowEvent" },
            { typeof(BoundaryEvent), "bpmn:BoundaryEvent" },
            { typeof(TimerBoundaryEvent), "bpmn:TimerBoundaryEvent" },
        };

        public async Task SetupVariables(IPage page)
        {
            InitializeModel();
            await page.EvaluateAsync(@"() =>{ const modeler = window.modeler;// (1) Get the modules
                            window.elementFactory = modeler.get('elementFactory');
                            window.elementRegistry = modeler.get('elementRegistry');
                            window.modeling = modeler.get('modeling');
                            window.bpmnFactory = modeler.get('bpmnFactory');

                            window.process = elementRegistry.get('Process_1');
                            window.startEvent = elementRegistry.get('StartEvent_1');
                    }");
        }

        public XElement GetContractAsXElement()
        {
            return _contract.ToXElement();
        }

        public async Task<string> CreateNewProcess(IPage page, bool first)
        {
            if (first)
            {
                await page.EvaluateAsync(@"window.modeling.makeProcess();");
            }

            dynamic result = await page.EvaluateAsync<ExpandoObject>(
                @$"const participant = window.elementFactory.createParticipantShape({{ type: 'bpmn:Participant' }});
                   const addedParticipant = window.modeling.createShape(participant, {{ x: 400, y: 100 }}, process);
                   Object.freeze({{procId: addedParticipant.businessObject.processRef.id, participantId: addedParticipant.id}})");

            if (!first)
            {
                _contract.Processes.Add(new Process { Id = result.procId });
            }

            _contract.Processes.Single(p => p.Id == result.procId).ParticipantId = result.participantId;

            return result.participantId;
        }

        public async Task<string> AddProcessElement<T>(IPage page, string processId) where T : ProcessElement, new()
        {
            await page.EvaluateAsync(
                $"window.addedElement = window.elementFactory.createShape({{ type: '{typeMappings[typeof(T)]}' }})");
            dynamic result = await page.EvaluateAsync<ExpandoObject>("window.addedElement");
            var id = result.id;

            var addedElement = new T { Id = id };
            var process = _contract.Processes.Single(p => p.Id == processId);
            process.ProcessElements.Add(id, addedElement);

            await page.EvaluateAsync(
                $"window.modeling.createShape(window.addedElement, {{ x: {_posCounter}, y: {_posCounter} }}, process)");
            _posCounter += 100;
            return id;
        }

        public async Task RemoveProcessElement(IPage page, string processId, string elementId)
        {
            await page.EvaluateAsync(
                $"window.modeling.removeShape(window.elementRegistry.get('{elementId}'))");

            var process = _contract.Processes.Single(p => p.Id == processId);
            process.ProcessElements.Remove(elementId);

        }

        public async Task RenameElement(IPage page, string processId, string elementId, string newName)
        {
            await page.EvaluateAsync(
                $"window.modeling.updateProperties(window.elementRegistry.get('{elementId}'), {{name: '{newName}'}})");

            var process = _contract.Processes.Single(p => p.Id == processId);
            var element = process.ProcessElements[elementId];
            element.Name = newName;
        }

        public async Task SetTaskLoopCharacteristic(IPage page, string processId, string elementId, InstanceType instanceType)
        {
            string loopCharacteristicInput;
            if (instanceType == InstanceType.Single)
                loopCharacteristicInput = "undefined";
            else
                loopCharacteristicInput = "window.bpmnFactory.create('bpmn:MultiInstanceLoopCharacteristics')";


            await page.EvaluateAsync(
                @$"window.modeling.updateProperties(window.elementRegistry.get('{elementId}'), {{
                    loopCharacteristics: {loopCharacteristicInput}
                }})");
            var process = _contract.Processes.Single(p => p.Id == processId);
            process.Tasks.Single(t => t.Id == elementId).InstanceType = instanceType;
        }

        public async Task RenameConnection(IPage page, string processId, string connectionId, string newName)
        {
            await page.EvaluateAsync(
                $"window.modeling.updateProperties(window.elementRegistry.get('{connectionId}'), {{name: '{newName}'}})");

            var process = _contract.Processes.Single(p => p.Id == processId);
            var sequenceFlow = process.SequenceFlows[connectionId];
            sequenceFlow.Name = newName;
        }

        public async Task<string> ConnectElements(IPage page, string processId, string sourceId, string targetId)
        {
            dynamic result = await page.EvaluateAsync<ExpandoObject>(
                $"window.modeling.connect(window.elementRegistry.get('{sourceId}'), window.elementRegistry.get('{targetId}'))");
            var connectionId = result.id;

            var process = _contract.Processes.Single(p => p.Id == processId);
            var connection = new SequenceFlow { Id = connectionId, SourceId = sourceId, TargetId = targetId };
            process.SequenceFlows.Add(connectionId, connection);
            process.ProcessElements[sourceId].Outgoing.Add(connectionId);
            process.ProcessElements[targetId].Incoming.Add(connectionId);

            return connectionId;
        }

        private void InitializeModel()
        {
            _contract = new Contract { Id = "Contract" };

            var startEvent = new StartEvent { Id = "StartEvent_1" };
            var process = new Process { Id = "Process_1", IsExecutable = true };
            process.ProcessElements.Add("StartEvent_1", startEvent);
            _contract.Processes.Add(process);
        }
    }
}
