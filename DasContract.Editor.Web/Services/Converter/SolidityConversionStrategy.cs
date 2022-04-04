using DasContract.Abstraction;
using DasContract.Blockchain.Solidity.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Converter
{
    public class SolidityConversionStrategy : ConversionStrategy
    {

        public override bool Convert(Contract contract)
        {
            try
            {
                var converter = new ContractConverter(contract);
                converter.ConvertContract();
                ConvertedCode = converter.GetSolidityCode();
                ErrorMessage = null;
                return true;
            }
            catch (Exception e)
            {
                ConvertedCode = null;
                ErrorMessage = e.Message;
                return false;
            }
        }
    }
}
