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
                    migrator.Notify(() => name, p => name = p,
                            MigratorMode.Smart);
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
                    migrator.Notify(() => primitiveProperties, d => primitiveProperties = d,
                            MigratorMode.Smart);
                primitiveProperties = value;
            }
        }
        List<PrimitiveContractProperty> primitiveProperties = new List<PrimitiveContractProperty>();

        public void AddProperty(PrimitiveContractProperty newProperty)
        {
            PrimitiveProperties.Add(newProperty);
            migrator.Notify(
                () => PrimitiveProperties,
                () => PrimitiveProperties.Add(newProperty),
                () => PrimitiveProperties.Remove(newProperty), MigratorMode.EveryChange);
        }

        public void RemoveProperty(PrimitiveContractProperty removeProperty)
        {
            PrimitiveProperties.Remove(removeProperty);
            migrator.Notify(
                () => PrimitiveProperties,
                () => PrimitiveProperties.Remove(removeProperty),
                () => PrimitiveProperties.Add(removeProperty), MigratorMode.EveryChange);
        }


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
                    migrator.Notify(() => referenceProperties, d => referenceProperties = d,
                            MigratorMode.Smart);
                referenceProperties = value;
            }
        }
        List<ReferenceContractProperty> referenceProperties = new List<ReferenceContractProperty>();

        public void AddProperty(ReferenceContractProperty newProperty)
        {
            ReferenceProperties.Add(newProperty);
            migrator.Notify(
                () => ReferenceProperties,
                () => ReferenceProperties.Add(newProperty),
                () => ReferenceProperties.Remove(newProperty), MigratorMode.EveryChange);
        }

        public void RemoveProperty(ReferenceContractProperty removeProperty)
        {
            ReferenceProperties.Remove(removeProperty);
            migrator.Notify(
                () => ReferenceProperties,
                () => ReferenceProperties.Remove(removeProperty),
                () => ReferenceProperties.Add(removeProperty), MigratorMode.EveryChange);
        }

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
