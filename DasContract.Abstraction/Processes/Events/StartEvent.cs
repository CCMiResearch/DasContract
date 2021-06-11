using DasContract.Abstraction.UserInterface;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Events
{
    public class StartEvent : Event
    {
        public UserForm StartForm { get; set; }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "StartEvent";
            xElement.Add(StartForm?.ToXElement());
            return xElement;
        }
    }
}
