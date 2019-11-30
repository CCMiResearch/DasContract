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
        SolidityContract contract;

        static readonly string SOLIDITY_VERSION = "0.5.1";
        public static readonly string STATE_NAME = "currentState";

            
        //SolidityConstructor constructor;
        LiquidTemplate template = LiquidTemplate.Create("pragma solidity " + SOLIDITY_VERSION + ";\n\n" +
            "{{contract}}").LiquidTemplate;

        public ProcessConverter(Contract contract)
        {
            this.contract = new SolidityContract("GeneratedContract");
            IterateProcess(contract.Process);
        }

        void IterateProcess(Process process)
        {
            var flagged = new HashSet<string>();
            var toVisit = new Queue<ProcessElement>();

            var startEvent = FindStartEvent(process.Events);
            toVisit.Enqueue(startEvent);
            flagged.Add(startEvent.Id);

            while(toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                var nextElements = new List<ElementConverter>();

                foreach(var outSequenceFlowId in current.Outgoing)
                {
                    var nextElement = GetSequenceFlowTarget(outSequenceFlowId, process);
                    nextElements.Add(ConverterFactory.CreateConverter(nextElement));
                    if (flagged.Contains(nextElement.Id))
                        continue;

                    toVisit.Enqueue(nextElement);
                    flagged.Add(nextElement.Id);
                }

                var elementConverter = ConverterFactory.CreateConverter(current);
                if (elementConverter != null)
                {
                    var elementCode = elementConverter.GetElementCode(nextElements, SeqFlowIdToObject(current.Outgoing,process));
                    contract.AddComponents(elementCode);
                }
            }
        }

        IList<SequenceFlow> SeqFlowIdToObject(IList<string> sequenceFlowIds, Process process)
        {
            var seqFlows = new List<SequenceFlow>();
            foreach(var id in sequenceFlowIds)
            {
                seqFlows.Add(process.SequenceFlows[id]);
            }

            return seqFlows;
        }

        ProcessElement GetSequenceFlowTarget(string seqFlowId, Process process)
        {
            var sequenceFlow = process.SequenceFlows[seqFlowId];
            return process.ProcessElements[sequenceFlow.TargetId];
        }

        StartEvent FindStartEvent(IEnumerable<Event> events)
        {
            foreach (var e in events)
            {
                if (e.GetType() == typeof(StartEvent)) 
                    return (StartEvent) e;
            }
            throw new NoStartEventException("The process must contain a startEvent");
        }
    
        public string generateSolidity()
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("contract", contract.ToLiquidString(0));

            return template.Render(ctx).Result;
        }
    }
}
