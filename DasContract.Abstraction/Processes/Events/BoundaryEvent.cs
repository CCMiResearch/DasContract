using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Events
{
    public class BoundaryEvent : Event
    {
        public string AttachedTo { get; set; }

        public BoundaryEvent() { }
        public BoundaryEvent(XElement xElement) : base(xElement)
        {
            AttachedTo = xElement.Element("AttachedTo")?.Value;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = ElementNames.BOUNDARY_EVENT;
            xElement.Add(
                new XElement("AttachedTo", AttachedTo));
            return xElement;
        }
    }
}
