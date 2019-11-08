using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Abstraction.Processes
{
    public class Process
    {
        public string Id { get; set; }

        public IDictionary<string, SequenceFlow> SequenceFlows { get; set; } = new Dictionary<string, SequenceFlow>(); 
        public IDictionary<string, IProcessElement> ProcessElements { get; set; } = new Dictionary<string, IProcessElement>();
        
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
