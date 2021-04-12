using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction.Processes;

namespace DasContract.Editor.Web.Services
{
    public interface IProcessManager
    {
        public bool TryRetrieveElementById<T>(string elementId, out T element) where T : ProcessElement;
        public void AddElement<T>(T addedElement) where T : ProcessElement;
        public void AddElement(string id, string type);
        public void RemoveElement(string id);
        public void AddOrReplaceElement(string id, string type);
        public void UpdateId(string prevId, string newId);
    }
}
