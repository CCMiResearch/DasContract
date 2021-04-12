using DasContract.Abstraction.Processes.Tasks;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class Task : ProcessElement
    {
        public InstanceType InstanceType { get; set; }
        /// <summary>
        /// Numbers of repetitions for the process
        /// </summary>
        public int LoopCardinality { get; set; }
        /// <summary>
        /// ID of the referenced collection entity (must be an entity that is saved inside of the contract)
        /// </summary>
        public string LoopCollection { get; set; }
    }
}
