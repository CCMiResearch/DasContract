using System;
using System.Collections.Generic;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters
{
    class StartEventConverter : ElementConverter
    {

        StartEvent startEvent;

        public StartEventConverter(StartEvent startEvent)
        {
            this.startEvent = startEvent;
        }
        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows, IList<SolidityStruct> dataModel = null)
        {
            SolidityConstructor constructor = new SolidityConstructor();
            constructor.AddToBody(nextElements[0].GetStatementForPrevious(startEvent));
            return new List<SolidityComponent> { constructor };
        }

        public override string GetElementId()
        {
            return startEvent.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            throw new NotImplementedException();
        }
    }
}
