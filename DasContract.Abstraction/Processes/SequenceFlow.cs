using System.Xml.Linq;

namespace DasContract.Abstraction.Processes
{
    /// <summary>
    /// An arrow between process elements. 
    /// </summary>
    public class SequenceFlow: IProcessElement
    {
        public string Id { get; set; }
        /// <summary>
        /// An arrow description. 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Source process element id. 
        /// </summary>
        public string SourceId { get; set; }
        /// <summary>
        /// Target process element id. 
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// Condition in solidity under which the sequence flow can be traversed
        /// </summary>
        public string Condition { get; set; }

        public XElement ToXElement()
        {
            return new XElement("SequenceFlow",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("SourceId", SourceId),
                new XElement("TargetId", TargetId),
                new XElement("Condition", Condition)
            );
        }
    }
}
