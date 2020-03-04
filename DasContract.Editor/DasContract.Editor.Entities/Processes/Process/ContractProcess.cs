using DasContract.Editor.Entities.Processes.Process.Activities;
using DasContract.Editor.Entities.Processes.Process.Events;
using DasContract.Editor.Entities.Processes.Process.Gateways;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DasContract.Editor.Migrator.Extensions;


namespace DasContract.Editor.Entities.Processes.Process
{
    public class ContractProcess: IMigratableComponent<ContractProcess, IMigrator>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<ContractSequenceFlow> SequenceFlows { get; set; } = new List<ContractSequenceFlow>();


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<ContractProcessElement> ProcessElements { get; set; } = new List<ContractProcessElement>();

        public IEnumerable<ContractEvent> Events => ProcessElements.OfType<ContractEvent>();
        public IEnumerable<ContractGateway> Gateways => ProcessElements.OfType<ContractGateway>();

        public IEnumerable<ContractActivity> Activities => ProcessElements.OfType<ContractActivity>();
        public IEnumerable<ContractBusinessRuleActivity> BusinessActivities => ProcessElements.OfType<ContractBusinessRuleActivity>().WithMigrator(migrator);
        public IEnumerable<ContractScriptActivity> ScriptActivities => ProcessElements.OfType<ContractScriptActivity>().WithMigrator(migrator);
        public IEnumerable<ContractUserActivity> UserActivities => ProcessElements.OfType<ContractUserActivity>().WithMigrator(migrator);

        public IEnumerable<ContractStartEvent> StartEvents => ProcessElements.OfType<ContractStartEvent>().WithMigrator(migrator);
        public IEnumerable<ContractEndEvent> EndEvents => ProcessElements.OfType<ContractEndEvent>().WithMigrator(migrator);

        public static ContractProcess Empty()
        {
            return new ContractProcess();
        }

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public ContractProcess WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }
}
