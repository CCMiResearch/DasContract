﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn.Diagram
{
    public class InputData
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = String.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = String.Empty;

        //Methods and Constructors
        public InputData() { }
    }
}
