using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.BpmnEvents.Exceptions
{
    public class InvalidIdException: Exception
    {
        public InvalidIdException(string msg) : base(msg) { }
    }
}
