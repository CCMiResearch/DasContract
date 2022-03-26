using System.Xml.Linq;

namespace DasContract.Abstraction.Data
{
    public class Property
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMandatory { get; set; } = true;

        public PropertyType? PropertyType { get; set; }

        /// <summary>
        /// The keytype in case of PropertyType=PropertyType.Dictionary
        /// </summary>
        public PropertyDataType? KeyType { get; set; }
        public PropertyDataType? DataType { get; set; }
        /// <summary>
        /// A linked entity's id in case of Type=PropertyDataType.Entity. 
        /// </summary>
        public string ReferencedDataType { get; set; }

        public Property() { }
        public Property(XElement xElement)
        {
            Id = xElement.Attribute("Id").Value;
            Name = xElement.Attribute("Name").Value;
            ReferencedDataType = xElement.Attribute("ReferencedDataType")?.Value;

            if (bool.TryParse(xElement.Attribute("IsMandatory")?.Value, out var isMandatory))
                IsMandatory = isMandatory;
            if (System.Enum.TryParse<PropertyType>(xElement.Attribute("PropertyType")?.Value, true, out var propertyType))
                PropertyType = propertyType;
            if (System.Enum.TryParse<PropertyDataType>(xElement.Attribute("KeyType")?.Value, true, out var keyType))
                KeyType = keyType;
            if (System.Enum.TryParse<PropertyDataType>(xElement.Attribute("DataType")?.Value, true, out var dataType))
                DataType = dataType;

        }

        public XElement ToXElement()
        {
            var xElement = new XElement("Property",
                new XAttribute("Id", Id),
                new XAttribute("Name", Name),
                new XAttribute("IsMandatory", IsMandatory)
            );

            if (PropertyType != null)
                xElement.Add(new XAttribute("PropertyType", PropertyType));
            if (KeyType != null)
                xElement.Add(new XAttribute("KeyType", KeyType));
            if (DataType != null)
                xElement.Add(new XAttribute("DataType", DataType));
            if (ReferencedDataType != null)
                xElement.Add(new XAttribute("ReferencedDataType", ReferencedDataType));

            return xElement;
        }
    }
}
