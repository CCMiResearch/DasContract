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
        IContractManager contractManager;

        private Dictionary<string, Stack<ProcessElement>> _deletedElements = new Dictionary<string, Stack<ProcessElement>>();
        private Dictionary<string, SequenceFlow> _deletedSequenceFlows = new Dictionary<string, SequenceFlow>();

        public ProcessManager(IContractManager contractManager)
        {
            this.contractManager = contractManager;
        }

        public bool TryRetrieveIElementById(string elementId, out IProcessElement element)
        {
            var found = TryRetrieveElementById(elementId, out ProcessElement processElement);

            if (found)
            {
                element = processElement;
                return true;
            }

            found = TryRetrieveSequenceFlowById(elementId, out SequenceFlow sequenceFlow);

            if (found)
            {
                element = sequenceFlow;
                return true;
            }
            element = null;
            return false;
        }

        public bool TryRetrieveElementById<T>(string elementId, out T element) where T : ProcessElement
        {
            var process = contractManager.GetProcess();
            ProcessElement processElement;
            var found = process.ProcessElements.TryGetValue(elementId, out processElement);
            if (found)
            {
                element = processElement as T;
            }
            else
            {
                element = null;
            }
            return found;
        }

        public void AddElement(ProcessElement addedElement)
        {
            var process = contractManager.GetProcess();
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
            var process = contractManager.GetProcess();

            if (!process.ProcessElements.ContainsKey(id))
                throw new InvalidIdException($"Process element cannot be deleted, because id {id} does not exist");

            //Store the deleted element in the deletion buffer
            var element = process.ProcessElements[id];
            AddElementToDeletedBuffer(element);

            process.ProcessElements.Remove(id);
            Console.WriteLine($"Number of process elements: {process.ProcessElements.Count()}");
        }

        public void UpdateId(string prevId, string newId)
        {
            var process = contractManager.GetProcess();

            if (!process.ProcessElements.ContainsKey(prevId))
                throw new InvalidIdException($"Process element cannot be updated, because id {prevId} does not exist");
            if (process.ProcessElements.ContainsKey(newId))
                throw new DuplicateIdException($"Id cannot be changed, because process with id {newId} already exists");

            var element = process.ProcessElements[prevId];
            process.ProcessElements.Remove(prevId);
            element.Id = newId;
            process.ProcessElements.Add(newId, element);
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

        public void AddSequenceFlow(SequenceFlow addedSequenceFlow)
        {
            var process = contractManager.GetProcess();
            var id = addedSequenceFlow.Id;

            if (process.SequenceFlows.ContainsKey(id))
                throw new DuplicateIdException($"Process already contains id {id}");

            //Check if the element is not stored in the deletion buffer
            if(_deletedSequenceFlows.TryGetValue(id, out var deletedSequenceFlow)) 
            { 
                addedSequenceFlow = deletedSequenceFlow;
                _deletedSequenceFlows.Remove(id);
            }

            process.SequenceFlows.Add(id, addedSequenceFlow);
            Console.WriteLine($"Number of sequence flows: {process.SequenceFlows.Count()}");
        }

        public void RemoveSequenceFlow(string id)
        {
            var process = contractManager.GetProcess();

            if (!process.SequenceFlows.ContainsKey(id))
                throw new InvalidIdException($"Process element cannot be deleted, because id {id} does not exist");

            //Store the deleted element in the deletion buffer
            var sequenceFlow = process.SequenceFlows[id];
            _deletedSequenceFlows.Add(id, sequenceFlow);

            process.SequenceFlows.Remove(id);
            Console.WriteLine($"Number of sequence flows: {process.SequenceFlows.Count()}");
        }

        public bool TryRetrieveSequenceFlowById(string sequenceFlowId, out SequenceFlow sequenceFlow)
        {
            var process = contractManager.GetProcess();

            return process.SequenceFlows.TryGetValue(sequenceFlowId, out sequenceFlow);
        }
    }
}
