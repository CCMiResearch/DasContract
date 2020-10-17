using DasContract.Abstraction.Data;
using DasContract.Blockchain.Solidity.SolidityComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Blockchain.Solidity.Converters
{
    public class DataModelConverter
    {
        ProcessConverter converterService;

        List<SolidityStruct> structs;
        List<SolidityEnum> enums;


        public DataModelConverter(ProcessConverter converterService)
        {
            this.converterService = converterService;
        }

        public void ConvertLogic()
        {
            ConvertEntities();
            ConvertEnums();
        }

        void ConvertEnums()
        {
            foreach (var enumType in converterService.Contract.Enums)
            {
                var solidityEnum = new SolidityEnum(enumType.Name);
                solidityEnum.Add(enumType.Values);

                enums.Add(solidityEnum);
            }
        }

        void ConvertEntities()
        {
            foreach (var entity in converterService.Contract.Entities)
            {
                SolidityStruct solidityStruct = new SolidityStruct(entity);
                foreach (var property in entity.Properties)
                {
                    solidityStruct.AddToBody(ConvertProperty(property));
                }
                structs.Add(solidityStruct);
            }
        }

        void ConvertTokens()
        {
            foreach (var token in converterService.Contract.Tokens)
            {
                
            }
        }

        SolidityStatement ConvertProperty(Property property)
        {
            SolidityStatement propertyStatement = new SolidityStatement();
            string propertyType = Helpers.PropertyTypeToString(property);
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
