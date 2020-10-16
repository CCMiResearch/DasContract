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
        IList<ProcessConverter> processConverters = new List<ProcessConverter>();
        IList<DataType> dataTypes;
        IList<SolidityStruct> dataModel = new List<SolidityStruct>();

        static readonly string SOLIDITY_VERSION = "^0.6.6";


        // Import for ERC721 token usage only, remove otherwise
        LiquidTemplate template = LiquidTemplate.Create("pragma solidity " + SOLIDITY_VERSION + ";\n\n" +
            "import \"https://github.com/OpenZeppelin/openzeppelin-contracts/blob/release-v3.1.0/contracts/token/ERC721/IERC721.sol\";\n\n" +
            "{{contracts}}").LiquidTemplate;

        public ContractConverter(Contract contract)
        {
            var processes = contract.Processes;
            dataTypes = contract.DataTypes.ToList();
            
            AddDataModel();

            foreach(var process in processes)
            {
                var processConverter = new ProcessConverter(process);
                processConverter.ConvertProcess();
                processConverters.Add(processConverter);
            }

            
        }

        //TODO: Working, but not very nice solution. Might need update
        void AddDataModel()
        {
            /*
            foreach (var dataType in dataTypes)
            {
                SolidityStruct s = new SolidityStruct(dataType);
                SolidityStatement statement = new SolidityStatement();
                List<Property> newProperties = new List<Property>();
                newProperties.Clear();
                foreach (var p in dataType.Properties)
                {
                    string type = Helpers.GetSolidityStringType(p);
                    if (!p.IsCollection)
                        statement.Add(type + " " + Helpers.GetPropertyVariableName(p.Name));
                    else if (p.IsCollection)
                    {
                        statement.Add("mapping (uint => " + type + ") " + Helpers.GetPropertyVariableName(p.Name));

                        Property property = new Property();
                        string name = Helpers.GetPropertyVariableName(p.Name) + "Length";
                        property.DataType = PropertyDataType.Int;
                        property.Name = name;
                        statement.Add("uint " + name);
                        newProperties.Add(property);
                    }
                }
                foreach (var np in newProperties)
                {
                    s.En.Properties.Add(np);
                }
                s.AddToBody(statement);
                solidityContract.AddComponent(s);
                dataModel.Add(s);
            }
            */
        }

        private LiquidCollection ProcessesToLiquid()
        {
            var collection = new LiquidCollection();
            foreach (var processConverter in processConverters)
            {
                collection.Add(processConverter.ToLiquidString());
            }
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
