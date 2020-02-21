using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.Exceptions;

namespace DasContract.Editor.Entities.Processes.Factories.Exceptions
{
    public class ProcessFactoryException: EditorContractException
    {
        public ProcessFactoryException()
        {
        }

        public ProcessFactoryException(string message) : base(message)
        {
        }

        public ProcessFactoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        
    }
}
