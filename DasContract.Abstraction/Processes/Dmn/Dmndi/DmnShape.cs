using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class DmnShape
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = String.Empty;

        [XmlAttribute("dmnElementRef")]
        public string DmnElementRef { get; set; } = String.Empty;

        //Elements
        [XmlElement("Bounds", Namespace = "http://www.omg.org/spec/DMN/20180521/DC/")]
        public Bounds Bounds { get; set; } = new Bounds();

        //Methods and Constructors
        public DmnShape() { }
    }
}
