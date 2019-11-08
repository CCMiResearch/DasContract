using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes.Events
{
    public abstract class Event : IProcessElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
