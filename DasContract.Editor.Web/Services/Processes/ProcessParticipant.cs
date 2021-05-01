using DasContract.Abstraction.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Processes
{
    public class ProcessParticipant: IProcessElement
    {
        public Process ReferencedProcess { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
