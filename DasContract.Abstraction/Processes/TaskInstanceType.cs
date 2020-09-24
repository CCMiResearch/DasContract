using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes
{
    //TODO: change to class
    public enum TaskInstanceType
    {
        Single,
        ParallelMulti,
        SequentialMulti,
        Loop
    }
}
