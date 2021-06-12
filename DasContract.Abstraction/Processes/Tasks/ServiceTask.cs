using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class ServiceTask : PayableTask
    {
        public string ImplementationType { get; set; }
        public string Configuration { get; set; }

        public ServiceTask() { }
        public ServiceTask(XElement xElement) : base(xElement)
        {
            ImplementationType = xElement.Element("ImplementationType")?.Value;
            Configuration = xElement.Element("Configuration")?.Value;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "ServiceTask";
            xElement.Add(
                new XElement("ImplementationType", ImplementationType),
                new XElement("Configuration", Configuration));
            return xElement;
        }
    }
}
