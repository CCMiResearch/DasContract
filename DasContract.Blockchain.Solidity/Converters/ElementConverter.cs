using DasContract.Abstraction.Processes;
using System.Collections.Generic;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters
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
