using Blazored.LocalStorage;
using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Blockchain.Solidity.Converters;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using DasContract.Editor.Web.Services.Converter;
using DasContract.Editor.Web.Services.UndoRedo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DasContract.Editor.Web.Services.Processes
{
    public class ContractManager : IContractManager
    {
        protected Contract Contract { get; set; }

        private IJSRuntime _jsRuntime;

        private NavigationManager _navigationManager;

        private ILocalStorageService _localStorage;

        private Dictionary<string, Process> _deletedProcesses = new Dictionary<string, Process>();

        public event EventHandler<ProcessUser> UserRemoved;
        public event EventHandler<ProcessRole> RoleRemoved;
        public event EventHandler<ProcessUser> UserAdded;
        public event EventHandler<ProcessRole> RoleAdded;

        public string GeneratedContract { get; private set; }
        public string SerializedContract { get; private set; }

        public ContractManager(IJSRuntime jsRuntime, NavigationManager navigationManager, ILocalStorageService localStorage)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
            _localStorage = localStorage;
        }

        public async Task InitAsync()
        {
            await _jsRuntime.InvokeVoidAsync("exitGuardLib.setContractManagerInstance", DotNetObjectReference.Create(this));
            var contract = await _localStorage.GetItemAsync<string>("contract");
            if (contract != null)
            {
                RestoreContract(contract);
                SerializedContract = Contract.ToXElement().ToString();
            }
        }

        public bool IsContractInitialized()
        {
            return Contract != null;
        }

        public void InitializeNewContract()
        {
            Contract = new Contract();
            Contract.Id = Guid.NewGuid().ToString();
            SerializedContract = Contract.ToXElement().ToString();
        }

        public bool TryGetProcess(string id, out Process process)
        {
            return Contract.TryGetProcess(id, out process);
        }

        public string GetProcessIdFromParticipantId(string participantId)
        {
            var proc = Contract.Processes.Where(p => p.ParticipantId == participantId);
            if (proc.Count() != 1)
                throw new InvalidIdException($"Participant id {participantId} could not be found");
            return proc.First().Id;
        }
        //A participant might be associated with the process (but not always)
        public void AddNewProcess(string processId, string participantId = null)
        {
            Process process;
            //Copy the existing process if the input participant is not defined
            //and a process already exists (this happens when the last participant is removed),
            //or when an input participant is defined and no other participants are yet present in the model
            //(this happens when the first participant is added)
            if (Contract.Processes.Count == 1 && Contract.Processes.First().ParticipantId == null && participantId != null
                || (participantId == null && Contract.Processes.Count > 0))
            {
                process = Contract.Processes.First();
                process.ParticipantId = participantId;
                //The id needs to be updated due to a bug in the bpmn modeller
                //process.BpmnId = processId;
            }
            else
            {
                if (Contract.Processes.Any(p => p.Id == processId))
                    throw new DuplicateIdException($"Could not add new process, contract already contains process id {processId}");

                if (_deletedProcesses.TryGetValue(processId, out process))
                {
                    _deletedProcesses.Remove(processId);
                }
                else
                {
                    process = new Process { Id = processId, ParticipantId = participantId, BpmnId = processId };
                }
                Contract.Processes.Add(process);
            }

            if (Contract.Processes.Count == 1)
            {
                process.IsExecutable = true;
            }
        }

        public IList<ProcessUser> GetProcessUsers()
        {
            return Contract.Users;
        }

        public IList<ProcessRole> GetProcessRoles()
        {
            return Contract.Roles;
        }

        public ProcessUser AddNewUser()
        {
            var user = new ProcessUser { Id = Guid.NewGuid().ToString() };
            Contract.Users.Add(user);
            UserAdded?.Invoke(this, user);
            return user;
        }

        public void AddUser(ProcessUser user)
        {
            if (Contract.Users.Any(u => user.Id == u.Id))
            {
                throw new DuplicateIdException($"Contract already contains user id {user.Id}");
            }
            Contract.Users.Add(user);
            UserAdded?.Invoke(this, user);
        }

        public void RemoveUser(ProcessUser user)
        {
            if (!Contract.Users.Contains(user))
            {
                throw new InvalidIdException($"User id {user.Id} could not be removed, contract does not contain user");
            }
            Contract.Users.Remove(user);
            UserRemoved?.Invoke(this, user);
        }

        public ProcessRole AddNewRole()
        {
            var role = new ProcessRole { Id = Guid.NewGuid().ToString() };
            Contract.Roles.Add(role);
            RoleAdded?.Invoke(this, role);
            return role;
        }

        public void AddRole(ProcessRole role)
        {
            RoleAdded?.Invoke(this, role);
            Contract.Roles.Add(role);
        }
        public void RemoveRole(ProcessRole role)
        {
            if (!Contract.Roles.Contains(role))
            {
                throw new InvalidIdException($"User id {role.Id} could not be removed, contract does not contain user");
            }
            Contract.Roles.Remove(role);
            RoleRemoved?.Invoke(this, role);
        }

        public void RemoveProcess(string processId)
        {
            if (!TryGetProcess(processId, out var process))
                throw new InvalidIdException($"Could not delete process, contract does not contain process id {processId}");

            if (Contract.Processes.Count > 1)
            {
                _deletedProcesses.Add(processId, process);
                Contract.Processes.Remove(process);
            }
        }

        public IList<Process> GetAllProcesses()
        {
            return Contract.Processes;
        }

        public string SerializeContract()
        {
            SerializedContract = Contract.ToXElement().ToString();
            return SerializedContract;
        }

        public void RestoreContract(string contractXML)
        {
            SerializedContract = contractXML;
            var xElement = XElement.Parse(contractXML);
            Contract = new Contract(xElement);
        }

        public string GetProcessDiagram()
        {
            return Contract.ProcessDiagram;
        }

        public void SetProcessDiagram(string diagramXml)
        {
            Contract.ProcessDiagram = diagramXml;
        }

        public IList<Property> GetCollectionProperties()
        {
            var collectionProperties = new List<Property>();
            foreach (var entity in Contract.Entities)
            {
                collectionProperties.AddRange(entity.Properties
                    .Where(p => p.PropertyType == PropertyType.Collection || p.PropertyType == PropertyType.Dictionary).ToList());
            }

            return collectionProperties;
        }

        public Property GetPropertyById(string propertyId)
        {
            foreach (var entity in Contract.Entities)
            {
                foreach (var property in entity.Properties)
                {
                    if (property.Id == propertyId)
                        return property;
                }
            }
            return null;
        }

        public string GetDataModelXml()
        {
            return Contract.DataModelDefinition;
        }

        public IDictionary<string, DataType> GetDataTypes()
        {
            return Contract.DataTypes;
        }

        public void SetDataModelXml(string dataModelXml)
        {
            Contract.DataModelDefinition = dataModelXml;
            var xDataModel = XElement.Parse(dataModelXml);
            Contract.SetDataModelFromXml(xDataModel);
        }

        public bool ConvertContract(IConverterService contractConverter, out string data)
        {
            return contractConverter.TryConvertContract(Contract, out data);
        }

        public string GetContractName()
        {
            return Contract?.Name;
        }

        public void SetContractName(string name)
        {
            Contract.Name = name;
        }

        [JSInvokable]
        public bool CanSafelyExit()
        {
            if (Contract == null)
                return true;

            return Contract.ToXElement().ToString() == SerializedContract;
        }

        public bool IsElementIdAvailable(string id)
        {
            return Contract.Processes.All(p => !p.ProcessElements.ContainsKey(id) && !p.SequenceFlows.ContainsKey(id));
        }

        public void UpdateProcessId(Process process, string newProcessId)
        {
            if (Contract.Processes.Count(p => p.Id == newProcessId) > 0)
                throw new DuplicateIdException($"Id cannot be changed, because process with id {newProcessId} already exists");

            foreach (var p in Contract.Processes)
            {
                var callActivites = p.Tasks
                    .Where(e => e is Abstraction.Processes.Tasks.CallActivity)
                    .Select(e => e as Abstraction.Processes.Tasks.CallActivity);
                foreach (var callActivity in callActivites)
                {
                    if (callActivity.CalledElement == process.Id)
                    {
                        callActivity.CalledElement = newProcessId;
                    }
                }
            }
            process.Id = newProcessId;
        }

        public string TranslateBpmnProcessId(string bpmnProcessId)
        {
            var proc = Contract.Processes.SingleOrDefault(p => p.BpmnId == bpmnProcessId);

            if (proc != null)
                return proc.Id;

            proc = _deletedProcesses.Values.SingleOrDefault(p => p.BpmnId == bpmnProcessId);

            return proc?.Id;
        }
    }
}
