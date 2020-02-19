using DasContract.Abstraction.DataModel.Property.Primitive;
using DasContract.Abstraction.DataModel.Property.Reference;
using DasContract.DasContract.Abstraction.Interface;
using DasContract.Migrator;
using DasContract.Migrator.Interface;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DasContract.Abstraction.DataModel.Entity
{
    public class ContractEntity: IIdentifiable, INamable, IMigratableComponent<ContractEntity, IMigrator>
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
        /// Primitive properties of this entity
        /// </summary>
        public List<PrimitiveContractProperty> PrimitiveProperties
        {
            get
            {
                if (primitivePropertiesMigrated)
                    return primitiveProperties;

                foreach (var item in primitiveProperties)
                    item.WithMigrator(migrator);
                primitivePropertiesMigrated = true;
                return primitiveProperties;
            }
            set
            {
                if (value != primitiveProperties)
                    migrator.Notify(() => primitiveProperties, d => primitiveProperties = d);
                primitiveProperties = value;
            }
        }
        bool primitivePropertiesMigrated = false;
        List<PrimitiveContractProperty> primitiveProperties = new List<PrimitiveContractProperty>();

        /// <summary>
        /// Reference properties of this entity
        /// </summary>
        public List<ReferenceContractProperty> ReferenceProperties
        {
            get
            {
                if (referencePropertiesMigrated)
                    return referenceProperties;

                foreach (var item in referenceProperties)
                    item.WithMigrator(migrator);
                referencePropertiesMigrated = true;
                return referenceProperties;
            }
            set
            {
                if (value != referenceProperties)
                    migrator.Notify(() => referenceProperties, d => referenceProperties = d);
                referenceProperties = value;
            }
        }
        bool referencePropertiesMigrated = false;
        List<ReferenceContractProperty> referenceProperties = new List<ReferenceContractProperty>();

        /// <summary>
        /// Collection of reference properties of this entity
        /// </summary>
        public List<CollectionReferenceContractProperty> CollectionReferenceProperties
        {
            get
            {
                if (collectionReferencePropertiesMigrated)
                    return collectionReferenceProperties;

                foreach (var item in collectionReferenceProperties)
                    item.WithMigrator(migrator);
                collectionReferencePropertiesMigrated = true;
                return collectionReferenceProperties;
            }
            set
            {
                if (value != collectionReferenceProperties)
                    migrator.Notify(() => collectionReferenceProperties, d => collectionReferenceProperties = d);
                collectionReferenceProperties = value;
            }
        }
        bool collectionReferencePropertiesMigrated = false;
        List<CollectionReferenceContractProperty> collectionReferenceProperties = new List<CollectionReferenceContractProperty>();

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        [XmlIgnore]
        IMigrator migrator = new SimpleMigrator();

        public ContractEntity WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
