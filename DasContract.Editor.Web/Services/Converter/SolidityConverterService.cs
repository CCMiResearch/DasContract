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

        public bool TryConvertContract(Contract contract, out string data)
        {
            ContractConverter converter = new ContractConverter(contract);
            try
            {
                converter.ConvertContract();
                data = converter.GetSolidityCode();
                return true;
            }
            catch (Exception e)
            {
                data = e.Message;
                return false;
            }
        }
    }
}
