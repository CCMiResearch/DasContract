using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Exceptions;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters
{
    public class ProcessConverter
    {
        public Process Process { get; private set; }
        public Contract Contract { get { return ContractConverter.Contract; } }
        public ContractConverter ContractConverter { get; private set; }
        IDictionary<string, ElementConverter> elementConverters = new Dictionary<string, ElementConverter>();

        IList<SolidityComponent> generalProcessComponents = new List<SolidityComponent>();

        StartEvent GetRootStartEvent()
        {
            foreach (var e in Process.Events)
            {
                if (e.GetType() == typeof(StartEvent))
                    return (StartEvent)e;
            }
            throw new NoStartEventException("The process must contain a startEvent");
        }

        public ProcessConverter(Process process, ContractConverter contractConverter)
        {
            Process = process;
            ContractConverter = contractConverter;
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
            return generatedComponents;
        }

        public void ConvertProcess()
        {
            generalProcessComponents.Clear();
            //Current state declaration
            generalProcessComponents.Add(new SolidityStatement("mapping (string => bool) public " + ConverterConfig.ACTIVE_STATES_NAME));
            //TODO: this address mapping will not work for roles
            generalProcessComponents.Add(new SolidityStatement("mapping (string => address) public " + ConverterConfig.ADDRESS_MAPPING_VAR_NAME));

            //Method for retrieving current state
            var getStateFunction = new SolidityFunction(ConverterConfig.IS_STATE_ACTIVE_FUNCTION_NAME, SolidityVisibility.Public, "bool");
            getStateFunction.AddToBody(new SolidityStatement("return " + ConverterConfig.ACTIVE_STATES_NAME + "[" + ConverterConfig.STATE_PARAMETER_NAME + "]"));
            getStateFunction.AddParameter(new SolidityParameter("string", ConverterConfig.STATE_PARAMETER_NAME));
            generalProcessComponents.Add(getStateFunction);

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
            where T: ElementConverter
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

        private void CreateElementConverters()
        {
            foreach (var element in Process.ProcessElements.Values)
            {
                elementConverters[element.Id] = ConverterFactory.CreateConverter(element, this);
            }
        }
    }
}
