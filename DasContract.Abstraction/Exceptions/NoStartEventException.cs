using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.Exceptions
{
    class NoStartEventException: Exception
    {
        public NoStartEventException(string message) : base(message) { }
    }
}
