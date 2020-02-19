using DasContract.Abstraction.Interface.Processes.Tasks;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class ScriptTask : Task, ICustomDataCopyable<ScriptTask>
    {
        public string Script { get; set; }

        public void CopyCustomDataFrom(ScriptTask task)
        {
            Script = task.Script;
        }
    }
}
