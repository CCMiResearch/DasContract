using System;
using System.Collections.Generic;
using System.Text;

namespace DasToSolidity.Exceptions
{
    class NoStartEventException: Exception
    {
        public NoStartEventException(string message) : base(message) { }
    }
}
