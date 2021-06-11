using System.Xml.Linq;

namespace DasContract.Abstraction.Processes
{
    public class ProcessRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public XElement ToXElement()
        {
            return new XElement("ProcessRole",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("Description", Description)
            );
        }
    }
}
