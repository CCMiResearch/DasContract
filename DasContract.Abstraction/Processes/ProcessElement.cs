using System.Collections.Generic;

namespace DasContract.Abstraction.Processes
{
    public abstract class ProcessElement: IProcessElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<string> Incoming { get; set; } = new List<string>();
        public IList<string> Outgoing { get; set; } = new List<string>();
    }
}
