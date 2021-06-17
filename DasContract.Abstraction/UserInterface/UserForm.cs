using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface
{
    public class UserForm
    {
        [XmlAttribute("Label")]
        public string Label { get; set; } = "";
        [XmlAttribute("FuncBind")]
        public string FuncBind { get; set; } = "";
        [XmlAttribute("StateMachine")]
        public string StateMachine { get; set; } = "";
        [XmlAttribute("Role")]
        public string Role { get; set; } = "";
        [XmlAttribute("RoleMachine")]
        public string RoleMachine { get; set; } = "";

        [XmlElement("FieldGroup")]
        public List<FieldGroup> FieldGroups { get; set; } = new List<FieldGroup>();
        public UserForm() { }
        public UserForm(XElement xElement)
        {

        }

        public XElement ToXElement()
        {
            return new XElement("UserForm");
        }
    }
}
