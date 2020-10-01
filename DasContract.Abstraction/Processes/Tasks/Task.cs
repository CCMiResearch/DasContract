using DasContract.Abstraction.Processes.Tasks;

namespace DasContract.Abstraction.Processes.Tasks
{
    public abstract class Task : ProcessElement
    {
        public MultiInstance MultiInstance { get; set; }
    }
}
