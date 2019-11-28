using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using System.Collections.Generic;

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
        /// Contract main process. Currently only one process is allowed. 
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        /// Data Model
        /// </summary>
        public IList<Entity> Entities { get; set; } = new List<Entity>(); 
        public IList<ProcessRole> Roles { get; set; } = new List<ProcessRole>();
    }
}
