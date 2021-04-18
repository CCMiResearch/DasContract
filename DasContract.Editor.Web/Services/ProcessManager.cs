using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.CamundaEvents.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services
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
            var found =  process.ProcessElements.TryGetValue(elementId, out processElement);
            if(found)
            {
                element = processElement as T;
            }
            else
            {
                element = null;
            }
            return found;
        }

        public void AddElement<T>(T addedElement) where T : ProcessElement
        {
            var process = contractManager.GetProcess();
            process.ProcessElements.Add(addedElement.Id, addedElement);
        }

        public void AddOrReplaceElement(string id, string type)
        {
            var process = contractManager.GetProcess();
            var newElement = CreateElementFromType(type);

            ProcessElement existingElement;
            if (process.ProcessElements.TryGetValue(id, out existingElement))
            {
                //TODO: Copy general values
                process.ProcessElements.Remove(id);
            }
            process.ProcessElements.Add(id, newElement);

            Console.WriteLine($"Number of process elements: {process.ProcessElements.Count()}");
        }

        public void AddElement(string id, string type)
        {
            var process = contractManager.GetProcess();

            if (process.ProcessElements.ContainsKey(id))
                throw new DuplicateIdException($"Process already contains id {id}");

            //Check if the element is not stored in the deletion buffer
            if (!TryGetElementFromDeletedBuffer(id, out ProcessElement element))
            {
                element = CreateElementFromType(type);
                element.Id = id;
            }

            process.ProcessElements.Add(id, element);
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

        private bool TryGetElementFromDeletedBuffer(string elementId, out ProcessElement element) {
            var deletedStack = _deletedElements.GetValueOrDefault(elementId);

            if(deletedStack == null || deletedStack.Count == 0)
            {
                element = null;
                return false;
            }

            element = deletedStack.Pop();
            Console.WriteLine($"Retrieved id {element.Id} from deletion buffer, it contains {deletedStack.Count} elements");
            return true;
        }

        private ProcessElement CreateElementFromType(string type)
        {
            switch(type)
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
                default:
                    throw new InvalidCamundaElementTypeException($"{type} is not a valid element type");
            }
        }


    }
}
