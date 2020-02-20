using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using DasContract.Editor.Entities.DataModels.Entities.Properties;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Forms
{
    public class ContractPropertyBinding : IMigratableComponent<ContractPropertyBinding, IMigrator>
    {
        [XmlIgnore]
        public ContractProperty Property
        {
            get => property.WithMigrator(migrator);
            set
            {
                property = value;
                if (property != value)
                    migrator.Notify(() => property, e => property = e);
                if (value != null)
                    PropertyId = value.Id;
            }
        }
        ContractProperty property;

        public string PropertyId { get; set; }

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public ContractPropertyBinding WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
