using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Blockchain.Solidity.Converters.Gateways
{
    public class ParallelGatewayConverter : ElementConverter
    {
        ParallelGateway gatewayElement;
        string incrementVariableName;

        SolidityFunction mainFunction;
        SolidityStatement incomingFlowsVariable;

        public ParallelGatewayConverter(ParallelGateway gatewayElement, ProcessConverter converterService)
        {
            this.gatewayElement = gatewayElement;
            processConverter = converterService;
            incrementVariableName = $"{GetElementCallName()}Incoming";
        }

        public override void ConvertElementLogic()
        {
            mainFunction = CreateMainFunction();
            incomingFlowsVariable = CreateIncomingFlowsVariable();
        }

        SolidityFunction CreateMainFunction()
        {
            var logicFunction = new SolidityFunction($"{GetElementCallName()}Logic", SolidityVisibility.Internal);
            var body = CreateCallsToOutgoing();

            if (gatewayElement.Incoming.Count == 1)
            {
                logicFunction.AddToBody(body);
            }
            else
            {
                //Increment the incoming variable
                logicFunction.AddToBody(new SolidityStatement($"{incrementVariableName} += 1"));

                var ifElseBlock = new SolidityIfElse();
                string ifElseCondition = $"{incrementVariableName}=={gatewayElement.Incoming.Count}";
                //reset the incoming flow count
                body.Add(incrementVariableName + " = 0");
                ifElseBlock.AddConditionBlock(ifElseCondition, body);
                logicFunction.AddToBody(ifElseBlock);
            }

            return logicFunction;
        }

        SolidityStatement CreateIncomingFlowsVariable()
        {
            if (gatewayElement.Incoming.Count > 1)
                return new SolidityStatement($"int {incrementVariableName} = 0");
            return null;
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var elementComponents = new List<SolidityComponent>
            {
                mainFunction,
            };
            if (incomingFlowsVariable != null)
                elementComponents.Add(incomingFlowsVariable);

            return elementComponents;
        }

        SolidityStatement CreateCallsToOutgoing()
        {
            var nextElementConverters = processConverter.GetTargetConvertersOfElement(gatewayElement).ToList();
            var statement = new SolidityStatement();
            nextElementConverters.ForEach(e =>
            {
                e.GetStatementForPrevious(gatewayElement).GetStatements().
                           ForEach(f => statement.Add(f));
            });
            return statement;
        }

        public override string GetElementId()
        {
            return gatewayElement.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            return new SolidityStatement($"{GetElementCallName()}Logic()");
        }

        public override string GetElementCallName()
        {
            return GetElementCallName(gatewayElement);
        }
    }
}
