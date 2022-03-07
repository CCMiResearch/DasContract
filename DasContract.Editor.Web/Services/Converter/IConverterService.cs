using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Converter
{
    public interface IConverterService
    {
        /// <summary>
        /// Tries to convert the contract to the code of the intended platform.
        /// </summary>
        /// <param name="contract">Contract to convert</param>
        /// <param name="data">Converted code if conversion was successful, error message if unsuccessful</param>
        /// <returns>Boolean indicating if conversion was successful</returns>
        bool TryConvertContract(Contract contract, out string data);
    }
}
