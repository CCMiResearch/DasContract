using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction.Processes;

namespace DasContract.Editor.Web.Services.Processes
{
    public interface IProcessManager
    {
        public bool TryRetrieveElementById<T>(string elementId, out T element) where T : ProcessElement;
        public void AddElement(ProcessElement addedElement);
        //public void AddSequenceFlow();
        public void RemoveElement(string id);
        public void UpdateId(string prevId, string newId);
    }
}
