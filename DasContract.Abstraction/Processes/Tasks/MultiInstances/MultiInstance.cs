using DasContract.Abstraction.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class MultiInstance
    {
        public int LoopCardinality { get; set; }
        public Property LoopCollection { get; set; }
        public MultiInstanceType InstanceType { get; set; }
    }
}
