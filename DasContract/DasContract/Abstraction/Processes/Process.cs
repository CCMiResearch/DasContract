using DasContract.Abstraction.Processes.Events;
using DasContract.Abstraction.Processes.Gateways;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.DasContract.Abstraction.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Abstraction.Processes
{
    public class Process
    {
        public List<SequenceFlow> SequenceFlows { get; set; } = new List<SequenceFlow>(); 
        public List<ProcessElement> ProcessElements { get; set; } = new List<ProcessElement>();

        public IEnumerable<Event> Events { get { return ProcessElements.OfType<Event>(); } }
        public IEnumerable<Gateway> Gateways { get { return ProcessElements.OfType<Gateway>(); } }


        public IEnumerable<Task> Activities => ProcessElements.OfType<Task>(); 

        public IEnumerable<BusinessRuleTask> BusinessActivities => ProcessElements.OfType<BusinessRuleTask>(); 

        public IEnumerable<ScriptTask> ScriptActivities => ProcessElements.OfType<ScriptTask>();

        public IEnumerable<UserTask> UserActivities => ProcessElements.OfType<UserTask>();

        public StartEvent StartEvent => ProcessElements.OfType<StartEvent>().SingleOrDefault();
    }
}
