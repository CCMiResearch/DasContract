using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Exceptions.Specific
{
    public class InvalidElementException: ContractException
    {
        public InvalidElementException(string message): base(message) { }
    }
}
