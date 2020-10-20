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


        public UserTaskConverter(UserTask userTaskElement, ProcessConverter converterService)
        {
            this.userTaskElement = userTaskElement;
            processConverter = converterService;
        }


        public override void ConvertElementLogic()
        {
            if (IsAddressGuardRequired())
                addressGuard = CreateAddressGuard();

            stateGuard = CreateStateGuard();
            mainFunction = CreateElementMainFunction();
        }

        public override IList<SolidityComponent> GetGeneratedSolidityComponents()
        {
            var components = new List<SolidityComponent>();
            components.Add(stateGuard);
            if (addressGuard != null)
                components.Add(addressGuard);
            components.Add(mainFunction);
            return components;
        }

        SolidityModifier CreateStateGuard()
        {
            SolidityModifier stateGuard = new SolidityModifier(ConversionTemplates.StateGuardModifierName(GetElementCallName()));
            SolidityStatement requireStatement = ConversionTemplates.RequireActiveStateStatement(GetElementCallName());
            stateGuard.AddToBody(requireStatement);
            return stateGuard;
        }

        SolidityModifier CreateAddressGuard()
        {
            //Initialize the modifier using the generated name
            SolidityModifier addressGuard = new SolidityModifier(ConversionTemplates.AddressGuardModifierName(GetElementCallName()));
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
            function.AddModifier(ConversionTemplates.StateGuardModifierName(GetElementCallName()));

            if (IsAddressGuardRequired())
                function.AddModifier(ConversionTemplates.AddressGuardModifierName(GetElementCallName()));

            boundaryEventCalls.ForEach(c => function.AddToBody(c));

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
                    var propertySetter = new SolidityStatement($"{Helpers.ToUpperCamelCase(entity.Name)}.{Helpers.ToLowerCamelCase(property.Name)} " +
                        $"= {formPropertyDisplayName}");
                    function.AddToBody(propertySetter);
                }
            }

            //Sets the state flag for this activity to false
            function.AddToBody(ConversionTemplates.ChangeActiveStateStatement(GetElementCallName(), false));
            //Get call of next element
            function.AddToBody(processConverter.GetStatementOfNextElement(userTaskElement));
            return function;
        }

        bool IsAddressGuardRequired()
        {
            return userTaskElement.Assignee != null
                && (userTaskElement.Assignee.Address != null || userTaskElement.Assignee.Name != null);
        }


        public override SolidityStatement GetStatementForPrevious(ProcessElement previous)
        {
            return ConversionTemplates.ChangeActiveStateStatement(GetElementCallName(), true);
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
