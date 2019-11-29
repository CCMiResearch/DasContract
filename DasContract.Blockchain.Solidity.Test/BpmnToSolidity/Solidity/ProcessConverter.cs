using BpmnToSolidity.Exceptions;
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
        List<SolidityContract> contracts;

        static readonly string SOLIDITY_VERSION = "0.5.1";
        public static readonly string STATE_NAME = "currentState";

            
        //SolidityConstructor constructor;
        LiquidTemplate template = LiquidTemplate.Create("pragma solidity " + SOLIDITY_VERSION + ";\n\n" +
            "{{contracts}}").LiquidTemplate;

        public ProcessConverter(Contract contract)
        {
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
                var nextElements = new List<ProcessElement>();

                foreach(var outSequenceFlowId in current.Outgoing)
                {
                    var nextElement = GetSequenceFlowTarget(outSequenceFlowId, process);
                    nextElements.Add(nextElement);
                    if (flagged.Contains(nextElement.Id))
                        continue;

                    toVisit.Enqueue(nextElement);
                    flagged.Add(nextElement.Id);
                }
            }
        }

        IList<SolidityComponent> CreateElementCode(ProcessElement element, IList<ProcessElement> nextElements)
        {
            return null;
        }

        SolidityFunction CreateFunctionForPrev(ProcessElement element)
        {

            return null;
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
        

        void createStatements()
        {
            /*
            SolidityEnum enumComponent = new SolidityEnum("State", states);
            enumComponent.setIndent(1);
            SolidityStatement enumDefinition = new SolidityStatement("State public state");
            enumDefinition.setIndent(1);

            statements.Add(enumComponent);
            statements.Add(enumDefinition);
            */
        }
        /*

        void createGetStateFunction()
        {
            foreach (var s in states) {
                SolidityFunction function = new SolidityFunct ion("getState", SolidityVisibility.Public, "uint");
                function.setIndent(1);
                function.addToBody(new SolidityStatement("uint " + s));
                function.addToBody(new SolidityStatement("return uint(state)"));
                function.addParameter(new SolidityParameter("uint", "par1"));
                function.addParameter(new SolidityParameter("uint", "par2"));
                functions.Add(function);
            }
        }


        void createAdvanceFunction()
        {
            
            SolidityFunction function = new SolidityFunction("advance", SolidityVisibility.Public, "");
            function.setIndent(1);
            bool first = true;
            for (int i = 0; i < states.Count-1; i++)
            {
                var ifStatement = new SolidityIf("state == State." + states[i], !first);
                function.addToBody(ifStatement);
                ifStatement.AddToBody(new SolidityStatement("state = State." + states[i + 1]));
                first = false;
            }
            functions.Add(function);
            
        }
    */
    
        public string generateSolidity()
        {
            ITemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("contracts", contractsToLiquid());

            return template.Render(ctx).Result;
        }

        LiquidCollection contractsToLiquid()
        {
            var collection = new LiquidCollection();
            foreach (var c in contracts)
                collection.Add(c.ToLiquidString());
            return collection;
        }
    }
}
