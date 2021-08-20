using System.Xml.Linq;

namespace DasContract.Abstraction.Data
{
    public class Property
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMandatory { get; set; } = true;

        public PropertyType PropertyType { get; set; }

        /// <summary>
        /// The keytype in case of PropertyType=PropertyType.Dictionary
        /// </summary>
        public PropertyDataType KeyType { get; set; }
        public PropertyDataType DataType { get; set; }
        /// <summary>
        /// A linked entity's id in case of Type=PropertyDataType.Entity. 
        /// </summary>
        public string ReferencedDataType { get; set; }

        public Property() { }
        public Property(XElement xElement)
        {
            Id = xElement.Attribute("Id").Value;
            Name = xElement.Element("Name")?.Value;
            ReferencedDataType = xElement.Element("ReferencedDataType")?.Value;

            if (bool.TryParse(xElement.Element("IsMandatory")?.Value, out var isMandatory))
                IsMandatory = isMandatory;
            if (System.Enum.TryParse<PropertyType>(xElement.Element("PropertyType")?.Value, out var propertyType))
                PropertyType = propertyType;
            if (System.Enum.TryParse<PropertyDataType>(xElement.Element("KeyType")?.Value, out var keyType))
                KeyType = keyType;
            if (System.Enum.TryParse<PropertyDataType>(xElement.Element("DataType")?.Value, out var dataType))
                DataType = dataType;

        }

        public XElement ToXElement()
        {
            return new XElement("Property",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("IsMandatory", IsMandatory),
                new XElement("PropertyType", PropertyType),
                new XElement("KeyType", KeyType),
                new XElement("DataType", DataType),
                new XElement("ReferencedDataType", ReferencedDataType)
            );
        }
    }
}
