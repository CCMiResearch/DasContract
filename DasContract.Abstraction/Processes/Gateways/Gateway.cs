using System.Collections.Generic;

namespace DasContract.Abstraction.Processes.Gateways
{
    public abstract class Gateway : ProcessElement
    {
        public string DefaultSequenceFlowId { get; set; }
    }
}
