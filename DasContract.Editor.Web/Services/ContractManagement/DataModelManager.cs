using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public class DataModelManager : IDataModelManager
    {
        private Contract Contract { get; set; }

        public void SetContract(Contract contract)
        {
            Contract = contract;
        }

        public IList<Property> GetCollectionProperties()
        {
            var collectionProperties = new List<Property>();
            foreach (var entity in Contract.Entities)
            {
                collectionProperties.AddRange(entity.Properties
                    .Where(p => p.PropertyType == PropertyType.Collection || p.PropertyType == PropertyType.Dictionary).ToList());
            }

            return collectionProperties;
        }

        public Property GetPropertyById(string propertyId)
        {
            foreach (var entity in Contract.Entities)
            {
                foreach (var property in entity.Properties)
                {
                    if (property.Id == propertyId)
                        return property;
                }
            }
            return null;
        }

        public string GetDataModelXml()
        {
            return Contract.DataModelDefinition;
        }

        public IDictionary<string, DataType> GetDataTypes()
        {
            return Contract.DataTypes;
        }

        public void SetDataModelXml(string dataModelXml)
        {
            Contract.DataModelDefinition = dataModelXml;
            var xDataModel = XElement.Parse(dataModelXml);
            Contract.SetDataModelFromXml(xDataModel);
        }
    }
}
