using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Abstraction
{
    public class Contract
    {
        public string Id { get; set; }
        /// <summary>
        /// BPMN 2.0 XML with process description and a visual process information. 
        /// </summary>
        public string ProcessDiagram { get; set; }

        /// <summary>
        /// Currently used for backwards compatibility, when only one 
        /// process was allowed.
        /// </summary>
        public Process Process { 
            get { return Processes.ElementAtOrDefault(0); } 
            set { Processes[0] = value; } 
        }

        /// <summary>
        /// List of processes present in the contract.
        /// </summary>
        public IList<Process> Processes { get; set; } = new List<Process>();
        //TODO list with backwards compatibility (route Processes to Processes.First)

        /// <summary>
        /// Data Model
        /// </summary>
        public IList<Entity> Entities { get; set; } = new List<Entity>(); 
        public IList<ProcessRole> Roles { get; set; } = new List<ProcessRole>();
    }
}
