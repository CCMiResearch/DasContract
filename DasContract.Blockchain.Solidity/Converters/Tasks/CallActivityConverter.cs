using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters.Tasks
{
    public class CallActivityConverter : TaskConverter
    {
        CallActivity callActivity;

        SolidityFunction mainFunction;
        SolidityFunction processReturnFunction;
        SolidityStatement stateTracker;

        public CallActivityConverter(CallActivity callActivity, ProcessConverter converterService)
        {
            this.callActivity = callActivity;
            processConverter = converterService;
        }

        SolidityStatement CreateNextElementStatement()
        {
            var statement = new SolidityStatement();

            statement.Add(GetChangeActiveStateStatement(false));
            statement.Add(processConverter.GetStatementOfNextElement(callActivity));
            return statement;
        }

        SolidityFunction CreateProcessReturnFunction()
        {
            var function = new SolidityFunction(ConversionTemplates.CallActivityReturnFunctionName(GetElementCallName()), SolidityVisibility.Internal);
            function.AddParameters(processConverter.GetIdentifiersAsParameters());

            var nextElementStatement = CreateNextElementStatement();
            var incrementStatement = new SolidityStatement($"{ConversionTemplates.CallActivityCounter(GetElementCallName())}++");
            var checkConditionBlock = new SolidityIfElse();
            var calledStartEventConverter = processConverter.ContractConverter.GetStartEventConverter(callActivity);
            var callSubprocessStatement = calledStartEventConverter.GetStatementForPrevious(callActivity);

            checkConditionBlock.AddConditionBlock($"{ConversionTemplates.CallActivityCounter(GetElementCallName())} >= {GetCountTarget(callActivity)}",
                        nextElementStatement);

            switch (callActivity.InstanceType)
            {
                case InstanceType.Single:
                    function.AddToBody(nextElementStatement);
                    break;
                case InstanceType.Sequential:
                    //increment the counter, check if counter reached limit.
                    //If counter reached limit, then call the next element. If not, call the subprocess again.
                    function.AddToBody(incrementStatement);
                    
                    checkConditionBlock.AddConditionBlock("", callSubprocessStatement);
                    function.AddToBody(checkConditionBlock);

                    break;
                case InstanceType.Parallel:
                    //increment the counter, check if counter reached limit.
                    //If counter reached limit, then call the next element. If not, do nothing.
                    function.AddToBody(incrementStatement);
                    function.AddToBody(checkConditionBlock);
                    break;
            }
            return function;
        }

        public override void ConvertElementLogic()
        {
            processReturnFunction = CreateProcessReturnFunction();
            mainFunction = new SolidityFunction(GetElementCallName(), SolidityVisibility.Internal);
            mainFunction.AddParameters(processConverter.GetIdentifiersAsParameters());
            var calledStartEventConverter = processConverter.ContractConverter.GetStartEventConverter(callActivity);
            var callSubprocessStatement = calledStartEventConverter.GetStatementForPrevious(callActivity);
            stateTracker = new SolidityStatement($"uint256 {ConversionTemplates.CallActivityCounter(GetElementCallName())}");
            switch (callActivity.InstanceType)
            {
                case InstanceType.Single:
                    //Call the subprocess.
                    mainFunction.AddToBody(callSubprocessStatement);
                    break;
                case InstanceType.Sequential:
                    //Call the subprocess and create a counter.
                    mainFunction.AddToBody(new SolidityStatement($"{ConversionTemplates.CallActivityCounter(GetElementCallName())} = 0"));
                    mainFunction.AddToBody(callSubprocessStatement);
                    break;
                case InstanceType.Parallel:
                    //Call all of the subprocesses using an identifier, create a counter.
                    mainFunction.AddToBody(new SolidityStatement($"{ConversionTemplates.CallActivityCounter(GetElementCallName())} = 0"));
                    var solidityForLoop = new SolidityFor(GetLoopVariable(), GetCountTarget(callActivity));
                    solidityForLoop.AddToBody(callSubprocessStatement);
                    mainFunction.AddToBody(solidityForLoop);
                    break;
            }
        }

        string GetLoopVariable()
        {
            if (callActivity.LoopCollection != null || callActivity.LoopCardinality != "0")
            {
                return ConversionTemplates.IdentifierVariableName(callActivity.Id);
            }
            return null; //TODO exception
        }

        public override string GetElementCallName()
        {
            return GetElementCallName(callActivity);
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var components = new List<SolidityComponent>
            {
                mainFunction,
                processReturnFunction
            };

            if(callActivity.InstanceType != InstanceType.Single)
            {
                components.Add(stateTracker);
            }
            return components;
        }

        public override string GetElementId()
        {
            return callActivity.Id;
        }

        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            var statement = new SolidityStatement();
            statement.Add(GetChangeActiveStateStatement(true));
            statement.Add(GetFunctionCallStatement());
            return statement;
        }
    }
}
