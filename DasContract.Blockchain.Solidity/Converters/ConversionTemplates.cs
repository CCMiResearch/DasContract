using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters
{
    public static class ConversionTemplates
    {
        public static string StateGuardModifierName(string elementCallName)
        {
            return $"is{elementCallName}State";
        }

        public static string AddressGuardModifierName(string elementCallName)
        {
            return $"is{elementCallName}Authorized";
        }

        public static string MultiInstanceCounterVariable(string elementCallName)
        {
            return $"{Helpers.ToLowerCamelCase(elementCallName)}Counter";
        }

        public static string IdentifierVariableName(string identifierPropertyName)
        {
            return $"{Helpers.ToLowerCamelCase(identifierPropertyName)}Identifier";
        }

        public static string MappingKeysArrayName(string mappingName)
        {
            return $"{mappingName}Keys";
        }

        public static string ProcessConverterId(string processId, string callActivityId)
        {
            if (callActivityId != null)
                return $"{processId}_{callActivityId}";
            else
                return processId;
        }

        public static string CallActivityCounter(string callActivityCallName)
        {
            return $"{callActivityCallName.ToLowerCamelCase()}Counter";
        }

        public static string ActiveStatesMappingName(string processConverterId)
        {
            return $"{processConverterId}{ConverterConfig.ACTIVE_STATES_NAME}";
        }

        public static string ActiveStatesFunctionName(string processConverterId)
        {
            return $"{ConverterConfig.IS_STATE_ACTIVE_FUNCTION_NAME}{processConverterId}";
        }

        public static string CallActivityReturnFunctionName(string callActivityCallname)
        {
            return $"{callActivityCallname}ReturnLogic";
        }

        public static string ActiveStateAssignment(string changedStateName, string converterId, List<ProcessInstanceIdentifier> identifiers, bool isChangedStateParameter = false)
        {
            var activeState = new StringBuilder();
            activeState.Append(ActiveStatesMappingName(converterId));
            foreach (var identifier in identifiers)
            {
                activeState.Append($"[{identifier.IdentifierName}]");
            }
            if(isChangedStateParameter)
                activeState.Append($"[{changedStateName}]");
            else
                activeState.Append($"[\"{changedStateName}\"]");
            return activeState.ToString();
        }


        public static SolidityStatement MappingTypeVariableDefinition(string propertyName, string keyType, string valueType)
        {
            return new SolidityStatement($"mapping ({keyType} => {valueType}) {propertyName}");
        }
    }
}
