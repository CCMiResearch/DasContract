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
        public Process(XElement xElement)
        {
            Id = xElement.Attribute("Id")?.Value;
            Name = xElement.Element("Name")?.Value;
            CustomScript = xElement.Element("CustomScript")?.Value;
            if (bool.TryParse(xElement.Element("IsExecutable")?.Value, out var isExecutable))
                IsExecutable = isExecutable;

            SequenceFlows = xElement.Element("SequenceFlows")?.Elements("SequenceFlow")?.
                Select(e => new SequenceFlow(e)).ToDictionary(s => s.Id) ?? SequenceFlows;
            ProcessElements = CreateProcessElements(xElement.Element("ProcessElements"));
        }

        private IDictionary<string, ProcessElement> CreateProcessElements(XElement xElement)
        {
            IEnumerable<ProcessElement> processElements = new List<ProcessElement>();
            if (xElement != null)
            {
                var tasks = CreateTasks(xElement);
                var gateways = CreateGateways(xElement);
                var events = CreateEvents(xElement);

                if (tasks != null)
                    processElements = processElements.Concat(tasks);
                if (gateways != null)
                    processElements = processElements.Concat(gateways);
                if (events != null)
                    processElements = processElements.Concat(events);
            }
            return processElements.ToDictionary(e => e.Id);
        }

        private IEnumerable<Task> CreateTasks(XElement xElement)
        {
            var tasks = xElement.Elements("Task")?.Select(e => new Task(e));
            var businessRuleTasks = xElement.Elements("BusinessRuleTask")?.Select(e => new BusinessRuleTask(e));
            var callActivities = xElement.Elements("CallActivity")?.Select(e => new CallActivity(e));
            var scriptTasks = xElement.Elements("ScriptTask")?.Select(e => new ScriptTask(e));
            var serviceTasks = xElement.Elements("ServiceTask")?.Select(e => new ServiceTask(e));
            var userTasks = xElement.Elements("UserTask")?.Select(e => new UserTask(e));
            
            if (tasks == null)
                tasks = new List<Task>();
            if (businessRuleTasks != null)
                tasks = tasks.Concat(businessRuleTasks);
            if (callActivities != null)
                tasks = tasks.Concat(callActivities);
            if (scriptTasks != null)
                tasks = tasks.Concat(scriptTasks);
            if (serviceTasks != null)
                tasks = tasks.Concat(serviceTasks);
            if (userTasks != null)
                tasks = tasks.Concat(userTasks);

            return tasks.ToList();
        }

        private IEnumerable<Gateway> CreateGateways(XElement xElement)
        {
            IEnumerable<Gateway> gateways = new List<Gateway>();
            var parallelGateways = xElement.Elements("ParallelGateway")?.Select(e => new ParallelGateway(e));
            var exclusiveGateways = xElement.Elements("ExclusiveGateway")?.Select(e => new ExclusiveGateway(e));

            if (parallelGateways != null)
                gateways = gateways.Concat(parallelGateways);
            if (exclusiveGateways != null)
                gateways = gateways.Concat(exclusiveGateways);

            return gateways.ToList();
        }

        private IEnumerable<Event> CreateEvents(XElement xElement)
        {
            var events = xElement.Elements("Event")?.Select(e => new Event(e));
            var endEvents = xElement.Elements("EndEvent")?.Select(e => new EndEvent(e));
            var startEvents = xElement.Elements("StartEvent")?.Select(e => new StartEvent(e));
            var boundaryEvents = xElement.Elements("BoundaryEvent")?.Select(e => new BoundaryEvent(e));
            var timerBoundaryEvents = xElement.Elements("TimerBoundaryEvent")?.Select(e => new TimerBoundaryEvent(e));

            if (events == null)
                events = new List<Event>();
            if (endEvents != null)
                events = events.Concat(endEvents);
            if (startEvents != null)
                events = events.Concat(startEvents);
            if (boundaryEvents != null)
                events = events.Concat(boundaryEvents);
            if (timerBoundaryEvents != null)
                events = events.Concat(timerBoundaryEvents);

            return events.ToList();
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
