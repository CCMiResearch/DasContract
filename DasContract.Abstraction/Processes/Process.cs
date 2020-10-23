using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Abstraction.Processes
{
    public class Process
    {
        public string Id { get; set; }

        public bool IsExecutable { get; set; }

        public IDictionary<string, SequenceFlow> SequenceFlows { get; set; } = new Dictionary<string, SequenceFlow>(); 
        public IDictionary<string, ProcessElement> ProcessElements { get; set; } = new Dictionary<string, ProcessElement>();
        
        public IEnumerable<Task> Tasks { get { return ProcessElements.Values.OfType<Task>(); } }
        public IEnumerable<Event> Events { get { return ProcessElements.Values.OfType<Event>(); } }
        public IEnumerable<Gateway> Gateways { get { return ProcessElements.Values.OfType<Gateway>(); } }

        public Process() { }
        public Process(string bpmnXml)
        {
            throw new NotImplementedException();
        }

        public string ToBpmnXml()
        {
            throw new NotImplementedException(); 
        }
    }
}
