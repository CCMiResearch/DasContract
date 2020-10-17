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

        public CallActivityConverter(CallActivity callActivity, ProcessConverter converterService)
        {
            this.callActivity = callActivity;
            this.processConverter = converterService;
        }


        public override void ConvertElementLogic()
        {
            throw new NotImplementedException();
        }

        public override string GetElementCallName()
        {
            throw new NotImplementedException();
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
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
