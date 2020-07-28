using System;
using System.Collections.Generic;
using System.Text;
using DasToSolidity.SolidityConverter;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;

namespace DasToSolidity.Solidity.ConversionHelpers
{
    class EndEventConverter : ElementConverter
    {
        EndEvent endEvent;
        public EndEventConverter(EndEvent endEvent)
        {
            this.endEvent = endEvent;
        }

        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows, IList<SolidityStruct> dataModel = null)
        {
            return new List<SolidityComponent>();
        }

        public override string GetElementId()
        {
            return endEvent.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            return new SolidityStatement(ProcessConverter.ACTIVE_STATES_NAME + "[\"" + GetEventName() + "\"] = true");
        }

        string GetEventName()
        {
            if (endEvent.Name != null)
                return endEvent.Name;
            return endEvent.Id;
        }


    }
}
