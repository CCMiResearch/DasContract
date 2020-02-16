using DasContract.Abstraction.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.BPMN.Exceptions
{
    class NoStartEventException: ContractException
    {
        public NoStartEventException(string message) : base(message) { }
    }
}
