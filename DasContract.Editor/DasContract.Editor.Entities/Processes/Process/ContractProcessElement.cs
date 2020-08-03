using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Entities.Processes.Process.Activities;
using DasContract.Editor.Entities.Processes.Process.Events;
using DasContract.Editor.Entities.Processes.Process.Gateways;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Processes.Process
{
    [
        XmlInclude(typeof(ContractActivity)),
        XmlInclude(typeof(ContractBusinessRuleActivity)),
        XmlInclude(typeof(ContractScriptActivity)),
        XmlInclude(typeof(ContractUserActivity)),

        XmlInclude(typeof(ContractGateway)),
        XmlInclude(typeof(ContractExclusiveGateway)),
        XmlInclude(typeof(ContractParallelGateway)),

        XmlInclude(typeof(ContractEvent)),
        XmlInclude(typeof(ContractEndEvent)),
        XmlInclude(typeof(ContractStartEvent))
    ]
    public abstract class ContractProcessElement : IIdentifiable, INamable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<string> Incoming { get; set; } = new List<string>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "DTO class")]
        public List<string> Outgoing { get; set; } = new List<string>();

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        protected IMigrator Migrator { get; set; } = new SimpleMigrator();
    }
}
