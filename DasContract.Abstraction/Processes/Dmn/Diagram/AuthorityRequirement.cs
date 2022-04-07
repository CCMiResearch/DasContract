using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn.Diagram
{
    public class AuthorityRequirement
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = String.Empty;

        //Elements
        [XmlElement("requiredAuthority")]
        public RequiredAuthority RequiredAuthority { get; set; } = new RequiredAuthority();

        //Methods and Constructors
        public AuthorityRequirement() { }
    }
}
