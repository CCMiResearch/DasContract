using DasContract.Abstraction.DataModel.Entity;
using DasContract.Abstraction.DataModel.Property.Primitive;
using DasContract.DasContract.Abstraction.Interface;
using DasContract.Migrator;
using DasContract.Migrator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.DataModel
{
    public class ContractDataModel: IIdentifiable, IMigratableComponent<ContractDataModel, IMigrator>
    {
        public string Id { get; set; }

        /// <summary>
        /// Entities of this data model
        /// </summary>
        public List<ContractEntity> Entities
        {
            get
            {
                if (entitiesMigrated)
                    return entities;

                foreach (var item in entities)
                    item.WithMigrator(migrator);
                entitiesMigrated = true;
                return entities;
            }
            set
            {
                if (value != entities)
                    migrator.Notify(() => entities, d => entities = d);
                entities = value;
            }
        }
        bool entitiesMigrated = false;
        List<ContractEntity> entities = new List<ContractEntity>();

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        [XmlIgnore]
        IMigrator migrator = new SimpleMigrator();

        public ContractDataModel WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}

