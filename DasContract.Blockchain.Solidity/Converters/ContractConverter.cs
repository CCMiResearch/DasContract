using DasContract.Abstraction;
using Liquid.NET;
using System.Collections.Generic;
using DasContract.Blockchain.Solidity.SolidityComponents;
using Liquid.NET.Constants;
using DasContract.Abstraction.Data;
using System.Linq;

namespace DasContract.Blockchain.Solidity.Converters
{
    public class ContractConverter
    {
        public Contract Contract { get; private set; }
        public DataModelConverter DataModelConverter { get; private set; }

        IList<ProcessConverter> processConverters = new List<ProcessConverter>();

        SolidityContract mainSolidityContract;

        // Import for ERC721 token usage only, remove otherwise
        LiquidTemplate template = LiquidTemplate.Create("pragma solidity " + ConverterConfig.SOLIDITY_VERSION + ";\n\n" +
            "{{imports}}\n\n" +
            "{{tokencontracts}}\n\n" +
            "{{maincontract}}").LiquidTemplate;

        public ContractConverter(Contract contract)
        {
            Contract = contract;
            mainSolidityContract = new SolidityContract(contract.Id);

            DataModelConverter = new DataModelConverter(this);
            foreach (var process in Contract.Processes)
            {
                processConverters.Add(new ProcessConverter(process, this));
            }
        }

        public void ConvertContract()
        {
            //Convert data model logic
            DataModelConverter.ConvertLogic();
            //Add enum and struct definitions to the main contract
            mainSolidityContract.AddComponents(DataModelConverter.GetDataStructuresDefinitions());
            //Convert all processes and add their logic to the main contract
            foreach (var processConverter in processConverters)
            {
                processConverter.ConvertProcess();
                mainSolidityContract.AddComponents(processConverter.GetGeneratedSolidityComponents());
            }
        }

        public DataType GetDataType(string dataTypeId)
        {
            return Contract.DataTypes[dataTypeId];
        }

        public T GetDataTypeOfType<T>(string dataTypeId) where T: DataType
        {
            var dataType = Contract.DataTypes[dataTypeId];
            if (dataType is T)
                return dataType as T;
            return null; //TODO throw exception
        }

        public string GetSolidityCode()
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("maincontract", mainSolidityContract.ToLiquidString(0))
                .DefineLocalVariable("tokencontracts", DataModelConverter.TokenContractsToLiquid())
                .DefineLocalVariable("imports", DataModelConverter.GetDependencies().ToLiquidString(0));
            return template.Render(ctx).Result;
        }


    }
}
