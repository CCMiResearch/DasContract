using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction;
using DasContract.Abstraction.Processes;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public interface IProcessModelManager
    {
        void SetContract(Contract contract);

        IList<string> GetAllProcessIds();
        void AddNewProcess(string processId, string participantId = null);
        void RemoveProcess(string processId);
        void UpdateProcessId(Process process, string newProcessId);
        bool TryGetProcess(string id, out Process process);
        string GetProcessIdFromParticipantId(string participantId);
        string TranslateBpmnProcessId(string bpmnProcessId);

        bool IsElementIdAvailable(string id);
        bool TryRetrieveIElementById(string elementId, string processId, out IProcessElement element);
        bool TryRetrieveIElementById(string elementId, out IProcessElement element);
        bool TryRetrieveSequenceFlowById(string sequenceFlowId, string processId, out SequenceFlow sequenceFlow);
        bool TryRetrieveElementById(string elementId, string processId, out ProcessElement processElement);
        bool TryRetrieveProcessOfElement(string elementId, out Process process);
        ProcessElement AddElement(string elementType, string elementId, string processId);
        void RemoveElement(string id);
        bool ProcessExists(string processId);
        void UpdateId(string prevId, string newId, string processId);
        void UpdateSequenceFlowSourceAndTarget(SequenceFlow sequenceFlow, string newSource, string newTarget, string processId);
        SequenceFlow AddSequenceFlow(string id, string target, string source, string processId);
        void RemoveSequenceFlow(string id);
        void ChangeProcessOfElement(IProcessElement element, string prevProcessId, string newProcessId);

        void SetProcessBpmnDefinition(string bpmnDefinition);
        string GetProcessBpmnDefinition();
    }
}
