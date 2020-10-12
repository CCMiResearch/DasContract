using System.Collections.Generic;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using System.Linq;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters
{
    class UserTaskConverter : ElementConverter
    {
        UserTask userTask;
        public UserTaskConverter(UserTask userTask)
        {
            this.userTask = userTask;
        }
        public override IList<SolidityComponent> GetElementCode(List<ElementConverter> nextElements, IList<SequenceFlow> outgoingSeqFlows, IList<SolidityStruct> dataModel = null)
        {
            List<SolidityComponent> components = new List<SolidityComponent>();

            if (userTask.Assignee != null && (userTask.Assignee.Address != null || userTask.Assignee.Name != null))
                   components.Add(CreateAddressGuard());

            components.Add(CreateStateGuard());
            components.Add(CreateElementMainFunction(nextElements[0], (List<SolidityStruct>) dataModel));
            return components;
        }

        SolidityModifier CreateStateGuard()
        {
            SolidityModifier stateGuard = new SolidityModifier("is" + GetTaskName() + "State");
            SolidityStatement requireStatement = new SolidityStatement(
                "require(" + ConverterConfig.IS_STATE_ACTIVE_FUNCTION_NAME + "(\"" + GetTaskName()+ "\")==true)");

            stateGuard.AddToBody(requireStatement);
            return stateGuard;
        }

        SolidityModifier CreateAddressGuard()
        {
            SolidityModifier addressGuard = new SolidityModifier("is" + GetTaskName() + "Authorized");
            if (userTask.Assignee.Address != null)
            {
                SolidityStatement requireStatement = new SolidityStatement("require(msg.sender==" + GetAssigneeAddress() + ")");
                addressGuard.AddToBody(requireStatement);
            }
            else if (userTask.Assignee.Name != null)
            {
                var addressPosition = ConverterConfig.ADDRESS_MAPPING_VAR_NAME + "[\"" + userTask.Assignee.Name + "\"]";
                var ifElseBlock = new SolidityIfElse();
                ifElseBlock.AddConditionBlock(addressPosition + " == address(0x0)", new SolidityStatement(addressPosition + " = msg.sender"));
                SolidityStatement requireStatement = new SolidityStatement("require(msg.sender==" + addressPosition + ")");
                addressGuard.AddToBody(ifElseBlock);
                addressGuard.AddToBody(requireStatement);
            }
            return addressGuard;
        }

        SolidityFunction CreateElementMainFunction(ElementConverter nextElement, List<SolidityStruct> dataModel)
        {
            SolidityFunction function = new SolidityFunction(GetTaskName(), SolidityVisibility.Public);
            function.AddModifier("is" + GetTaskName() + "State");

            if (userTask.Assignee != null && (userTask.Assignee.Address != null || userTask.Assignee.Name != null))
                function.AddModifier("is" + GetTaskName() + "Authorized");

            SolidityStatement statement = new SolidityStatement(ConverterConfig.ACTIVE_STATES_NAME + "[\"" + GetTaskName() + "\"] = false");

            function.AddToBody(statement);

            if (userTask.Form.Fields != null)
            {
                foreach (var field in userTask.Form.Fields)
                {
                    foreach (var s in dataModel)
                    {
                        Property p = s.En.Properties.FirstOrDefault(e => e.Id == field.PropertyExpression);
                        if (p != null)
                        {
                            function.AddParameter(new SolidityParameter(Helpers.GetSolidityStringType(p), field.DisplayName));
                            function.AddToBody(new SolidityStatement(s.VariableName() + "." + Helpers.GetPropertyVariableName(p.Name) + " = " + field.DisplayName));
                        }
                    }
                }
            }

            function.AddToBody(nextElement.GetStatementForPrevious(userTask));
            return function;
        }


        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            return new SolidityStatement("ActiveStates[\"" + GetTaskName() + "\"] = true");
        }

        string GetTaskName()
        {
            return userTask.Name;
        }

        string GetAssigneeAddress()
        {
            if (userTask.Assignee != null)
                return userTask.Assignee.Address ?? "msg.sender";
            return "msg.sender";
        }

        public override string GetElementId()
        {
            return userTask.Id;
        }
    }
}
