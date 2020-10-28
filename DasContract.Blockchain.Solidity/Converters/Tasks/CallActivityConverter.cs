using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters.Tasks
{
    public class CallActivityConverter : TaskConverter
    {
        CallActivity callActivity;

        SolidityFunction mainFunction;
        SolidityStatement stateTracker;

        public CallActivityConverter(CallActivity callActivity, ProcessConverter converterService)
        {
            this.callActivity = callActivity;
            processConverter = converterService;
        }

        SolidityFunction CreateProcessReturnFunction()
        {
            var function = new SolidityFunction(ConversionTemplates.CallActivityReturnFunctionName(callActivity.Id), SolidityVisibility.Internal);
            var nextElementStatement = processConverter.GetStatementOfNextElement(callActivity);
            switch (callActivity.InstanceType)
            {
                case InstanceType.Single:
                    function.AddToBody(nextElementStatement);
                    break;
                case InstanceType.Sequential:
                    //Decrement the counter, check if counter 0.
                    //If counter reached 0, then call the next element. If not, call the subprocess again.
                    break;
                case InstanceType.Parallel:
                    //Decrement the counter, check if counter 0.
                    //If counter reached 0, then call the next element. If not, do nothing.
                    break;
            }
            return function;
        }

        public override void ConvertElementLogic()
        {
            mainFunction = new SolidityFunction(GetElementCallName(), SolidityVisibility.Internal);
            var calledStartEventConverter = processConverter.ContractConverter.GetStartEventConverter(callActivity);
            var callElementStatement = calledStartEventConverter.GetStatementForPrevious(callActivity);
            stateTracker = new SolidityStatement($"uint {GetElementCallName()}Counter");
            switch (callActivity.InstanceType)
            {
                case InstanceType.Single:
                    //Call the subprocess.
                    mainFunction.AddToBody(callElementStatement);
                    break;
                case InstanceType.Sequential:
                    //Call the subprocess and create a counter.
                    mainFunction.AddToBody(new SolidityStatement($"{GetElementCallName()}Counter = "));
                    break;
                case InstanceType.Parallel:
                    //Call all of the subprocesses using an identifier, create a counter.
                    
                    break;
            }
            //TODO
        }

        public override string GetElementCallName()
        {
            return GetElementCallName(callActivity);
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
