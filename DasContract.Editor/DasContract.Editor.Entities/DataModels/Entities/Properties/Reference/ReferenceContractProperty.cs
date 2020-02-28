using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Editor.Entities.DataModels.Entities.Properties.Reference
{
    public class ReferenceContractProperty : ContractProperty
    {
        /// <summary>
        /// The linked contract entity
        /// </summary>
        [XmlIgnore]
        public ContractEntity Entity
        {
            get => entity?.WithMigrator(migrator);
            set
            {
                if (value != entity)
                    migrator.Notify(() => entity, b => entity = b);
                entity = value;

                if (value != null)
                    EntityId = value.Id;
                else
                    EntityId = null;
            }
        }
        ContractEntity entity;

        public string EntityId { get; set; }

        public ReferenceContractPropertyType Type
        {
            get => type;
            set
            {
                if (value != type)
                    migrator.Notify(() => type, b => type = b);
                type = value;
            }
        }
        ReferenceContractPropertyType type = ReferenceContractPropertyType.SingleReference;
    }
}
