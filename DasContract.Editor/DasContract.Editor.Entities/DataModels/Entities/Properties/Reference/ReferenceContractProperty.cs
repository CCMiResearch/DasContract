using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Target entity type")]
        [Description("Entity type that will be used as a data type of this property")]
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

        [DisplayName("Property type")]
        [Description("Property type further specifies the data type of this property")]
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
