using System;
using System.Collections.Generic;
using System.Text;
using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction.Processes.Tasks;

namespace BpmnToSolidity.Solidity.ConversionHelpers
{
    class UserTaskConverter : ElementConverter
    {
        UserTask userTask;
        UserTaskConverter(UserTask userTask)
        {
            this.userTask = userTask;
        }
        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements)
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
            SolidityModifier stateGuard = new SolidityModifier("is" + GetTaskName());
            SolidityStatement requireStatement = new SolidityStatement(
                "require(" + ProcessConverter.STATE_NAME + "==" + GetTaskName() + ")");

            stateGuard.AddToBody(requireStatement);
            return stateGuard;
        }

        SolidityFunction CreateElementMainFunction(ElementConverter nextElement)
        {
            SolidityFunction function = new SolidityFunction(GetTaskName(), SolidityVisibility.Public);
            //TODO: Add modifier
            function.addToBody(nextElement.GetStatementForPrevious());
            return function;
        }

        SolidityFunction CreateFunctionForPrevious()
        {
            string enumName = GetTaskName();

            SolidityFunction solFunction = new SolidityFunction("setTaskEnum" + enumName, SolidityVisibility.Private);
            SolidityStatement statement = new SolidityStatement(ProcessConverter.STATE_NAME + "=" + enumName);
            solFunction.addToBody(statement);
            return solFunction;
        }



        public override SolidityStatement GetStatementForPrevious()
        {
            return new SolidityStatement(GetTaskName() + "()");
        }

        string GetTaskName()
        {
            if (userTask.Name != null)
                return userTask.Name;
            return userTask.Id;
        }


    }
}
