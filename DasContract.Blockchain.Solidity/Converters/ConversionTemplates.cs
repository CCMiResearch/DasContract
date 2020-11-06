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

        public static string ProcessConverterId(string processId, string callActivityId)
        {
            if (callActivityId != null)
                return $"{processId}_{callActivityId}";
            else
                return processId;
        }

        public static string CallActivityCounter(string callActivityCallName)
        {
            return $"{callActivityCallName}Counter";
        }

        public static string ActiveStatesMappingName(string processConverterId)
        {
            return $"{processConverterId}{ConverterConfig.ACTIVE_STATES_NAME}";
        }

        public static string CallActivityReturnFunctionName(string callActivityCallname)
        {
            return $"{callActivityCallname}ReturnLogic";
        }

        public static SolidityStatement ChangeActiveStateStatement(string processConverterId, string elementCallName, bool isActive)
        {
            return new SolidityStatement($"{ActiveStatesMappingName(processConverterId)}[\"{elementCallName}\"] = {isActive}");
        }

        public static SolidityStatement RequireActiveStateStatement(string elementCallName)
        {
            return new SolidityStatement($"require({ConverterConfig.IS_STATE_ACTIVE_FUNCTION_NAME}(\"{elementCallName}\")==true");
        }

        public static SolidityStatement MappingTypeVariableDefinition(string propertyName, string keyType, string valueType)
        {
            return new SolidityStatement($"mapping ({keyType} => {valueType}) {propertyName}");
        }
    }
}
