using DasContract.Abstraction.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.DataModel.Property.Reference
{
    public class CollectionReferenceContractProperty : ContractProperty
    {
        /// <summary>
        /// The linked contract entity
        /// </summary>
        public List<string> EntityIds
        {
            get => entityIds;
            set
            {
                if (value != entityIds)
                    migrator.Notify(() => entityIds, d => entityIds = d);
                entityIds = value;
            }
        }
        bool entitiesMigrated = false;
        List<string> entityIds = new List<string>();
    }
}
