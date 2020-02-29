using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.Exceptions
{
    public class AtLeastOneEndEventRequiredException : EditorContractException
    {
        public AtLeastOneEndEventRequiredException(string message) : base(message)
        {
        }

        public AtLeastOneEndEventRequiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AtLeastOneEndEventRequiredException()
        {
        }
    }
}
