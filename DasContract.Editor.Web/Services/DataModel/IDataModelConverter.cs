using DasContract.Abstraction.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.DataModel
{
    public interface IDataModelConverter
    {
        string ConvertToMermaid(IDictionary<string, DataType> dataTypes);
    }
}
