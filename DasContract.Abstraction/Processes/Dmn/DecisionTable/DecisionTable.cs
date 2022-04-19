using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class DecisionTable
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("hitPolicy")]
        public string HitPolicy { get; set; } = string.Empty;

        [XmlAttribute("aggregation")]
        public string Aggregation { get; set; } = string.Empty;

        [XmlAttribute("annotationsWidth", Namespace = "http://bpmn.io/schema/dmn/biodi/2.0")]
        public string AnnotationsWidth { get; set; } = string.Empty;

        //Elements
        [XmlElement("input")]
        public List<DecisionTableInput> Inputs { get; set; } = new List<DecisionTableInput>();

        [XmlElement("output")]
        public List<DecisionTableOutput> Outputs { get; set; } = new List<DecisionTableOutput>();

        [XmlElement("rule")]
        public List<DecisionTableRule> Rules { get; set; } = new List<DecisionTableRule>();

        //Methods and Constructors
        public DecisionTable() { }
    }
}
