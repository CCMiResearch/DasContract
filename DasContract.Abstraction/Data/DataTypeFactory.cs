using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DasContract.Abstraction.Data;

namespace DasContract.DasContract.Abstraction.Data
{
    public class DataTypeFactory
    {
        //TODO: Refactor entity factory
        /*
        public static List<Entity> FromDasFile(string processXml)
        {
            return FromDasFile(XElement.Parse(processXml));
        }

        public static List<Entity> FromDasFile(XElement xElement)
        {
            List<Entity> entities = new List<Entity>();
            var entityElements = xElement.Descendants().ToList();

            foreach (var e in entityElements)
            {
                if (e.Name == "ContractEntity")
                {
                    Entity entity = new Entity();
                    var contractElements = e.Descendants().ToList();

                    foreach (var i in contractElements)
                    {
                        if (i.Name == "Name" && i.Ancestors().First().Name == "ContractEntity")
                            entity.Name = RemoveWhitespaces(i.Value);
                        else if (i.Name == "Id" && i.Ancestors().First().Name == "ContractEntity")
                            entity.Id = i.Value;
                        else if (i.Name == "PrimitiveContractProperty" || i.Name == "ReferenceContractProperty")
                            entity.Properties.Add(GetEntityProperty(i));
                    }
                    entities.Add(entity);
                }
            }

            FillReferences(entities);
            return entities;
        }

        private static void FillReferences(List<Entity> entities)
        {
            foreach(var e in entities)
            {
                foreach(var p in e.Properties)
                {
                    if (p.DataType == PropertyDataType.Entity && p.ReferencedDataType != null)
                    {
                        foreach (var en in entities)
                        {
                            if(en.Id == p.ReferencedDataType.Id)
                            {
                                p.ReferencedDataType = en;
                            }
                        }
                    }
                }
            }
        }

        private static Property GetEntityProperty(XElement element)
        {
            Property property = new Property();
            property.Id = element.Descendants("Id").FirstOrDefault().Value;
            property.Name = RemoveWhitespaces(element.Descendants("Name").FirstOrDefault().Value);
            if (element.Descendants("IsMandatory").FirstOrDefault().Value == "False")
            {
                property.IsMandatory = false;
            }
            if (element.Descendants("EntityId").FirstOrDefault() != null)
            {
                property.ReferencedDataType = new Entity();
                property.ReferencedDataType.Id = element.Descendants("EntityId").FirstOrDefault().Value;
            }
            var type = element.Descendants("Type").FirstOrDefault().Value;
            switch (type)
            {
                // TODO: Fill the rest
                case "ReferenceCollection":
                    property.DataType = PropertyDataType.Entity;
                    property.IsCollection = true;
                    break;
                case "SingleReference":
                    property.DataType = PropertyDataType.Entity;
                    break;
                case "Number":
                    property.DataType = PropertyDataType.Uint;
                    break;
                case "Address":
                    property.DataType = PropertyDataType.Address;
                    break;
                case "AddressPayable":
                    property.DataType = PropertyDataType.AddressPayable;
                    break;
                case "Bool":
                    property.DataType = PropertyDataType.Bool;
                    break;
                default:
                    property.DataType = PropertyDataType.String;
                    break;
            }
            return property;
        }

        private static string RemoveWhitespaces(string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
        */
    }
}
