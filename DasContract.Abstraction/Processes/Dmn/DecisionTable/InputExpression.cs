using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class InputExpression
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = String.Empty;

        [XmlAttribute("typeRef")]
        public string TypeRef { get; set; } = String.Empty;

        //Elements
        [XmlElement("text")]
        public string Text { get; set; } = String.Empty;

        //Methods and Constructors
        public InputExpression() { }
    }
}
