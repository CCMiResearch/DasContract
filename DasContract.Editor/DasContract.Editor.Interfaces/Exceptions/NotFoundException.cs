using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Interfaces.Exceptions
{
    public class NotFoundException : CRUDException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotFoundException()
        {
        }
    }
}
