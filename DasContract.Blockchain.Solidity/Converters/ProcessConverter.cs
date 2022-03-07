using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Exceptions;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Blockchain.Solidity.Converters.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters
{
    public class ProcessConverter
    {
        public string Id { get { return ConversionTemplates.ProcessConverterId(Process.Id, ParentCallActivityId); } }


        public bool IsRootProcess { get; private set; }
        public string ParentCallActivityId { get; private set; }
        public ProcessConverter ParentProcessConverter { get; private set; }

        /// <summary>
        /// If the process is a subprocess called by a parallel multi-instance CallActivity,
        /// then the instances need to be identified by some Property. These multi-instance
        /// subprocesses could theoretically be nested, hence the list.
        /// </summary>
        public List<ProcessInstanceIdentifier> InstanceIdentifiers { get; } = new List<ProcessInstanceIdentifier>();


        public Process Process { get; private set; }
        public Contract Contract { get { return ContractConverter.Contract; } }
        public ContractConverter ContractConverter { get; }
        IDictionary<string, ElementConverter> elementConverters = new Dictionary<string, ElementConverter>();

        IList<SolidityComponent> generalProcessComponents = new List<SolidityComponent>();

        public ProcessConverter(Process process, ContractConverter contractConverter, string parentCallActivityCallName = null, ProcessConverter parentProcessConverter = null)
        {
            Process = process;
            ParentCallActivityId = parentCallActivityCallName;
            ParentProcessConverter = parentProcessConverter;
            ContractConverter = contractConverter;
            if (parentCallActivityCallName == null)
                IsRootProcess = true;
            else
                IsRootProcess = false;
            CreateElementConverters();
        }

        public IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var generatedComponents = new List<SolidityComponent>();
            generatedComponents.AddRange(generalProcessComponents);
            
            foreach (var converter in elementConverters.Values)
            {
                generatedComponents.AddRange(converter.GetGeneratedSolidityComponents());
            }
            generatedComponents.Add(new SolidityStatement(Process.CustomScript, false));
            return generatedComponents;
        }

        public StartEventConverter GetStartEventConverter()
        {
            var startEvents = Process.Events.Where(e => e is StartEvent);
            if (startEvents.Count() != 1)
                return null; //TODO throw exception
            return GetConverterOfElementOfType<StartEventConverter>(startEvents.First().Id);
        }

        SolidityStatement CreateActiveStatesMapping()
        {
            var mappingStatement = new SolidityMappingStatement(
                "string",
                "bool",
                ConversionTemplates.ActiveStatesMappingName(Id),
                InstanceIdentifiers.Select(i => new Property { DataType = PropertyDataType.Uint }).ToList());
            return new SolidityStatement(mappingStatement.ToString());
        }

        SolidityFunction CreateGetStateFunction()
        {
            var functionName = ConversionTemplates.ActiveStatesFunctionName(Id);
            var getStateFunction = new SolidityFunction(functionName, SolidityVisibility.Public, "bool", true);
            getStateFunction.AddToBody(new SolidityStatement($"return " +
                $"{ConversionTemplates.ActiveStateAssignment(ConverterConfig.STATE_PARAMETER_NAME, Id, InstanceIdentifiers, true)}"));
            //Add the potential process identifiers
            getStateFunction.AddParameters(GetIdentifiersAsParameters());
            getStateFunction.AddParameter(new SolidityParameter("string", ConverterConfig.STATE_PARAMETER_NAME));
            return getStateFunction;
        }

        public void ConvertProcess()
        {
            generalProcessComponents.Clear();
            //Mapping of the current states
            generalProcessComponents.Add(CreateActiveStatesMapping());
            //TODO: this address mapping will not work for roles
            //generalProcessComponents.Add(new SolidityStatement("mapping (string => address) public " + ConverterConfig.ADDRESS_MAPPING_VAR_NAME));

            //Method for retrieving current state
            generalProcessComponents.Add(CreateGetStateFunction());

            //Convert process elements
            foreach (var converter in elementConverters.Values)
            {
                converter.ConvertElementLogic();
            }
        }

        /// <summary>
        /// Returns a list of of converters that are connected to the given element via outgoing sequence flows.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public IList<ElementConverter> GetTargetConvertersOfElement(ProcessElement element)
        {
            var targetConverters = new List<ElementConverter>();
            foreach (var outgoingSequenceFlow in element.Outgoing)
            {
                var target = GetSequenceFlowTarget(outgoingSequenceFlow);
                targetConverters.Add(elementConverters[target.Id]);
            }
            return targetConverters;
        }

        public Tuple<Property, Entity> GetPropertyAndEntity(string propertyId)
        {
            foreach (var entity in Contract.Entities)
            {
                var property = entity.Properties.FirstOrDefault(e => e.Id == propertyId);
                if (property != null)
                    return Tuple.Create(property, entity);
            }
            return null;
        }
        public Property GetProperty(string propertyId)
        {
            foreach (var entity in Contract.Entities)
            {
                var property = entity.Properties.FirstOrDefault(e => e.Id == propertyId);
                if (property != null)
                    return property;
            }
            return null;
        }

        public SolidityStatement GetStatementOfNextElement(string elementId)
        {
            return GetStatementOfNextElement(Process.ProcessElements[elementId]);
        }

        public SolidityStatement GetStatementOfNextElement(ProcessElement element)
        {
            var targetConverters = GetTargetConvertersOfElement(element);
            //TODO throw exception if more than one target converter or none have been found
            return targetConverters.First().GetStatementForPrevious(element);
        }

        public ElementConverter GetConverterOfElement(ProcessElement element)
        {
            return elementConverters[element.Id];
        }

        public ElementConverter GetConverterOfElement(string elementId)
        {
            return elementConverters[elementId];
        }

        public T GetConverterOfElementOfType<T>(string elementId)
            where T : ElementConverter
        {
            var elementConverter = elementConverters[elementId];
            if (elementConverter is T)
                return elementConverter as T;
            return null; //TODO throw exception
        }

        //TODO: Check whether it works properly
        public IList<SequenceFlow> GetOutgoingSequenceFlows(ProcessElement element)
        {
            return element.Outgoing.Select(id => Process.SequenceFlows[id]).ToList();
        }

        /// <summary>
        /// Returns the callname of the element that is connected to the provided
        /// element via an outgoing sequence flow
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public string GetCallNameOfNextElement(ProcessElement element)
        {
            var targetConverters = GetTargetConvertersOfElement(element);
            //TODO throw exception if more than one target converter or none have been found
            return targetConverters.First().GetElementCallName();
        }

        ProcessElement GetSequenceFlowTarget(string seqFlowId)
        {
            var sequenceFlow = Process.SequenceFlows[seqFlowId];
            return Process.ProcessElements[sequenceFlow.TargetId];
        }

        public string GetIdentifierNames()
        {
            return string.Join(",", InstanceIdentifiers.Select(i => i.IdentifierName)
                .ToList());
        }

        public List<SolidityParameter> GetIdentifiersAsParameters()
        {
            var parameters = new List<SolidityParameter>();
            foreach (var identifier in InstanceIdentifiers)
            {
                parameters.Add(new SolidityParameter(Helpers.PrimitivePropertyTypeToString(PropertyDataType.Uint), identifier.IdentifierName));
            }
            return parameters;
        }

        private void CreateElementConverters()
        {
            foreach (var element in Process.ProcessElements.Values)
            {
                elementConverters[element.Id] = ConverterFactory.CreateConverter(element, this);
            }
        }

        public string GetElementCallName(string elementId)
        {
            //TODO check if ID valid
            return GetElementCallName(Process.ProcessElements[elementId]);
        }

        public string GetElementCallName(ProcessElement element)
        {
            string callName;
            //TODO: check whether name is unique, short enough, etc...
            if (!string.IsNullOrWhiteSpace(element.Name) && element.Name.Length <= 20)
                callName = Helpers.ToUpperCamelCase(element.Name);
            else
                callName = element.Id;

            if (ParentCallActivityId != null)
                callName = $"{ParentCallActivityId}_{callName}";

            return callName;
        }

        public string GetParentCallActivityCallName()
        {
            //TODO check invalid id
            if (ParentCallActivityId == null)
                return null;
            return ParentProcessConverter.GetElementCallName(ParentCallActivityId);
        }
    }
}
