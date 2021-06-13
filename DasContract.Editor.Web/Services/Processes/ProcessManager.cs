using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Processes
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
            foreach(var process in _contractManager.GetAllProcesses())
            {
                if (TryRetrieveIElementById(elementId, process.Id, out element))
                    return true;
            }
            element = null;
            return false;
        }



        public void AddElement(ProcessElement addedElement, string processId)
        {
            var process = GetProcess(processId);
            var id = addedElement.Id;

            if (process.ProcessElements.ContainsKey(addedElement.Id))
                throw new DuplicateIdException($"Process already contains id {addedElement.Id}");

             //Check if the element is not stored in the deletion buffer
            if (TryGetElementFromDeletedBuffer(id, out ProcessElement element))
            {
                addedElement = element;
            }

            process.ProcessElements.Add(id, addedElement);
            Console.WriteLine($"Number of process elements: {process.ProcessElements.Count()}");
        }

        public void RemoveElement(string id)
        {
            foreach(var process in _contractManager.GetAllProcesses())
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

            if (!process.ProcessElements.ContainsKey(prevId))
                throw new InvalidIdException($"Process element cannot be updated, because id {prevId} does not exist");
            if (process.ProcessElements.ContainsKey(newId))
                throw new DuplicateIdException($"Id cannot be changed, because process with id {newId} already exists");

            var element = process.ProcessElements[prevId];
            process.ProcessElements.Remove(prevId);
            element.Id = newId;
            process.ProcessElements.Add(newId, element);
        }

        public void AddSequenceFlow(SequenceFlow addedSequenceFlow, string processId)
        {
            var process = GetProcess(processId);
            var id = addedSequenceFlow.Id;

            if (process.SequenceFlows.ContainsKey(id))
                throw new DuplicateIdException($"Process already contains id {id}");

            //Check if the element is not stored in the deletion buffer
            if (_deletedSequenceFlows.TryGetValue(id, out var deletedSequenceFlow))
            {
                addedSequenceFlow = deletedSequenceFlow;
                _deletedSequenceFlows.Remove(id);
            }

            process.SequenceFlows.Add(id, addedSequenceFlow);
            //Add sequence flow references to the source and target elements
            UpdateIncomingOfElement(addedSequenceFlow.TargetId, addedSequenceFlow.Id, processId, true);
            UpdateOutgoingOfElement(addedSequenceFlow.SourceId, addedSequenceFlow.Id, processId, true);
            Console.WriteLine($"Number of sequence flows: {process.SequenceFlows.Count()}");
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
                    UpdateIncomingOfElement(sequenceFlow.TargetId, sequenceFlow.Id, process.Id, false);
                    UpdateOutgoingOfElement(sequenceFlow.SourceId, sequenceFlow.Id, process.Id, false);
                    process.SequenceFlows.Remove(id);
                    Console.WriteLine($"Number of sequence flows: {process.SequenceFlows.Count()}");
                    return;
                }
            }
            throw new InvalidIdException($"Process element cannot be deleted, because id {id} does not exist");
        }

        public void UpdateSequenceFlowSourceAndTarget(SequenceFlow sequenceFlow, string newSource, string newTarget, string processId)
        {
            if(sequenceFlow.SourceId != newSource)
            {
                //Remove the reference about the sequence flow in the old source
                UpdateOutgoingOfElement(sequenceFlow.SourceId, sequenceFlow.Id, processId, false);
                //Add the reference about the sequence flow to the new source
                UpdateOutgoingOfElement(newSource, sequenceFlow.Id, processId, true);
                sequenceFlow.SourceId = newSource;
            }
            if (sequenceFlow.TargetId != newTarget)
            {
                //Remove the reference about the sequence flow in the old target
                UpdateIncomingOfElement(sequenceFlow.TargetId, sequenceFlow.Id, processId, false);
                //Add the reference about the sequence flow to the new target
                UpdateIncomingOfElement(newTarget, sequenceFlow.Id, processId, true);
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
                if(p.ProcessElements.ContainsKey(elementId) || p.SequenceFlows.ContainsKey(elementId))
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

        private void UpdateIncomingOfElement(string elementId, string flowId, string processId, bool add)
        {
            if (!TryRetrieveElementById(elementId, processId, out var element))
                throw new InvalidIdException($"Element id {elementId} does not exist");

            UpdateSeqFlowList(element.Incoming, flowId, add);
        }

        private void UpdateOutgoingOfElement(string elementId, string flowId, string processId, bool add)
        {
            if (!TryRetrieveElementById(elementId, processId, out var element))
                throw new InvalidIdException($"Element id {elementId} does not exist");

            UpdateSeqFlowList(element.Outgoing, flowId, add);
        }

        private void UpdateSeqFlowList(IList<string> seqFlowList, string flowId, bool add)
        {
            if(seqFlowList.Contains(flowId))
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

    }
}
