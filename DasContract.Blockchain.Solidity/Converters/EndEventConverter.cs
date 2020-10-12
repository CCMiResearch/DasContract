using System.Collections.Generic;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters
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
            return new SolidityStatement(ConverterConfig.ACTIVE_STATES_NAME + "[\"" + GetEventName() + "\"] = true");
        }

        string GetEventName()
        {
            if (endEvent.Name != null)
                return endEvent.Name;
            return endEvent.Id;
        }


    }
}
