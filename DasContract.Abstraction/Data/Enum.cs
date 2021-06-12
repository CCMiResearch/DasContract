using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Data
{
    public class Enum : DataType
    {
        public IList<string> Values { get; set; } = new List<string>();

        public Enum() { }
        public Enum(XElement xElement) : base(xElement)
        {
            Values = xElement.Element("Values")?.Elements("Value")?.Select(e => e.Value).ToList()
                ?? Values;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "Enum";
            xElement.Add(
                new XElement("Values", Values.Select(v => new XElement("Value", v)).ToList())
            );
            return xElement;
        }
    }
}
