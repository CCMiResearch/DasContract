using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.BpmnEvents.Exceptions;
using DasContract.JSON;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Processes
{
    public class ContractManager : IContractManager
    {
        public Contract Contract { get; set; }
        private IDictionary<string, ProcessParticipant> _processParticipants = new Dictionary<string, ProcessParticipant>();

        private IJSRuntime _jsRuntime;

        public ContractManager(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public void InitializeNewContract()
        {
            Contract = new Contract();
        }

        public bool TryGetProcess(string id, out Process process)
        {
            return Contract.TryGetProcess(id, out process);
        }

        public bool TryGetParticipant(string id, out ProcessParticipant participant)
        {
            return _processParticipants.TryGetValue(id, out participant);
        }
        //A participant might be associated with the process (but not always)
        public void AddNewProcess(string processId, string participantId = null)
        {
            Process process;
            //Copy the existing process if the input participant is not defined
            //and a process already exists (this happens when the last participant is removed),
            //or when an input participant is defined and no other participants are yet present in the model
            //(this happens when the first participant is added)
            if (_processParticipants.Count == 0 && participantId != null || (participantId == null && Contract.Processes.Count > 0))
            {
                process = Contract.Processes.First();
                //The id needs to be updated due to a bug in the bpmn modeller
                process.Id = processId;
            }
            else 
            {
                if (Contract.Processes.Any(p => p.Id == processId))
                    throw new DuplicateIdException($"Could not add new process, contract already contains process id {processId}");
                process = new Process { Id = processId };
                Contract.Processes.Add(process);
            }

            //Create a participant if it is associated with the process
            if (participantId != null)
            {
                var participant = new ProcessParticipant { Id = participantId, ReferencedProcess = process };
                _processParticipants.Add(participantId, participant);
            }

            if(Contract.Processes.Count == 1)
            {
                process.IsExecutable = true;
            }
        }

        public void RemoveProcess(string processId, string participantId = null)
        {
            if (!TryGetProcess(processId, out var process))
                throw new InvalidIdException($"Could not delete process, contract does not contain process id {processId}");

            if (participantId != null)
            {
                if (!_processParticipants.TryGetValue(participantId, out _))
                {
                    throw new InvalidIdException($"Could not delete participant, contract does not contain participant id {participantId}");
                }
                //_deletedParticipants[participantId] = participant;
                _processParticipants.Remove(participantId);
            }

            if(_processParticipants.Count > 0)
                Contract.Processes.Remove(process);
        }

        public IList<Process> GetAllProcesses()
        {
            return Contract.Processes;
        }

        public async Task<string> SerializeContract()
        {
            var diagramXml = await _jsRuntime.InvokeAsync<string>("modellerLib.getDiagramXML");
            Contract.ProcessDiagram = diagramXml;
            return DasContractJSON.Serialize(Contract);
        }
    }
}
