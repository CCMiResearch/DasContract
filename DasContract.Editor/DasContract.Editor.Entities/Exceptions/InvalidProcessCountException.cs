using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Exceptions
{
    public class InvalidProcessCountException : EditorContractException
    {
        public InvalidProcessCountException(string message) : base(message)
        {
        }

        public InvalidProcessCountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidProcessCountException()
        {
        }
    }
}
