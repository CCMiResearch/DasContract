using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Gateways
{
    public class ParallelGateway : Gateway
    {
        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "ParallelGateway";
            return xElement;
        }
    }
}
