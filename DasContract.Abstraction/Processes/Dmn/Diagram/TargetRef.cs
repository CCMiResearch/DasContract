using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn.Diagram
{
    public class TargetRef
    {
        //Attributes
        [XmlAttribute("href")]
        public string Href { get; set; } = String.Empty;

        //Methods and Constructors
        public TargetRef() { }
    }
}
