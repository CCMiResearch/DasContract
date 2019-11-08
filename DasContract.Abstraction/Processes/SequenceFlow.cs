namespace DasContract.Abstraction.Processes
{
    /// <summary>
    /// An arrow between process elements. 
    /// </summary>
    public class SequenceFlow
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
    }
}
