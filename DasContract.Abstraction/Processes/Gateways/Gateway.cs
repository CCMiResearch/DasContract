using System.Collections.Generic;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Gateways
{
    public abstract class Gateway : ProcessElement
    {
        public string DefaultSequenceFlowId { get; set; }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "Gateway";
            xElement.Add(
                new XElement("DefaultSequenceFlowId", DefaultSequenceFlowId));
            return xElement;
        }
    }
}
