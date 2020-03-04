using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasContract.Editor.Entities.Forms
{
    public class ContractForm : IIdentifiable, IMigratableComponent<ContractForm, IMigrator>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<ContractFormField> Fields
        {
            get
            {
                foreach (var item in fields)
                    item.WithMigrator(migrator);

                return fields;
            }
            set
            {
                if (value != fields)
                    migrator.Notify(() => fields, d => fields = d,
                            MigratorMode.Smart);
                fields = value;
            }
        }
        List<ContractFormField> fields = new List<ContractFormField>();

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public ContractForm WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
