using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters.Events
{
    public class TimerBoundaryEventConverter : ElementConverter
    {
        TimerBoundaryEvent eventElement;

        public TimerBoundaryEventConverter(TimerBoundaryEvent eventElement, ProcessConverter processConverter)
        {
            this.eventElement = eventElement;
            this.processConverter = processConverter;
        }

        public override void ConvertElementLogic()
        {

        }

        /*
        SolidityFunction CreateTouchFunction()
        {

        }
        */

        public override string GetElementCallName()
        {
            return GetElementCallName(eventElement);
        }

        public override string GetElementId()
        {
            return eventElement.Id;
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            //TODO
            return new List<SolidityComponent>();
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            throw new NotImplementedException();
        }
    }
}
