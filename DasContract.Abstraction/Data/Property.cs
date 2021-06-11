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

        public XElement ToXElement()
        {
            return new XElement("DataType",
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
