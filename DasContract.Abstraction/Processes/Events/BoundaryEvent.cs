using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes.Events
{
    public class BoundaryEvent : Event
    {
        public string AttachedTo { get; set; }
    }
}
