using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class DecisionTableInput
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("label")]
        public string Label { get; set; } = string.Empty;

        [XmlAttribute("width", Namespace = "http://bpmn.io/schema/dmn/biodi/2.0")]
        public string Width { get; set; } = string.Empty;

        //Elements
        [XmlElement("inputExpression")]
        public InputExpression InputExpression { get; set; } = new InputExpression();

        [XmlElement("inputValues")]
        public InputValues InputValues { get; set; } = new InputValues();

        //Methods and Constructors
        public DecisionTableInput() { }
    }
}
