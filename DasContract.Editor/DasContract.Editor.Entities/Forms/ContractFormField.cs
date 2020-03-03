
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Forms
{
    public class ContractFormField : IIdentifiable, INamable, IMigratableComponent<ContractFormField, IMigrator>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [DisplayName("Name")]
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
        [DisplayName("Label")]
        [Description("How this field should be labeled in a result contract application")]
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
        [DisplayName("Description")]
        [Description("More detailed description of this field in case in might not be clear enough")]
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
        [DisplayName("Read only")]
        [Description("If true, this field will not be editable by the user")]
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
        [DisplayName("Property bind")]
        [Description("The input will bind to an entity property. The values between the input and the property are synchronized.")]
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
