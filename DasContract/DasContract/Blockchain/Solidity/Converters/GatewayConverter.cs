using System;
using System.Collections.Generic;
using System.Text;
using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Gateways;

namespace BpmnToSolidity.Solidity.ConversionHelpers
{
    class GatewayConverter : ElementConverter
    {
        ExclusiveGateway gateway;

        public GatewayConverter(ExclusiveGateway gateway)
        {
            this.gateway = gateway;
        }

        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows)
        {
            var logicFunction = new SolidityFunction(gateway.Id + "Logic", SolidityVisibility.Internal);
            if (nextElements.Count == 1)
                logicFunction.AddToBody(nextElements[0].GetStatementForPrevious());
            else
                logicFunction.AddToBody(CreateIfElseBlock(nextElements, outgoingSeqFlows));
            return new List<SolidityComponent> { logicFunction };
        }

        SolidityIfElse CreateIfElseBlock(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows)
        {
            var ifElseBlock = new SolidityIfElse();
            foreach(var seqFlow in outgoingSeqFlows)
            {
                foreach(var nextElement in nextElements)
                {
                    if (seqFlow.TargetId == nextElement.GetElementId())
                        ifElseBlock.AddConditionBlock(seqFlow.Condition, nextElement.GetStatementForPrevious());
                }
            }
            return ifElseBlock;
        }

        public override string GetElementId()
        {
            return gateway.Id;
        }

        public override SolidityStatement GetStatementForPrevious()
        {
            return new SolidityStatement(gateway.Id + "Logic" + "()");
        }
    }
}
