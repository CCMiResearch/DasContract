using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DasContract.Editor.Entities.Exceptions;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Entities.Processes.Diagrams;
using DasContract.Editor.Entities.Processes.Factories;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Processes.Process.Activities;
using DasContract.Editor.Migrator;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Entities.Processes
{
    public class ContractProcesses : IMigratableComponent<ContractProcesses, IMigrator>
    {
        public event OnDiagramChangeHandler OnDiagramChange;

        /// <summary>
        /// BPMN 2.0 XML with process description and a visual process information for the main process
        /// </summary>
        public BPMNProcessDiagram Diagram
        {
            get => diagram;
            set
            {
                var oldDiagram = diagram;
                diagram = value;
                OnDiagramChange?.Invoke(this, oldDiagram, diagram);
            }
        }
        BPMNProcessDiagram diagram = BPMNProcessDiagram.Default();

        /// <summary>
        /// Contract main process. Currently only one process is allowed. 
        /// </summary>
        public ContractProcess Main
        {
            get => main.WithMigrator(migrator);
            set => main = value;
        }

        ContractProcess main = ContractProcess.Empty();

        //--------------------------------------------------
        //                  MIGRATOR
        //--------------------------------------------------
        IMigrator migrator = new SimpleMigrator();

        public ContractProcesses WithMigrator(IMigrator parentMigrator)
        {
            migrator = parentMigrator;
            return this;
        }
    }

    public delegate void OnDiagramChangeHandler(ContractProcesses caller, BPMNProcessDiagram oldValue, BPMNProcessDiagram currentValue);
}
