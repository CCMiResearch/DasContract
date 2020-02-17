namespace DasContract.Abstraction.Processes.Tasks
{
    public class ServiceTask : Task
    {
        public string ImplementationType { get; set; }
        public string Configuration { get; set; }
    }
}
