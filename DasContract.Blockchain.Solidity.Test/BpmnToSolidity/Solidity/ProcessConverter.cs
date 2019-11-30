using BpmnToSolidity.Exceptions;
using BpmnToSolidity.Solidity.ConversionHelpers;
using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Abstraction.Solidity;
using Liquid.NET;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.SolidityConverter
{
    class ProcessConverter
    {
        SolidityContract solidityContract;
        Process process;

        static readonly string SOLIDITY_VERSION = "0.5.1";
        public static readonly string STATE_NAME = "currentState";

        LiquidTemplate template = LiquidTemplate.Create("pragma solidity " + SOLIDITY_VERSION + ";\n\n" +
            "{{contract}}").LiquidTemplate;

        public ProcessConverter(Contract contract)
        {
            process = contract.Process;
            solidityContract = new SolidityContract("GeneratedContract");

            solidityContract.AddComponent(new SolidityStatement("int a")); //TODO TEMPORARY FÖR TESTING

            //Current state declaration
            solidityContract.AddComponent(new SolidityStatement("string " + STATE_NAME));
            //Method for retrieving current state
            var getStateFunction = new SolidityFunction("getCurrentState", SolidityVisibility.Public, "string");
            getStateFunction.AddToBody(new SolidityStatement("return " + STATE_NAME));
            solidityContract.AddComponent(getStateFunction);
            
            IterateProcess();
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
                var elementCode = elementConverter.GetElementCode(nextElements, SeqFlowIdToObject(current.Outgoing));
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
