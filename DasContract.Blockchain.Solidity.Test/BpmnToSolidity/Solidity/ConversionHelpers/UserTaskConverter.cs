using System;
using System.Collections.Generic;
using System.Text;
using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;

namespace BpmnToSolidity.Solidity.ConversionHelpers
{
    class UserTaskConverter : ElementConverter
    {
        UserTask userTask;
        public UserTaskConverter(UserTask userTask)
        {
            this.userTask = userTask;
        }
        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows)
        {
            //TODO: Check address
            List<SolidityComponent> components = new List<SolidityComponent>();
            components.Add(CreateStateGuard());
            components.Add(CreateElementMainFunction(nextElements[0]));
            components.Add(CreateFunctionForPrevious());
            return components;
        }

        SolidityModifier CreateStateGuard()
        {
            SolidityModifier stateGuard = new SolidityModifier("is" + GetTaskName() + "State");
            SolidityStatement requireStatement = new SolidityStatement(
                "require(keccak256(bytes(" + ProcessConverter.STATE_NAME + "))==keccak256(bytes(\"" + GetTaskName() + "\")))");

            stateGuard.AddToBody(requireStatement);
            return stateGuard;
        }

        SolidityFunction CreateElementMainFunction(ElementConverter nextElement)
        {
            SolidityFunction function = new SolidityFunction(GetTaskName(), SolidityVisibility.Public);
            //TODO: Add address guard modifier
            function.AddModifier("is" + GetTaskName() + "State");
            function.addToBody(nextElement.GetStatementForPrevious());
            return function;
        }

        SolidityFunction CreateFunctionForPrevious()
        {
            string enumName = GetTaskName();

            SolidityFunction solFunction = new SolidityFunction("setState" + enumName, SolidityVisibility.Internal);
            SolidityStatement statement = new SolidityStatement(ProcessConverter.STATE_NAME + "=\"" + enumName + "\"");
            solFunction.addToBody(statement);
            return solFunction;
        }



        public override SolidityStatement GetStatementForPrevious()
        {
            return new SolidityStatement("setState" + GetTaskName() + "()");
        }

        string GetTaskName()
        {
            if (userTask.Name != null)
                return userTask.Name;
            return userTask.Id;
        }

        public override string GetElementId()
        {
            return userTask.Id;
        }
    }
}
