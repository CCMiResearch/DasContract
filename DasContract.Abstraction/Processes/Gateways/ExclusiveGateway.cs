using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Gateways
{
    public class ExclusiveGateway : Gateway
    {
        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "ExclusiveGateway";
            return xElement;
        }
    }
}
