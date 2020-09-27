using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class CallActivity: Task
    {
        public string CalledElement { get; set; }
    }
}
