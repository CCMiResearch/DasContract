using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface
{
    [XmlRoot("Form")]
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

        public static UserForm DeserializeFormScript(string formScript)
        {
            using (TextReader reader = new StringReader(formScript))
            {
                XmlSerializer serializer = CreateThrowingSerializer();
                XmlReader xmlReader = new XmlTextReader(reader);

                return (UserForm)serializer.Deserialize(xmlReader);
            }
        }

        private static void Serializer_Throw(object sender, XmlElementEventArgs e)
        {
            throw new Exception("XML format exception.");
        }
        private static void Serializer_Throw(object sender, XmlAttributeEventArgs e)
        {
            throw new Exception("XML format exception.");
        }
        private static void Serializer_Throw(object sender, XmlNodeEventArgs e)
        {
            throw new Exception("XML format exception.");
        }

        private static XmlSerializer CreateThrowingSerializer()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserForm));
            serializer.UnknownAttribute += new XmlAttributeEventHandler(Serializer_Throw);
            serializer.UnknownElement += new XmlElementEventHandler(Serializer_Throw);
            serializer.UnknownNode += new XmlNodeEventHandler(Serializer_Throw);
            return serializer;
        }

        public XElement ToXElement()
        {
            return new XElement("UserForm");
        }
    }
}
