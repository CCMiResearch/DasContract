using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Events
{
    public class BoundaryEvent : Event
    {
        public string AttachedTo { get; set; }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "BoundaryEvent";
            xElement.Add(
                new XElement("AttachedTo", AttachedTo));
            return xElement;
        }
    }
}
