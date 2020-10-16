using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters
{
    public class CallActivityConverter : ElementConverter
    {
        CallActivity callActivity;

        public CallActivityConverter(CallActivity callActivity)
        {
            this.callActivity = callActivity;
        }

        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows, IList<SolidityStruct> dataModel = null)
        {
            //throw new NotImplementedException();
            return new List<SolidityComponent>();
        }

        public override string GetElementId()
        {
            return callActivity.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            return new SolidityStatement("ActiveStates[\"" + callActivity.Id + "\"] = true");
        }
    }
}
