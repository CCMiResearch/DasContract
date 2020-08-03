using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Interfaces.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BadRequestException()
        {
        }
    }
}
