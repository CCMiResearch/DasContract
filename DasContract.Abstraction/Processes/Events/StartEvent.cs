using DasContract.Abstraction.UserInterface;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Events
{
    public class StartEvent : Event
    {
        public UserForm StartForm { get; set; }

        public StartEvent() { }
        public StartEvent(XElement xElement) : base(xElement)
        {
            var xStartForm = xElement.Element("StartForm");
            if (xStartForm != null)
                StartForm = new UserForm(xStartForm);
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "StartEvent";

            var xStartForm = StartForm?.ToXElement();
            if (xStartForm != null)
            {
                xStartForm.Name = "StartForm";
                xElement.Add(xStartForm);
            }
            
            return xElement;
        }
    }
}
