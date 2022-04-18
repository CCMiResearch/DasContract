using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public class ProcessModelManager : IProcessModelManager
    {
        private Contract Contract { get; set; }

        private readonly Dictionary<string, Stack<ProcessElement>> _deletedElements = new();
        private readonly Dictionary<string, SequenceFlow> _deletedSequenceFlows = new();
        private readonly Dictionary<string, Process> _deletedProcesses = new();


        public void SetContract(Contract contract)
        {
            _deletedElements.Clear();
            _deletedSequenceFlows.Clear();
            _deletedProcesses.Clear();
            Contract = contract;
        }

        public bool TryGetProcess(string id, out Process process)
        {
            return Contract.TryGetProcess(id, out process);
        }

        public IList<string> GetAllProcessIds()
        {
            return Contract.Processes.Select(p => p.Id).ToList();
        }

        public bool IsElementIdAvailable(string id)
        {
            return Contract.Processes.All(
                p => !p.ProcessElements.ContainsKey(id) &&
                !p.SequenceFlows.ContainsKey(id) &&
                p.Id != id);
        }

        public string GetProcessIdFromParticipantId(string participantId)
        {
            var proc = Contract.Processes.Where(p => p.ParticipantId == participantId);
            if (proc.Count() != 1)
                throw new InvalidIdException($"Participant id {participantId} could not be found");
            return proc.First().Id;
        }
        //A participant might be associated with the process (but not always)
        public void AddNewProcess(string processId, string participantId = null)
        {
            Process process;
            if (participantId == null && Contract.Processes.Count > 0)
            {
                RemoveProcess(Contract.Processes.First().Id);
            }
            //Copy the existing process if the input participant is not defined
            //and a process already exists (this happens when the last participant is removed),
            //or when an input participant is defined and no other participants are yet present in the model
            //(this happens when the first participant is added)
            if (Contract.Processes.Count == 1 && participantId != null && Contract.Processes.First().ParticipantId == null)
            {
                if (_deletedProcesses.TryGetValue(processId, out process))
                {
                    RemoveProcess(Contract.Processes.First().Id);
                    _deletedProcesses.Remove(processId);
                    Contract.Processes.Add(process);
                }
                else
                {
                    process = Contract.Processes.First();
                    process.ParticipantId = participantId;
                }
            }
            else
            {
                if (Contract.Processes.Any(p => p.Id == processId))
                    throw new DuplicateIdException($"Could not add new process, contract already contains process id {processId}");

                if (_deletedProcesses.TryGetValue(processId, out process))
                {
                    _deletedProcesses.Remove(processId);
                    process.ParticipantId = participantId;
                }
                else
                {
                    process = new Process { Id = processId, ParticipantId = participantId, BpmnId = processId };
                }
                Contract.Processes.Add(process);
            }

            if (Contract.Processes.Count == 1)
            {
                process.IsExecutable = true;
            }
        }

        public void UpdateProcessId(Process process, string newProcessId)
        {
            if (Contract.Processes.Count(p => p.Id == newProcessId) > 0)
                throw new DuplicateIdException($"Id cannot be changed, because process with id {newProcessId} already exists");

            foreach (var p in Contract.Processes)
            {
                var callActivites = p.Tasks
                    .Where(e => e is CallActivity)
                    .Select(e => e as CallActivity);
                foreach (var callActivity in callActivites)
                {
                    if (callActivity.CalledElement == process.Id)
                    {
                        callActivity.CalledElement = newProcessId;
                    }
                }
            }
            process.Id = newProcessId;
        }

        public string TranslateBpmnProcessId(string bpmnProcessId)
        {
            var proc = Contract.Processes.SingleOrDefault(p => p.BpmnId == bpmnProcessId);

            if (proc != null)
                return proc.Id;

            proc = _deletedProcesses.Values.SingleOrDefault(p => p.BpmnId == bpmnProcessId);

            return proc?.Id;
        }

        public void RemoveProcess(string processId)
        {
            if (!TryGetProcess(processId, out var process))
                throw new InvalidIdException($"Could not delete process, contract does not contain process id {processId}");

            _deletedProcesses.Add(processId, process);
            Contract.Processes.Remove(process);
        }

        public IList<Process> GetAllProcesses()
        {
            return Contract.Processes;
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
            foreach (var process in Contract.Processes)
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
                catch (InvalidBpmnElementTypeException)
                {
                    return null;
                }
            }

            var process = GetProcess(processId);

            if (process.ProcessElements.ContainsKey(elementId))
                throw new DuplicateIdException($"Process already contains id {elementId}");

            process.ProcessElements.Add(elementId, element);
            return element;
        }

        public void RemoveElement(string id)
        {
            foreach (var process in Contract.Processes)
            {
                if (process.ProcessElements.ContainsKey(id))
                {
                    //Store the deleted element in the deletion buffer
                    var element = process.ProcessElements[id];
                    AddElementToDeletedBuffer(element);

                    process.ProcessElements.Remove(id);
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
            return sequenceFlow;
        }

        public void RemoveSequenceFlow(string id)
        {
            foreach (var process in Contract.Processes)
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
            if (!TryGetProcess(processId, out var process))
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
            return Contract.Processes.Any(p => p.Id == processId);
        }

        public bool TryRetrieveProcessOfElement(string elementId, out Process process)
        {
            foreach (var p in Contract.Processes)
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
            if (!TryGetProcess(prevProcessId, out var prevProcess))
                throw new InvalidIdException($"Process id {prevProcessId} does not exist");
            if (!TryGetProcess(newProcessId, out var newProcess))
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
            if (!TryRetrieveProcessOfElement(elementId, out var process))
                throw new InvalidIdException($"Element id {elementId} is not located in any process");
            if (!TryRetrieveElementById(elementId, process.Id, out var element))
                throw new InvalidIdException($"Element id {elementId} does not exist");

            UpdateSeqFlowList(element.Incoming, flowId, add);
        }

        private void UpdateOutgoingOfElement(string elementId, string flowId, bool add)
        {
            //The target element might be in a different process than the sequence flow (the order of updates in bpmn is a bit weird)
            //For that reason, the process of the element must be first retrieved
            if (!TryRetrieveProcessOfElement(elementId, out var process))
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

        public string GetProcessBpmnDefinition()
        {
            return Contract.ProcessDiagram;
        }

        public void SetProcessBpmnDefinition(string diagramXml)
        {
            Contract.ProcessDiagram = diagramXml;
        }

    }
}
