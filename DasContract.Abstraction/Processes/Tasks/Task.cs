namespace DasContract.Abstraction.Processes.Tasks
{
    public abstract class Task : IProcessElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
