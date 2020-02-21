using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Exceptions
{
    public class EditorContractException: Exception
    {
        public EditorContractException(string message) : base(message)
        {
        }

        public EditorContractException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public EditorContractException()
        {
        }
    }
}
