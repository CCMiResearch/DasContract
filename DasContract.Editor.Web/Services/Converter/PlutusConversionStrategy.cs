using DasContract.Abstraction;
using DasContract.Blockchain.Plutus.Data.DasContractConversion.DataModels;
using DasContract.Blockchain.Plutus.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Converter
{
    public class PlutusConversionStrategy : ConversionStrategy
    {

        public override bool Convert(Contract contract)
        {
            try
            {
                var plutusContract = PlutusContractConvertor.Default.Convert(contract);
                ConvertedCode = PlutusContractGenerator.Default(plutusContract).Generate().InString();
                ErrorMessage = null;
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                ConvertedCode = null;
                ErrorMessage = e.Message;
                return false;
            }
        }
    }
}
