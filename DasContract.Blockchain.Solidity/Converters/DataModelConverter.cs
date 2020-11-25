using DasContract.Abstraction.Data;
using DasContract.Blockchain.Solidity.SolidityComponents;
using Liquid.NET.Constants;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Blockchain.Solidity.Converters
{
    public class DataModelConverter
    {
        ContractConverter contractConverter;
        List<TokenConverter> tokenConverters = new List<TokenConverter>();

        List<SolidityStatement> rootProperties = new List<SolidityStatement>();
        List<SolidityStruct> structs = new List<SolidityStruct>();
        List<SolidityEnum> enums = new List<SolidityEnum>();

        HashSet<string> dependencies = new HashSet<string>();

        public void AddDependency(string dependency)
        {
            dependencies.Add(dependency);
        }

        public DataModelConverter(ContractConverter contractConverter)
        {
            this.contractConverter = contractConverter;
            CreateTokenConverters();
        }

        public void ConvertLogic()
        {
            ConvertEntities();
            ConvertEnums();
            foreach(var converter in tokenConverters)
            {
                converter.ConvertLogic();
            }
        }

        void CreateTokenConverters()
        {
            foreach(var token in contractConverter.Contract.Tokens)
            {
                tokenConverters.Add(new TokenConverter(token, this));
            }
        }

        public HashSet<string> GetDependencies()
        {
            return dependencies;
        }

        public LiquidCollection TokenContractsToLiquid()
        {
            var collection = new LiquidCollection();
            foreach (var tokenConverter in tokenConverters)
            {
                collection.Add(tokenConverter.GetTokenContract().ToLiquidString(0));
            }
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
                if (entity.IsRootEntity)
                {
                    foreach (var property in entity.Properties)
                    {
                        rootProperties.Add(ConvertProperty(property));
                    }
                }
                else
                {
                    SolidityStruct solidityStruct = new SolidityStruct(entity.ToStructureName());
                    foreach (var property in entity.Properties)
                    {
                        solidityStruct.AddToBody(ConvertProperty(property));
                    }
                    structs.Add(solidityStruct);
                }
            }
        }

        public SolidityStatement ConvertProperty(Property property)
        {
            SolidityStatement propertyStatement = new SolidityStatement();
            string propertyType = Helpers.PropertyTypeToString(property, contractConverter);
            var propertyName = Helpers.ToLowerCamelCase(property.Name);

            if (property.PropertyType == PropertyType.Single || property.PropertyType == PropertyType.Collection)
                propertyStatement.Add($"{propertyType} {propertyName}");
            else if (property.PropertyType == PropertyType.Dictionary)
            {
                var keyType = Helpers.PrimitivePropertyTypeToString(property.KeyType);
                propertyStatement.Add(ConversionTemplates.MappingTypeVariableDefinition(propertyName, keyType, propertyType));
                //Also an an array to store the key values (used when iteration through the mapping is required)
                propertyStatement.Add($"{keyType}[] {ConversionTemplates.MappingKeysArrayName(propertyName)}");
            }
            return propertyStatement;
            //TODO: exception propertytype not defined
        }

        public SolidityStatement GetConstructorStatements()
        {
            var statement = new SolidityStatement();
            foreach (var tokenConverter in tokenConverters)
            {
                statement.Add(tokenConverter.GetConstructorStatement());
            }
            return statement;
        }


        public IList<SolidityComponent> GetMainContractComponents()
        {
            var components = new List<SolidityComponent>();
            components.AddRange(rootProperties);
            foreach (var tokenConverter in tokenConverters)
            {
                components.Add(tokenConverter.GetTokenVariableStatement());
            }
            components.AddRange(enums);
            components.AddRange(structs);
            return components;
        }
    }
}
