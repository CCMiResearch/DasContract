using System;
using System.Collections.Generic;
using System.Text;
using DasToSolidity.SolidityConverter;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;

namespace DasToSolidity.Solidity.ConversionHelpers
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
