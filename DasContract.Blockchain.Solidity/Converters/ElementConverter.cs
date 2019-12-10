using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction.Processes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BpmnToSolidity.Solidity.ConversionHelpers
{

    public abstract class ElementConverter
    {
        /// <summary>
        /// Returns generated logic for the given element
        /// </summary>
        /// <param name="nextElements"></param>
        /// <param name="outgoingSeqFlows"></param>
        /// <returns></returns>
        public abstract IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows);
        /// <summary>
        /// Returns a statement that allows the previous element to call the main logic function
        /// </summary>
        /// <returns></returns>
        public abstract SolidityStatement GetStatementForPrevious();

        public abstract string GetElementId();
    }
}
