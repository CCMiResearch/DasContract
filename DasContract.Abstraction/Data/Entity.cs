using DasContract.Abstraction.Processes;
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
            if (bool.TryParse(xElement.Attribute("IsRootEntity")?.Value, out var isRootEntity))
                IsRootEntity = isRootEntity;
            Properties = xElement.Elements("Property")?.Select(e => new Property(e)).ToList()
                ?? Properties;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = ElementNames.ENTITY;
            xElement.Add(
                new XAttribute("IsRootEntity", IsRootEntity)
            );

            foreach (var property in Properties.Select(p => p.ToXElement()).ToList())
            {
                xElement.Add(property);
            }

            return xElement;
        }
    }
}
