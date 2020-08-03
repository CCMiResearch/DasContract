using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Interfaces
{
    public interface IDataCopyable<TInputType>
    {
        void CopyDataFrom(TInputType source);
    }
}
