using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class ScriptTask : PayableTask
    {
        public string Script { get; set; }

        public ScriptTask() { }
        public ScriptTask(XElement xElement) : base(xElement)
        {
            Script = xElement.Element("Script")?.Value;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = ElementNames.SCRIPT_TASK;
            xElement.Add(
                new XElement("Script", Script));
            return xElement;
        }
    }
}
