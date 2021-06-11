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

        public virtual XElement ToXElement()
        {
            return new XElement("DataType",
                new XAttribute("Id", Id),
                new XElement("Name", Name)
            );
        }
    }
}
