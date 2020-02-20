using System.Collections.Generic;
using DasContract.Editor.Entities.Processes.Process;

namespace DasContract.Editor.Entities.Processes.Process.Gateways
{
    public abstract class ContractGateway : ContractProcessElement
    {
        public string DefaultSequenceFlowId { get; set; }
    }
}
