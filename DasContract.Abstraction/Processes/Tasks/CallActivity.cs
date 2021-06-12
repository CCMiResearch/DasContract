using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class CallActivity : Task
    {
        public string CalledElement { get; set; }

        public CallActivity() { }
        public CallActivity(XElement xElement) : base(xElement)
        {
            CalledElement = xElement.Element("CalledElement")?.Value;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "CallActivity";
            xElement.Add(
                new XElement("CalledElement", CalledElement));
            return xElement;
        }
    }
}
