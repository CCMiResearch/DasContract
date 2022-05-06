using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class DecisionTableOutput
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("label")]
        public string Label { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("typeRef")]
        public string TypeRef { get; set; } = string.Empty;

        [XmlAttribute("width", Namespace = "http://bpmn.io/schema/dmn/biodi/2.0")]
        public string Width { get; set; } = string.Empty;

        //Elements
        [XmlElement("outputValues")]
        public OutputValues OutputValues { get; set; } = new OutputValues();

        //Methods and Constructors
        public DecisionTableOutput() { }
    }
}
