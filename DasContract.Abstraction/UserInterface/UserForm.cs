using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.UserInterface
{
    public class UserForm
    {
        public string Id { get; set; }
        public IList<FormField> Fields { get; set; }

        public UserForm() { }
        public UserForm(XElement xElement)
        {
            Id = xElement.Attribute("Id").Value;
            Fields = xElement.Element("FormFields")?.Elements("FormField")?.Select(e => new FormField(e)).ToList();
        }

        public XElement ToXElement()
        {
            return new XElement("UserForm",
                new XAttribute("Id", Id),
                new XElement("FormFields", Fields.Select(f => f.ToXElement()).ToList())
            );
        }
    }
}
