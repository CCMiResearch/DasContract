using DasContract.Abstraction.Processes.Dmn.Diagram;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class DmnEdge
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = String.Empty;

        [XmlAttribute("dmnElementRef")]
        public string DmnElementRef { get; set; } = String.Empty;

        //Elements
        [XmlElement("waypoint", Namespace = "http://www.omg.org/spec/DMN/20180521/DI/")]
        public List<Waypoint> Waypoints { get; set; } = new List<Waypoint>();

        //Methods and Constructors
        public DmnEdge() { }
    }
}
