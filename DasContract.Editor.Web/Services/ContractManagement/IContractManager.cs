using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public interface IContractManager
    {
        event EventHandler<ProcessUser> UserRemoved;
        event EventHandler<ProcessRole> RoleRemoved;
        event EventHandler<ProcessUser> UserAdded;
        event EventHandler<ProcessRole> RoleAdded;

        string GeneratedContract { get; }

        bool IsContractInitialized();
        bool IsElementIdAvailable(string id);
        Task InitAsync();
        Task InitializeNewContract();
        bool ConvertContract(out string data);
        void SetProcessDiagram(string diagramXml);
        bool CanSafelyExit();

        void AddNewProcess(string processId, string participantId = null);
        void UpdateProcessId(Process process, string newProcessId);
        bool TryGetProcess(string id, out Process process);
        string GetProcessIdFromParticipantId(string participantId);
        IList<Process> GetAllProcesses();
        string TranslateBpmnProcessId(string bpmnProcessId);

        ProcessUser AddNewUser();
        ProcessRole AddNewRole();
        void AddUser(ProcessUser user);
        void RemoveUser(ProcessUser user);
        void AddRole(ProcessRole role);
        void RemoveRole(ProcessRole role);
        void RemoveProcess(string processId);
        IList<ProcessUser> GetProcessUsers();
        IList<ProcessRole> GetProcessRoles();

        string SerializeContract();
        void RestoreContract(string contractXML);
        string GetProcessDiagram();
        string GetContractName();
        string GetContractId();
        void SetContractName(string name);
        
        IDictionary<string, DataType> GetDataTypes();
        Property GetPropertyById(string propertyId);
        IList<Property> GetCollectionProperties();
        string GetDataModelXml();
        void SetDataModelXml(string dataModelXml);

    }
}
