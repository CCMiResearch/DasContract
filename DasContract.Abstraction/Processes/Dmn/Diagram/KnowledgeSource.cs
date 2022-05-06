using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn.Diagram
{
    public class KnowledgeSource
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        //Elements
        [XmlElement("authorityRequirement")]
        public List<AuthorityRequirement> AuthorityRequirements { get; set; } = new List<AuthorityRequirement>();

        //Methods and Constructors
        public KnowledgeSource() { }
    }
}
