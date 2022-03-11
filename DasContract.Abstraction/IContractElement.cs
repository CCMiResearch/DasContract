using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction
{
    public interface IContractElement
    {
        string Id { get; set; }
        string Name { get; set; }
    }
}
