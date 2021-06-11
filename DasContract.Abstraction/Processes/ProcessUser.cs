using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes
{
    public class ProcessUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public IList<ProcessRole> Roles { get; set; } = new List<ProcessRole>();

        public XElement ToXElement()
        {
            return new XElement("ProcessUser",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("Description", Description),
                new XElement("Address", Address),
                new XElement("Roles", Roles.Select(r => r.ToXElement()).ToList())
            );
        }
    }
}
