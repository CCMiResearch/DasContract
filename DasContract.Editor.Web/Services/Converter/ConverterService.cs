using DasContract.Abstraction;
using DasContract.Blockchain.Solidity.Converters;
using DasContract.Editor.Web.Services.Processes;
using DasContract.JSON;
using DasContract.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Converter
{
    public class ConverterService: IConverterService
    {
        private IContractManager _contractManager;

        public ConverterService(IContractManager contractManager)
        {
            _contractManager = contractManager;
        }

        public void ConvertContract()
        {

            /*
            ContractConverter converter = new ContractConverter(_contractManager.Contract);
            Console.WriteLine(_contractManager.Contract);
            converter.ConvertContract();
            Console.WriteLine(converter.GetSolidityCode());
            */
        }
    }
}
