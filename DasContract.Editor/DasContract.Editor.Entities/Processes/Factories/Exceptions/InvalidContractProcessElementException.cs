using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Processes.Factories.Exceptions
{
    public class InvalidContractProcessElementException : ProcessFactoryException
    {
        public InvalidContractProcessElementException()
        {
        }

        public InvalidContractProcessElementException(string message) : base(message)
        {
        }

        public InvalidContractProcessElementException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
