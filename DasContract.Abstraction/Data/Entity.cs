using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction.Data
{
    public class Entity: DataType
    {
        public IList<Property> Properties { get; set; } = new List<Property>(); 

        public bool IsRootEntity { get; set; }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "Entity";
            xElement.Add(
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("IsRootEntity", IsRootEntity),
                new XElement("Properties", Properties.Select(p => p.ToXElement()).ToList())
            );
            return xElement;
        }
    }
}
