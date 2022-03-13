using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes
{
    public class Process : IContractElement
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public bool IsExecutable { get; set; }
        public string BpmnId { get; set; }
        public string ParticipantId { get; set; }

        public string CustomScript { get; set; }

        public IDictionary<string, SequenceFlow> SequenceFlows { get; set; } = new Dictionary<string, SequenceFlow>(); 
        public IDictionary<string, ProcessElement> ProcessElements { get; set; } = new Dictionary<string, ProcessElement>();
        
        public IEnumerable<Task> Tasks { get { return ProcessElements.Values.OfType<Task>(); } }
        public IEnumerable<Event> Events { get { return ProcessElements.Values.OfType<Event>(); } }
        public IEnumerable<Gateway> Gateways { get { return ProcessElements.Values.OfType<Gateway>(); } }

        public Process() { }
        public Process(XElement xElement, IDictionary<string, ProcessRole> roles, IDictionary<string, ProcessUser> users)
        {
            Id = xElement.Attribute("Id")?.Value;
            BpmnId = xElement.Element("BpmnId")?.Value ?? Id;
            Name = xElement.Element("Name")?.Value;
            CustomScript = xElement.Element("CustomScript")?.Value;
            ParticipantId = xElement.Element("ParticipantId")?.Value;
            if (bool.TryParse(xElement.Element("IsExecutable")?.Value, out var isExecutable))
                IsExecutable = isExecutable;

            SequenceFlows = xElement.Element("SequenceFlows")?.Elements("SequenceFlow")?
                .Select(e => new SequenceFlow(e)).ToDictionary(s => s.Id) ?? SequenceFlows;
            ProcessElements = xElement.Element("ProcessElements")?.Elements()?
                .Select(e => CreateProcessElement(e, roles, users)).ToDictionary(e => e.Id) ?? ProcessElements;
        }

        private ProcessElement CreateProcessElement(XElement element, IDictionary<string, ProcessRole> roles, IDictionary<string, ProcessUser> users)
        {
            switch(element.Name.LocalName)
            {
                case ElementNames.TASK: return new Task(element);
                case ElementNames.BUSINESS_RULE_TASK: return new BusinessRuleTask(element); 
                case ElementNames.SCRIPT_TASK: return new ScriptTask(element); 
                case ElementNames.SERVICE_TASK: return new ServiceTask(element); 
                case ElementNames.USER_TASK: return new UserTask(element, roles, users);
                case ElementNames.CALL_ACTIVITY: return new CallActivity(element);
                case ElementNames.EXCLUSIVE_GATEWAY: return new ExclusiveGateway(element); 
                case ElementNames.PARALLEL_GATEWAY: return new ParallelGateway(element); 
                case ElementNames.EVENT: return new Event(element); 
                case ElementNames.BOUNDARY_EVENT: return new BoundaryEvent(element); 
                case ElementNames.END_EVENT: return new EndEvent(element); 
                case ElementNames.START_EVENT: return new StartEvent(element); 
                case ElementNames.TIMER_BOUNDARY_EVENT: return new TimerBoundaryEvent(element);
                default: throw new Exception($"Invalid process element name: {element.Name.LocalName}");
            }
        }

        public XElement ToXElement()
        {
            return new XElement("Process",
                new XAttribute("Id", Id),
                new XElement("BpmnId", BpmnId),
                new XElement("Name", Name),
                new XElement("IsExecutable", IsExecutable),
                new XElement("ParticipantId", ParticipantId),
                new XElement("CustomScript", CustomScript),
                new XElement("SequenceFlows", SequenceFlows.Select(s => s.Value.ToXElement()).ToList()),
                new XElement("ProcessElements", ProcessElements.Select(e => e.Value.ToXElement()).ToList())
            );
        }
    }
}
