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

        public ProcessManager(IContractManager contractManager)
        {
            this.contractManager = contractManager;
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
            Console.WriteLine($"Added id {e.Id} to deletion buffer, it contains {deletedStack.Count} elements");
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
            Console.WriteLine($"Retrieved id {element.Id} from deletion buffer, it contains {deletedStack.Count} elements");
            return true;
        }

        


    }
}
