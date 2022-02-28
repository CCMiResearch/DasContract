using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes.Events
{
    public abstract class BoundaryEvent : Event
    {
        public string AttachedTo { get; set; }
    }
}
