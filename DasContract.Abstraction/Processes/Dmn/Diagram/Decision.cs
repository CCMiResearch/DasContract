using DasContract.Abstraction.Processes.Dmn.Diagram;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class Decision
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        //Elements
        [XmlElement("decisionTable")]
        public DecisionTable DecisionTable { get; set; } = new DecisionTable();

        [XmlElement("authorityRequirement")]
        public List<AuthorityRequirement> AuthorityRequirements { get; set; } = new List<AuthorityRequirement>();

        [XmlElement("knowledgeRequirement")]
        public List<KnowledgeRequirement> KnowledgeRequirements { get; set; } = new List<KnowledgeRequirement>();

        [XmlElement("informationRequirement")]
        public List<InformationRequirement> InformationRequirements { get; set; } = new List<InformationRequirement>();

        [XmlElement("variable")]
        public Variable Variable { get; set; } = new Variable();

        [XmlElement("literalExpression")]
        public LiteralExpression LiteralExpression { get; set; } = new LiteralExpression();

        //Methods and Constructors
        public Decision() { }
    }
}
