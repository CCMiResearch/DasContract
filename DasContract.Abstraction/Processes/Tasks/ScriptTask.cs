using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class ScriptTask : PayableTask
    {
        public string Script { get; set; }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "ScriptTask";
            xElement.Add(
                new XElement("Script", Script));
            return xElement;
        }
    }
}
