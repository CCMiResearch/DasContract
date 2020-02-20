using DasContract.Editor.Entities.Interfaces;

namespace DasContract.Editor.Entities.Processes.Process
{
    /// <summary>
    /// An arrow between process elements. 
    /// </summary>
    public class ContractSequenceFlow : IIdentifiable, INamable
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
    }
}
