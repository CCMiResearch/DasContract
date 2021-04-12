using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.CamundaEvents.Exceptions
{
    public class DuplicateIdException : Exception
    {
        public DuplicateIdException(string msg) : base(msg) { }
    }
}
