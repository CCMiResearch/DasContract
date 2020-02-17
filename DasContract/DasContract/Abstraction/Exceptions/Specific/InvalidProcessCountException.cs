using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Exceptions.Specific
{
    public class InvalidProcessCountException : ContractException
    {
        public InvalidProcessCountException(string message): base(message) { }
    }
}
