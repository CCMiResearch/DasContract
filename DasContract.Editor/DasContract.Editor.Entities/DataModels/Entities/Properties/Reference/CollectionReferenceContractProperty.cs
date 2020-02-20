using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Editor.Entities.DataModels.Entities.Properties.Reference
{
    public class CollectionReferenceContractProperty : ContractProperty
    {
        /// <summary>
        /// The linked contract entity
        /// </summary>
        [XmlIgnore]
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

                EntityIds = value.Select(e => e.Id).ToList();
            }
        }
        List<ContractEntity> entities = new List<ContractEntity>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<string> EntityIds { get; set; } = new List<string>();
    }
}
