using System;
using System.Collections.Generic;
using System.Text;
using DasToSolidity.SolidityConverter;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Gateways;

namespace DasToSolidity.Solidity.ConversionHelpers
{
    class ExclusiveGatewayConverter : ElementConverter
    {
        ExclusiveGateway gateway;

        public ExclusiveGatewayConverter(ExclusiveGateway gateway)
        {
            this.gateway = gateway;
        }

        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows, IList<SolidityStruct> dataModel = null)
        {
            var logicFunction = new SolidityFunction(gateway.Id + "Logic", SolidityVisibility.Internal);
            if (nextElements.Count == 1)
                logicFunction.AddToBody(nextElements[0].GetStatementForPrevious(gateway));
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
                        ifElseBlock.AddConditionBlock(seqFlow.Condition, nextElement.GetStatementForPrevious(gateway));
                }
            }
            return ifElseBlock;
        }

        public override string GetElementId()
        {
            return gateway.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            return new SolidityStatement(gateway.Id + "Logic" + "()");
        }
    }
}
