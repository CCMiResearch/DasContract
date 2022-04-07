using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn.Diagram
{
    public class KnowledgeRequirement
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = String.Empty;

        //Elements
        [XmlElement("requiredKnowledge")]
        public RequiredKnowledge RequiredKnowledge { get; set; } = new RequiredKnowledge();

        //Methods and Constructors
        public KnowledgeRequirement() { }
    }
}
