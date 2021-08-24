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
            Values = xElement.Elements("Value")?.Select(e => e.Value).ToList()
                ?? Values;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "Enum";
            foreach (var value in Values.Select(v => new XElement("Value", v)).ToList())
            {
                xElement.Add(value);
            }

            return xElement;
        }
    }
}
