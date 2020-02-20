using System;
using System.Xml.Serialization;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.DataModels.Entities.Properties
{
    public abstract class ContractProperty : IIdentifiable, INamable, IMigratableComponent<ContractProperty, IMigrator>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name
        {
            get => name;
            set
            {
                if (value != name)
                    migrator.Notify(() => name, p => name = p);
                name = value;
            }
        }
        string name;

        /// <summary>
        /// Tells if this property is allowed to be the default value or not
        /// </summary>
        public bool IsMandatory
        {
            get => isMandatory;
            set
            {
                if (value != isMandatory)
                    migrator.Notify(() => isMandatory, b => isMandatory = b);
                isMandatory = value;
            }
        }
        bool isMandatory = true;

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        protected IMigrator migrator { get; set; } = new SimpleMigrator();

        public ContractProperty WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
