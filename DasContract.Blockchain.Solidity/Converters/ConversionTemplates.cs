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

        public static string MultiInstanceCollectionVariable(string elementCallName)
        {
            return $"{Helpers.ToLowerCamelCase(elementCallName)}LoopCollection";
        }

        public static string MultiInstanceCountVariable(string elementCallName)
        {
            return $"{Helpers.ToLowerCamelCase(elementCallName)}LoopCount";
        }

        public static SolidityStatement ChangeActiveStateStatement(string elementCallName, bool isActive)
        {
            return new SolidityStatement($"{ConverterConfig.ACTIVE_STATES_NAME}[\"{elementCallName}\"] = {isActive}");
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
