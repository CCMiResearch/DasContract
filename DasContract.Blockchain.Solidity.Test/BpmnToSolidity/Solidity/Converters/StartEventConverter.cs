using System;
using System.Collections.Generic;
using System.Text;
using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Events;

namespace BpmnToSolidity.Solidity.ConversionHelpers
{
    class StartEventConverter : ElementConverter
    {

        StartEvent startEvent;

        public StartEventConverter(StartEvent startEvent)
        {
            this.startEvent = startEvent;
        }
        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows)
        {
            SolidityConstructor constructor = new SolidityConstructor();
            constructor.AddToBody(nextElements[0].GetStatementForPrevious());
            return new List<SolidityComponent> { constructor };
        }

        public override string GetElementId()
        {
            throw new NotImplementedException();
        }

        public override SolidityStatement GetStatementForPrevious()
        {
            throw new NotImplementedException();
        }
    }
}
