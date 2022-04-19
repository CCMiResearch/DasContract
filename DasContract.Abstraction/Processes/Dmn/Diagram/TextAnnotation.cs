using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn.Diagram
{
    public class TextAnnotation
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        //Elements
        [XmlElement("text")]
        public string Text { get; set; } = string.Empty;

        //Methods and Constructors
        public TextAnnotation() { }
    }
}
