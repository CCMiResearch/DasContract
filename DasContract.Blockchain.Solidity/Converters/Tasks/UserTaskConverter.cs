using System.Collections.Generic;
using DasContract.Abstraction.Data;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using System.Linq;
using DasContract.Blockchain.Solidity.SolidityComponents;

namespace DasContract.Blockchain.Solidity.Converters.Tasks
{
    public class UserTaskConverter : TaskConverter
    {
        UserTask userTaskElement;

        SolidityFunction mainFunction;
        SolidityModifier stateGuard;
        SolidityModifier addressGuard;

        SolidityStatement multiInstanceCounter;

        bool counterVariablePresent = false;

        public UserTaskConverter(UserTask userTaskElement, ProcessConverter converterService)
        {
            this.userTaskElement = userTaskElement;
            processConverter = converterService;
        }

        public override void ConvertElementLogic()
        {
            /*
            if (IsAddressGuardRequired())
                addressGuard = CreateAddressGuard();
            */
            stateGuard = CreateStateGuard();
            mainFunction = CreateElementMainFunction();

            if (userTaskElement.InstanceType != InstanceType.Single)
                multiInstanceCounter = CreateMultiInstanceCounterDefinition();
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var components = new List<SolidityComponent>();
            components.Add(stateGuard);
            if (addressGuard != null)
                components.Add(addressGuard);
            if (multiInstanceCounter != null)
                components.Add(multiInstanceCounter);
            components.Add(mainFunction);
            return components;
        }

        SolidityModifier CreateStateGuard()
        {
            SolidityModifier stateGuard = new SolidityModifier(ConversionTemplates.StateGuardModifierName(GetElementCallName()));
            stateGuard.AddParameters(processConverter.GetIdentifiersAsParameters());
            var activeStateFunctionName = ConversionTemplates.ActiveStatesFunctionName(processConverter.Id);
            var callParameters = processConverter.GetIdentifierNames();
            if (callParameters.Length > 0)
                callParameters += ",";
            callParameters += $"\"{GetElementCallName()}\"";
            SolidityStatement requireStatement = new SolidityStatement($"require({activeStateFunctionName}({callParameters}) == true)");
            stateGuard.AddToBody(requireStatement);
            return stateGuard;
        }

        SolidityModifier CreateAddressGuard()
        {
            //Initialize the modifier using the generated name
            SolidityModifier addressGuard = new SolidityModifier(ConversionTemplates.AddressGuardModifierName(GetElementCallName()));
            addressGuard.AddParameters(processConverter.GetIdentifiersAsParameters());
            //Address is force assigned
            if (userTaskElement.Assignee.Address != null)
            {
                SolidityStatement requireStatement = new SolidityStatement($"require(msg.sender=={GetAssigneeAddress()})");
                addressGuard.AddToBody(requireStatement);
            }
            //Address is not force assigned, it will be assigned to the first caller of the first task method containing this assignee
            else if (userTaskElement.Assignee.Name != null)
            {
                var addressPosition = $"{ConverterConfig.ADDRESS_MAPPING_VAR_NAME}[\"{userTaskElement.Assignee.Name}\"]";

                var ifElseBlock = new SolidityIfElse();
                //Assigns the address, if address has not been been yet assigned to the assignee
                ifElseBlock.AddConditionBlock($"{addressPosition} == address(0x0)", new SolidityStatement($"{addressPosition} = msg.sender"));
                //Checks whether the sender has the required address
                SolidityStatement requireStatement = new SolidityStatement($"require(msg.sender=={addressPosition})");
                addressGuard.AddToBody(ifElseBlock);
                addressGuard.AddToBody(requireStatement);
            }
            return addressGuard;
        }

        SolidityFunction CreateElementMainFunction()
        {
            SolidityFunction function = new SolidityFunction(GetElementCallName(), SolidityVisibility.Public);
            function.AddModifier($"{ConversionTemplates.StateGuardModifierName(GetElementCallName())}({processConverter.GetIdentifierNames()})");
            function.AddParameters(processConverter.GetIdentifiersAsParameters());

            /*
            if (IsAddressGuardRequired())
                function.AddModifier($"{ConversionTemplates.AddressGuardModifierName(GetElementCallName())}({processConverter.GetIdentifierNames()})");
            */

            boundaryEventCalls.ForEach(c => function.AddToBody(c));

            function.AddToBody(new SolidityStatement(userTaskElement.ValidationScript, false));

            /* TODO reimplement for new forms definitions
            if (userTaskElement.Form.Fields != null)
            {
                foreach (var field in userTaskElement.Form.Fields)
                {
                    //TODO: throw exception if no property has been found
                    var propertyAndEntity = processConverter.GetPropertyAndEntity(field.PropertyExpression);
                    var property = propertyAndEntity.Item1;
                    var entity = propertyAndEntity.Item2;
                    var formPropertyDisplayName = Helpers.ToLowerCamelCase(field.DisplayName);


                    function.AddParameter(new SolidityParameter(Helpers.PropertyTypeToString(property, processConverter.ContractConverter), formPropertyDisplayName));
                }
            }
            */

            function.AddToBody(CreateCallNextBody());
            return function;
        }

        List<SolidityComponent> CreateCallNextBody()
        {
            var components = new List<SolidityComponent>();
            var callNextStatement = GetChangeActiveStateStatement(false);
            callNextStatement.Add(processConverter.GetStatementOfNextElement(userTaskElement));

            if (userTaskElement.InstanceType != InstanceType.Single)
            {
                //The tasks will run until it gets interrupted by some boundary event
                if (userTaskElement.LoopCardinality == -1 || (userTaskElement.LoopCardinality == 0 && userTaskElement.LoopCollection == null))
                {
                    return components;
                }

                var ifStatement = new SolidityIfElse();
                var counterVariableName = ConversionTemplates.MultiInstanceCounterVariable(GetElementCallName());
                
                components.Add(new SolidityStatement($"{counterVariableName}++"));
                counterVariablePresent = true;
                if (userTaskElement.LoopCardinality > 0)
                {
                    ifStatement.AddConditionBlock($"{counterVariableName} >= {userTaskElement.LoopCardinality}", callNextStatement);
                }
                else if(userTaskElement.LoopCollection != null)
                {
                    ifStatement.AddConditionBlock($"{counterVariableName} >= {GetCountTarget(userTaskElement)}", callNextStatement);
                }
                components.Add(ifStatement);
            }
            else
            {
                components.Add(callNextStatement);
            }
 
            return components;
        }

        SolidityStatement CreateMultiInstanceCounterDefinition()
        {
            if (userTaskElement.LoopCardinality != 0 || userTaskElement.LoopCollection != null)
            {
                var variableName = ConversionTemplates.MultiInstanceCounterVariable(GetElementCallName());
                return new SolidityStatement($"uint256 {variableName}");
            }
            return null;
        }

        bool IsAddressGuardRequired()
        {
            return userTaskElement.Assignee != null
                && (userTaskElement.Assignee.Address != null || userTaskElement.Assignee.Name != null);
        }


        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            var statement = new SolidityStatement();
            if (counterVariablePresent)
                statement.Add(new SolidityStatement($"{ConversionTemplates.MultiInstanceCounterVariable(GetElementCallName())} = 0"));
            statement.Add(GetChangeActiveStateStatement(true));
            return statement;
        }

        string GetAssigneeAddress()
        {
            if (userTaskElement.Assignee != null && userTaskElement.Assignee.Address != null)
                return userTaskElement.Assignee.Address;
            return "msg.sender";
        }

        public override string GetElementId()
        {
            return userTaskElement.Id;
        }

        public override string GetElementCallName()
        {
            return GetElementCallName(userTaskElement);
        }
    }
}
