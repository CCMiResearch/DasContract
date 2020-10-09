using DasContract.Abstraction.Processes.Tasks;

namespace DasContract.Abstraction.Processes.Tasks
{
    public abstract class Task : ProcessElement
    {
        public InstanceType InstanceType { get; set; }
        public int LoopCardinality { get; set; }
        public string LoopCollection { get; set; }
    }
}
