using System.Collections.Generic;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters.Events
{
    public class EndEventConverter : ElementConverter
    {
        EndEvent endEventElement;

        public EndEventConverter(EndEvent endEventElement, ProcessConverter converterService)
        {
            this.endEventElement = endEventElement;
            processConverter = converterService;
        }

        public override void ConvertElementLogic()
        {

        }


        public override string GetElementCallName()
        {
            return GetElementCallName(endEventElement);
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            return new List<SolidityComponent>();
        }

        public override string GetElementId()
        {
            return endEventElement.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            var statement = new SolidityStatement();
            statement.Add(GetChangeActiveStateStatement(true));
            //Call the return function of the parent call element, if it exists
            if (processConverter.ParentProcessConverter != null)
            {
                var callActivityReturnName = ConversionTemplates.CallActivityReturnFunctionName(processConverter.GetParentCallActivityCallName());
                statement.Add($"{callActivityReturnName}({processConverter.ParentProcessConverter.GetIdentifierNames()})");
            }
            return statement;
        }
    }
}
