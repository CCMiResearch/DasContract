using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn.Diagram
{
    public class InformationRequirement
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = String.Empty;

        //Elements
        [XmlElement("requiredDecision")]
        public RequiredDecision RequiredDecision { get; set; } = new RequiredDecision();

        //Methods and Constructors
        public InformationRequirement() { }
    }
}
