using DasContract.Editor.Entities.Processes.Process.Activities;
using DasContract.Editor.Entities.Processes.Process.Events;
using DasContract.Editor.Entities.Processes.Process.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Editor.Entities.Processes.Process
{
    public class ContractProcess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<ContractSequenceFlow> SequenceFlows { get; set; } = new List<ContractSequenceFlow>();


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<ContractProcessElement> ProcessElements { get; set; } = new List<ContractProcessElement>();

        public IEnumerable<ContractEvent> Events => ProcessElements.OfType<ContractEvent>();
        public IEnumerable<ContractGateway> Gateways => ProcessElements.OfType<ContractGateway>();

        public IEnumerable<ContractActivity> Activities => ProcessElements.OfType<ContractActivity>();
        public IEnumerable<ContractBusinessRuleActivity> BusinessActivities => ProcessElements.OfType<ContractBusinessRuleActivity>();
        public IEnumerable<ContractScriptActivity> ScriptActivities => ProcessElements.OfType<ContractScriptActivity>();
        public IEnumerable<ContractUserActivity> UserActivities => ProcessElements.OfType<ContractUserActivity>();

        public IEnumerable<ContractStartEvent> StartEvents => ProcessElements.OfType<ContractStartEvent>().ToList();
        public IEnumerable<ContractEndEvent> EndEvents => ProcessElements.OfType<ContractEndEvent>().ToList();

        public static ContractProcess Empty()
        {
            return new ContractProcess();
        }
    }
}
