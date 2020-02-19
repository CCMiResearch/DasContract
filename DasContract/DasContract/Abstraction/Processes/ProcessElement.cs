using System.Collections.Generic;
using System.Xml.Serialization;
using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;

namespace DasContract.Abstraction.Processes
{
    [
        XmlInclude(typeof(Task)), 
        XmlInclude(typeof(ServiceTask)),
        XmlInclude(typeof(BusinessRuleTask)),
        XmlInclude(typeof(ScriptTask)),
        XmlInclude(typeof(UserTask)),

        XmlInclude(typeof(Gateway)),
        XmlInclude(typeof(ExclusiveGateway)),
        XmlInclude(typeof(ParallelGateway)),

        XmlInclude(typeof(Event)),
        XmlInclude(typeof(EndEvent)),
        XmlInclude(typeof(StartEvent))
    ]
    public abstract class ProcessElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Incoming { get; set; } = new List<string>();
        public List<string> Outgoing { get; set; } = new List<string>();
    }
}
