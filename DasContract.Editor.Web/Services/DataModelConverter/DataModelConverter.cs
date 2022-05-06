using DasContract.Abstraction.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.DataModel
{
    public class DataModelConverter : IDataModelConverter
    {
        public string ConvertToDiagramCode(IDictionary<string, DataType> dataTypes)
        {
            var mermaidDiagram = new StringBuilder();
            mermaidDiagram.Append("classDiagram\n");

            var referenceTranslations = dataTypes.ToDictionary(d => d.Key, d => d.Value.Name);

            foreach (var dataType in dataTypes.Values)
            {
                switch(dataType)
                {
                    case Token t:
                        mermaidDiagram.Append(ConvertToken(t, referenceTranslations));
                        break;
                    case Entity e:
                        mermaidDiagram.Append(ConvertEntity(e, referenceTranslations));
                        break;
                    case Abstraction.Data.Enum e:
                        mermaidDiagram.Append(ConvertEnum(e));
                        break;
                }
            }

            var relationships = mermaidDiagram.AppendJoin("\n", GetModelRelationships(dataTypes).Select(i => $"{i.Item1} --> {i.Item2}: references"));

            return mermaidDiagram.ToString();
        }


        private static string ConvertEntity(Entity entity, IDictionary<string, string> referenceTranslations)
        {
            var convertedProperties = entity.Properties.Select(p => ConvertProperty(p, referenceTranslations));

            return $"class {entity.Name} {{ \n" +
                $"<<entity>> \n" +
                $"{string.Join("\n", convertedProperties)} \n" +
                $"}}\n";
        }

        private static string ConvertEnum(Abstraction.Data.Enum enumeration)
        {

            return $"class {enumeration.Name} {{ \n" +
                $"<<enum>> \n" +
                $"{string.Join("\n", enumeration.Values)} \n" +
                $"}}\n";
        }
        private static string ConvertToken(Token token, IDictionary<string, string> referenceTranslations)
        {
            var convertedProperties = token.Properties.Select(p => ConvertProperty(p, referenceTranslations));
            return $"class {token.Name} {{ \n" +
                $"<<token>> \n" +
                $"{string.Join("\n", convertedProperties)} \n" +
                $"symbol() {token.Symbol}\n" +
                $"isFungible() {token.IsFungible}\n" +
                $"isIssued() {token.IsIssued}\n" +
                $"}}\n";
        }

        private static IList<Tuple<string, string>> GetModelRelationships(IDictionary<string, DataType> dataTypes)
        {
            var entities = dataTypes.Values.OfType<Entity>();
            var relationships = new List<Tuple<string, string>>();
            foreach (var entity in entities)
            {
                foreach (var property in entity.Properties)
                {
                    if (property.DataType == PropertyDataType.Reference && property.ReferencedDataType != null
                        && dataTypes.TryGetValue(property.ReferencedDataType, out var referenced))
                    {
                        relationships.Add(new Tuple<string, string>(entity.Name, referenced.Name));
                    }
                }
            }
            return relationships;
        }

        private static string ConvertProperty(Property property, IDictionary<string, string> referenceTranslations)
        {
            string dataType;
            string propertyType;

            if (property.DataType == PropertyDataType.Reference)
            {
                if (property.ReferencedDataType is null || !referenceTranslations.TryGetValue(property.ReferencedDataType, out var referencedName))
                {
                    throw new DataModelConversionException($"Property {property.Id} is defined as reference, " +
                        $"but {property.ReferencedDataType} is not a valid reference");
                }
                dataType = referencedName;
            }
            else
            {
                dataType = property.DataType.ToString();
            }

            if (property.PropertyType == PropertyType.Collection)
            {
                propertyType = "[]";
            }
            else if(property.PropertyType == PropertyType.Dictionary)
            {
                propertyType = $"&lt{property.KeyType}&gt";
            }
            else
            {
                propertyType = "";
            }

            return $"{dataType}{propertyType} {property.Name}";
        }
    }
}
