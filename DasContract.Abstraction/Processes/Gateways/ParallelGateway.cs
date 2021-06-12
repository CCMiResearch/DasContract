using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Gateways
{
    public class ParallelGateway : Gateway
    {
        public ParallelGateway() { }
        public ParallelGateway(XElement xElement) : base(xElement)
        {

        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "ParallelGateway";
            return xElement;
        }
    }
}
