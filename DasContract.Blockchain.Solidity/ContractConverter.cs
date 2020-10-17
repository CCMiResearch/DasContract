using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using Liquid.NET;
using System.Linq;
using System.Collections.Generic;
using DasContract.Abstraction.Exceptions;
using DasContract.Blockchain.Solidity.SolidityComponents;
using DasContract.Blockchain.Solidity.Converters;
using Liquid.NET.Constants;

namespace DasContract.Blockchain.Solidity
{
    public class ContractConverter
    {
        public Contract Contract { get; private set; }

        IList<ProcessConverter> processConverters = new List<ProcessConverter>();
        
        static readonly string SOLIDITY_VERSION = "^0.6.6";


        // Import for ERC721 token usage only, remove otherwise
        LiquidTemplate template = LiquidTemplate.Create("pragma solidity " + SOLIDITY_VERSION + ";\n\n" +
            "import \"https://github.com/OpenZeppelin/openzeppelin-contracts/blob/release-v3.1.0/contracts/token/ERC721/IERC721.sol\";\n\n" +
            "{{contracts}}").LiquidTemplate;

        public ContractConverter(Contract contract)
        {
            this.Contract = contract;
            var processes = contract.Processes;
            
            
            AddDataModel();

            foreach(var process in processes)
            {
                var processConverter = new ProcessConverter(process, this);
                processConverter.ConvertProcess();
                processConverters.Add(processConverter);
            }

            
        }

        //TODO: Working, but not very nice solution. Might need update
        void AddDataModel()
        {

            
            
        }

        private LiquidCollection ProcessesToLiquid()
        {
            var collection = new LiquidCollection();
            /* //TODO
            foreach (var processConverter in processConverters)
            {
                collection.Add(processConverter.ToLiquidString());
            }
            */
            return collection;
        }

        public string GetSolidityCode()
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("contracts", ProcessesToLiquid());
            return template.Render(ctx).Result;
        }


    }
}
