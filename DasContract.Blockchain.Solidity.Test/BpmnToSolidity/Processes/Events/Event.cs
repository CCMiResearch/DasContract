using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes.Events
{
    public abstract class Event : ProcessElement
    {
        public string Name { get; set; }
    }
}
