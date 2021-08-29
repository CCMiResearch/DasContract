using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
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
        void AddNewProcess(string processId, string participantId = null);
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
        
        IDictionary<string, DataType> GetDataTypes();
        Property GetPropertyById(string propertyId);
        IList<Property> GetCollectionProperties();
        string GetDataModelXml();
        void SetDataModelXml(string dataModelXml);
        string ConvertToSolidity();
        void SetProcessDiagram(string diagramXml);
    }
}
