using DasContract.Editor.Migrator.Interfaces;
using DasContract.Editor.Migrator;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Primitive;
using DasContract.Editor.Entities.DataModels.Entities.Properties.Reference;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Entities.DataModels.Entities.Properties;
using System.Linq;

namespace DasContract.Editor.Entities.DataModels.Entities
{
    public class ContractEntity : IIdentifiable, INamable, IMigratableComponent<ContractEntity, IMigrator>
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
        /// Primitive properties of this entity
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<PrimitiveContractProperty> PrimitiveProperties
        {
            get
            {
                foreach (var item in primitiveProperties)
                    item.WithMigrator(migrator);
                return primitiveProperties;
            }
            set
            {
                if (value != primitiveProperties)
                    migrator.Notify(() => primitiveProperties, d => primitiveProperties = d);
                primitiveProperties = value;
            }
        }
        List<PrimitiveContractProperty> primitiveProperties = new List<PrimitiveContractProperty>();


        /// <summary>
        /// Reference properties of this entity
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<ReferenceContractProperty> ReferenceProperties
        {
            get
            {
                foreach (var item in referenceProperties)
                    item.WithMigrator(migrator);
                return referenceProperties;
            }
            set
            {
                if (value != referenceProperties)
                    migrator.Notify(() => referenceProperties, d => referenceProperties = d);
                referenceProperties = value;
            }
        }
        List<ReferenceContractProperty> referenceProperties = new List<ReferenceContractProperty>();

        public IEnumerable<ContractProperty> Properties => new List<ContractProperty>()
            .Concat(PrimitiveProperties)
            .Concat(ReferenceProperties)
            .ToList();

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public ContractEntity WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
