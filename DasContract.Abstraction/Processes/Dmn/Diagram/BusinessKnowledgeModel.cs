using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn.Diagram
{
    public class BusinessKnowledgeModel
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = String.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = String.Empty;

        //Elements
        [XmlElement("authorityRequirement")]
        public List<AuthorityRequirement> AuthorityRequirements { get; set; } = new List<AuthorityRequirement>();

        [XmlElement("knowledgeRequirement")]
        public List<KnowledgeRequirement> KnowledgeRequirements { get; set; } = new List<KnowledgeRequirement>();

        //Methods and Constructors
        public BusinessKnowledgeModel() { }
    }
}
