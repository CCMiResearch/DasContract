using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class Bounds
    {
        //Attributes
        [XmlAttribute("height")]
        public string Height { get; set; } = string.Empty;

        [XmlAttribute("width")]
        public string Width { get; set; } = string.Empty;

        [XmlAttribute("x")]
        public string X { get; set; } = string.Empty;

        [XmlAttribute("y")]
        public string Y { get; set; } = string.Empty;

        //Methods and Constructors
        public Bounds() { }
    }
}
