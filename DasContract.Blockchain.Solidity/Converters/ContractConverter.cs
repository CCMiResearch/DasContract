using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.Converters.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;
using Liquid.NET;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Blockchain.Solidity.Converters
{
    public class ContractConverter
    {
        public Contract Contract { get; private set; }
        public DataModelConverter DataModelConverter { get; private set; }

        IDictionary<string, ProcessConverter> processConverters = new Dictionary<string, ProcessConverter>();

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
            CreateProcessConverters();
        }

        //TODO try to simplify this monstrous method
        void CreateProcessConverters()
        {
            var rootProcessConverter = new ProcessConverter(GetExecutableProcess(), this);
            processConverters.Add(rootProcessConverter.Id, rootProcessConverter);

            var processConverterQueue = new Queue<ProcessConverter>();
            processConverterQueue.Enqueue(rootProcessConverter);

            while (processConverterQueue.Count() > 0)
            {
                var currentConverter = processConverterQueue.Dequeue();
                var inheritedIdentifiers = processConverters[currentConverter.Id].InstanceIdentifiers;
                var callActivities = currentConverter.Process.Tasks.Where(t => t is CallActivity).Cast<CallActivity>();

                foreach (var callActivity in callActivities)
                {
                    if (Contract.TryGetProcess(callActivity.CalledElement, out var referencedProcess))
                    {
                        var calledProcessConverter = new ProcessConverter(referencedProcess, this, callActivity.Id);
                        calledProcessConverter.InstanceIdentifiers.AddRange(inheritedIdentifiers);
                        if(TryGetIdentifier(callActivity, out var identifier))
                        {
                            calledProcessConverter.InstanceIdentifiers.Add(identifier);
                        }
                        processConverters.Add(calledProcessConverter.Id, calledProcessConverter);
                        processConverterQueue.Enqueue(calledProcessConverter);
                    }
                }
            }

        }

        bool TryGetIdentifier(CallActivity callActivity, out Property identifier)
        {
            if (callActivity.InstanceType != InstanceType.Parallel)
            {
                identifier = null;
                return false;
            }
            if(callActivity.LoopCollection != null && Contract.TryGetProperty(callActivity.LoopCollection, out var property))
            {
                identifier = property;
                return true;
            }
            else if(callActivity.LoopCardinality != 0)
            {
                identifier = new Property { DataType = PropertyDataType.Int };
                return true;
            }

            identifier = null;
            return false;
        }

        public bool IsProcessCompatible(ProcessConverter processConverter, CallActivity callActivity, IList<Property> inheritedIdentifiers)
        {
            var currentIdentifiers = new List<Property>(inheritedIdentifiers);
            var processIdentifiers = processConverter.InstanceIdentifiers;
            if (callActivity.LoopCollection != null && Contract.TryGetProperty(callActivity.LoopCollection, out var property))
            {
                currentIdentifiers.Add(property);
            }
            else if (callActivity.LoopCardinality != 0)
            {
                if (processIdentifiers.Count == 0)
                    return false;
                return processIdentifiers.Take(processIdentifiers.Count - 1).SequenceEqual(currentIdentifiers)
                        && processIdentifiers.Last().DataType == PropertyDataType.Int;
            }

            return processIdentifiers.SequenceEqual(currentIdentifiers);
        }

        public StartEventConverter GetStartEventConverter(CallActivity callActivity)
        {
            var converterId = ConversionTemplates.ProcessConverterId(callActivity.CalledElement, callActivity.Id);
            if(processConverters.TryGetValue(converterId, out var processConverter))
            {
                return processConverter.GetStartEventConverter();
            }
            //TODO throw exception
            return null;
        }

        public Process GetExecutableProcess()
        {
            var executable = Contract.Processes.Where(p => p.IsExecutable);
            if (executable.Count() != 1)
                return null;
            //TODO throw exception
            return executable.First();
        }

        public void ConvertContract()
        {
            //Convert data model logic
            DataModelConverter.ConvertLogic();
            //Add enum and struct definitions to the main contract
            mainSolidityContract.AddComponents(DataModelConverter.GetDataStructuresDefinitions());
            //Convert all processes and add their logic to the main contract
            foreach (var processConverter in processConverters.Values)
            {
                processConverter.ConvertProcess();
                mainSolidityContract.AddComponents(processConverter.GetGeneratedSolidityComponents());
            }
        }

        public void AddProcessConverter(ProcessConverter processConverter)
        {
            processConverters.Add(processConverter.Process.Id, processConverter);
        }

        public DataType GetDataType(string dataTypeId)
        {
            return Contract.DataTypes[dataTypeId];
        }

        public T GetDataTypeOfType<T>(string dataTypeId) where T : DataType
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
