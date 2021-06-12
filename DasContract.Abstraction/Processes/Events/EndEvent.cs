using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Events
{
    public class EndEvent : Event
    {
        public EndEvent() { }
        public EndEvent(XElement xElement) : base(xElement)
        {

        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "EndEvent";
            return xElement;
        }
    }
}
