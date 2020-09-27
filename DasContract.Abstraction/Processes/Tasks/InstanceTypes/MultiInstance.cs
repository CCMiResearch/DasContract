using DasContract.Abstraction.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes.Tasks.InstanceTypes
{
    public abstract class MultiInstance
    {
        public int LoopCardinality { get; set; }
        public Property LoopCollection { get; set; }
    }
}
