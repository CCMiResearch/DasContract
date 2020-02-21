using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.DataPersistence.Repositories.Interfaces.Exceptions
{
    public class AlreadyExistsException : RepositoryException
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
