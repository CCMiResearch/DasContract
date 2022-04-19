using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn.Diagram
{
    public class Association
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        //Elements
        [XmlElement("sourceRef")]
        public SourceRef SourceRef { get; set; } = new SourceRef();

        [XmlElement("targetRef")]
        public TargetRef TargetRef { get; set; } = new TargetRef();

        //Methods and Constructors
        public Association() { }
    }
}
