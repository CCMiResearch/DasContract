
using System;
using System.Linq.Expressions;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Forms
{
    public class ContractFormField : IIdentifiable, INamable, IMigratableComponent<ContractFormField, IMigrator>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                    migrator.Notify(() => name, e => name = e);
                name = value;
            }
        }
        string name;

        /// <summary>
        /// Field label (what is the field about)
        /// </summary>
        public string Label
        {
            get => label;
            set
            {
                if (label != value)
                    migrator.Notify(() => label, e => label = e);
                label = value;
            }
        }
        string label;

        /// <summary>
        /// Description of this form field
        /// </summary>
        public string Description
        {
            get => description;
            set
            {
                if (description != value)
                    migrator.Notify(() => description, e => description = e);
                description = value;
            }
        }
        string description;

        /// <summary>
        /// Tells if this field is for read only or not
        /// </summary>
        public bool ReadOnly
        {
            get => readOnly;
            set
            {
                if (readOnly != value)
                    migrator.Notify(() => readOnly, e => readOnly = e);
                readOnly = value;
            }
        }
        bool readOnly;

        /// <summary>
        /// Optional binding to a property
        /// </summary>
        public ContractPropertyBinding PropertyBinding
        {
            get => propertyBinding?.WithMigrator(migrator);
            set
            {
                if (propertyBinding != value)
                    migrator.Notify(() => propertyBinding, e => propertyBinding = e);
                propertyBinding = value;
            }
        }
        ContractPropertyBinding propertyBinding = null;


        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public ContractFormField WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
