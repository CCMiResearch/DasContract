using System.Xml.Linq;

namespace DasContract.Abstraction.Processes
{
    public class ProcessRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ProcessRole() { }
        public ProcessRole(XElement xElement)
        {
            Id = xElement.Attribute("Id").Value;
            Name = xElement.Element("Name")?.Value;
            Description = xElement.Element("Description")?.Value;
        }

        public XElement ToXElement()
        {
            return new XElement("ProcessRole",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("Description", Description)
            );
        }
    }
}
