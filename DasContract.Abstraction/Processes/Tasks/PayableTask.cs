using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes.Tasks
{
    public abstract class PayableTask: Task
    {
        public TokenOperationType OperationType { get; set; }
    }
}
