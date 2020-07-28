using System;
using System.Collections.Generic;
using System.Text;
using DasToSolidity.SolidityConverter;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Gateways;
using System.Linq;
using DasContract.Abstraction.Data;

namespace DasToSolidity.Solidity.ConversionHelpers
{
    class ParallelGatewayConverter : ElementConverter
    {
        ParallelGateway gateway;
        string incVaribaleName;

        public ParallelGatewayConverter(ParallelGateway gateway)
        {
            this.gateway = gateway;
            incVaribaleName = gateway.Id + "Incoming";
        }

        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows, IList<SolidityStruct> dataModel = null)
        {
            var logicFunction = new SolidityFunction(gateway.Id + "Logic", SolidityVisibility.Internal);
            var body = CreateParallelism(nextElements);
            if (gateway.Incoming.Count == 1)
            {
                logicFunction.AddToBody(body);
                return new List<SolidityComponent> { logicFunction };
            }
            else
            {
                var incomingFlowsVar = new SolidityStatement("int " + incVaribaleName + " = 0");
                var ifElseBlock = new SolidityIfElse();
                string ifElseCondition = incVaribaleName + "==" + gateway.Incoming.Count.ToString();
                body.Add(incVaribaleName + " = 0");
                ifElseBlock.AddConditionBlock(ifElseCondition, body);
                logicFunction.AddToBody(ifElseBlock);
                return new List<SolidityComponent> { incomingFlowsVar, logicFunction };
            }

        }

        SolidityStatement CreateParallelism(List<ElementConverter> nextElements)
        {
            var statement = new SolidityStatement();
            nextElements.ForEach(e => { e.GetStatementForPrevious(gateway).GetStatements().ForEach(e => statement.Add(e)); });
            return statement;
        }

        public override string GetElementId()
        {
            return gateway.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            SolidityStatement statement = new SolidityStatement();

            if (gateway.Incoming.Count > 1)
            {
                var commonFlow = gateway.Incoming.Intersect(previous.Outgoing);
                statement.Add(incVaribaleName + " += 1");
            }

            statement.Add(gateway.Id + "Logic" + "()");

            return statement;
        }
    }
}
