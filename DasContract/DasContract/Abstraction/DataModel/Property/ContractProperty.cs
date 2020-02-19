using System.Xml.Serialization;
using DasContract.DasContract.Abstraction.Interface;
using DasContract.Migrator;
using DasContract.Migrator.Interface;

namespace DasContract.Abstraction.DataModel.Property
{
    public abstract class ContractProperty : IIdentifiable, INamable, IMigratableComponent<ContractProperty, IMigrator>
    {
        public string Id { get; set; }

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
        [XmlIgnore]
        protected IMigrator migrator = new SimpleMigrator();

        public ContractProperty WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
