using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Interfaces.Exceptions
{
    public class CRUDException : Exception
    {
        public CRUDException(string message) : base(message)
        {
        }

        public CRUDException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CRUDException()
        {
        }
    }
}
