using DasContract.Abstraction;
using DasContract.Blockchain.Solidity.Converters;
using DasContract.Editor.Web.Services.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Converter
{
    public class SolidityConverterService: IConverterService
    {

        public string ConvertContract(Contract contract)
        {
            ContractConverter converter = new ContractConverter(contract);
            converter.ConvertContract();
            return converter.GetSolidityCode();
        }
    }
}
