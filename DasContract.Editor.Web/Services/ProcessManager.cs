using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
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

            var newElement = CreateElementFromType(type);
            process.ProcessElements.Add(id, newElement);
            Console.WriteLine($"Number of process elements: {process.ProcessElements.Count()}");
        }

        public void RemoveElement(string id)
        {
            var process = contractManager.GetProcess();

            if (!process.ProcessElements.ContainsKey(id))
                throw new InvalidIdException($"Process element cannot be deleted, because id {id} does not exist");
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
                default:
                    throw new InvalidCamundaElementTypeException($"{type} is not a valid element type");
            }
        }


    }
}
