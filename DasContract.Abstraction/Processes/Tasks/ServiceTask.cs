namespace DasContract.Abstraction.Processes.Tasks
{
    public class ServiceTask : PayableTask
    {
        public string ImplementationType { get; set; }
        public string Configuration { get; set; }
    }
}
