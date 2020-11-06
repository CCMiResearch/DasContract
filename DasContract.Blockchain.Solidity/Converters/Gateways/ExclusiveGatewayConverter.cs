using System.Collections.Generic;
using System.Linq;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters.Gateways
{
    public class ExclusiveGatewayConverter : ElementConverter
    {
        ExclusiveGateway gatewayElement;

        SolidityFunction logicFunction;

        public ExclusiveGatewayConverter(ExclusiveGateway gatewayElement, ProcessConverter converterService)
        {
            this.gatewayElement = gatewayElement;
            processConverter = converterService;
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            return new List<SolidityComponent>
            {
                logicFunction
            };
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            return GetFunctionCallStatement();
        }

        public override void ConvertElementLogic()
        {
            logicFunction = CreateLogicFunction();
        }

        SolidityFunction CreateLogicFunction()
        {
            var logicFunction = new SolidityFunction($"{GetElementCallName()}", SolidityVisibility.Internal);
            logicFunction.AddParameters(processConverter.GetIdentifiersAsParameters());
            var outgoingSequenceFlows = processConverter.GetOutgoingSequenceFlows(gatewayElement);
            if (outgoingSequenceFlows.Count == 1)
            {
                ElementConverter nextConverter = processConverter.GetConverterOfElement(outgoingSequenceFlows.First().TargetId);
                logicFunction.AddToBody(nextConverter.GetStatementForPrevious(gatewayElement));
            }
            else
                logicFunction.AddToBody(CreateIfElseBlock(outgoingSequenceFlows));

            return logicFunction;
        }

        SolidityIfElse CreateIfElseBlock(IList<SequenceFlow> outgoingSequenceFlows)
        {
            var ifElseBlock = new SolidityIfElse();
            foreach (var sequenceFlow in outgoingSequenceFlows)
            {
                var targetConverter = processConverter.GetConverterOfElement(sequenceFlow.TargetId);
                ifElseBlock.AddConditionBlock(sequenceFlow.Condition, targetConverter.GetStatementForPrevious(gatewayElement));
            }
            return ifElseBlock;
        }

        public override string GetElementId()
        {
            return gatewayElement.Id;
        }

        public override string GetElementCallName()
        {
            return GetElementCallName(gatewayElement);
        }
    }
}
