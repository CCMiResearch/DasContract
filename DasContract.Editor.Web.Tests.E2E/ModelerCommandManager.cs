﻿using DasContract.Abstraction;
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
        public const string DEFAULT_PROCESS_ID = "Process_1";

        private Contract _contract;
        private int _posCounter;

        private Stack<XElement> _undoableStates = new Stack<XElement>();
        private Stack<XElement> _redoableStates = new Stack<XElement>();

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
                            window.commandStack = window.modeling._commandStack;

                            window.process = elementRegistry.get('Process_1');
                            window.startEvent = elementRegistry.get('StartEvent_1');
                    }");
        }

        public XElement GetContractAsXElement()
        {
            return new Contract(_contract.ToXElement()).ToXElement();
        }

        public async Task<string> CreateNewProcess(IPage page, bool first)
        {
            AddCurrentStateAsUndoable();

            dynamic result;
            if (first)
            {
                result = await page.EvaluateAsync<ExpandoObject>(@$"const participant = window.elementFactory.createParticipantShape({{ type: 'bpmn:Participant' }});
                   const addedParticipant = window.modeling.createShape(participant, {{ x: 400, y: 100 }}, process);
                   Object.freeze({{procId: addedParticipant.businessObject.processRef.id, participantId: addedParticipant.id}});");
            }
            else
            {
                result = await page.EvaluateAsync<ExpandoObject>(@"const collaboration = window.elementRegistry.filter(function (element) {
                        return element.type == ""bpmn:Collaboration"";
                        } )[0];

                        const participant = window.elementFactory.createParticipantShape({
                            type: ""bpmn:Participant""
                        });
                        window.modeling.createShape(participant, { x: 400, y: 500 }, collaboration);
                        Object.freeze({procId: participant.businessObject.processRef.id, participantId: participant.id});"
                    );
            }

            if (!first)
            {
                _contract.Processes.Add(new Process { Id = result.procId, BpmnId = result.procId });
            }

            _contract.Processes.Single(p => p.Id == result.procId).ParticipantId = result.participantId;

            return result.participantId;
        }

        public async Task<string> AddProcessElement<T>(IPage page, string processId) where T : ProcessElement, new()
        {
            AddCurrentStateAsUndoable();

            await page.EvaluateAsync(
                $"window.addedElement = window.elementFactory.createShape({{ type: '{typeMappings[typeof(T)]}' }})");
            dynamic result = await page.EvaluateAsync<ExpandoObject>("window.addedElement");
            var id = result.id;

            var addedElement = new T { Id = id };
            var process = GetProcess(processId);
            process.ProcessElements.Add(id, addedElement);

            var procSelector = processId == DEFAULT_PROCESS_ID ? "process" : $"window.elementRegistry.get('{processId}')";

            await page.EvaluateAsync(
                $"window.modeling.createShape(window.addedElement, {{ x: {_posCounter}, y: {_posCounter} }}, {procSelector})");
            _posCounter += 100;
            return id;
        }

        public async Task RemoveProcessElement(IPage page, string processId, string elementId)
        {
            AddCurrentStateAsUndoable();

            await page.EvaluateAsync(
                $"window.modeling.removeShape(window.elementRegistry.get('{elementId}'))");

            var process = _contract.Processes.Single(p => p.Id == processId);
            process.ProcessElements.Remove(elementId);

        }

        public async Task RenameElement(IPage page, string processId, string elementId, string newName)
        {
            AddCurrentStateAsUndoable();

            await page.EvaluateAsync(
                $"window.modeling.updateProperties(window.elementRegistry.get('{elementId}'), {{name: '{newName}'}})");

            var process = _contract.Processes.Single(p => p.Id == processId);
            var element = process.ProcessElements[elementId];
            element.Name = newName;
        }

        public async Task EditProcessElementId(IPage page, string processId, string oldId, string newId)
        {
            AddCurrentStateAsUndoable();
            await page.EvaluateAsync(
                $"window.modeling.updateProperties(window.elementRegistry.get('{oldId}'), {{id: '{newId}'}})");

            var process = _contract.Processes.Single(p => p.Id == processId);

            var element = process.ProcessElements[oldId];

            var incomingToUpdate = process.SequenceFlows.Where(s => element.Incoming.Contains(s.Key));
            foreach (var seqFlow in incomingToUpdate)
            {
                seqFlow.Value.TargetId = newId;
            }
            var outgoingToUpdate = process.SequenceFlows.Where(s => element.Outgoing.Contains(s.Key));
            foreach (var seqFlow in outgoingToUpdate)
            {
                seqFlow.Value.SourceId = newId;
            }
            //If the element is a task, then it might have boundary events attached to it
            if (element is Abstraction.Processes.Tasks.Task)
            {
                var boundaryEvents = process.ProcessElements.Values
                    .Where(e => e is BoundaryEvent)
                    .Select(e => e as BoundaryEvent);
                foreach (var boundaryEvent in boundaryEvents)
                {
                    if (boundaryEvent.AttachedTo == element.Id)
                        boundaryEvent.AttachedTo = newId;
                }
            }

            process.ProcessElements.Remove(oldId);
            element.Id = newId;
            process.ProcessElements.Add(newId, element);
        }

        public async Task EditConnectionId(IPage page, string processId, string oldId, string newId)
        {
            AddCurrentStateAsUndoable();
            await page.EvaluateAsync(
                $"window.modeling.updateProperties(window.elementRegistry.get('{oldId}'), {{id: '{newId}'}})");

            var process = _contract.Processes.Single(p => p.Id == processId);

            var sequenceFlow = process.SequenceFlows[oldId];

            if (process.ProcessElements.TryGetValue(sequenceFlow.SourceId, out var source))
            {
                source.Outgoing.Remove(sequenceFlow.Id);
                source.Outgoing.Add(newId);
            }

            if (process.ProcessElements.TryGetValue(sequenceFlow.TargetId, out var target))
            {
                target.Incoming.Remove(sequenceFlow.Id);
                target.Incoming.Add(newId);
            }

            process.SequenceFlows.Remove(oldId);
            sequenceFlow.Id = newId;
            process.SequenceFlows.Add(newId, sequenceFlow);
        }

        public async Task SetTaskLoopCharacteristic(IPage page, string processId, string elementId, InstanceType instanceType)
        {
            AddCurrentStateAsUndoable();

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
            AddCurrentStateAsUndoable();

            await page.EvaluateAsync(
                $"window.modeling.updateProperties(window.elementRegistry.get('{connectionId}'), {{name: '{newName}'}})");

            var process = _contract.Processes.Single(p => p.Id == processId);
            var sequenceFlow = process.SequenceFlows[connectionId];
            sequenceFlow.Name = newName;
        }

        public async Task<string> ConnectElements(IPage page, string processId, string sourceId, string targetId)
        {
            AddCurrentStateAsUndoable();

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

        public void AddCurrentStateAsUndoable()
        {
            _undoableStates.Push(_contract.ToXElement());
        }

        public async Task Undo(IPage page)
        {
            await page.EvaluateAsync("window.commandStack.undo()");
            _redoableStates.Push(_contract.ToXElement());
            var state = _undoableStates.Pop();
            _contract = new Contract(state);
        }

        public async Task Redo(IPage page)
        {
            await page.EvaluateAsync("window.commandStack.redo()");
            _undoableStates.Push(_contract.ToXElement());
            var state = _redoableStates.Pop();
            _contract = new Contract(state);
        }

        private Process GetProcess(string id)
        {
            return _contract.Processes.Single(p => p.Id == id || p.ParticipantId == id);
        }

        private void InitializeModel()
        {
            _contract = new Contract { Id = "Contract" };

            var startEvent = new StartEvent { Id = "StartEvent_1" };
            var process = new Process { Id = DEFAULT_PROCESS_ID, IsExecutable = true, BpmnId = DEFAULT_PROCESS_ID };
            process.ProcessElements.Add("StartEvent_1", startEvent);
            _contract.Processes.Add(process);
        }
    }
}
