using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Integrity.Exceptions
{
    public class ContractIntegrityException : Exception
    {
        public ContractIntegrityException(string message) : base(message)
        {
        }

        public ContractIntegrityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ContractIntegrityException()
        {
        }
    }
}
