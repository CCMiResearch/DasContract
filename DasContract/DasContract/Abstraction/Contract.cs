using DasContract.Abstraction.DataModel;
using DasContract.Abstraction.Processes;
using DasContract.DasContract.Abstraction.Interface;
using System.Collections.Generic;

namespace DasContract.Abstraction.Entity
{
    public class Contract: IIdentifiable
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
        /// Contract data model
        /// </summary>
        public ContractDataModel DataModel { get; set; }


        public IList<ProcessRole> Roles { get; set; } = new List<ProcessRole>();
    }
}
