using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes
{
    public abstract class ProcessElement : IProcessElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<string> Incoming { get; set; } = new List<string>();
        public IList<string> Outgoing { get; set; } = new List<string>();

        public ProcessElement() { }
        public ProcessElement(XElement xElement)
        {
            Id = xElement.Attribute("Id").Value;
            Name = xElement.Element("Name")?.Value;
            Incoming = xElement.Element("Incoming")?.Elements("IncomingId")?.Select(e => e.Value).ToList()
                ?? Incoming;
            Outgoing = xElement.Element("Outgoing")?.Elements("OutgoingId")?.Select(e => e.Value).ToList()
                ?? Outgoing;
        }

        public virtual XElement ToXElement()
        {
            return new XElement("ProcessElement",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("Incoming", Incoming.Select(e => new XElement("IncomingId", e))),
                new XElement("Outgoing", Outgoing.Select(e => new XElement("OutgoingId", e)))
            );
        }
    }
}
