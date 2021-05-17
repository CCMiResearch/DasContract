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

        void AddNewProcess(string processId, string participantId = null);
        void RemoveProcess(string processId, string participantId = null);
        void InitializeNewContract();
        bool TryGetProcess(string id, out Process process);
        bool TryGetParticipant(string id, out ProcessParticipant participant);
        IList<Process> GetAllProcesses();
        Task<string> SerializeContract();
    }
}
