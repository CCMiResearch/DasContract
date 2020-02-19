using DasContract.Abstraction.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.DataModel.Property.Reference
{
    public class ReferenceContractProperty: ContractProperty
    {
        /// <summary>
        /// The linked contract entity
        /// </summary>
        public string EntityId
        {
            get => entityId;
            set
            {
                if (value != entityId)
                    migrator.Notify(() => entityId, b => entityId = b);
                entityId = value;
            }
        }

        string entityId;
    }
}
