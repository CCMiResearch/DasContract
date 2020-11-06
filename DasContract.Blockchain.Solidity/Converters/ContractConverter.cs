using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.Converters.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;
using Liquid.NET;
using Liquid.NET.Constants;
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
        static readonly LiquidTemplate template = LiquidTemplate.Create("pragma solidity " + ConverterConfig.SOLIDITY_VERSION + ";\n\n" +
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
            //Find the root proces (the one marked as executable) and create its process converter
            var rootProcessConverter = new ProcessConverter(GetExecutableProcess(), this);
            processConverters.Add(rootProcessConverter.Id, rootProcessConverter);
            //Store the newly found converters in a queue
            var processConverterQueue = new Queue<ProcessConverter>();
            processConverterQueue.Enqueue(rootProcessConverter);
            
            while (processConverterQueue.Count() > 0)
            {
                var currentConverter = processConverterQueue.Dequeue();
                var inheritedIdentifiers = processConverters[currentConverter.Id].InstanceIdentifiers;
                var callActivities = currentConverter.Process.Tasks.Where(t => t is CallActivity).Cast<CallActivity>();
                //Look through all call activities of the current process converter
                foreach (var callActivity in callActivities)
                {
                    if (Contract.TryGetProcess(callActivity.CalledElement, out var referencedProcess))
                    {
                        //Create a new process converter based on the target process of the callactivity
                        var calledProcessConverter = new ProcessConverter(referencedProcess, this, callActivity.Id, currentConverter);
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

        bool TryGetIdentifier(CallActivity callActivity, out ProcessInstanceIdentifier identifier)
        {

            if(callActivity.InstanceType == InstanceType.Parallel && (callActivity.LoopCollection != null || callActivity.LoopCardinality != 0))
            {
                identifier = new ProcessInstanceIdentifier(callActivity.Id);
                return true;
            }

            identifier = null;
            return false;
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
            mainSolidityContract.AddComponents(DataModelConverter.GetSolidityComponents());
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

        LiquidCollection GetDependencies()
        {
            LiquidCollection liquidCol = new LiquidCollection();
            HashSet<string> dependencies = new HashSet<string>();
            dependencies.UnionWith(DataModelConverter.GetDependencies());

            foreach (var dependency in dependencies)
            {
                var statement = new SolidityStatement($"import {dependency}");
                liquidCol.Add(statement.ToLiquidString(0));
            }

            return liquidCol;
        }

        public string GetSolidityCode()
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("maincontract", mainSolidityContract.ToLiquidString(0))
                .DefineLocalVariable("tokencontracts", DataModelConverter.TokenContractsToLiquid())
                .DefineLocalVariable("imports", GetDependencies());
            return template.Render(ctx).Result;
        }


    }
}
