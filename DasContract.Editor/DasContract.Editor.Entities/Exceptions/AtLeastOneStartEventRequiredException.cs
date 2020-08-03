using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Exceptions
{
    public class AtLeastOneStartEventRequiredException : EditorContractException
    {
        public AtLeastOneStartEventRequiredException(string message) : base(message)
        {
        }

        public AtLeastOneStartEventRequiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AtLeastOneStartEventRequiredException()
        {
        }
    }
}
