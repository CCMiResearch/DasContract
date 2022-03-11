using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public abstract class PayableTask : Task
    {
        public TokenOperationType OperationType { get; set; }

        public PayableTask() { }
        public PayableTask(XElement xElement) : base(xElement)
        {
            if (System.Enum.TryParse<TokenOperationType>(xElement.Element("TokenOperationType")?.Value, out var type))
                OperationType = type;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Add(
                new XElement("TokenOperationType", OperationType));
            return xElement;
        }
    }
}
