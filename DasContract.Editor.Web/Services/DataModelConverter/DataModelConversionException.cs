using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.DataModel
{
    public class DataModelConversionException : Exception
    {
        public DataModelConversionException(string message) : base(message) { }
    }
}
