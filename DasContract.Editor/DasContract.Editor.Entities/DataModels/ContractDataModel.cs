using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Entities.DataModels
{
    public class ContractDataModel : IMigratableComponent<ContractDataModel, IMigrator>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Entities of this data model
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<ContractEntity> Entities
        {
            get
            {
                foreach (var item in entities)
                    item.WithMigrator(migrator);
                return entities;
            }
            set
            {
                if (value != entities)
                    migrator.Notify(() => entities, d => entities = d);
                entities = value;
            }
        }
        List<ContractEntity> entities = new List<ContractEntity>();

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public ContractDataModel WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
