using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Converter
{
    public interface IConverterService
    {
        public IConversionStrategy ConversionStrategy { get; set; }
        string GetConvertedCode();
        string GetErrorMessage();
        /// <summary>
        /// Tries to convert the contract to the code of the intended platform.
        /// </summary>
        /// <param name="contract">Contract to convert</param>
        /// <returns>Boolean indicating if conversion was successful</returns>
        bool ConvertContract(Contract contract);
    }
}
