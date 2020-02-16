using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Exceptions
{
    class InvalidElementException: Exception
    {
        public InvalidElementException(string message): base(message) { }
    }
}
