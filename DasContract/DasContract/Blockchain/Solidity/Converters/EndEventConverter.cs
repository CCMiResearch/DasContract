using System;
using System.Collections.Generic;
using System.Text;
using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;

namespace BpmnToSolidity.Solidity.ConversionHelpers
{
    class EndEventConverter : ElementConverter
    {
        EndEvent endEvent;
        public EndEventConverter(EndEvent endEvent)
        {
            this.endEvent = endEvent;
        }

        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows)
        {
            return new List<SolidityComponent>();
        }

        public override string GetElementId()
        {
            return endEvent.Id;
        }

        public override SolidityStatement GetStatementForPrevious()
        {
            return new SolidityStatement(ProcessConverter.STATE_NAME + "=\"" + GetEventName() + "\"");
        }

        string GetEventName()
        {
            if (endEvent.Name != null)
                return endEvent.Name;
            return endEvent.Id;
        }


    }
}
