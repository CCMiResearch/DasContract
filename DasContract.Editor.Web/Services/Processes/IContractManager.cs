using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Processes
{
    public interface IContractManager
    {
        Contract Contract { get; set; }

        bool IsContractInitialized();
        void AddNewProcess(string processId, string participantId = null);
        void RemoveProcess(string processId);
        void InitializeNewContract();
        bool TryGetProcess(string id, out Process process);
        string GetProcessIdFromParticipantId(string participantId);
        IList<Process> GetAllProcesses();
        string SerializeContract();
        void RestoreContract(string contractJSON);
    }
}
