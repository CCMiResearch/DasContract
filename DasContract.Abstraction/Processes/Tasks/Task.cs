using DasContract.Abstraction.Processes.Tasks.InstanceTypes;

namespace DasContract.Abstraction.Processes.Tasks
{
    public abstract class Task : ProcessElement
    {
        public MultiInstance InstanceType { get; set; }
    }
}
