using DasContract.Abstraction.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.DataModel.Property.Reference
{
    public class ReferenceContractProperty: ContractProperty<ReferenceContractPropertyType>
    {
        override public ReferenceContractPropertyType Type { get; set; }

        /// <summary>
        /// The linked contract entity
        /// </summary>
        public ContractEntity Entity { get; set; }
    }
}
