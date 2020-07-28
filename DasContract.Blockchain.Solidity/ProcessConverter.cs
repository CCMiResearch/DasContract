using DasToSolidity.Exceptions;
using DasToSolidity.Solidity.ConversionHelpers;
using DasContract.Abstraction;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Solidity;
using Liquid.NET;
using System.Linq;
using System;
using System.Collections.Generic;
using DasContract.DasContract.Blockchain.Solidity;

namespace DasToSolidity.SolidityConverter
{
    class ProcessConverter
    {
        SolidityContract solidityContract;
        Process process;
        List<Entity> entities;
        List<SolidityStruct> dataModel = new List<SolidityStruct>();

        static readonly string SOLIDITY_VERSION = "^0.6.6";
        public static readonly string IS_STATE_ACTIVE_FUNCTION_NAME = "isStateActive";
        public static readonly string ACTIVE_STATES_NAME = "ActiveStates";
        public static readonly string STATE_PARAMETER_NAME = "state";

        // Import for ERC721 token usage only, remove otherwise
        LiquidTemplate template = LiquidTemplate.Create("pragma solidity " + SOLIDITY_VERSION + ";\n\n" +
            "import \"https://github.com/OpenZeppelin/openzeppelin-contracts/blob/release-v3.1.0/contracts/token/ERC721/IERC721.sol\";\n\n" +
            "{{contract}}").LiquidTemplate;

        public ProcessConverter(Contract contract)
        {
            process = contract.Process;
            entities = (List<Entity>)contract.Entities;
            solidityContract = new SolidityContract("GeneratedContract");

            AddDataModel();

            // ERC721 HouseToken address, only for Mortgage usecase, remove otherwise
            solidityContract.AddComponent(new SolidityStatement("IERC721 houseTokenAddress = IERC721(address(0x93b6784CE509d1cEA255aE8269af4Ee258943Bd0))"));

            //Current state declaration
            solidityContract.AddComponent(new SolidityStatement("mapping (string => bool) public " + ACTIVE_STATES_NAME));
            solidityContract.AddComponent(new SolidityStatement("mapping (string => address) public " + Helpers.ADDRESS_MAPPING_VAR_NAME));

            //Method for retrieving current state
            var getStateFunction = new SolidityFunction(IS_STATE_ACTIVE_FUNCTION_NAME, SolidityVisibility.Public, "bool");
            getStateFunction.AddToBody(new SolidityStatement("return " + ACTIVE_STATES_NAME + "[" + STATE_PARAMETER_NAME + "]"));
            getStateFunction.AddParameter(new SolidityParameter("string", STATE_PARAMETER_NAME));
            solidityContract.AddComponent(getStateFunction);

            IterateProcess();
        }

        //TODO: Working, but not very nice solution. Might need update
        void AddDataModel()
        {
            foreach (var e in entities)
            {
                SolidityStruct s = new SolidityStruct(e);
                SolidityStatement statement = new SolidityStatement();
                List<Property> newProperties = new List<Property>();
                newProperties.Clear();
                foreach (var p in e.Properties)
                {
                    string type = Helpers.GetSolidityStringType(p);
                    if (!p.IsCollection)
                        statement.Add(type + " " + Helpers.GetPropertyVariableName(p.Name));
                    else if (p.IsCollection)
                    {
                        statement.Add("mapping (uint => " + type + ") " + Helpers.GetPropertyVariableName(p.Name));

                        Property property = new Property();
                        string name = Helpers.GetPropertyVariableName(p.Name) + "Length";
                        property.Type = PropertyType.Int;
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
        }

        /// <summary>
        /// Iterates through the process model and creates the parsed solidity structure
        /// BFS algorithm is used to go through the model.
        /// !!! This approach is not very efficient and may create further issues if expanded upon.
        /// I have chosen it for simplicity, better solution would probably be recreating the given process model
        /// into a graph with references as links between each element. That would allow more flexibility for the converter
        /// objects, which is quite limited in the current implementation.
        /// </summary>
        /// <param name="process">The BPMN process model</param>
        void IterateProcess()
        {
            var flagged = new HashSet<string>();
            var toVisit = new Queue<ProcessElement>();
            //Find the startEvent
            var startEvent = FindStartEvent();
            toVisit.Enqueue(startEvent);
            flagged.Add(startEvent.Id);
            //BFS - go through every element
            while(toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                var nextElements = new List<ElementConverter>();
                //Iterate through all outgoing sequence flow ids of this element
                foreach (var outSequenceFlowId in current.Outgoing)
                {
                    //Convert the sequence flow id to its target element
                    var nextElement = GetSequenceFlowTarget(outSequenceFlowId);
                    nextElements.Add(ConverterFactory.CreateConverter(nextElement));
                    //Add to queue if not visited yet and flag it
                    if (!flagged.Contains(nextElement.Id))
                    {
                        toVisit.Enqueue(nextElement);
                        flagged.Add(nextElement.Id);
                    }
                }
                //Create converter for the current element and use it to generate code elements
                var elementConverter = ConverterFactory.CreateConverter(current);
                var elementCode = elementConverter.GetElementCode(nextElements, SeqFlowIdToObject(current.Outgoing), dataModel);
                solidityContract.AddComponents(elementCode);
            }
        }

        /// <summary>
        /// Converts List of sequence flow IDs to a list of Sequence flows with the corresponding ID
        /// </summary>
        /// <param name="sequenceFlowIds">List of sequence flow IDs</param>
        /// <returns>List of sequence flow objects</returns>
        IList<SequenceFlow> SeqFlowIdToObject(IList<string> sequenceFlowIds)
        {
            var seqFlows = new List<SequenceFlow>();
            foreach(var id in sequenceFlowIds)
            {
                seqFlows.Add(process.SequenceFlows[id]);
            }

            return seqFlows;
        }

        ProcessElement GetSequenceFlowTarget(string seqFlowId)
        {
            var sequenceFlow = process.SequenceFlows[seqFlowId];
            return process.ProcessElements[sequenceFlow.TargetId];
        }
        /// <summary>
        /// Finds the start event inside of the process
        /// </summary>
        /// <returns>Start Event</returns>
        StartEvent FindStartEvent()
        {
            foreach (var e in process.Events)
            {
                if (e.GetType() == typeof(StartEvent)) 
                    return (StartEvent) e;
            }
            throw new NoStartEventException("The process must contain a startEvent");
        }
    
        /// <summary>
        /// Generates the solidity code
        /// </summary>
        /// <returns>Solidity code in string</returns>
        public string GenerateSolidity()
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("contract", solidityContract.ToLiquidString(0));

            return template.Render(ctx).Result;
        }
    }
}
