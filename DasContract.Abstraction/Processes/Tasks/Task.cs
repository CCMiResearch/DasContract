namespace DasContract.Abstraction.Processes.Tasks
{
    public abstract class Task : ProcessElement
    {
        public TaskInstanceType InstanceType { get; set; }
    }
}
