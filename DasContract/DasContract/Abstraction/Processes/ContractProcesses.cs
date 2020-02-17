using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes
{
    public class ContractProcesses
    {
        /// <summary>
        /// BPMN 2.0 XML with process description and a visual process information for the main process
        /// </summary>
        public string Diagram { get; set; }

        /// <summary>
        /// Contract main process. Currently only one process is allowed. 
        /// </summary>
        public Process Main { get; set; }
    }
}
