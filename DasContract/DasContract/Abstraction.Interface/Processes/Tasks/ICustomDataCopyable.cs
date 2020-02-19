using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Interface.Processes.Tasks
{
    public interface ICustomDataCopyable<TInputType>
    {
        void CopyCustomDataFrom(TInputType source);
    }
}
