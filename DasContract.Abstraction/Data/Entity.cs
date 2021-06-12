using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction.Data
{
    public class Entity : DataType
    {
        public IList<Property> Properties { get; set; } = new List<Property>();

        public bool IsRootEntity { get; set; }

        public Entity() { }
        public Entity(XElement xElement) : base(xElement)
        {
            if (bool.TryParse(xElement.Element("IsRootEntity")?.Value, out var isRootEntity))
                IsRootEntity = isRootEntity;
            Properties = xElement.Element("Properties")?.Elements("Property")?.Select(e => new Property(e)).ToList()
                ?? Properties;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "Entity";
            xElement.Add(
                new XElement("IsRootEntity", IsRootEntity),
                new XElement("Properties", Properties.Select(p => p.ToXElement()).ToList())
            );
            return xElement;
        }
    }
}
