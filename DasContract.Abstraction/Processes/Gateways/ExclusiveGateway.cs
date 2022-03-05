using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Gateways
{
    public class ExclusiveGateway : Gateway
    {
        public ExclusiveGateway() { }
        public ExclusiveGateway(XElement xElement) : base(xElement) { }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = ElementNames.EXCLUSIVE_GATEWAY;
            return xElement;
        }
    }
}
