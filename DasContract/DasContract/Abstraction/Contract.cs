using DasContract.Abstraction.DataModel;
using DasContract.Abstraction.Processes;
using DasContract.DasContract.Abstraction.Interface;
using System.Collections.Generic;

namespace DasContract.Abstraction.Entity
{
    public class Contract: IIdentifiable, INamable
    {
        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Contract processes
        /// </summary>
        public ContractProcesses Processes { get; set; }

        /// <summary>
        /// Contract data model
        /// </summary>
        public ContractDataModel DataModel { get; set; }


        public IList<ProcessRole> Roles { get; set; } = new List<ProcessRole>();
       
    }
}
