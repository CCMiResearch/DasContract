using DasContract.DasContract.Abstraction.Interface;
using System.Collections.Generic;

namespace DasContract.Abstraction.Data
{
    public class ContractEntity: IIdentifiable
    {
        public string Id { get; set; }

        /// <summary>
        /// Display name of this contract entity
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Properties of this entity
        /// </summary>
        public IList<ContractProperty> Properties { get; set; } = new List<ContractProperty>(); 
    }
}
