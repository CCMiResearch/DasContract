using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Processes
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
        void AddNewProcess(string processId, string participantId = null);
        void UpdateProcessId(Process process, string newProcessId);
        ProcessUser AddNewUser();
        void AddUser(ProcessUser user);
        void RemoveUser(ProcessUser user);
        ProcessRole AddNewRole();
        void AddRole(ProcessRole role);
        void RemoveRole(ProcessRole role);
        void RemoveProcess(string processId);
        IList<ProcessUser> GetProcessUsers();
        IList<ProcessRole> GetProcessRoles();
        void InitializeNewContract();
        bool TryGetProcess(string id, out Process process);
        string GetProcessIdFromParticipantId(string participantId);
        IList<Process> GetAllProcesses();
        string SerializeContract();
        void RestoreContract(string contractXML);
        string GetProcessDiagram();
        string GetContractName();
        void SetContractName(string name);
        
        IDictionary<string, DataType> GetDataTypes();
        Property GetPropertyById(string propertyId);
        IList<Property> GetCollectionProperties();
        string GetDataModelXml();
        void SetDataModelXml(string dataModelXml);
        bool ConvertContract(IConverterService converterService, out string data);
        void SetProcessDiagram(string diagramXml);
        string TranslateBpmnProcessId(string bpmnProcessId);
    }
}
