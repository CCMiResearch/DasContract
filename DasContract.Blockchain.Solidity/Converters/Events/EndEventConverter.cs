using System.Collections.Generic;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters.Events
{
    class EndEventConverter : ElementConverter
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
            //Set the event state
            return ConversionTemplates.ChangeActiveStateStatement(GetElementCallName(), true);
        }
    }
}
