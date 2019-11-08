using System.Collections.Generic;

namespace DasContract.Abstraction.Processes.Gateways
{
    public abstract class Gateway : IProcessElement
    {
        public string Id { get; set; }

        public IList<string> Incoming { get; set; } = new List<string>();
        public IList<string> Outgoing { get; set; } = new List<string>();

        public string DefaultSequenceFlowId { get; set; }

    }
}
