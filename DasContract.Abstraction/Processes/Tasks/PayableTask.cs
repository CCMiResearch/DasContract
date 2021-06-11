using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public abstract class PayableTask: Task
    {
        public TokenOperationType OperationType { get; set; }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "PayableTask";
            xElement.Add(
                new XElement("TokenOperationType", OperationType));
            return xElement;
        }
    }
}
