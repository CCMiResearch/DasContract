using DasToSolidity.SolidityConverter;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasToSolidity.Solidity.ConversionHelpers
{

    public abstract class ElementConverter
    {
        /// <summary>
        /// Returns generated logic for the given element
        /// </summary>
        /// <param name="nextElements"></param>
        /// <param name="outgoingSeqFlows"></param>
        /// <returns></returns>
        public abstract IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows, IList<SolidityStruct> dataModel = null);
        /// <summary>
        /// Returns a statement that allows the previous element to call the main logic function
        /// </summary>
        /// <returns></returns>
        public abstract SolidityStatement GetStatementForPrevious(ProcessElement previous);

        public abstract string GetElementId();
    }
}
