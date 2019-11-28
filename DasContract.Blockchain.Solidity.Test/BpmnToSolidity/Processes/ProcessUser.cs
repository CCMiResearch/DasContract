using System.Collections.Generic;

namespace DasContract.Abstraction.Processes
{
    public class ProcessUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public IList<ProcessRole> Roles { get; set; } = new List<ProcessRole>();
    }
}
