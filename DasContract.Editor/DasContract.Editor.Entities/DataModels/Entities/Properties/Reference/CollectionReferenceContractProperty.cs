using DasContract.Editor.Entities.DataModels.Entities;
using DasContract.Editor.Entities.DataModels.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Editor.Entities.DataModels.Entities.Properties.Reference
{
    public class CollectionReferenceContractProperty : ContractProperty
    {
        public CollectionReferenceContractProperty()
        {
            entities.CollectionChanged += EntitiesCollectionChanged;
        }

        private void EntitiesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            EntityIds = Entities.Select(e => e.Id).ToList();
        }

        /// <summary>
        /// The linked contract entity
        /// </summary>
        [XmlIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public ObservableCollection<ContractEntity> Entities
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

                if (value != null)
                {
                    EntityIds = value.Select(e => e.Id).ToList();
                    value.CollectionChanged += EntitiesCollectionChanged;
                }
            }
        }
        ObservableCollection<ContractEntity> entities = new ObservableCollection<ContractEntity>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<string> EntityIds { get; set; }
    }
}
