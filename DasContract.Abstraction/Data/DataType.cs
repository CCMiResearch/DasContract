using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Data
{
    public abstract class DataType
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public DataType() { }
        public DataType(XElement xElement)
        {
            Id = xElement.Attribute("Id").Value;
            Name = xElement.Attribute("Name").Value;
        }

        public virtual XElement ToXElement()
        {
            return new XElement("DataType",
                new XAttribute("Id", Id),
                new XAttribute("Name", Name)
            );
        }
    }
}
