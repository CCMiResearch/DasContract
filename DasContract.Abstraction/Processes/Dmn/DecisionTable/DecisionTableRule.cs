using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class DecisionTableRule
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        //Elements
        [XmlElement("description")]
        public string Description { get; set; } = string.Empty;

        [XmlElement("inputEntry")]
        public List<InputEntry> InputEntries { get; set; } = new List<InputEntry>();

        [XmlElement("outputEntry")]
        public List<OutputEntry> OutputEntries { get; set; } = new List<OutputEntry>();

        //Methods and Constructors
        public DecisionTableRule() { }
    }
}
