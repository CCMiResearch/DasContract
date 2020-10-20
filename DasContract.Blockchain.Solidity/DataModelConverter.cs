using DasContract.Abstraction.Data;
using DasContract.Blockchain.Solidity.Converters;
using DasContract.Blockchain.Solidity.SolidityComponents;
using Liquid.NET.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DasContract.Blockchain.Solidity
{
    public class DataModelConverter
    {
        ContractConverter contractConverter;

        List<SolidityStruct> structs = new List<SolidityStruct>();
        List<SolidityEnum> enums = new List<SolidityEnum>();
        List<SolidityContract> tokens = new List<SolidityContract>();

        List<string> dependencies = new List<string>();


        public DataModelConverter(ContractConverter contractConverter)
        {
            this.contractConverter = contractConverter;
        }

        public void ConvertLogic()
        {
            ConvertEntities();
            ConvertEnums();
            ConvertTokens();
        }

        public SolidityStatement GetDependencies()
        {
            if (dependencies.Count == 0)
                return null;
            return new SolidityStatement(dependencies);
        }

        public List<SolidityContract> GetTokenContracts()
        {
            return tokens;
        }

        public List<SolidityComponent> GetDataStructuresDefinitions()
        {
            var structureDefinitions = new List<SolidityComponent>();
            structureDefinitions.AddRange(enums);
            structureDefinitions.AddRange(structs);
            return structureDefinitions;
        }

        public LiquidCollection TokenContractsToLiquid()
        {
            var collection = new LiquidCollection();
            tokens.ForEach(t => collection.Add(t.ToLiquidString(0)));
            return collection;
        }

        void ConvertEnums()
        {
            foreach (var enumType in contractConverter.Contract.Enums)
            {
                var solidityEnum = new SolidityEnum(enumType.Name);
                solidityEnum.Add(enumType.Values);

                enums.Add(solidityEnum);
            }
        }

        void ConvertEntities()
        {
            foreach (var entity in contractConverter.Contract.Entities)
            {
                SolidityStruct solidityStruct = new SolidityStruct(entity.Name);
                foreach (var property in entity.Properties)
                {
                    solidityStruct.AddToBody(ConvertProperty(property));
                }
                structs.Add(solidityStruct);
            }
        }

        void ConvertTokens()
        {
            //TODO: Create the contract only if custom logic is required
            foreach (var token in contractConverter.Contract.Tokens)
            {
                SolidityContract solidityContract = new SolidityContract(token.Name);
                if (token.IsFungible)
                {
                    solidityContract.AddInheritance(ConverterConfig.FUNGIBLE_TOKEN_NAME);
                    if (!dependencies.Contains(ConverterConfig.FUNGIBLE_TOKEN_IMPORT))
                        dependencies.Add(ConverterConfig.FUNGIBLE_TOKEN_IMPORT);
                }
                else
                {
                    solidityContract.AddInheritance(ConverterConfig.NON_FUNGIBLE_TOKEN_NAME);
                    if (!dependencies.Contains(ConverterConfig.NON_FUNGIBLE_TOKEN_IMPORT))
                        dependencies.Add(ConverterConfig.NON_FUNGIBLE_TOKEN_IMPORT);
                }

                foreach (var property in token.Properties)
                {
                    solidityContract.AddComponent(ConvertProperty(property));
                }
                tokens.Add(solidityContract);
            }
        }

        SolidityStatement ConvertProperty(Property property)
        {
            SolidityStatement propertyStatement = new SolidityStatement();
            string propertyType = Helpers.PropertyTypeToString(property, contractConverter);
            var propertyName = Helpers.ToLowerCamelCase(property.Name);

            if (property.PropertyType == PropertyType.Single)
                propertyStatement.Add($"{propertyType} {propertyName}");
            else if (property.PropertyType == PropertyType.Dictionary)
            {
                var keyType = Helpers.PropertyTypeToString(property.KeyType);
                propertyStatement.Add(ConversionTemplates.MappingTypeVariableDefinition(propertyName, keyType, propertyType));
            }
            else if (property.PropertyType == PropertyType.Collection)
            {
                propertyStatement.Add($"{propertyType}[] {propertyName}");
            }
            return propertyStatement;
            //TODO: exception propertytype not defined
        }


        public IList<SolidityComponent> GetSolidityComponents()
        {
            var components = new List<SolidityComponent>();
            components.AddRange(structs);
            return components;
        }
    }
}
