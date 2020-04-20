using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Exceptions
{
    class InvalidProcessCountException : Exception
    {
        public InvalidProcessCountException(string message): base(message) { }
    }
}
