using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction.Processes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.Solidity.ConversionHelpers
{
    public abstract class ElementConverter
    {
        public abstract IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows);
        public abstract SolidityStatement GetStatementForPrevious();

        public abstract string GetElementId();
    }
}
