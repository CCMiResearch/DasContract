using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Exceptions
{
    public class NoStartEventException: Exception
    {
        public NoStartEventException(string message) : base(message) { }
    }
}
