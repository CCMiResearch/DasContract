using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Interfaces.Exceptions
{
    public class AlreadyExistsException : CRUDException
    {
        public AlreadyExistsException(string message) : base(message)
        {
        }

        public AlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AlreadyExistsException()
        {
        }
    }
}
