using DasContract.Abstraction.UserInterface;

namespace DasContract.Abstraction.Processes.Events
{
    public class StartEvent : Event
    {
        public UserForm StartForm { get; set; }
    }
}
