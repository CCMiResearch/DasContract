using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public class ProcessManager : IProcessManager
    {
        private IContractManager _contractManager;

        private Dictionary<string, Stack<ProcessElement>> _deletedElements = new Dictionary<string, Stack<ProcessElement>>();
        private Dictionary<string, SequenceFlow> _deletedSequenceFlows = new Dictionary<string, SequenceFlow>();

        public ProcessManager(IContractManager contractManager)
        {
            _contractManager = contractManager;
        }

        public bool TryRetrieveIElementById(string elementId, string processId, out IProcessElement element)
        {
            if (TryRetrieveElementById(elementId, processId, out var processElement))
            {
                element = processElement;
                return true;
            }

            if (TryRetrieveSequenceFlowById(elementId, processId, out var sequenceFlow))
            {
                element = sequenceFlow;
                return true;
            }

            element = null;
            return false;
        }

        public bool TryRetrieveIElementById(string elementId, out IProcessElement element)
        {
            foreach (var process in _contractManager.GetAllProcesses())
            {
                if (TryRetrieveIElementById(elementId, process.Id, out element))
                    return true;
            }
            element = null;
            return false;
        }



        public ProcessElement AddElement(string elementType, string elementId, string processId)
        {
            //Try to retrieve element from the deletion buffer (the re-adding of an element might be an undo operation)
            if (!TryGetElementFromDeletedBuffer(elementId, out ProcessElement element))
            {
                //Create a new element, if it is not in the deletion buffer
                try
                {
                    element = ProcessElementFactory.CreateElementFromType(elementType);
                    element.Id = elementId;
                }
                //Invalid element type is ignored (the element is not defined in the dascontract model, for example labels)
                catch (InvalidCamundaElementTypeException)
                {
                    return null;
                }
            }

            var process = GetProcess(processId);

            if (process.ProcessElements.ContainsKey(elementId))
                throw new DuplicateIdException($"Process already contains id {elementId}");

            process.ProcessElements.Add(elementId, element);
            Console.WriteLine($"Number of process elements: {process.ProcessElements.Count()}");
            return element;
        }

        public void RemoveElement(string id)
        {
            foreach (var process in _contractManager.GetAllProcesses())
            {
                if (process.ProcessElements.ContainsKey(id))
                {
                    //Store the deleted element in the deletion buffer
                    var element = process.ProcessElements[id];
                    AddElementToDeletedBuffer(element);

                    process.ProcessElements.Remove(id);
                    Console.WriteLine($"Number of process elements: {process.ProcessElements.Count()}");
                    return;
                }
            }
            throw new InvalidIdException($"Process element cannot be deleted, because id {id} does not exist");
        }

        public void UpdateId(string prevId, string newId, string processId)
        {
            var process = GetProcess(processId);

            if (process.ProcessElements.ContainsKey(newId) || process.ProcessElements.ContainsKey(newId))
                throw new DuplicateIdException($"Id cannot be changed, because process element with id {newId} already exists");

            if (process.ProcessElements.TryGetValue(prevId, out var processElement))
            {
                UpdateProcessElementId(process, processElement, newId);
            }
            else if (process.SequenceFlows.TryGetValue(prevId, out var sequenceFlow))
            {
                UpdateSequenceFlowId(process, sequenceFlow, newId);
            }
            else
            {
                throw new InvalidIdException($"Process element cannot be updated, because id {prevId} does not exist");
            }
        }

        public SequenceFlow AddSequenceFlow(string id, string target, string source, string processId)
        {
            var process = GetProcess(processId);

            if (process.SequenceFlows.ContainsKey(id))
                throw new DuplicateIdException($"Process already contains id {id}");

            //Check if the element is not stored in the deletion buffer
            if (_deletedSequenceFlows.TryGetValue(id, out var sequenceFlow))
            {
                _deletedSequenceFlows.Remove(id);
            }
            else
            {
                sequenceFlow = new SequenceFlow
                {
                    Id = id,
                    SourceId = source,
                    TargetId = target
                };
            }

            process.SequenceFlows.Add(id, sequenceFlow);
            //Add sequence flow references to the source and target elements
            UpdateIncomingOfElement(sequenceFlow.TargetId, sequenceFlow.Id, true);
            UpdateOutgoingOfElement(sequenceFlow.SourceId, sequenceFlow.Id, true);
            Console.WriteLine($"Number of sequence flows: {process.SequenceFlows.Count()}");
            return sequenceFlow;
        }

        public void RemoveSequenceFlow(string id)
        {
            foreach (var process in _contractManager.GetAllProcesses())
            {
                if (process.SequenceFlows.ContainsKey(id))
                {
                    //Store the deleted element in the deletion buffer
                    var sequenceFlow = process.SequenceFlows[id];
                    _deletedSequenceFlows.Add(id, sequenceFlow);
                    //Remove sequence flow references in the source and target elements
                    UpdateIncomingOfElement(sequenceFlow.TargetId, sequenceFlow.Id, false);
                    UpdateOutgoingOfElement(sequenceFlow.SourceId, sequenceFlow.Id, false);
                    process.SequenceFlows.Remove(id);
                    Console.WriteLine($"Number of sequence flows: {process.SequenceFlows.Count()}");
                    return;
                }
            }
            throw new InvalidIdException($"Process element cannot be deleted, because id {id} does not exist");
        }

        public void UpdateSequenceFlowSourceAndTarget(SequenceFlow sequenceFlow, string newSource, string newTarget, string processId)
        {
            if (sequenceFlow.SourceId != newSource)
            {
                //Remove the reference about the sequence flow in the old source
                UpdateOutgoingOfElement(sequenceFlow.SourceId, sequenceFlow.Id, false);
                //Add the reference about the sequence flow to the new source
                UpdateOutgoingOfElement(newSource, sequenceFlow.Id, true);
                sequenceFlow.SourceId = newSource;
            }
            if (sequenceFlow.TargetId != newTarget)
            {
                //Remove the reference about the sequence flow in the old target
                UpdateIncomingOfElement(sequenceFlow.TargetId, sequenceFlow.Id, false);
                //Add the reference about the sequence flow to the new target
                UpdateIncomingOfElement(newTarget, sequenceFlow.Id, true);
                sequenceFlow.TargetId = newTarget;
            }
        }

        public bool TryRetrieveSequenceFlowById(string sequenceFlowId, string processId, out SequenceFlow sequenceFlow)
        {
            var process = GetProcess(processId);

            return process.SequenceFlows.TryGetValue(sequenceFlowId, out sequenceFlow);
        }

        public bool TryRetrieveElementById(string elementId, string processId, out ProcessElement element)
        {
            var process = GetProcess(processId);

            if (process.ProcessElements.TryGetValue(elementId, out var processElement))
            {
                element = processElement;
                return true;
            }
            else
            {
                element = null;
                return false;
            }
        }

        private Process GetProcess(string processId)
        {
            if (!_contractManager.TryGetProcess(processId, out var process))
                throw new InvalidIdException($"Process id {processId} does not exist");
            return process;
        }

        private void AddElementToDeletedBuffer(ProcessElement e)
        {
            if (!_deletedElements.ContainsKey(e.Id))
            {
                _deletedElements.Add(e.Id, new Stack<ProcessElement>());
            }

            var deletedStack = _deletedElements.GetValueOrDefault(e.Id);

            deletedStack.Push(e);
            //Console.WriteLine($"Added id {e.Id} to deletion buffer, it contains {deletedStack.Count} elements");
        }

        private bool TryGetElementFromDeletedBuffer(string elementId, out ProcessElement element)
        {
            var deletedStack = _deletedElements.GetValueOrDefault(elementId);

            if (deletedStack == null || deletedStack.Count == 0)
            {
                element = null;
                return false;
            }

            element = deletedStack.Pop();
            return true;
        }

        public bool ProcessExists(string processId)
        {
            return _contractManager.GetAllProcesses().Any(p => p.Id == processId);
        }

        public bool TryGetProcessOfElement(string elementId, out Process process)
        {
            foreach (var p in _contractManager.GetAllProcesses())
            {
                if (p.ProcessElements.ContainsKey(elementId) || p.SequenceFlows.ContainsKey(elementId))
                {
                    process = p;
                    return true;
                }
            }
            process = null;
            return false;
        }

        public void ChangeProcessOfElement(IProcessElement element, string prevProcessId, string newProcessId)
        {
            if (!_contractManager.TryGetProcess(prevProcessId, out var prevProcess))
                throw new InvalidIdException($"Process id {prevProcessId} does not exist");
            if (!_contractManager.TryGetProcess(newProcessId, out var newProcess))
                throw new InvalidIdException($"Process id {newProcessId} does not exist");

            if (element is ProcessElement)
            {
                prevProcess.ProcessElements.Remove(element.Id);
                newProcess.ProcessElements.Add(element.Id, element as ProcessElement);
            }
            else
            {
                prevProcess.SequenceFlows.Remove(element.Id);
                newProcess.SequenceFlows.Add(element.Id, element as SequenceFlow);
            }
        }

        private void UpdateIncomingOfElement(string elementId, string flowId, bool add)
        {
            //The source element might be in a different process than the sequence flow (the order of updates in bpmn is a bit weird)
            //For that reason, the process of the element must be first retrieved
            if (!TryGetProcessOfElement(elementId, out var process))
                throw new InvalidIdException($"Element id {elementId} is not located in any process");
            if (!TryRetrieveElementById(elementId, process.Id, out var element))
                throw new InvalidIdException($"Element id {elementId} does not exist");

            UpdateSeqFlowList(element.Incoming, flowId, add);
        }

        private void UpdateOutgoingOfElement(string elementId, string flowId, bool add)
        {
            //The target element might be in a different process than the sequence flow (the order of updates in bpmn is a bit weird)
            //For that reason, the process of the element must be first retrieved
            if (!TryGetProcessOfElement(elementId, out var process))
                throw new InvalidIdException($"Element id {elementId} is not located in any process");
            if (!TryRetrieveElementById(elementId, process.Id, out var element))
                throw new InvalidIdException($"Element id {elementId} does not exist");

            UpdateSeqFlowList(element.Outgoing, flowId, add);
        }

        private void UpdateSeqFlowList(IList<string> seqFlowList, string flowId, bool add)
        {
            if (seqFlowList.Contains(flowId))
            {
                if (!add)
                    seqFlowList.Remove(flowId);
            }
            else
            {
                if (add)
                    seqFlowList.Add(flowId);
            }
        }

        private void UpdateProcessElementId(Process process, ProcessElement processElement, string newId)
        {
            //The ids need to be updated in sequence flows that reference this element
            var incomingToUpdate = process.SequenceFlows.Where(s => processElement.Incoming.Contains(s.Key));
            foreach (var seqFlow in incomingToUpdate)
            {
                seqFlow.Value.TargetId = newId;
            }
            var outgoingToUpdate = process.SequenceFlows.Where(s => processElement.Outgoing.Contains(s.Key));
            foreach (var seqFlow in outgoingToUpdate)
            {
                seqFlow.Value.SourceId = newId;
            }
            //If the element is a task, then it might have boundary events attached to it
            if (processElement is Abstraction.Processes.Tasks.Task)
            {
                var boundaryEvents = process.ProcessElements.Values
                    .Where(e => e is BoundaryEvent)
                    .Select(e => e as BoundaryEvent);
                foreach (var boundaryEvent in boundaryEvents)
                {
                    if (boundaryEvent.AttachedTo == processElement.Id)
                        boundaryEvent.AttachedTo = newId;
                }
            }

            process.ProcessElements.Remove(processElement.Id);
            processElement.Id = newId;
            process.ProcessElements.Add(newId, processElement);
        }
        private void UpdateSequenceFlowId(Process process, SequenceFlow sequenceFlow, string newId)
        {
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

            process.SequenceFlows.Remove(sequenceFlow.Id);
            sequenceFlow.Id = newId;
            process.SequenceFlows.Add(newId, sequenceFlow);
        }

    }
}
