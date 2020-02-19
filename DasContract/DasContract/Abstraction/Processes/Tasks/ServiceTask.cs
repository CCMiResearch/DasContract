using DasContract.Abstraction.Interface.Processes.Tasks;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class ServiceTask : Task, ICustomDataCopyable<ServiceTask>
    {
        public string ImplementationType { get; set; }

        public string Configuration { get; set; }

        public void CopyCustomDataFrom(ServiceTask task)
        {
            ImplementationType = task.ImplementationType;
            Configuration = task.Configuration;
        }
    }
}
