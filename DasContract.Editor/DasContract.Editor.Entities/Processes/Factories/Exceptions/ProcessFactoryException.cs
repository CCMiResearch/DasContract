using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Processes.Factories.Exceptions
{
    public class ProcessFactoryException: Exception
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
