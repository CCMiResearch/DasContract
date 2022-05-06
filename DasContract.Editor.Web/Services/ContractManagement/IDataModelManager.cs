using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public interface IDataModelManager
    {
        void SetContract(Contract contract);
        IDictionary<string, DataType> GetDataTypes();
        Property GetPropertyById(string propertyId);
        IList<Property> GetCollectionProperties();
        string GetDataModelXml();
        void SetDataModelXml(string dataModelXml);
    }
}
