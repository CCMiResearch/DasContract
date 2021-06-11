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
    public class Process
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public bool IsExecutable { get; set; }

        public string CustomScript { get; set; }

        public IDictionary<string, SequenceFlow> SequenceFlows { get; set; } = new Dictionary<string, SequenceFlow>(); 
        public IDictionary<string, ProcessElement> ProcessElements { get; set; } = new Dictionary<string, ProcessElement>();
        
        [JsonIgnore]
        public IEnumerable<Task> Tasks { get { return ProcessElements.Values.OfType<Task>(); } }
        [JsonIgnore]
        public IEnumerable<Event> Events { get { return ProcessElements.Values.OfType<Event>(); } }
        [JsonIgnore]
        public IEnumerable<Gateway> Gateways { get { return ProcessElements.Values.OfType<Gateway>(); } }

        public Process() { }
        public Process(string bpmnXml)
        {
            throw new NotImplementedException();
        }

        public XElement ToXElement()
        {
            return new XElement("Process",
                new XAttribute("Id", Id),
                new XElement("Name", Name),
                new XElement("IsExecutable", IsExecutable),
                new XElement("CustomScript", CustomScript),
                new XElement("SequenceFlows", SequenceFlows.Select(s => s.Value.ToXElement()).ToList()),
                new XElement("ProcessElements", ProcessElements.Select(e => e.Value.ToXElement()).ToList())
            );
        }
    }
}
