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

        public XElement ToXElement()
        {
            return new XElement("UserForm",
                new XAttribute("Id", Id),
                new XElement("Fields", Fields.Select(f => f.ToXElement()).ToList())
            );
        }
    }
}
