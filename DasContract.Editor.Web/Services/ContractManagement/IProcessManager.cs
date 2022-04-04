using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction.Processes;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public interface IProcessManager
    {
        bool TryRetrieveIElementById(string elementId, string processId, out IProcessElement element);
        bool TryRetrieveIElementById(string elementId, out IProcessElement element);
        ProcessElement AddElement(string elementType, string elementId, string processId);
        void RemoveElement(string id);
        bool ProcessExists(string processId);
        void UpdateId(string prevId, string newId, string processId);
        void UpdateSequenceFlowSourceAndTarget(SequenceFlow sequenceFlow, string newSource, string newTarget, string processId);
        public SequenceFlow AddSequenceFlow(string id, string target, string source, string processId);
        void RemoveSequenceFlow(string id);
        bool TryRetrieveSequenceFlowById(string sequenceFlowId, string processId, out SequenceFlow sequenceFlow);
        bool TryRetrieveElementById(string sequenceFlowId, string processId, out ProcessElement sequenceFlow);
        bool TryGetProcessOfElement(string elementId, out Process process);
        void ChangeProcessOfElement(IProcessElement element, string prevProcessId, string newProcessId);
    }
}
