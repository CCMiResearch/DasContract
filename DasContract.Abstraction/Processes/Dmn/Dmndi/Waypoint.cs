using DasContract.Abstraction.Processes.Dmn.Diagram;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class Waypoint
    {
        //Attributes
        [XmlAttribute("x")]
        public string X { get; set; } = string.Empty;

        [XmlAttribute("y")]
        public string Y { get; set; } = string.Empty;

        //Methods and Constructors
        public Waypoint() { }
    }
}