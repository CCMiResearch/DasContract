using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.DataPersistence.Repositories.Interfaces.Exceptions
{
    public class NotFoundException : RepositoryException
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
